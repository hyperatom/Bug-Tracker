using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Client.Helpers;
using System.Collections.ObjectModel;
using Client.ServiceReference;
using System.Windows.Input;
using Client.ViewModels.Controls.DTOs;

namespace Client.ViewModels.Controls.ProjectPanel
{
    public class ProjectUsersPanelViewModel : ObservableObject, IProjectUsersPanelViewModel
    {

        private ITrackerService _Service;
        private IMessenger _Messenger;

        private ProjectViewModel _Project;
        private ObservableCollection<UserViewModel> _UsersInProjectList;
        private ObservableCollection<UserViewModel> _PendingUsersList;

        private ICommand _RemoveUserCommand;
        private ICommand _AcceptUserCommand;
        private ICommand _RejectUserCommand;

        private bool _IsDeleteButtonVisible = false;
        private bool _IsPendingUserButtonsVisible = false;

        
        public ProjectUsersPanelViewModel(ITrackerService svc, IMessenger messenger, ProjectViewModel proj)
        {
            this._Service = svc;
            this._Messenger = messenger;
            this._Project = proj;

            ListenForMessages();
        }


        public bool IsDeleteButtonVisible
        {
            get { return _IsDeleteButtonVisible; }
            set { _IsDeleteButtonVisible = value; OnPropertyChanged("IsDeleteButtonVisible"); }
        }


        public bool IsPendingUserButtonsVisible
        {
            get { return _IsPendingUserButtonsVisible; }
            set { _IsPendingUserButtonsVisible = value; OnPropertyChanged("IsPendingUserButtonsVisible"); }
        }


        public ObservableCollection<UserViewModel> PendingUsersList
        {
            get 
            {
                if (_PendingUsersList == null)
                    _PendingUsersList = new ObservableCollection<UserViewModel>();

                return _PendingUsersList;
            }

            set { _PendingUsersList = value; OnPropertyChanged("PendingUsersList"); }
        }


        public ObservableCollection<UserViewModel> UsersInProjectList
        {
            get 
            {
                if (_UsersInProjectList == null)
                    _UsersInProjectList = new ObservableCollection<UserViewModel>();

                return _UsersInProjectList;
            }

            set { _UsersInProjectList = value; OnPropertyChanged("UserList"); }
        }


        #region Commands

        public ICommand AcceptUserCommand
        {
            get
            {
                if (_AcceptUserCommand == null)
                {
                    _AcceptUserCommand = new RelayCommand(p => AcceptUserOnProject((UserViewModel)p));
                }

                return _AcceptUserCommand;
            }
        }

        
        public ICommand RejectUserCommand
        {
            get
            {
                if (_RejectUserCommand == null)
                {
                    _RejectUserCommand = new RelayCommand(p => RejectUserFromProject((UserViewModel)p));
                }

                return _RejectUserCommand;
            }
        }

        public ICommand RemoveUserCommand
        {
            get
            {
                if (_RemoveUserCommand == null)
                {
                    _RemoveUserCommand = new RelayCommand(p => RemoveUserFromProject((UserViewModel)p));
                }

                return _RemoveUserCommand;
            }
        }

        #endregion Commands


        private void ListenForMessages()
        {
            _Messenger.Register<ProjectViewModel>(Messages.ManagedProjectSelected, p => PopulateUserLists(p));
            _Messenger.Register<ProjectViewModel>(Messages.AssignedProjectSelected, p => PopulateUserLists(p));
        }


        private void AcceptUserOnProject(UserViewModel user)
        {
            _Service.AcceptUserOnProject(user.ToUserModel(), _Project.ToProjectModel());

            PendingUsersList.Remove(user);
            UsersInProjectList.Add(user);
        }


        private void RejectUserFromProject(UserViewModel user)
        {
            _Service.RejectUserFromProject(user.ToUserModel(), _Project.ToProjectModel());

            PendingUsersList.Remove(user);
        }


        private void RemoveUserFromProject(UserViewModel user)
        {
            _Service.LeaveProject(_Project.ToProjectModel(), user.ToUserModel());
            UsersInProjectList.Remove(user);
        }


        private void PopulatePendingList(ProjectViewModel project)
        {
            _Service.GetUsersPendingProjectJoin(project.ToProjectModel())
                .ForEach(p => PendingUsersList.Add(new UserViewModel(p)));

            if (IsManagerOfProject(project))
                IsPendingUserButtonsVisible = true;
            else
                IsPendingUserButtonsVisible = false;
        }


        private bool IsManagerOfProject(ProjectViewModel project)
        {
            User myUser = _Service.GetMyUser();
            IList<int> idList = _Service.GetProjectsManagedBy(myUser).Select(p => p.Id).Distinct().ToList();

            if (idList != null && idList.Contains(project.Id))
                return true;
            else
                return false;
        }


        private void PopulateUserLists(ProjectViewModel project)
        {
            _Project = project;

            PopulatePendingList(project);

            if (IsManagerOfProject(project))
                IsDeleteButtonVisible = true;
            else
                IsDeleteButtonVisible = false;

            UsersInProjectList.Clear();
            
            Project proj = project.ToProjectModel();
            _Service.GetAssignedUsersByProject(proj).ForEach(p => UsersInProjectList.Add(new UserViewModel(p)));
        }

    }
}
