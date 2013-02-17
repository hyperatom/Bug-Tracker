using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Client.ServiceReference;
using AutoMapper;

namespace Client.ViewModels
{
    /// <summary>
    /// This class is a client representation of a bug data structure.
    /// It is an observable object and notifies its container of changes.
    /// </summary>
    public class BugViewModel : ViewModel
    {

        private Bug _Bug;

        private bool _IsSelected;


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
        /// Field controls access to the bug's ID field. Changes cause
        /// objects view to be notified using SetAndNotify method.
        /// </summary>
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
            set { _Bug.Description = value; OnPropertyChanged("Description"); }
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

        public bool IsSelected
        {
            get { return this._IsSelected; }
            set { _IsSelected = value; OnPropertyChanged("IsSelected"); }
        }

    }
}
