using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataEntities.Entity;

namespace DataEntities.Repository
{
    public class RoleRepository : Repository
    {

        public RoleRepository() { }


        public Role Create(Role role)
        {
            using (var ctx = new WcfEntityContext())
            {
                ctx.Roles.AddObject(role);
                ctx.SaveChanges();

                return role;
            }
        }


        public IList<Role> GetAll()
        {
            using (var ctx = new WcfEntityContext())
            {
                IList<Role> roles = ctx.Roles.ToList();

                return roles;
            }
        }


        public Role Update(Role role)
        {
            using (var ctx = new WcfEntityContext())
            {
                ctx.AttachModify("Roles", role);
                ctx.SaveChanges();

                return role;
            }
        }


        public void Delete(Role role)
        {
            using (var ctx = new WcfEntityContext())
            {
                Role myrole = ctx.Roles.Where(p => p.Id == role.Id).FirstOrDefault();
                ctx.Roles.Attach(myrole);
                ctx.Roles.DeleteObject(myrole);

                ctx.SaveChanges();
            }
        }

    }
}
