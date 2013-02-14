using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataEntities.Entity;

namespace DataEntities.Repository
{
    public class RoleRepository : Repository
    {

        public RoleRepository() : base() { }


        public Role Create(Role role)
        {
            Context.Roles.AddObject(role);
            Context.SaveChanges();

            return role;
        }


        public IQueryable<Role> GetAll()
        {
            IQueryable<Role> roles = Context.Roles;

            return roles;
        }


        public Role Update(Role role)
        {
            Context.AttachModify("Roles", role);
            Context.SaveChanges();

            return role;
        }


        public void Delete(Role role)
        {
            Role myrole = Context.Roles.Where(p => p.Id == role.Id).FirstOrDefault();
            Context.Roles.Attach(myrole);
            Context.Roles.DeleteObject(myrole);

            Context.SaveChanges();
        }

    }
}
