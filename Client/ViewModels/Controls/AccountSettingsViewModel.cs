using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Client.Helpers;
using Client.ServiceReference;
using System.Windows.Input;
using System.Text.RegularExpressions;
using System.ServiceModel;
using System.Windows;
using System.ComponentModel;
using Client.Views.Controls.Notifications;

namespace Client.ViewModels.Controls
{

    public interface IAccountSettingsViewModel : IContentPanel, IDataErrorInfo
    {
        ICommand SaveCommand { get; }

        string FirstAndLastName { get; set; }
        string Email { get; set; }
        string Password { get; set; }
        string this[string Field] { get; }
        string Error { get; }
        Dictionary<string, string> Errors { get; set; }
    }

    public class AccountSettingsViewModel : ObservableObject, IAccountSettingsViewModel
    {
        private ITrackerService _Service;
        private IMessenger _Messenger;

        private string _FirstName;
        private string _LastName;
        private string _FirstAndLastName;
        private string _Email;
        private string _Password;
        private bool _IsValidating = false;
        private bool _IsSaveEnabled = true;

        private IGrowlNotifiactions _Notifier;

        private RelayCommand _SaveCommand;

        private Dictionary<string, string> _Errors = new Dictionary<string, string>();

        
        /// <summary>
        /// Stores references to dependencies and initialses object fields.
        /// </summary>
        /// <param name="loader">The window loader.</param>
        /// <param name="svc">The registration web service.</param>
        public AccountSettingsViewModel(ITrackerService svc, IMessenger mess, IGrowlNotifiactions notifier) 
        {
            _Service = svc;
            _Messenger = mess;
            _Notifier = notifier;

            User myUser = _Service.GetMyUser();

            _FirstName = myUser.FirstName;
            _LastName = myUser.Surname;
            _FirstAndLastName = _FirstName + " " + _LastName;
            _Email = myUser.Username;
            _Password = myUser.Password;
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
                EnableSaveButton();
                OnPropertyChanged("FirstAndLastName"); 
            }
        }


        public string Email
        {
            get { return _Email; }
            set { _Email = value.Trim(); OnPropertyChanged("Email"); EnableSaveButton(); }
        }


        public string Password
        {
            get { return _Password; }
            set { _Password = value.Trim(); OnPropertyChanged("Password"); EnableSaveButton(); }
        }


        public bool IsSaveButtonEnabled
        {
            get { return _IsSaveEnabled; }
            set { _IsSaveEnabled = value; OnPropertyChanged("IsRegisterButtonEnabled"); }
        }


        public Dictionary<string, string> Errors
        {
            get { return _Errors; }
            set { _Errors = value; OnPropertyChanged("Errors"); }
        }


        #region Commands

        public ICommand SaveCommand
        {
            get
            {
                if (_SaveCommand == null)
                {
                    _SaveCommand = new RelayCommand(param => this.SaveCredentials());
                }

                return _SaveCommand;
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
        /// Triggers validation of each text box by invoking property changed
        /// method for each field.
        /// </summary>
        /// <returns>Returns true if there are no errors, false otherwise.</returns>
        private bool CanSave()
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

            return (Errors.Count() == 0);
        }


        /// <summary>
        /// Attempts to register a new user and organisation with the service
        /// using form data.
        /// </summary>
        private void SaveCredentials()
        {
            IsSaveButtonEnabled = false;

            if (CanSave())
            {
                try
                {
                    User user = new User
                    {
                        Id = _Service.GetMyUser().Id,
                        FirstName = this.FirstName,
                        Surname = this.LastName,
                        Password = this.Password,
                        Username = this.Email,
                    };

                    // Execute registration operation concurrently
                    _Service.SaveUserCredentials(user);
                    _Notifier.AddNotification(new Notification { Title = "Saved!", Message = "Your account settings have been saved." });
                    IsSaveButtonEnabled = false;

                }
                catch (FaultException e)
                {
                    IsSaveButtonEnabled = true;

                    MessageBox.Show(e.Message);
                }
            }
            else
            {
                IsSaveButtonEnabled = true;
            }
        }


        private void EnableSaveButton()
        {
            IsSaveButtonEnabled = true;
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
                Errors.Remove(Field);

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

                        else if (_Service.UserExists(Email))
                            result = "This email address is already in use, please choose another one.";

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
                    Errors.Add(Field, result);

                return result;
            }
        }

    }

    
}
