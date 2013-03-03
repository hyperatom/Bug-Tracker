using System;
using System.ServiceModel;
using System.ServiceModel.Description;
using Client.ServiceReference;
using Microsoft.Practices.Unity;

namespace Client.Factories
{
    /// <summary>
    /// This class allows objects dependant on the bug tracker web service to
    /// access and modify it via a factory interface.
    /// </summary>
    public class ServiceFactory : IServiceFactory
    {

        private IUnityContainer _container;
        private ClientBase<ITrackerService> _Service;


        /// <summary>
        /// Stores references to dependencies.
        /// </summary>
        /// <param name="container">Unity container.</param>
        /// <param name="svc">Web service channel.</param>
        public ServiceFactory(IUnityContainer container, ClientBase<ITrackerService> svc)
        {
            _container = container;
            _Service = svc;
        }


        /// <summary>
        /// Creates a new service while updating the IOC container reference.
        /// If the client is faulted, create a new channel and update the IOC reference.
        /// </summary>
        /// <param name="username">Web service username.</param>
        /// <param name="password">Web service password.</param>
        public ClientBase<ITrackerService> CreateService(String username, String password)
        {
            if (_Service.State.Equals(CommunicationState.Faulted))
            {
                _Service = new TrackerServiceClient();
            }

            ClientCredentials clientCredentials = new ClientCredentials();

            clientCredentials.UserName.UserName = username;
            clientCredentials.UserName.Password = password;

            _Service.ChannelFactory.Endpoint.Behaviors.Remove(typeof(ClientCredentials));
            _Service.ChannelFactory.Endpoint.Behaviors.Add(clientCredentials);

            _container.RegisterInstance<ITrackerService>((ITrackerService)_Service);

            return _Service;
        }

    }
}
