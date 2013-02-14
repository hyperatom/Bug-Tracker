using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Client.ServiceReference;

namespace Client.ViewModels
{
    /// <summary>
    /// This class is a client representation of a bug data structure.
    /// It is an observable object and notifies its container of changes.
    /// </summary>
    public class BugViewModel : ViewModel
    {

        private Int32 _Id;
        private String _Name;
        private String _Description;
        private String _Priority;
        private String _Status;
        private DateTime _DateFound;
        private DateTime _LastModified;
        private Boolean _Fixed;
        private User _CreatedBy;
        private Project _Project;
        private bool _IsSelected;

        


        /// <summary>
        /// A bug object is required to initialise an object of the view
        /// model. Attributes are mapped from the bug object to view model.
        /// </summary>
        /// <param name="bug">Bug object data structure.</param>
        public BugViewModel(Bug bug)
        {
            _Id = bug.Id;
            _Description = bug.Description;
            _Name = bug.Name;
            _Priority = bug.Priority;
            _Status = bug.Status;
            _CreatedBy = bug.CreatedBy;
            _DateFound = bug.DateFound;
            _Fixed = bug.Fixed;
            _LastModified = bug.LastModified;
            _Project = bug.Project;
            _IsSelected = false;
        }


        /// <summary>
        /// Converts the current view model object back to
        /// a simple bug data structure.
        /// </summary>
        /// <returns>A bug object resulting from the mapping.</returns>
        public Bug ToBugModel()
        {
            Bug bug = new Bug
            {
                Id = _Id,
                Description = _Description,
                Status = _Status,
                LastModified = _LastModified,
                DateFound = _DateFound,
                Fixed = _Fixed,
                CreatedBy = _CreatedBy,
                Priority = _Priority,
                Name = _Name,
                Project = _Project
            };

            return bug;
        }


        /// <summary>
        /// Field controls access to the bug's ID field. Changes cause
        /// objects view to be notified using SetAndNotify method.
        /// </summary>
        public Int32 Id
        {
            get { return this._Id; }
            set { _Id = value;  OnPropertyChanged("Id"); }
        }

        public String Name
        {
            get { return this._Name; }
            set { _Name = value; OnPropertyChanged("Name"); }
        }

        public String Description
        {
            get { return this._Description; }
            set { _Description = value; OnPropertyChanged("Description"); }
        }

        public String Priority
        {
            get { return this._Priority; }
            set { _Priority = value; OnPropertyChanged("Priority"); }
        }

        public String Status
        {
            get { return this._Status; }
            set { _Status = value; OnPropertyChanged("Status"); }
        }

        public DateTime DateFound
        {
            get { return this._DateFound; }
            set { DateFound = value; OnPropertyChanged("DateFound"); }
        }

        public DateTime LastModified
        {
            get { return this._LastModified; }
            set { _LastModified = value; OnPropertyChanged("LastModified"); }
        }

        public Boolean Fixed
        {
            get { return this._Fixed; }
            set { _Fixed = value; OnPropertyChanged("Fixed"); }
        }

        public User CreatedBy
        {
            get { return this._CreatedBy; }
            set { _CreatedBy = value; OnPropertyChanged("CreatedBy"); }
        }

        public Project Project
        {
            get { return this._Project; }
            set { _Project = value; OnPropertyChanged("Project"); }
        }

        public bool IsSelected
        {
            get { return this._IsSelected; }
            set { _IsSelected = value; OnPropertyChanged("IsSelected"); }
        }

    }
}
