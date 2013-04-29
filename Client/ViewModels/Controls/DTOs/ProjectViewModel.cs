using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Client.ServiceReference;
using System.Collections.ObjectModel;
using System.Windows;
using Client.Helpers;
using AutoMapper;
using System.ComponentModel;

namespace Client.ViewModels
{
    /// <summary>
    /// This class is a client representation of a project data structure.
    /// It is an observable object and notifies its view of changes.
    /// </summary>
    public class ProjectViewModel : ObservableObject, IDataErrorInfo
    {

        private Project _Project;

        private bool _IsValidating = false;
        private bool _IsSelected = false;

        private Dictionary<string, string> _Errors = new Dictionary<string, string>();


        public ProjectViewModel() 
        {
            _Project = new Project();
        }


        /// <summary>
        /// A project object is required to initialise an object of the view
        /// model. Attributes are mapped from the project object to view model.
        /// </summary>
        /// <param name="bug">Bug object data structure.</param>
        public ProjectViewModel(Project proj)
        {
            _Project = proj;
        }


        /// <summary>
        /// Useful for de-referencing an object by
        /// making a copy of it.
        /// </summary>
        /// <returns></returns>
        public ProjectViewModel Clone()
        {
            return new ProjectViewModel(this.ToProjectModel());
        }


        /// <summary>
        /// Converts the current view model object back into
        /// a data transfer object.
        /// </summary>
        /// <returns>A project object resulting from the mapping.</returns>
        public Project ToProjectModel()
        {
            Mapper.CreateMap<ProjectViewModel, Project>();

            return Mapper.Map<ProjectViewModel, Project>(this);
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


        public bool IsSelected
        {
            get { return _IsSelected; }
            set { _IsSelected = value; OnPropertyChanged("IsSelected"); }
        }


        public Int32 Id
        {
            get { return _Project.Id; }
            set { _Project.Id = value; OnPropertyChanged("Id"); }
        }

        public String Name
        {
            get { return _Project.Name; }
            set { _Project.Name = value; OnPropertyChanged("Name"); }
        }

        public String Description
        {
            get { return _Project.Description; }
            set { _Project.Description = value; OnPropertyChanged("Description"); }
        }

        public String Code
        {
            get { return _Project.Code; }
            set { _Project.Code = value.ToUpperInvariant(); OnPropertyChanged("Code"); }
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

                if (!_IsValidating)
                    return result;

                // Remove any previous errors from the dictionary
                _Errors.Remove(Field);

                switch (Field)
                {
                    case "Name":
                        {
                            const int MaxNameLength = 40;

                            if (String.IsNullOrEmpty(Name))
                            {
                                result = "This field cannot be left blank!";
                            }

                            else if (Name.Length > MaxNameLength)
                                result = "Name cannot exceed " + MaxNameLength + " characters.";

                            break;
                        }

                    case "Description":
                        {
                            const int MaxDescLength = 200;

                            if (Description.Length > MaxDescLength)
                                result = "Description cannot exceed " + MaxDescLength + " characters.";

                            break;
                        }

                    case "Code":
                        {
                            const int CodeLength = 5;

                            if (String.IsNullOrEmpty(Code))
                                result = "This field cannot be left blank!";

                            else if (Code.Length != CodeLength)
                                result = "Code must be " + CodeLength + " characters long.";

                            else if (_Errors.ContainsKey("ExistingCode"))
                                result = "A project with this code already exists.";

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
