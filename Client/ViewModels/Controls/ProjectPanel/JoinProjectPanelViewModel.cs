using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Client.ServiceReference;
using System.Collections.ObjectModel;
using Client.Helpers;
using System.Windows.Input;
using System.ComponentModel;
using System.Windows;
using Client.Views.Controls.Notifications;

namespace Client.ViewModels.Controls.ProjectPanel
{
    public class JoinProjectPanelViewModel : ObservableObject, IJoinProjectPanelViewModel, IDataErrorInfo
    {

        private ITrackerService _Service;
        private IGrowlNotifiactions _Notifier;

        private String _Code;
        private Role _SelectedRole;
        private User _User;
        private IList<Role> _RoleList;

        private bool _IsValidating = false;
        private Dictionary<string, string> _Errors = new Dictionary<string, string>();

        private ICommand _JoinProjectCommand;


        public JoinProjectPanelViewModel(ITrackerService svc, IGrowlNotifiactions notifier)
        {
            _Service = svc;
            _Notifier = notifier;
            _User = svc.GetMyUser();
        }


        public IList<Role> RoleList
        {
            get
            {
                if (_RoleList == null)
                    _RoleList = _Service.GetAllRoles();

                return _RoleList;
            }

            set { _RoleList = value; OnPropertyChanged("RoleList"); }
        }


        public Role SelectedRole
        {
            get 
            {
                if (_SelectedRole == null)
                    _SelectedRole = RoleList[0];

                return _SelectedRole;
            }

            set { _SelectedRole = value; OnPropertyChanged("SelectedRole"); }
        }


        public String Code
        {
            get 
            {
                if (_Code == null)
                    _Code = "";

                return _Code;
            }
            set { _Code = value.ToUpperInvariant(); OnPropertyChanged("Code"); }
        }


        public bool IsValidating
        {
            get { return _IsValidating; }
            set { _IsValidating = value; }
        }


        public Dictionary<string, string> Errors
        {
            get { return _Errors; }
            set { _Errors = value; }
        }


        private bool IsRequestButtonEnabled
        {
            get 
            { 
                return (_Code != null && this._Code.Length == 5);
            }
        }


        #region Commands

        public ICommand JoinProjectCommand
        {
            get
            {
                if (_JoinProjectCommand == null)
                {
                    _JoinProjectCommand = new RelayCommand(param => RequestJoinProject(), p => IsRequestButtonEnabled);
                }

                return _JoinProjectCommand;
            }
        }

        #endregion Commands


        protected bool InputIsValidated()
        {
            IsValidating = true;
            OnPropertyChanged("Code");
            IsValidating = false;

            return (Errors.Count == 0);
        }


        private void RequestJoinProject()
        {
            if (InputIsValidated())
            {
                User myUser = _Service.GetMyUser();

                try
                {
                    _Service.RequestProjectAssignment(Code, myUser, SelectedRole);

                    _Notifier.AddNotification(new Notification { 
                        ImageUrl = Notification.ICON_NOTIFICATION,
                        Title = "Request Sent!",
                        Message = "Your request to join this project has been sent." });
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }

                Code = "";
            }
        }


        public string Error
        {
            get { throw new NotImplementedException(); }
        }


        /// <summary>
        /// Validates the properties of the view model.
        /// </summary>
        /// <param name="Field">The field to validate.</param>
        /// <returns>An error message as a string.</returns>
        public string this[string Field]
        {
            get
            {
                string result = string.Empty;

                if (!_IsValidating)
                    return result;

                // Remove any previous errors from the dictionary
                _Errors.Remove(Field);

                switch (Field)
                {
                    case "Code":
                        {
                            const int CodeLength = 5;

                            if (String.IsNullOrEmpty(Code))
                                result = "This field cannot be left blank!";

                            else if (Code.Length != CodeLength)
                                result = "Code must be exactly " + CodeLength + " characters.";

                            else if (!_Service.IsValidProjectCode(Code))
                                result = "A project with this code does not exist.";

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
