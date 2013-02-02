using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataEntities.Entity;

namespace DataEntities.Repository
{
    public class BugRepository : Repository
    {

        public BugRepository() : base() { }


        public Bug Create(Bug bug)
        {
            Context.Bugs.AddObject(bug);
            Context.SaveChanges();

            return bug;
        }


        public IQueryable<Bug> GetAll()
        {
            IQueryable<Bug> bugs = Context.Bugs.Include("CreatedBy").Include("Project");

            return bugs;
        }


        public Bug Update(Bug bug)
        {
            Context.AttachModify("Bugs", bug);
            Context.SaveChanges();

            return bug;
        }


        public void Delete(Bug bug)
        {
            Bug mybug = Context.Bugs.Where(p => p.Id == bug.Id).FirstOrDefault();
            Context.Bugs.Attach(mybug);
            Context.Bugs.DeleteObject(mybug);

            Context.SaveChanges();
        }

    }
}
