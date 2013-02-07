﻿using System;
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
            Context.Users.AddObject(user);
            Context.SaveChanges();

            return user;
        }


        public IQueryable<User> GetAll()
        {
            IQueryable<User> users = Context.Users.Include("Projects").Include("Roles").Include("Organisation");

            return users;
        }


        public User Update(User user)
        {
            Context.AttachModify("Users", user);
            Context.SaveChanges();

            return user;
        }


        public void Delete(User user)
        {
            Context.AttachTo("Users", user);
            Context.Users.DeleteObject(user);

            Context.SaveChanges();
        }

    }
}
