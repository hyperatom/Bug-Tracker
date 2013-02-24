using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Windows;
using Client.Helpers;
using Client.ServiceReference;
using Client.ServiceRegistration;
using Client.ViewModels;
using Microsoft.Practices.Unity;
using System.ServiceModel.Description;

namespace Client.Controllers
{
    /// <summary>
    /// This class allows the loading of views bound to their
    /// corresponding view model classes.
    /// </summary>
    public class WindowLoader : IWindowLoader
    {
        // Stores the type mappings between view-viewmodel
        private IDictionary<Type, Type> _ViewDictionary;
        
        private IMessenger      _Messenger;
        private IUnityContainer _Container;
        private ClientBase<ITrackerService> _TrackerClient;


        /// <summary>
        /// Adds type mappings between view-viewmodels to dictionary.
        /// </summary>
        public WindowLoader()
        {
            _Messenger = new Messenger();
            _TrackerClient = new TrackerServiceClient();
            _Container = IOC.Container;

            InitialiseDIContainer();
            InitialiseDictionary();

            ShowView(Windows.Login);
        }


        private void InitialiseDIContainer()
        {
            // Register instances to resolve
            _Container.RegisterInstance<ITrackerService>((ITrackerService)_TrackerClient);
            _Container.RegisterInstance<ClientBase<ITrackerService>>(_TrackerClient);

            _Container.RegisterInstance<IMessenger>(_Messenger);
            _Container.RegisterInstance<IWindowLoader>(this);
            _Container.RegisterInstance<IRegistration>(new RegistrationClient());
        }


        /// <summary>
        /// Initialises mappings between view and viewmodels.
        /// </summary>
        private void InitialiseDictionary()
        {
            _ViewDictionary = new Dictionary<Type, Type>();

            _ViewDictionary.Add(typeof(LoginViewModel), typeof(LoginWindow));
            _ViewDictionary.Add(typeof(RegistrationViewModel), typeof(RegistrationWindow));
            _ViewDictionary.Add(typeof(MainWindowViewModel), typeof(MainWindow));
        }


        /// <summary>
        /// Takes a view model as argument and displays the
        /// corresponding view bound to the inputted view model.
        /// 
        /// If view model to view mapping is not in dictionary,
        /// throw an invalid argument execption.
        /// </summary>
        /// <param name="viewModel">The view model which binds to the view to be shown.</param>
        public void ShowView(IWindow viewModel)
        {
            Type viewModelType = viewModel.GetType();

            // If mapping is found, create a new instance of the view
            if (_ViewDictionary.ContainsKey(viewModelType))
            {
                Type viewType = _ViewDictionary[viewModelType];

                // Create new window of corresponding type found in dictionary
                Window myView = (Window)Activator.CreateInstance(viewType);

                // Allow the view model to close its corresponding view
                viewModel.RequestClose += delegate(object sender, EventArgs e) { myView.Close(); };

                // Bind the view model to the view
                myView.DataContext = viewModel;
                myView.Show();
            }
            else
            {
                // Throw exception if no mapping between view model and view are found
                throw new ArgumentException("This window is not supported by the window loader.");
            }
        }


        public void ShowView(Windows windows)
        {
            switch (windows)
            {
                case Windows.Login:
                    ShowView(IOC.Container.Resolve<LoginViewModel>());
                    break;

                case Windows.Main:
                    ShowView(IOC.Container.Resolve<MainWindowViewModel>());
                    break;

                case Windows.Registration:
                    ShowView(IOC.Container.Resolve<RegistrationViewModel>());
                    break;
            }
        }


        /// <summary>
        /// Creates a new service while notifying subscribers of changes
        /// and updating the IOC container reference.
        /// </summary>
        /// <param name="username">Web service username.</param>
        /// <param name="password">Web service password.</param>
        public void CreateService(String username, String password)
        {
            if (_TrackerClient.State.Equals(CommunicationState.Faulted))
            {
                _TrackerClient = new TrackerServiceClient();

                IOC.Container.RegisterInstance<ITrackerService>((ITrackerService)_TrackerClient);
                IOC.Container.RegisterInstance<ClientBase<ITrackerService>>(_TrackerClient);
                
                _Messenger.NotifyColleagues(Messages.WebServiceReferenceUpdated, _TrackerClient);
            }

            ClientCredentials clientCredentials = new ClientCredentials();

            clientCredentials.UserName.UserName = username;
            clientCredentials.UserName.Password = password;

            _TrackerClient.ChannelFactory.Endpoint.Behaviors.Remove(typeof(ClientCredentials));
            _TrackerClient.ChannelFactory.Endpoint.Behaviors.Add(clientCredentials);
        }
    }

}
