using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataEntities.Entity;
using System.Data.Objects;
using System.Data;
using DataEntities.Model;

namespace DataEntities.Repository
{

    public class BugActionLogRepository : Repository
    {

        public BugActionLogRepository() : base() { }


        public BugActionLog Create(BugActionLog log)
        {
            log.Date = DateTime.Now;

            using (var ctx = new WcfEntityContext())
            {
                log.Action = ctx.Actions.Where(p => p.Id == log.Action.Id).SingleOrDefault();
                log.Project = ctx.Projects.Where(p => log.Project.Id == p.Id).SingleOrDefault();

                ctx.BugActionLogs.AddObject(log);

                ctx.SaveChanges();

                return log;
            }
        }


        public BugActionLog Update(BugActionLog log)
        {
            using (var ctx = new WcfEntityContext())
            {
                ctx.AttachModify("BugActionLogs", log);

                return log;
            }
        }


        public IQueryable<BugActionLog> GetAll()
        {
            IQueryable<BugActionLog> bugs = Context.BugActionLogs.Include("Action").Include("Project");

            return bugs;
        }


        public void Delete(BugActionLog log)
        {
            using (var ctx = new WcfEntityContext())
            {
                BugActionLog logEntity = ctx.BugActionLogs.Where(p => p.Id == log.Id).FirstOrDefault();
                ctx.BugActionLogs.Attach(logEntity);
                ctx.BugActionLogs.DeleteObject(logEntity);

                ctx.SaveChanges();
            }
        }


        public IQueryable<BugActionLog> FullTextSearch(String searchText)
        {
            return Context.BugActionLogs.Include("Action").Include("Project").FullTextSearch(searchText);
        }

    }
}
