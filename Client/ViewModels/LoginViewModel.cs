using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Client.Commands;
using System.Windows.Input;
using Client.ServiceReference;
using System.ServiceModel;
using System.ServiceModel.Security;
using Client.Params.Login;

namespace Client.ViewModels
{
    /// <summary>
    /// This view model controls the login user interface. It inherits
    /// operations and variables from the ViewModelBase class.
    /// </summary>
    public class LoginViewModel : ViewModelBase<LoginWindow>
    {

        /// <summary>
        /// Inherits from the parent class.
        /// </summary>
        public LoginViewModel() : base() { }


        /// <summary>
        /// Inherits from the parent class. Stores a reference to the window
        /// controller class as a global variable.
        /// </summary>
        /// <param name="controller">Reference to window controller object.</param>
        public LoginViewModel(WindowController controller) : base(controller) { }


        /// <summary>
        /// Initialises the method to use when the login command is invoked.
        /// </summary>
        private RelayCommand _loginCommand;
        public ICommand LoginCommand
        {
            get
            {
                if (_loginCommand == null)
                {
                    _loginCommand = new RelayCommand(param => this.Login(param));
                }

                return _loginCommand;
            }
        }


        /// <summary>
        /// Validates a users credentials by calling the Login method
        /// on the web service object. If successful, main bug manager
        /// is displayed and login window closed.
        /// </summary>
        /// <param name="loginParameter">Custom object to store login parameters.</param>
        public void Login(object loginParameter)
        {
            // Cast object to known object type
            LoginParameters param = (LoginParameters)loginParameter;

            // If username/password is null then return
            if (IsNullParams(param)) return;

            // Set up credentials with the service client
            TrackerServiceClient svc = new TrackerServiceClient();
            svc.ClientCredentials.UserName.UserName = param.Username;
            svc.ClientCredentials.UserName.Password = param.Password;

            // Attempt to make a connection using credentials
            try
            {
                svc.Login();
                // If successful store reference to service in window controller
                _controller.svc = svc;

                _controller.mainVM.ShowView();
                _controller.loginVM.CloseView();
            }
            // Display message if invalid credentials.
            catch (MessageSecurityException)
            {
                MessageBox.Show("Invalid username or password!");
            }
        }


        /// <summary>
        /// Checks any parameters for being null or whitespaced.
        /// </summary>
        /// <param name="param">Data type to hold username/password.</param>
        /// <returns>Return true if credentials are null or whitespace.</returns>
        private bool IsNullParams(LoginParameters param)
        {
            // If username or password contains whitespace or equal to null, show error message
            if (String.IsNullOrWhiteSpace(param.Username) || String.IsNullOrWhiteSpace(param.Password))
            {
                MessageBox.Show("Please enter a username and password.");

                return true;
            }

            return false;
        }

    }
}
