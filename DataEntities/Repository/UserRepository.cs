using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataEntities.Entity;

namespace DataEntities.Repository
{
    public class UserRepository : Repository
    {

        public UserRepository() : base() { }


        public User Create(User user)
        {
            using (var ctx = new WcfEntityContext())
            {

                /*foreach (ProjectRole projRole in user.ProjectRoles)
                {
                    ctx.AttachTo("Roles", projRole.);
                }

                if (user.Projects != null)
                {
                    foreach (Project proj in user.Projects)
                    {
                        ctx.AttachTo("Projects", proj);
                    }
                }*/

                ctx.Users.AddObject(user);

                ctx.SaveChanges();

                return user;
            }
        }


        public IList<User> GetAll()
        {
            using (var ctx = new WcfEntityContext())
            {
                IList<User> users = ctx.Users.ToList();

                return users;
            }
        }


        public User Update(User user)
        {
            using (var ctx = new WcfEntityContext())
            {
                ctx.AttachModify("Users", user);
                ctx.SaveChanges();

                return user;
            }
        }


        public void Delete(User user)
        {
            using (var ctx = new WcfEntityContext())
            {
                ctx.AttachTo("Users", user);
                ctx.Users.DeleteObject(user);

                ctx.SaveChanges();
            }
        }

    }
}
