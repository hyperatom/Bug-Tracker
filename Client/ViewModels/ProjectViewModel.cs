using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Client.ServiceReference;
using System.Collections.ObjectModel;
using System.Windows;

namespace Client.ViewModels
{
    /// <summary>
    /// This class is a client representation of a project data structure.
    /// It is an observable object and notifies its view of changes.
    /// </summary>
    public class ProjectViewModel : ViewModel
    {

        private Int32 _Id;
        private String _Name;
        private String _Description;


        /// <summary>
        /// A project object is required to initialise an object of the view
        /// model. Attributes are mapped from the project object to view model.
        /// </summary>
        /// <param name="bug">Bug object data structure.</param>
        public ProjectViewModel(Project proj)
        {
            _Id = proj.Id;
            _Name = proj.Name;
            _Description = proj.Description;
        }


        /// <summary>
        /// Converts a list of bug objects to an observable
        /// collection of bug view models.
        /// </summary>
        /// <param name="bugList">List of bug objects.</param>
        /// <returns>Observable collection of bug view models.</returns>
        private ObservableCollection<BugViewModel> BugListToObservable(List<Bug> bugList)
        {
            ObservableCollection<BugViewModel> observableBugs = new ObservableCollection<BugViewModel>();
            
            foreach (Bug bug in bugList)
            {
                observableBugs.Add(new BugViewModel(bug));
            }

            return observableBugs;
        }


        /// <summary>
        /// Converts an observable collection of bug view models to bug objects.
        /// </summary>
        /// <param name="obsBugs">Observable collection of bug view models.</param>
        /// <returns>Returns a list of bug objects.</returns>
        private List<Bug> ObservableBugsToList(ObservableCollection<BugViewModel> obsBugs)
        {
            List<Bug> bugList = new List<Bug>();

            if (obsBugs != null)
            {
                foreach (BugViewModel vm in obsBugs)
                {
                    bugList.Add(vm.ToBugModel());
                }
            }

            return bugList;
        }


        /// <summary>
        /// Converts the current view model object back to
        /// a simple project data structure.
        /// </summary>
        /// <returns>A project object resulting from the mapping.</returns>
        public Project ToProjectModel()
        {
            Project proj = new Project
            {
                Id = _Id,
                Name = _Name,
                Description = _Description,
            };

            return proj;
        }


        /// <summary>
        /// Field controls access to the bug's ID field. Changes cause
        /// objects view to be notified using SetAndNotify method.
        /// </summary>
        public Int32 Id
        {
            get { return this._Id; }
            set { _Id = value; OnPropertyChanged("Id"); }
        }

        public String Name
        {
            get { return this._Name; }
            set { _Name = value; OnPropertyChanged("Name"); }
        }

        public String Description
        {
            get { return this._Description; }
            set { Description = value; OnPropertyChanged("Description"); }
        }

    }
}
