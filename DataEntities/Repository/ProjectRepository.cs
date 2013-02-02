using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataEntities.Entity;

namespace DataEntities.Repository
{
    public class ProjectRepository : Repository
    {

        public ProjectRepository() : base() { }


        public Project Create(Project project)
        {
            Context.Projects.AddObject(project);
            Context.SaveChanges();

            return project;
        }


        public IQueryable<Project> GetAll()
        {
            IQueryable<Project> projects = Context.Projects;

            return projects;
        }


        public Project Update(Project project)
        {
            Context.AttachModify("Projects", project);
            Context.SaveChanges();

            return project;
        }


        public void Delete(Project project)
        {
            Context.AttachTo("Projects", project);
            Context.Projects.DeleteObject(project);

            Context.SaveChanges();
        }

    }
}
