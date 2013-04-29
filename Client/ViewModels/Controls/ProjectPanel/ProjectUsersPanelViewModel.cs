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

        private ObservableCollection<UserViewModel> _ProjectManagersList;
        private ObservableCollection<UserViewModel> _AssignedUsersList;
        private ObservableCollection<UserViewModel> _PendingUsersList;

        private bool _IsVisible = false;

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


        public bool IsVisible
        {
            get { return _IsVisible; }
            set { _IsVisible = value; OnPropertyChanged("IsVisible"); }
        }


        public bool IsAssignedUserButtonsVisible
        {
            get { return _IsDeleteButtonVisible; }
            set { _IsDeleteButtonVisible = value; OnPropertyChanged("IsDeleteButtonVisible"); }
        }


        public bool IsPendingUserButtonsVisible
        {
            get { return _IsPendingUserButtonsVisible; }
            set { _IsPendingUserButtonsVisible = value; OnPropertyChanged("IsPendingUserButtonsVisible"); }
        }


        public ObservableCollection<String> ProjectRoleList
        {
            get 
            {
                return new ObservableCollection<String>() {
                    "Project Manager", "Developer"
                };
            }
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


        public ObservableCollection<UserViewModel> AssignedUsersList
        {
            get 
            {
                if (_AssignedUsersList == null)
                    _AssignedUsersList = new ObservableCollection<UserViewModel>();

                return _AssignedUsersList;
            }

            set { _AssignedUsersList = value; OnPropertyChanged("AssignedUsersList"); }
        }


        public ObservableCollection<UserViewModel> ProjectManagersList
        {
            get 
            {
                if (_ProjectManagersList == null)
                    _ProjectManagersList = new ObservableCollection<UserViewModel>();

                return _ProjectManagersList; 
            }
            
            set { _ProjectManagersList = value; OnPropertyChanged("ProjectManagersList"); }
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
            _Messenger.Register<ProjectViewModel>(Messages.DeletedProject, p => IsVisible = false);
        }


        private void AcceptUserOnProject(UserViewModel user)
        {
            _Service.AcceptUserOnProject(user.ToUserModel(), _Project.ToProjectModel(), user.RequestedProjectRole);

            PopulateUserLists(_Project);
        }


        private void RejectUserFromProject(UserViewModel user)
        {
            _Service.RejectUserFromProject(user.ToUserModel(), _Project.ToProjectModel());

            PopulateUserLists(_Project);
        }


        private void RemoveUserFromProject(UserViewModel user)
        {
            _Service.LeaveProject(_Project.ToProjectModel(), user.ToUserModel());

            PopulateUserLists(_Project);
        }


        private void PopulatePendingList(ProjectViewModel project)
        {
            List<User> users = _Service.GetUsersPendingProjectJoin(project.ToProjectModel()).ToList();

            foreach (User user in users)
            {
                String role = ProjectRoleList.Where(p => p.Equals(_Service.GetUsersRequestedRoleForProject(user, project.ToProjectModel()))).SingleOrDefault();

                UserViewModel userVm = new UserViewModel(user);

                if (role != null)
                    userVm.RequestedProjectRole = role;

                PendingUsersList.Add(userVm);
            }
        }


        private void PopulateAssignedList(ProjectViewModel project)
        {
            _Service.GetAssignedUsersByProject(project.ToProjectModel())
                .ForEach(p => AssignedUsersList.Add(new UserViewModel(p)));
        }


        private void PopulateManagerList(ProjectViewModel project)
        {
            _Service.GetManagerUsersByProject(project.ToProjectModel())
                .ForEach(p => ProjectManagersList.Add(new UserViewModel(p)));
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


        private void ClearUserLists()
        {
            PendingUsersList.Clear();
            ProjectManagersList.Clear();
            AssignedUsersList.Clear();
        }


        private void InitButtonVisibility(ProjectViewModel project)
        {
            if (IsManagerOfProject(project))
                IsPendingUserButtonsVisible = true;
            else
                IsPendingUserButtonsVisible = false;

            if (IsManagerOfProject(project))
                IsAssignedUserButtonsVisible = true;
            else
                IsAssignedUserButtonsVisible = false;
        }


        private void PopulateUserLists(ProjectViewModel project)
        {
            IsVisible = true;

            _Project = project;

            ClearUserLists();

            PopulatePendingList(project);
            PopulateAssignedList(project);
            PopulateManagerList(project);

            InitButtonVisibility(project);
        }

    }
}
