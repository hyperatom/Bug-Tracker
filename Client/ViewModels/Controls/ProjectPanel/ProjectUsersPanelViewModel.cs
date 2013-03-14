using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Client.Helpers;
using System.Collections.ObjectModel;
using Client.ServiceReference;

namespace Client.ViewModels.Controls.ProjectPanel
{
    public class ProjectUsersPanelViewModel : ObservableObject, IProjectUsersPanelViewModel
    {

        private ITrackerService _Service;
        private IMessenger _Messenger;

        private ProjectViewModel _Project;
        private ObservableCollection<User> _UserList;

        private bool _IsDeleteButtonVisible = false;

        
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


        public ObservableCollection<User> UserList
        {
            get 
            {
                if (_UserList == null)
                    _UserList = new ObservableCollection<User>();

                return _UserList;
            }

            set { _UserList = value; OnPropertyChanged("UserList"); }
        }


        private void ListenForMessages()
        {
            _Messenger.Register<ProjectViewModel>(Messages.ProjectSelected, p => PopulateUserList(p));
        }


        private void PopulateUserList(ProjectViewModel project)
        {
            User myUser = _Service.GetMyUser();
            IList<int> idList = _Service.GetProjectsManagedBy(myUser).Select(p => p.Id).Distinct().ToList();

            if (idList != null && idList.Contains(project.Id))
                IsDeleteButtonVisible = true;
            else
                IsDeleteButtonVisible = false;

            UserList.Clear();
            Project proj = project.ToProjectModel();
            IList<User> assUsers = _Service.GetAssignedUsersByProject(proj);

            foreach (User usr in assUsers)
            {
                usr.FirstName = usr.FirstName.Substring(0, 1) + ".";
                UserList.Add(usr);
            } 
        }

    }
}
