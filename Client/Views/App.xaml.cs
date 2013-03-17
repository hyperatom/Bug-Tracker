using System.ServiceModel;
using System.Windows;
using Client.Factories;
using Client.Helpers;
using Client.ServiceReference;
using Client.ServiceRegistration;
using Client.ViewModels;
using Client.ViewModels.Windows;
using Microsoft.Practices.Unity;
using Client.ViewModels.Controls;
using Client.ViewModels.Controls.ProjectPanel;
using Client.ViewModels.Controls.Dialogs;

namespace Client
{
    /// <summary>
    /// This is the first class loaded by the client. The constructor
    /// immediately creates an instance of the window controller
    /// to initiate the user interface flow.
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Default constructor creates a new instance of window controller
        /// which controls the flow of user interfaces in the client.
        /// </summary>
        public App() { }


        /// <summary>
        /// Overrides startup method and initialises the IOC container
        /// followed by resolving the application entry window.
        /// </summary>
        /// <param name="e">Startup parameters.</param>
        protected override void OnStartup(StartupEventArgs e)
        {
            IUnityContainer container = InitialiseUnityContainer(new UnityContainer());

            container.Resolve<ILoginWindow>().Show();

            base.OnStartup(e);
        }


        /// <summary>
        /// Registers singleton instances of dependency objects. When resolved,
        /// these objects are returned in their persistent state.
        /// </summary>
        /// <param name="container">Unity container to add instances to.</param>
        /// <returns>The new unity container.</returns>
        private IUnityContainer RegisterInstances(IUnityContainer container)
        {
            IMessenger mess = container.Resolve<IMessenger>();
            container.RegisterInstance<IMessenger>(mess);

            return container;
        }


        /// <summary>
        /// Initialise unity container with type mappings. When interfaces are
        /// resolves, a new instance of concrete data type is created.
        /// </summary>
        /// <param name="container">Unity container to add type mappings to.</param>
        /// <returns>The new unity container.</returns>
        private IUnityContainer InitialiseUnityContainer(IUnityContainer container)
        {
            // Services
            container.RegisterType<ClientBase<ITrackerService>, TrackerServiceClient>(new InjectionConstructor());
            container.RegisterType<IRegistration, RegistrationClient>(new InjectionConstructor());
            
            container.RegisterType<IMessenger, Messenger>();

            // Views
            container.RegisterType<ILoginWindow, LoginWindow>();
            container.RegisterType<IRegistrationWindow, RegistrationWindow>();
            container.RegisterType<IMainWindow, MainWindow>();

            // View Models
            container.RegisterType<ILoginViewModel, LoginViewModel>();
            container.RegisterType<IMainWindowViewModel, MainWindowViewModel>();
            container.RegisterType<IRegistrationViewModel, RegistrationViewModel>();
            container.RegisterType<BugPanelViewModel, BugAddPanelViewModel>("AddPanel");
            container.RegisterType<BugPanelViewModel, BugViewPanelViewModel>("ViewPanel");
            container.RegisterType<IBugTableViewModel, BugTableViewModel>();
            container.RegisterType<ICommandPanelViewModel, CommandPanelViewModel>();
            container.RegisterType<IProjectManagerViewModel, ProjectManagerViewModel>();
            container.RegisterType<ProjectPanelViewModel, ProjectAddPanelViewModel>("Add");
            container.RegisterType<ProjectPanelViewModel, ProjectViewPanelViewModel>("View");
            container.RegisterType<IDeleteProjectDialogViewModel, DeleteProjectDialogViewModel>();
            container.RegisterType<IAssignedProjectsPanelViewModel, AssignedProjectsPanelViewModel>();
            container.RegisterType<IManagedProjectsPanelViewModel, ManagedProjectsPanelViewModel>();
            container.RegisterType<IProjectUsersPanelViewModel, ProjectUsersPanelViewModel>();
            container.RegisterType<IJoinProjectPanelViewModel, JoinProjectPanelViewModel>();

            // Factories
            container.RegisterType<IServiceFactory, ServiceFactory>();
            container.RegisterType<IWindowFactory, WindowFactory>();
            container.RegisterType<IControlFactory, ControlFactory>();

            return RegisterInstances(container);
        }

    }
}
