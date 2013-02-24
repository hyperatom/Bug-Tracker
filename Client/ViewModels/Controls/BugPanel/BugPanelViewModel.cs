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
    public abstract class BugPanelViewModel : ObservableObject
    {

        private bool _IsVisible;

        protected BugViewModel      _EditedBug;
        protected User              _AssignedUser;
        protected BugTableViewModel _Parent;

        protected List<String> _PriorityList;
        protected List<User>   _UsersInActiveProject;
        protected List<String> _StatusList;

        protected IMessenger _Messenger;
        protected ITrackerService _Service;
        protected ProjectViewModel _ActiveProject;


        public BugPanelViewModel(IMessenger comm, ITrackerService svc, ProjectViewModel activeProj)
        {
            if (activeProj == null)
                throw new ArgumentNullException("The active project cannot be null.");

            _Messenger = comm;
            _Service = svc;
            _ActiveProject = activeProj;

            ListenForMessages();
        }


        private void ListenForMessages()
        {
            _Messenger.Register<ProjectViewModel>(Messages.ActiveProjectChanged, delegate { IsVisible = false; });
        }


        public bool IsVisible
        {
            get { return _IsVisible; }
            set { _IsVisible = value; OnPropertyChanged("IsVisible"); }
        }



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


        public User AssignedUser
        {
            get 
            {
                return UsersInActiveProject.Where(p => p.Id == EditedBug.AssignedUser.Id).FirstOrDefault();
            }

            set { EditedBug.AssignedUser = value; OnPropertyChanged("AssignedUser"); }
        }


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


        protected bool BugIsValidated()
        {
            EditedBug.IsValidating = true;

            OnPropertyChanged("EditedBug");

            EditedBug.IsValidating = false;

            return (EditedBug.Errors.Count == 0);
        }

    }
}
