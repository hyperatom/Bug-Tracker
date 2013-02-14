using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Client.ServiceReference;
using System.ServiceModel;

namespace Client.Services
{
    public static class TrackerService
    {

        private static TrackerServiceClient _Service;


        public static TrackerServiceClient Service
        {
            get 
            {
                if (_Service == null)
                {
                    _Service = new TrackerServiceClient();
                }
                else if (_Service.State.Equals(CommunicationState.Faulted))
                {
                    ReCreateChannel();
                }

                return _Service;
            }

            set
            {
                _Service = value;
            }
        }


        public static void ReCreateChannel()
        {
            _Service = new TrackerServiceClient();
        }

    }
}
