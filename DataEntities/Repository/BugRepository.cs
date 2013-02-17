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
            using (var ctx = new WcfEntityContext())
            {
                bug.AssignedUser.Projects = null;
                bug.AssignedUser.Organisation = null;
                bug.AssignedUser.Roles = null;

                bug.CreatedBy.Projects = null;
                bug.CreatedBy.Organisation = null;
                bug.CreatedBy.Roles = null;

                if (bug.AssignedUser != null)
                    ctx.AttachTo("Users", bug.AssignedUser);

                ctx.AttachTo("Projects", bug.Project);
                
                ctx.AttachTo("Users", bug.CreatedBy);

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
            using (var ctx = new WcfEntityContext())
            {
                var mybug = ctx.Bugs.Include("CreatedBy").Include("Project").Include("AssignedUser").Where(i => i.Id == bug.Id).SingleOrDefault();

                var userAssigned = ctx.Users.Where(u => u.Id == bug.AssignedUser.Id).SingleOrDefault();
                var userCreated = ctx.Users.Where(n => n.Id == bug.CreatedBy.Id).SingleOrDefault();
                var assignedProject = ctx.Projects.Where(c => c.Id == bug.Project.Id).SingleOrDefault();

                if (userAssigned != null)
                    mybug.AssignedUser = userAssigned;

                if (userCreated != null)
                    mybug.CreatedBy = userCreated;

                if (assignedProject != null)
                    mybug.Project = assignedProject;
    
                ctx.AttachModify("Bugs", mybug);
                ctx.SaveChanges();

                return mybug;
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
