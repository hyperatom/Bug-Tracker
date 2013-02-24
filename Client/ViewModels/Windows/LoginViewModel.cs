using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Security;
using System.Text;
using System.Windows;
using System.Windows.Input;
using Client.Controllers;
using Client.Helpers;
using Client.ServiceReference;
using Client.ViewModels;
using Microsoft.Practices.Unity;

namespace Client.ViewModels
{
    /// <summary>
    /// This view model controls the login user interface. It inherits
    /// operations and variables from the ViewModelBase class.
    /// </summary>
    public class LoginViewModel : ObservableObject, IWindow
    {

        private ClientBase<ITrackerService> _ServiceClient;
        private IWindowLoader   _WindowLoader;
        private IMessenger _Messenger;

        private bool _IsRememberMeChecked;
        private string _username;
        private string _password;


        /// <summary>
        /// Inherits from the parent class.
        /// </summary>
        public LoginViewModel(ClientBase<ITrackerService> client, IWindowLoader loader, IMessenger messenger)
        {
            _WindowLoader = loader;
            _ServiceClient = client;
            _Messenger = messenger;

            Username = GetStoredUsername();
            Password = GetStoredPassword();

            InitialiseRememberMeCheckBox();

            _Messenger.Register<ClientBase<ITrackerService>>
                (Messages.WebServiceReferenceUpdated, p => _ServiceClient = p);
        }


        //public IWindowLoader WindowLoader { get; set; }
        public EventHandler RequestClose { get; set; }


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


        private void ShowRegistrationWindow()
        {
            _WindowLoader.ShowView(IOC.Container.Resolve<RegistrationViewModel>());

            RequestClose.Invoke(this, null);
        }


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

            _WindowLoader.CreateService(Username, Password);

            try
            {
                _ServiceClient.Open();

                _WindowLoader.ShowView(Windows.Main);

                RequestClose.Invoke(this, null);
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

        private StreamWriter GetStreamWriter()
        {
            //First get the 'user-scoped' storage information location reference in the assembly
            IsolatedStorageFile isolatedStorage = IsolatedStorageFile.GetUserStoreForAssembly();
            //create a stream writer object to write content in the location
            StreamWriter srWriter = new StreamWriter(new IsolatedStorageFileStream("BugTrackerCredentials", FileMode.Create, isolatedStorage));
            
            return srWriter;
        }


        private void StoreUserCredentials()
        {
            try
            {
                StreamWriter srWriter = GetStreamWriter();

                //wriet to the isolated storage created in the above code section.
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


        private string GetStoredUsername()
        {
            try
            {
                string username = "";

                StreamReader srReader = GetStreamReader();

                //Open the isolated storage
                if (srReader == null)
                {
                    return "";
                }
                else
                {
                    username = srReader.ReadLine().ToString();
                }
                //close reader
                srReader.Close();

                return username;
            }
            catch(Exception)
            {
                return "";
            }

        }


        private StreamReader GetStreamReader()
        {
            //First get the 'user-scoped' storage information location reference in the assembly
            IsolatedStorageFile isolatedStorage = IsolatedStorageFile.GetUserStoreForAssembly();
            //create a stream reader object to read content from the created isolated location
            StreamReader srReader = new StreamReader(new IsolatedStorageFileStream("BugTrackerCredentials", FileMode.OpenOrCreate, isolatedStorage));
            
            return srReader;
        }


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
                //close reader
                srReader.Close();

                return password;
            }
            catch (Exception)
            {
                return "";
            }
        }

    }
}
