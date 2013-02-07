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
    public class WindowController : Client.IWindowController
    {

        private TrackerServiceClient _Service;
        private LoginViewModel _LoginVM;
        private MainWindowViewModel _MainVM;
        private RegistrationViewModel _RegistrationVM;


        /// <summary>
        /// Field to reference service. This is the only instance of the service
        /// in the entire client. All view models will access the service through
        /// this field by using a reference through this object.
        /// </summary>
        private TrackerServiceClient Service 
        {
            get
            {
                if (_Service == null)
                    _Service = new TrackerServiceClient();

                return _Service;
            }
            set
            {
                _Service = value;
            }
        }


        /// <summary>
        /// All view models will have access to each other (public).
        /// </summary>
        private LoginViewModel LoginVM
        {
            get { return _LoginVM; }
            set { _LoginVM = value; }
        }


        private MainWindowViewModel MainVM
        {
            get { return _MainVM; }
            set { _MainVM = value; }
        }


        private RegistrationViewModel RegistrationVM
        {
            get { return _RegistrationVM; }
            set { _RegistrationVM = value; }
        }


        /// <summary>
        /// Default constructor initialises all view models and displays
        /// the first window as the login screen.
        /// </summary>
        public WindowController()
        {
            ShowLoginWindow();
        }


        public void ShowRegistrationWindow()
        {
            RegistrationVM = new RegistrationViewModel(this, Service);
            RegistrationVM.ShowView();
        }


        public void CloseRegistrationWindow()
        {
            RegistrationVM.CloseView();
            RegistrationVM = null;
        }


        public void ShowLoginWindow()
        {
            LoginVM = new LoginViewModel(this, Service);
            LoginVM.ShowView();
        }


        public void CloseLoginWindow()
        {
            LoginVM.CloseView();
            LoginVM = null;
        }


        public void ShowMainWindow()
        {
            MainVM = new MainWindowViewModel(this, Service);
            MainVM.ShowView();
        }


        public void CloseMainWindow()
        {
            MainVM.CloseView();
            MainVM = null;
        }

    }
}
