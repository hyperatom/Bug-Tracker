using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Client.ServiceReference;
using Client.Services;
using System.Windows;

namespace Client.ViewModels.Controls
{
    public abstract class BugPanelViewModel : ViewModel
    {

        protected Bug _EditedBug;

        protected User _AssignedUser;

        protected List<String> _PriorityList;
        protected List<User>   _UsersInActiveProject;
        protected List<String> _StatusList;

        public BugTableViewModel _Parent;


        public BugPanelViewModel() : base() { }


        public virtual BugTableViewModel Parent
        {
            get { return _Parent; }
            set { _Parent = value; }
        }


        public Bug EditedBug
        {
            get { return _EditedBug; }
            set { _EditedBug = value; AssignedUser = value.AssignedUser; OnPropertyChanged("EditedBug"); }
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
                    _StatusList = TrackerService.Service.GetBugStatusList();

                return _StatusList;
            }

            set { _StatusList = value; }
        }


        public List<User> UsersInActiveProject
        {
            get
            {
                if (_UsersInActiveProject == null && Parent.Parent.SelectedActiveProject != null)
                {
                    _UsersInActiveProject = TrackerService.Service.GetUsersByProject
                        (Parent.Parent.SelectedActiveProject.ToProjectModel());
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
                    _PriorityList = TrackerService.Service.GetBugPriorityList();

                return _PriorityList;
            }

            set { _PriorityList = value; }
        }

    }
}
