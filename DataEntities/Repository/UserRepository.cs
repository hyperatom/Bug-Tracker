using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataEntities.Entity;
using DataEntities.Model;

namespace DataEntities.Repository
{
    public class UserRepository : Repository
    {

        public UserRepository() : base() { }


        public User Create(User user)
        {
            using (var ctx = new WcfEntityContext())
            {
                ctx.Users.AddObject(user);

                ctx.SaveChanges();

                return user;
            }
        }


        public IQueryable<User> GetAll()
        {
            return Context.Users;
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


        public IQueryable<User> FullTextSearch(String searchText)
        {
            return Context.Users.FullTextSearch(searchText);
        }

    }
}
