using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.ServiceModel;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using Client.Factories;
using Client.Helpers;
using Client.ServiceRegistration;
using Client.ViewModels.Windows;
using System.Threading.Tasks;

namespace Client.ViewModels
{
    /// <summary>
    /// This class is concerned with operations which allow a user to register.
    /// </summary>
    public class RegistrationViewModel : ObservableObject, IRegistrationViewModel, IDataErrorInfo
    {

        private string _FirstName;
        private string _LastName;
        private string _FirstAndLastName;
        private string _Email;
        private string _Password;
        private bool _IsValidating = false;
        private bool _IsRegisterEnabled = true;
        private bool _IsLoadingVisible = false;

        private RelayCommand _RegisterCommand;
        private RelayCommand _CancelCommand;

        private IWindowFactory _WindowFactory;
        private IRegistrationService _Service;

        private Dictionary<string, string> _Errors = new Dictionary<string, string>();


        /// <summary>
        /// Stores references to dependencies and initialses object fields.
        /// </summary>
        /// <param name="loader">The window loader.</param>
        /// <param name="svc">The registration web service.</param>
        public RegistrationViewModel(IRegistrationService svc, IWindowFactory winfactory) 
        {
            if (svc == null)
                throw new ArgumentNullException("Registration service cannot be null");

            if (winfactory == null)
                throw new ArgumentNullException("Window factory cannot be null");

            _Service = svc;
            _WindowFactory = winfactory;
        }


        public EventHandler RequestClose { get; set; }


        public bool IsLoadingVisible
        {
            get { return _IsLoadingVisible; }
            set { _IsLoadingVisible = value; OnPropertyChanged("IsLoadingVisible"); }
        }


        private string FirstName
        {
            get { return _FirstName; }
            set { _FirstName = value; OnPropertyChanged("Firstname"); }
        }


        private string LastName
        {
            get { return _LastName; }
            set {_LastName = value; OnPropertyChanged("Lastname"); }
        }


        /// <summary>
        /// Field splits users full name into first
        /// name and second name.
        /// </summary>
        public string FirstAndLastName
        {
            get { return _FirstAndLastName; }
            set 
            { 
                _FirstAndLastName = value.Trim();
                SplitFullName(_FirstAndLastName); 
                OnPropertyChanged("FirstAndLastName"); 
            }
        }


        public string Email
        {
            get { return _Email; }
            set { _Email = value.Trim(); OnPropertyChanged("Email"); }
        }


        public string Password
        {
            get { return _Password; }
            set { _Password = value.Trim(); OnPropertyChanged("Password"); }
        }


        public bool IsRegisterButtonEnabled
        {
            get { return _IsRegisterEnabled; }
            set { _IsRegisterEnabled = value; OnPropertyChanged("IsRegisterButtonEnabled"); }
        }


        #region Commands

        public ICommand RegisterCommand
        {
            get
            {
                if (_RegisterCommand == null)
                {
                    _RegisterCommand = new RelayCommand(param => this.Register());
                }

                return _RegisterCommand;
            }
        }


        public ICommand CancelCommand
        {
            get
            {
                if (_CancelCommand == null)
                {
                    _CancelCommand = new RelayCommand(param => this.NavigateToLoginWindow());
                }

                return _CancelCommand;
            }
        }

        #endregion


        /// <summary>
        /// Regex splits full name into first and second names.
        /// </summary>
        /// <param name="fullName">The users full name as a single string.</param>
        private void SplitFullName(string fullName)
        {
            FirstName = Regex.Match(fullName, @"^[A-Za-z]+").ToString();
            LastName = Regex.Match(fullName, @"[A-Za-z]+$").ToString();
        }


        /// <summary>
        /// Navigates the user back to the login window.
        /// </summary>
        private void NavigateToLoginWindow()
        {
            _WindowFactory.CreateLoginWindow().Show();

            RequestClose.Invoke(this, null);
        }


        /// <summary>
        /// Triggers validation of each text box by invoking property changed
        /// method for each field.
        /// </summary>
        /// <returns>Returns true if there are no errors, false otherwise.</returns>
        public bool CanRegister()
        {
            _IsValidating = true;

            try
            {
                OnPropertyChanged("FirstAndLastName");
                OnPropertyChanged("Email");
                OnPropertyChanged("Password");
            }
            finally
            {
                _IsValidating = false;
            }

            return (_Errors.Count() == 0);
        }


