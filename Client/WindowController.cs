using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Client.ViewModels;
using Client.ServiceReference;

namespace Client
{
    /// <summary>
    /// This class controls the flow of user interfaces
    /// throughout the client. A reference to this object is
    /// passed to each child view model. This allows all view models to
    /// control any other window in the client using this reference.
    /// </summary>
    public class WindowController
    {

        /// <summary>
        /// Field to reference service. This is the only instance of the service
        /// in the entire client. All view models will access the service through
        /// this field by using a reference through this object.
        /// </summary>
        public TrackerServiceClient svc { get; set; }


        /// <summary>
        /// All view models will have access to each other (public).
        /// </summary>
        public LoginViewModel      loginVM;
        public MainWindowViewModel mainVM;


        /// <summary>
        /// Default constructor initialises all view models and displays
        /// the first window as the login screen.
        /// </summary>
        public WindowController()
        {
            InitialiseViewModels();
            loginVM.ShowView();
        }


        /// <summary>
        /// All view models are initialised here while passing
        /// a reference to this object to allow access to all
        /// view models.
        /// </summary>
        private void InitialiseViewModels()
        {
            loginVM = new LoginViewModel(this);
            mainVM = new MainWindowViewModel(this);
        }

    }
}
