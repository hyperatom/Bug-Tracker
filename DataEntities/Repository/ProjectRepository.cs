using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataEntities.Entity;

namespace DataEntities.Repository
{
    public class ProjectRepository : Repository
    {

        public ProjectRepository() { }


        public Project Create(Project project)
        {
            using (var ctx = new WcfEntityContext())
            {
                ctx.Projects.AddObject(project);
                ctx.SaveChanges();

                return project;
            }
        }


        public IList<Project> GetAll()
        {
            using (var ctx = new WcfEntityContext())
            {
                IList<Project> projects = ctx.Projects.ToList();

                return projects;
            }
        }


        public Project Update(Project project)
        {
            using (var ctx = new WcfEntityContext())
            {
                ctx.AttachModify("Projects", project);
                ctx.SaveChanges();

                return project;
            }
        }


        public void Delete(Project project)
        {
            using (var ctx = new WcfEntityContext())
            {
                var proj = Context.Projects.Where(p => p.Id == project.Id).SingleOrDefault();

                ctx.AttachTo("Projects", proj);
                ctx.Projects.DeleteObject(proj);

                ctx.SaveChanges();
            }
        }

    }
}
