using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity;
using Client.ServiceReference;

namespace Client.Helpers
{
    public sealed class IOC
    {

        private static UnityContainer _container = new UnityContainer();


        public static UnityContainer Container
        {
            get
            {
                if (_container == null)
                {
                    _container = new UnityContainer();
                }

                return _container;
            }

            //set { _container = value; }
        }

    }
}
