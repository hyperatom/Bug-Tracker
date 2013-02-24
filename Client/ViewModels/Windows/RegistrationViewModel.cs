using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Controls;
using Client.Helpers;
using System.Windows.Input;
using System.Windows;
using System.Text.RegularExpressions;
using System.Net.Mail;
using System.ServiceModel;
using Client.ServiceRegistration;
//using Client.ServiceReference;
using System.Threading.Tasks;
using Client.Controllers;
using Client.ViewModels;
using Microsoft.Practices.Unity;

namespace Client.ViewModels
{
    public class RegistrationViewModel : ObservableObject, IWindow, IDataErrorInfo
    {

        private string _FirstName;
        private string _LastName;
        private string _FirstAndLastName;
        private string _Email;
        private string _Password;
        private string _Organisation;
        private bool _IsValidating = false;
        private bool _IsRegisterEnabled = true;

        private RelayCommand _RegisterCommand;
        private RelayCommand _CancelCommand;

        private IWindowLoader _WindowLoader;
        private IRegistration _Service;

        private Dictionary<string, string> _Errors = new Dictionary<string, string>();


        /// <summary>
        /// Inherits from the parent class.
        /// </summary>
        public RegistrationViewModel(IWindowLoader loader, IRegistration svc) 
        {
            _WindowLoader = loader;
            _Service = svc;

            InitialiseFields();
        }


        public EventHandler RequestClose { get; set; }
        public IWindowLoader WindowLoader { get; set; }


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


        public string Organisation
        {
            get { return _Organisation; }
            set { _Organisation = value.Trim(); OnPropertyChanged("Organisation"); }
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
                    _CancelCommand = new RelayCommand(param => this.Cancel());
                }

                return _CancelCommand;
            }
        }

        #endregion


        /// <summary>
        /// Initialises the form fields with empty strings to
        /// ensure validation detects an empty field.
        /// </summary>
        private void InitialiseFields()
        {
            FirstAndLastName = "";
            Email = "";
            Password = "";
            Organisation = "";
        }


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
        private void Cancel()
        {
            _WindowLoader.ShowView(IOC.Container.Resolve<LoginViewModel>());
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
                OnPropertyChanged("Organisation");
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
            IsRegisterButtonEnabled = false;

            if (CanRegister())
            {
                try
                {
                    Organisation org = new Organisation { Name = Organisation };

                    User user = new User
                    {
                        FirstName = this.FirstName,
                        Password = this.Password,
                        Surname = this.LastName,
                        Username = this.Email,
                        Organisation = org
                    };

                    _Service.BeginRegister(user, RegistrationComplete, _Service);
                }
                catch (FaultException e)
                {
                    IsRegisterButtonEnabled = true;

                    MessageBox.Show(e.Message);
                }
            }
            else
            {
                IsRegisterButtonEnabled = true;
            }
        }


        public void RegistrationComplete(IAsyncResult result)
        {
            ((IRegistration)result.AsyncState).EndRegister(result);

            IsRegisterButtonEnabled = true;
        }


        public string Error
        {
            get { throw new NotImplementedException(); }
        }


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

                        if (!Regex.IsMatch(FirstAndLastName, @"^[A-Za-z]{1,"+MaxFirstNameLength+"} [A-Za-z]+$"))
                            result = "Your first name can be as most "+MaxFirstNameLength+" characters.";

                        if (!Regex.IsMatch(FirstAndLastName, @"^[A-Za-z]+ [A-Za-z]{1,"+MaxLastNameLength+"}$"))
                            result = "Your last name can be as most "+MaxLastNameLength+" characters.";

                        if (!Regex.IsMatch(FirstAndLastName, @"^[A-Za-z]+ [A-Za-z]+$"))
                            result = "Please enter your First and Last name.";

                        if (String.IsNullOrEmpty(FirstAndLastName))
                            result = "This field cannot be left blank!";

                        break;
                    }

                    case "Email":
                    {
                        const int MaxEmailLength = 254;

                        if (!Regex.IsMatch(Email, @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$"))
                            result = "Please enter a valid email address.";
                       
                        if (Email.Length > MaxEmailLength)
                            result = "Email must be less than "+MaxEmailLength+" characters long.";

                        if (String.IsNullOrEmpty(Email))
                            result = "This field cannot be left blank!";

                        break;
                    }

                    case "Password":
                    {
                        const int MaxPasswordLength = 60;

                        if (Password.Length > MaxPasswordLength)
                            result = "Password can be at most " +MaxPasswordLength+ " characters long.";

                        if (String.IsNullOrEmpty(Password))
                            result = "This field cannot be left blank!";

                        break;
                    }

                    case "Organisation":
                    {
                        const int MaxOrgLength = 25;

                        if (Organisation.Length > MaxOrgLength)
                            result = "Organisation name can be at most "+MaxOrgLength+" characters long.";

                        if (String.IsNullOrEmpty(Organisation))
                            result = "This field cannot be left blank!";

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
