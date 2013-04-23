using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataEntities.Entity;
using System.Data.Objects;
using System.Data;
using DataEntities.Model;

namespace DataEntities.Repository
{

    public class BugActionRepository : Repository
    {

        public BugActionRepository() : base() { }


        public IQueryable<BugAction> GetAll()
        {
            return Context.Actions;
        }

    }
}
