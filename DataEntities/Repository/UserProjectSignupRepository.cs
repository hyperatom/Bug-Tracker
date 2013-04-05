using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataEntities.Entity;
using System.Data.Objects;
using System.Data;

namespace DataEntities.Repository
{
    public class UserProjectSignupRepository : Repository
    {

        public UserProjectSignupRepository() : base() { }


        public UserProjectSignup Create(UserProjectSignup projSignup)
        {
            using (var ctx = new WcfEntityContext())
            {
                ctx.UserProjectSignup.AddObject(projSignup);
             
                ctx.SaveChanges();

                return projSignup;
            }
        }


        public IList<UserProjectSignup> GetAll()
        {
            using (var ctx = new WcfEntityContext())
            {
                return ctx.UserProjectSignup.Include("User").Include("Project").Include("Role").ToList();
            }
        }


        public UserProjectSignup Update(UserProjectSignup projRole)
        {
            using (var ctx = new WcfEntityContext())
            {
                ctx.AttachModify("UserProjectSignups", projRole);

                return projRole;
            }
        }


        public void Delete(UserProjectSignup projRole)
        {
            using (var ctx = new WcfEntityContext())
            {
                var entityToDelete = ctx.UserProjectSignup.Where(p => p.ProjectId == projRole.ProjectId
                                                           && p.UserId == projRole.UserId
                                                           && p.RoleId == projRole.RoleId).SingleOrDefault();

                ctx.UserProjectSignup.DeleteObject(entityToDelete);
                ctx.SaveChanges();
            }
        }

    }
}
