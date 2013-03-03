using Microsoft.Practices.Unity;

namespace Client.Factories
{
    /// <summary>
    /// Responsible for creating different types of windows
    /// by resolving their interfaces using the unity container.
    /// </summary>
    public class WindowFactory : IWindowFactory
    {

        private IUnityContainer _Container;


        /// <summary>
        /// Initialises the unity container.
        /// </summary>
        /// <param name="container">Unity container</param>
        public WindowFactory(IUnityContainer container)
        {
            _Container = container;
        }


        /// <summary>
        /// Creates a new instance of a login window.
        /// </summary>
        /// <returns>Login window object.</returns>
        public ILoginWindow CreateLoginWindow()
        {
            return _Container.Resolve<ILoginWindow>();
        }


        /// <summary>
        /// Creates a new instance of a main window.
        /// </summary>
        /// <returns>Main window object.</returns>
        public IMainWindow CreateMainWindow()
        {
            return _Container.Resolve<IMainWindow>(); 
        }


        /// <summary>
        /// Creates a new instance of a registration window.
        /// </summary>
        /// <returns>Registration window object.</returns>
        public IRegistrationWindow CreateRegistrationWindow()
        {
            return _Container.Resolve<IRegistrationWindow>();
        }

    }
}
