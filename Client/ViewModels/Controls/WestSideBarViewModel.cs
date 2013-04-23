using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Client.Helpers;
using Client.ServiceReference;
using MediatorLib;

namespace Client.ViewModels.Controls
{

    public interface IWestSideBarViewModel { }


    public class WestSideBarViewModel : ObservableObject, IWestSideBarViewModel
    {

        private ITrackerService _Service;
        private IMessenger _Messenger;

        private ProjectViewModel _ActiveProject;

        private ObservableRangeCollection<BugActionLogViewModel> _ActivityList;


        public WestSideBarViewModel(ITrackerService svc, IMessenger mess, ProjectViewModel activeProj)
        {
            _Service = svc;
            _Messenger = mess;
            _ActiveProject = activeProj;

            ListenForMessages();
        }


        public ObservableRangeCollection<BugActionLogViewModel> ActivityList
        {
            get
            {
                if (_ActivityList == null && _ActiveProject.Id != 0)
                {
                    var activities = _Service.GetAllBugActionLogsInProject(_ActiveProject.ToProjectModel()).Take(15).ToList();

                    _ActivityList = new ObservableRangeCollection<BugActionLogViewModel>();

                    activities.ForEach(p => _ActivityList.Add(new BugActionLogViewModel(p)));
                }

                return _ActivityList;
            }
            set { _ActivityList = value; OnPropertyChanged("ActivityList"); }
        }


        private void ListenForMessages()
        {
            _Messenger.Register<ProjectViewModel>(Messages.ActiveProjectChanged, p => ProjectChangedEvent(p));
            _Messenger.Register<BugViewModel>(Messages.SelectedBugSaved, p => UpdateActivityList());
            _Messenger.Register<BugViewModel>(Messages.SelectedBugDeleted, p => UpdateActivityList());
            _Messenger.Register<BugViewModel>(Messages.AddPanelSavedBug, p => UpdateActivityList());
        }


        private void UpdateActivityList()
        {
            _ActivityList = null;

            OnPropertyChanged("ActivityList");
        }


        private void ProjectChangedEvent(ProjectViewModel project)
        {
            _ActiveProject = project;

            UpdateActivityList();
        }

    }
}
