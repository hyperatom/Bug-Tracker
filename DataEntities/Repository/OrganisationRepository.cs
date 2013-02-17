using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataEntities.Entity;

namespace DataEntities.Repository
{
    public class OrganisationRepository : Repository
    {

        public OrganisationRepository() { }


        public Organisation Create(Organisation organisation)
        {
            using (var ctx = new WcfEntityContext())
            {
                ctx.Organisations.AddObject(organisation);
                ctx.SaveChanges();

                return organisation;
            }
        }


        public IList<Organisation> GetAll()
        {
            using (var ctx = new WcfEntityContext())
            {
                IList<Organisation> organisations = ctx.Organisations.ToList();

                return organisations;
            }
        }


        public Organisation Update(Organisation organisation)
        {
            using (var ctx = new WcfEntityContext())
            {
                ctx.AttachModify("Organisations", organisation);
                ctx.SaveChanges();

                return organisation;
            }
        }


        public void Delete(Organisation organisation)
        {
            using (var ctx = new WcfEntityContext())
            {
                Organisation myOrganisation = ctx.Organisations.Where(p => p.Id == organisation.Id).FirstOrDefault();
                ctx.Organisations.Attach(myOrganisation);
                ctx.Organisations.DeleteObject(myOrganisation);

                ctx.SaveChanges();
            }
        }

    }
}

