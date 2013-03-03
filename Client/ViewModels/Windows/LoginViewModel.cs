using System;
using System.IO;
using System.IO.IsolatedStorage;
using System.ServiceModel;
using System.ServiceModel.Security;
using System.Windows;
using System.Windows.Input;
using Client.Helpers;
using Client.ServiceReference;
using Client.Factories;
using Client.ViewModels.Windows;

namespace Client.ViewModels
{
    /// <summary>
    /// This view model controls the login user interface. It is responsible
    /// for setting up the web service with valid credentials.
    /// </summary>
    public class LoginViewModel : ObservableObject, ILoginViewModel
    {

        private IMessenger _Messenger;
        private IServiceFactory _ServiceFactory;
        private IWindowFactory _WindowFactory;

        private bool _IsRememberMeChecked;
        private string _username;
        private string _password;


        /// <summary>
        /// Stores references to dependencies and initialises object properties.
        /// </summary>
        /// <param name="client"></param>
        /// <param name="loader"></param>
        /// <param name="messenger"></param>
        public LoginViewModel(IMessenger messenger, IServiceFactory svcfactory, IWindowFactory winfactory)
        {
            if (messenger == null)
                throw new ArgumentNullException("The messenger cannot be null.");

            if (svcfactory == null)
                throw new ArgumentNullException("The service factory cannot be null.");

            if (winfactory == null)
                throw new ArgumentNullException("The window factory cannot be null.");

            _Messenger = messenger;
            _ServiceFactory = svcfactory;
            _WindowFactory = winfactory;

            Username = GetStoredUsername();
            Password = GetStoredPassword();

            InitialiseRememberMeCheckBox();
        }


        public EventHandler RequestClose { get; set; }


        /// <summary>
        /// Stores the state of the remember me check box
        /// </summary>
        public bool IsRememberMeChecked
        {
            get 
            {
                return _IsRememberMeChecked;
            }
            set
            {
                _IsRememberMeChecked = value;
                OnPropertyChanged("IsRememberMeChecked");
            }
        }


        public string Username
        {
            get { return _username; }
            set 
            { 
                _username = value;
                OnPropertyChanged("Username");
            }
        }


        public string Password
        {
            get { return _password; }
            set 
            { 
                _password = value;
                OnPropertyChanged("Password");
            }
        }


        /// <summary>
        /// If the window loads with stored user credentials,
        /// check the remember me box, else set it unchecked.
        /// </summary>
        private void InitialiseRememberMeCheckBox()
        {
            if (CanLogin())
            {
                IsRememberMeChecked = true;
            }
            else
            {
                IsRememberMeChecked = false;
            }
        }


        #region Commands

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
                    _loginCommand = new RelayCommand(param => this.Login(), param => this.CanLogin());
                }

                return _loginCommand;
            }
        }


        private RelayCommand _registrationCommand;
        public ICommand RegistrationCommand
        {
            get
            {
                if (_registrationCommand == null)
                {
                    _registrationCommand = new RelayCommand(param => this.ShowRegistrationWindow());
                }

                return _registrationCommand;
            }
        }

        #endregion Commands


        /// <summary>
        /// Request the window loader displays the registration
        /// window and closes the current view.
        /// </summary>
        private void ShowRegistrationWindow()
        {
            _WindowFactory.CreateRegistrationWindow().Show();

            RequestClose.Invoke(this, null);
        }


        /// <summary>
        /// Checks if the user has populated both username and password text blocks.
        /// </summary>
        /// <returns>True if both fields populated, false otherwise</returns>
        private bool CanLogin()
        {
            if (String.IsNullOrWhiteSpace(Username) || String.IsNullOrWhiteSpace(Password))
            {
                return false;
            }
            
            return true;
        }


        /// <summary>
        /// Validates a users credentials by calling the Login method
        /// on the web service object. If successful, main bug manager
        /// is displayed and login window closed.
        /// </summary>
        /// <param name="loginParameter">Custom object to store login parameters.</param>
        public void Login()
        {
            if (IsRememberMeChecked)
            {
                StoreUserCredentials();
            }
            else
            {
                FlushUserCredentials();
            }

            // Tell the service container to create a new service with these credentials
            ClientBase<ITrackerService> svc = _ServiceFactory.CreateService(Username, Password);

            try
            {
                // Test if we can open the communication channel
                svc.Open();

                // If we can then show the main window
                _WindowFactory.CreateMainWindow().Show();
                RequestClose(this, null);
            }
            // Display message if invalid credentials.
            catch (MessageSecurityException)
            {
                MessageBox.Show("Invalid username or password!");
            }
            catch (FaultException fault)
            {
                MessageBox.Show(fault.Message);
            }
        }


        /// <summary>
        /// Deletes user credentials from the persistent file.
        /// </summary>
        private void FlushUserCredentials()
        {
            try
            {
                StreamWriter srWriter = GetStreamWriter();

                srWriter.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Sorry, we were unable to remove your user credentials.");
            }
        }


        /// <summary>
        /// Gets a reference to the stream writer for this applications settings.
        /// </summary>
        /// <returns>A reference to the stream writer.</returns>
        private StreamWriter GetStreamWriter()
        {
            IsolatedStorageFile isolatedStorage = IsolatedStorageFile.GetUserStoreForAssembly();
            
            StreamWriter srWriter = new StreamWriter(new IsolatedStorageFileStream("BugTrackerCredentials", FileMode.Create, isolatedStorage));
            
            return srWriter;
        }


        /// <summary>
        /// Stores the current user credentials in a persistent file.
        /// </summary>
        private void StoreUserCredentials()
        {
            try
            {
                StreamWriter srWriter = GetStreamWriter();

                srWriter.WriteLine(Username);
                srWriter.WriteLine(Password);

                srWriter.Flush();
                srWriter.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Sorry, we are unable to save your user credentials.");
            }
        }


        /// <summary>
        /// Retrieves a username from the persistent file
        /// </summary>
        /// <returns>A string representing the username.</returns>
        private string GetStoredUsername()
        {
            try
            {
                string username = "";

                StreamReader srReader = GetStreamReader();

                //Open the isolated storage
                if (srReader == null)
                    return "";
                else
                    username = srReader.ReadLine().ToString();

                srReader.Close();

                return username;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);

                return "";
            }

        }


        /// <summary>
        /// Gets a reference to the stream reader to read the persistent storage file for this application.
        /// </summary>
        /// <returns>A reference to the stream reader.</returns>
        private StreamReader GetStreamReader()
        {
            
            IsolatedStorageFile isolatedStorage = IsolatedStorageFile.GetUserStoreForAssembly();
            
            StreamReader srReader = new StreamReader(new IsolatedStorageFileStream("BugTrackerCredentials", FileMode.OpenOrCreate, isolatedStorage));
            
            return srReader;
        }


        /// <summary>
        /// Retrieves the users password from persistent storage file.
        /// </summary>
        /// <returns>A string representing the user's password.</returns>
        private string GetStoredPassword()
        {
            try
            {
                string password = "";

                StreamReader srReader = GetStreamReader();

                //Open the isolated storage
                if (srReader == null)
                {
                    return "";
                }
                else
                {
                    srReader.ReadLine();
                    password = srReader.ReadLine().ToString();
                }
                
                srReader.Close();

                return password;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);

                return "";
            }
        }

    }
}
