using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity;
using Client.Services;
using Client.ServiceReference;

namespace Client.Helpers
{
    public static class IOC
    {

        private static UnityContainer _container;


        public static UnityContainer Container
        {
            get
            {
                if (_container == null)
                {
                    _container = new UnityContainer();
                    Initialise();
                }

                return _container;
            }

            set { _container = value; }
        }


        public static void Initialise()
        {
            _container.RegisterInstance<ITrackerService>(TrackerService.Service);
            _container.RegisterInstance<IMessenger>(new Messenger());
        }

    }
}
