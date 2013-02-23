using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Client.ServiceReference;
using System.ServiceModel;
using System.ServiceModel.Description;

namespace Client.Services
{
    /// <summary>
    /// TrackerService encapsulates an instance of the service client.
    /// Static utility methods allow the setting of user credentials
    /// and provide access to the web service interface.
    /// </summary>
    public static class TrackerService
    {

        // Encapsulated service client
        private static TrackerServiceClient _Service;


        // Getter exposes only methods in service interface
        public static ITrackerService Service
        {
            get 
            {
                if (_Service == null)
                {
                    _Service = new TrackerServiceClient();
                }
                // If the channel gets faulted, create a new one
                else if (_Service.State.Equals(CommunicationState.Faulted))
                {
                    ReCreateChannel();
                }

                return _Service;
            }

            set
            {
                // Check if field is being set as expected type
                if (value.GetType() == typeof(TrackerServiceClient))
                    _Service = (TrackerServiceClient) value;
            }
        }


        /// <summary>
        /// Sets the credentials of a web service user by removing
        /// existing credentials and adding a new set.
        /// </summary>
        /// <param name="username">Username account identity.</param>
        /// <param name="password">Password of the service account.</param>
        public static void SetCredentials(String username, String password)
        {
            ClientCredentials clientCredentials = new ClientCredentials();

            clientCredentials.UserName.UserName = username;
            clientCredentials.UserName.Password = password;

            ReCreateChannel();

            _Service.ChannelFactory.Endpoint.Behaviors.Remove(typeof(ClientCredentials));
            _Service.ChannelFactory.Endpoint.Behaviors.Add(clientCredentials);
        }


        /// <summary>
        /// Allows the re-creation of the web service client
        /// </summary>
        public static void ReCreateChannel()
        {
            _Service = new TrackerServiceClient();
        }

    }
}
