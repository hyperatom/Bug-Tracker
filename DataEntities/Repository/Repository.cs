using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataEntities.Entity;

namespace DataEntities.Repository
{
    public abstract class Repository
    {
        
        private WcfEntityContext _context;
        public WcfEntityContext Context
        {
            get { return _context; }
            set { _context = value; }
        }


        public Repository()
        {
            Context = new WcfEntityContext();
        }

    }
}
