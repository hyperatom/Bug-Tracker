using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataEntities.Entity;
using System.Data.Objects;
using System.Data;

namespace DataEntities.Repository
{
    public class BugRepository : Repository
    {

        public BugRepository() : base() { }


        public Bug Create(Bug bug)
        {
            bug.DateFound    = DateTime.Now;
            bug.LastModified = DateTime.Now;

            using (var ctx = new WcfEntityContext())
            {
                var project      = ctx.Projects.Where(x => x.Id == bug.Project.Id)     .SingleOrDefault();
                var createdBy    = ctx.Users.Where   (u => u.Id == bug.CreatedBy.Id)   .SingleOrDefault();

                if (bug.AssignedUser != null)
                {
                    var assignedUser = ctx.Users.Where(p => p.Id == bug.AssignedUser.Id).SingleOrDefault();
                    bug.AssignedUser = assignedUser;
                }

                bug.CreatedBy    = createdBy;
                bug.Project      = project;

                ctx.Bugs.AddObject(bug);
                ctx.SaveChanges();

                return bug;
            }
        }


        public IQueryable<Bug> GetAll()
        {
            IQueryable<Bug> bugs = Context.Bugs.Include("CreatedBy").Include("Project").Include("AssignedUser");

            return bugs;
        }


        public Bug Update(Bug bug)
        {
            bug.LastModified = DateTime.Now;

            using (var ctx = new WcfEntityContext())
            {
                var oldBug = ctx.Bugs.Include("CreatedBy").Include("Project").Include("AssignedUser")
                    .Where(h => h.Id == bug.Id).SingleOrDefault();
           
                var userCreated = ctx.Users.Where(n => n.Id == bug.CreatedBy.Id).SingleOrDefault();
                var assignedProject = ctx.Projects.Where(c => c.Id == bug.Project.Id).SingleOrDefault();

                if (bug.AssignedUser != null)
                {
                    var userAssigned = ctx.Users.Where(u => u.Id == bug.AssignedUser.Id).SingleOrDefault();
                    oldBug.AssignedUser = userAssigned;
                }

                oldBug.CreatedBy = userCreated;
                oldBug.Project = assignedProject;

                ctx.Bugs.ApplyCurrentValues(bug);

                ctx.SaveChanges();

                return bug;
            }
        }


        public void Delete(Bug bug)
        {
            using (var ctx = new WcfEntityContext())
            {
                Bug mybug = ctx.Bugs.Where(p => p.Id == bug.Id).FirstOrDefault();
                ctx.Bugs.Attach(mybug);
                ctx.Bugs.DeleteObject(mybug);

                ctx.SaveChanges();
            }
        }

    }
}
