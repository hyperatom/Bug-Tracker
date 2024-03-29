﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataEntities.Entity;
using System.Data.Objects;
using System.Data;

namespace DataEntities.Repository
{
    public class ProjectRoleRepository : Repository
    {

        public ProjectRoleRepository() : base() { }


        public ProjectRole Create(ProjectRole projRole)
        {
            using (var ctx = new WcfEntityContext())
            {
                ctx.ProjectRole.AddObject(projRole);

                ctx.SaveChanges();

                return projRole;
            }
        }


        public IList<ProjectRole> GetAll()
        {
            using (var ctx = new WcfEntityContext())
            {
                return ctx.ProjectRole.Include("User").Include("Project").Include("Role").ToList();
            }
        }


        public ProjectRole Update(ProjectRole projRole)
        {
            using (var ctx = new WcfEntityContext())
            {
                ctx.AttachModify("ProjectRoles", projRole);

                return projRole;
            }
        }


        public void Delete(ProjectRole projRole)
        {
            using (var ctx = new WcfEntityContext())
            {
                var entityToDelete = ctx.ProjectRole.Where(p => p.ProjectId == projRole.ProjectId 
                                                           && p.UserId == projRole.UserId 
                                                           && p.RoleId == projRole.RoleId).SingleOrDefault();

                ctx.ProjectRole.DeleteObject(entityToDelete);
                ctx.SaveChanges();
            }
        }

    }
}
