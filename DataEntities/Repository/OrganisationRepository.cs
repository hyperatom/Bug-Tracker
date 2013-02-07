using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataEntities.Entity;

namespace DataEntities.Repository
{
    public class OrganisationRepository : Repository
    {

        public OrganisationRepository() : base() { }


        public Organisation Create(Organisation organisation)
        {
            Context.Organisations.AddObject(organisation);
            Context.SaveChanges();

            return organisation;
        }


        public IQueryable<Organisation> GetAll()
        {
            IQueryable<Organisation> organisations = Context.Organisations;

            return organisations;
        }


        public Organisation Update(Organisation organisation)
        {
            Context.AttachModify("Organisations", organisation);
            Context.SaveChanges();

            return organisation;
        }


        public void Delete(Organisation organisation)
        {
            Organisation myOrganisation = Context.Organisations.Where(p => p.Id == organisation.Id).FirstOrDefault();
            Context.Organisations.Attach(myOrganisation);
            Context.Organisations.DeleteObject(myOrganisation);

            Context.SaveChanges();
        }

    }
}

