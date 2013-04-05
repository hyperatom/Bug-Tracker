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

    public class ActionRepository : Repository
    {

        public ActionRepository() : base() { }


        public IQueryable<DataEntities.Entity.Action> GetAll()
        {
            return Context.Actions;
        }

    }
}
