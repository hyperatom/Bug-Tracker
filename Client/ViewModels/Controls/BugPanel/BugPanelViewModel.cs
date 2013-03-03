using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Client.ServiceReference;
using System.Windows;
using System.ComponentModel;
using System.Text.RegularExpressions;
using Microsoft.Practices.Unity;
using Client.Helpers;
using Client.ViewModels;

namespace Client.ViewModels
{

    /// <summary>
    /// Abstracts the operations of a generic bug panel and
    /// initialises the data bound to user input fields.
    /// </summary>
    public abstract class BugPanelViewModel : ObservableObject
    {

        private bool _IsVisible;

        protected BugViewModel      _EditedBug;
        protected User              _AssignedUser;

        protected List<String> _PriorityList;
        protected List<User>   _UsersInActiveProject;
        protected List<String> _StatusList;

        protected IMessenger _Messenger;
        protected ITrackerService _Service;
        protected ProjectViewModel _ActiveProject;


        /// <summary>
        /// Stores references to dependencies and sets up a message listener.
        /// </summary>
        /// <param name="comm">The communication channel with other view models.</param>
        /// <param name="svc">The bug tracker web service.</param>
        /// <param name="activeProj">The currently active project.</param>
        public BugPanelViewModel(IMessenger comm, ITrackerService svc, ProjectViewModel activeProj)
        {
            if (comm == null)
                throw new ArgumentNullException("The messenger cannot be null.");

            if (activeProj == null)
                throw new ArgumentNullException("The active project cannot be null.");

            if (svc == null)
                throw new ArgumentNullException("The web service cannot be null.");

            _Messenger = comm;
            _Service = svc;
            _ActiveProject = activeProj;

            ListenForMessages();
        }


        /// <summary>
        /// Listens for incoming messages.
        /// </summary>
        private void ListenForMessages()
        {
            _Messenger.Register<ProjectViewModel>(Messages.ActiveProjectChanged, delegate { IsVisible = false; });
        }


        /// <summary>
        /// Property bound to the views visibility.
        /// </summary>
        public bool IsVisible
        {
            get { return _IsVisible; }
            set { _IsVisible = value; OnPropertyChanged("IsVisible"); }
        }


        /// <summary>
        /// Stores the bug view model which is currently
        /// being edited.
        /// </summary>
        public BugViewModel EditedBug
        {
            get { return _EditedBug; }
            set 
            { 
                _EditedBug = value;
                if (value != null)
                    AssignedUser = value.AssignedUser;
                
                OnPropertyChanged("EditedBug"); 
            }
        }


        /// <summary>
        /// Gets the currently assigned user of the bug being viewed.
        /// </summary>
        public User AssignedUser
        {
            get 
            {
                return UsersInActiveProject.Where(p => p.Id == EditedBug.AssignedUser.Id).FirstOrDefault();
            }

            set { EditedBug.AssignedUser = value; OnPropertyChanged("AssignedUser"); }
        }


        /// <summary>
        /// Gets a list of possible bug status from the web service.
        /// </summary>
        public List<String> StatusList
        {
            get
            {
                if (_StatusList == null)
                    _StatusList = _Service.GetBugStatusList();

                return _StatusList;
            }

            set { _StatusList = value; }
        }


        /// <summary>
        /// Gets a collection of all users associated with the active project.
        /// </summary>
        public List<User> UsersInActiveProject
        {
            get
            {
                if (_UsersInActiveProject == null && _ActiveProject != null)
                {
                    _UsersInActiveProject = _Service.GetUsersByProject
                        (_ActiveProject.ToProjectModel());
                }

                return _UsersInActiveProject;
            }

            set { _UsersInActiveProject = value; }
        }


        /// <summary>
        /// Gets a collection of possible bug priorities from the web service.
        /// </summary>
        public List<String> PriorityList
        {
            get
            {
                if (_PriorityList == null)
                    _PriorityList = _Service.GetBugPriorityList();

                return _PriorityList;
            }

            set { _PriorityList = value; }
        }


        /// <summary>
        /// Validates the bug view model by triggering the property changed
        /// notification and returns true or false.
        /// </summary>
        /// <returns>True if bug is validated, false otherwise.</returns>
        protected bool BugIsValidated()
        {
            EditedBug.IsValidating = true;

            OnPropertyChanged("EditedBug");

            EditedBug.IsValidating = false;

            return (EditedBug.Errors.Count == 0);
        }

    }
}
