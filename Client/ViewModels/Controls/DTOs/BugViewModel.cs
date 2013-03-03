using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Client.ServiceReference;
using AutoMapper;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.ComponentModel;
using System.Text.RegularExpressions;
using Client.Helpers;


namespace Client.ViewModels
{
    /// <summary>
    /// This class is a client representation of a bug data structure.
    /// It is an observable object and notifies its container of changes.
    /// 
    /// </summary>
    public class BugViewModel : ObservableObject, IDataErrorInfo
    {

        private Bug _Bug;

        private bool _IsSelected;
        private bool _IsValidating = false;

        private Dictionary<string, string> _Errors = new Dictionary<string, string>();


        /// <summary>
        /// Default constructor initialises an empty bug view model.
        /// </summary>
        public BugViewModel()
        {
            _Bug = new Bug();

            _IsSelected = false;
        }


        /// <summary>
        /// A bug object is required to initialise an object of the view
        /// model. Attributes are mapped from the bug object to view model.
        /// </summary>
        /// <param name="bug">Bug object data structure.</param>
        public BugViewModel(Bug bug)
        {
            _Bug = bug;

            _IsSelected = false;
        }


        /// <summary>
        /// Converts the current view model object back to
        /// a simple bug data structure.
        /// </summary>
        /// <returns>A bug object resulting from the mapping.</returns>
        public Bug ToBugModel()
        {
            Mapper.CreateMap<BugViewModel, Bug>();

            return Mapper.Map<BugViewModel, Bug>(this);
        }


        /// <summary>
        /// Useful for de-referencing an object by
        /// making a copy of it.
        /// </summary>
        /// <returns></returns>
        public BugViewModel Clone()
        {
            return new BugViewModel(this.ToBugModel());
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
            get { return this._Bug.Id; }
            set { _Bug.Id = value; OnPropertyChanged("Id"); }
        }

        public String Name
        {
            get { return this._Bug.Name; }
            set { _Bug.Name = value; OnPropertyChanged("Name"); }
        }

        public String Description
        {
            get { return this._Bug.Description; }
            set { _Bug.Description = value.Trim(); OnPropertyChanged("Description"); }
        }

        public String Priority
        {
            get { return this._Bug.Priority; }
            set { _Bug.Priority = value; OnPropertyChanged("Priority"); }
        }

        public String Status
        {
            get { return this._Bug.Status; }
            set { _Bug.Status = value; OnPropertyChanged("Status"); }
        }

        public DateTime DateFound
        {
            get { return this._Bug.DateFound; }
            set { _Bug.DateFound = value; OnPropertyChanged("DateFound"); }
        }

        public DateTime LastModified
        {
            get { return this._Bug.LastModified; }
            set { _Bug.LastModified = value; OnPropertyChanged("LastModified"); }
        }

        public Boolean Fixed
        {
            get { return this._Bug.Fixed; }
            set { _Bug.Fixed = value; OnPropertyChanged("Fixed"); }
        }

        public User CreatedBy
        {
            get { return this._Bug.CreatedBy; }
            set { _Bug.CreatedBy = value; OnPropertyChanged("CreatedBy"); }
        }

        public Project Project
        {
            get { return this._Bug.Project; }
            set { _Bug.Project = value; OnPropertyChanged("Project"); }
        }

        public User AssignedUser
        {
            get { return this._Bug.AssignedUser; }
            set { _Bug.AssignedUser = value; OnPropertyChanged("AssignedUser"); }
        }

        // Tracks if the bug is selected in a view
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

                switch (Field)
                {
                    case "Name":
                    {
                        const int MaxNameLength = 30;

                        if (String.IsNullOrEmpty(Name))
                            result = "This field cannot be left blank!";
                        
                        else if (Name.Length > MaxNameLength)
                            result = "Name cannot exceed "+MaxNameLength+" characters.";

                        break;
                    }

                    case "Description":
                    {
                        const int MaxDescLength = 200;

                        if (Description.Length > MaxDescLength)
                            result = "Description cannot exceed "+MaxDescLength+" characters.";

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
