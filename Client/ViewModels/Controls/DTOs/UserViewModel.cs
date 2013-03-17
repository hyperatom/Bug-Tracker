using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Client.ServiceReference;
using AutoMapper;
using Client.Helpers;

namespace Client.ViewModels.Controls.DTOs
{
    public class UserViewModel : ObservableObject
    {

        private User _User;

        private bool _IsSelected = false;
        private bool _IsValidating = false;

        private Dictionary<string, string> _Errors = new Dictionary<string, string>();



        public UserViewModel(User user)
        {
            _User = user;

            _IsSelected = false;
        }


        public UserViewModel()
        {
            _User = new User();

            _IsSelected = false;
        }


        public User ToUserModel()
        {
            Mapper.CreateMap<UserViewModel, User>();

            return Mapper.Map<UserViewModel, User>(this);
        }


        public UserViewModel Clone()
        {
            return new UserViewModel(this.ToUserModel());
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


        public Int32 Id
        {
            get { return _User.Id; }
            set { _User.Id = value; OnPropertyChanged("Id"); }
        }


        public String FirstName
        {
            get { return _User.FirstName; }
            set { _User.FirstName = value; OnPropertyChanged("FirstName"); }
        }


        public String Surname
        {
            get { return _User.Surname; }
            set { _User.Surname = value; OnPropertyChanged("Surname"); }
        }


        public String AbreviatedFullName
        {
            get
            {
                return FirstName.Substring(0, 1) + ". " + Surname;
            }
        }


        public String Username
        {
            get { return _User.Username; }
            set { _User.Username = value; OnPropertyChanged("Username"); }
        }


        public String Password
        {
            get { return _User.Password; }
            set { _User.Password = value; OnPropertyChanged("Password"); }
        }


        // Tracks if the user is selected in a view
        public bool IsSelected
        {
            get { return this._IsSelected; }
            set { _IsSelected = value; OnPropertyChanged("IsSelected"); }
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

                /*switch (Field)
                {
                    
                }*/

                // If there was an error add it to dictionary
                if (result != string.Empty)
                    _Errors.Add(Field, result);

                return result;
            }
        }

    }
}