        /// <summary>
        /// Attempts to register a new user and organisation with the service
        /// using form data.
        /// </summary>
        private void Register()
        {
            IsLoadingVisible = true;
            IsRegisterButtonEnabled = false;

            if (CanRegister())
            {

                Task openConnection = new Task(() =>
                {

                    try
                    {
                        User user = new User
                        {
                            FirstName = this.FirstName,
                            Surname = this.LastName,
                            Password = this.Password,
                            Username = this.Email,
                        };

                        // Execute registration operation concurrently
                        _Service.Register(user);
                    }
                    catch (FaultException e)
                    {
                        IsRegisterButtonEnabled = true;
                        IsLoadingVisible = false;

                        MessageBox.Show(e.Message);
                    }
                });

                Task openWindow = openConnection.ContinueWith(p =>
                {
                    if (p.Exception == null)
                    {
                        _WindowFactory.CreateLoginWindow().Show();
                        RequestClose(this, null);
                    }
                    else
                    {
                        IsLoadingVisible = false;
                        IsRegisterButtonEnabled = true;
                    }

                }, TaskScheduler.FromCurrentSynchronizationContext());

                openConnection.Start();
            }
            else
            {
                IsLoadingVisible = false;
                IsRegisterButtonEnabled = true;
            }
        
            

            /*IsRegisterButtonEnabled = false;
            IsLoadingVisible = true;

            if (CanRegister())
            {
                try
                {
                    User user = new User
                    {
                        FirstName = this.FirstName,
                        Surname = this.LastName,
                        Password = this.Password,
                        Username = this.Email,
                    };

                    // Execute registration operation concurrently
                    _Service.BeginRegister(user, RegistrationComplete, _Service);
                }
                catch (FaultException e)
                {
                    IsRegisterButtonEnabled = true;
                    IsLoadingVisible = false;

                    MessageBox.Show(e.Message);
                }
            }
            else
            {
                IsRegisterButtonEnabled = true;
            }*/
        }


        /// <summary>
        /// Enables button when registration is complete.
        /// </summary>
        /// <param name="result"></param>
        public void RegistrationComplete(IAsyncResult result)
        {
            IsRegisterButtonEnabled = true;
            IsLoadingVisible = false;
        }


        public string Error
        {
            get { throw new NotImplementedException(); }
        }


        /// <summary>
        /// Validate each field in the data entry form.
        /// </summary>
        /// <param name="Field">The field to be validated.</param>
        /// <returns>A string representing the error message.</returns>
        public string this[string Field]
        {
            get
            {
                string result = string.Empty;

                // If field is not being validated return empty string
                if (!_IsValidating)
                    return result;

                // Remove any previous errors from the dictionary
                _Errors.Remove(Field);

                switch (Field)
                {
                    case "FirstAndLastName":
                    {
                        const int MaxFirstNameLength = 25;
                        const int MaxLastNameLength = 40;

                        if (String.IsNullOrEmpty(FirstAndLastName))
                            result = "This field cannot be left blank!";

                        else if (!Regex.IsMatch(FirstAndLastName, @"^[A-Za-z]{1,"+MaxFirstNameLength+"} [A-Za-z]+$"))
                            result = "Your first name can be as most "+MaxFirstNameLength+" characters.";

                        else if (!Regex.IsMatch(FirstAndLastName, @"^[A-Za-z]+ [A-Za-z]{1,"+MaxLastNameLength+"}$"))
                            result = "Your last name can be as most "+MaxLastNameLength+" characters.";

                        else if (!Regex.IsMatch(FirstAndLastName, @"^[A-Za-z]+ [A-Za-z]+$"))
                            result = "Please enter your First and Last name.";

                        break;
                    }

                    case "Email":
                    {
                        const int MaxEmailLength = 254;

                        if (String.IsNullOrEmpty(Email))
                            result = "This field cannot be left blank!";

                        else if (!Regex.IsMatch(Email, @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$"))
                            result = "Please enter a valid email address.";
                       
                        else if (Email.Length > MaxEmailLength)
                            result = "Email must be less than "+MaxEmailLength+" characters long.";


                        break;
                    }

                    case "Password":
                    {
                        const int MaxPasswordLength = 60;

                        if (String.IsNullOrEmpty(Password))
                            result = "This field cannot be left blank!";

                        else if (Password.Length > MaxPasswordLength)
                            result = "Password can be at most " +MaxPasswordLength+ " characters long.";

                        break;
                    }
                }

                // If there was an error add it to dictionary
                if (result != string.Empty)
                    _Errors.Add(Field, result);

                return result;
            }
        }

    }
}
