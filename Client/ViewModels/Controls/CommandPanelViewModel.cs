using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Client.Helpers;
using Client.ServiceReference;
using Client.ViewModels;
using Microsoft.Practices.Unity;

namespace Client.ViewModels
{
    public class CommandPanelViewModel : ObservableObject
    {

        private IMessenger _Messenger;

        private RelayCommand _DeleteSelectedBugsCommand;
        private RelayCommand _EditBugCommand;
        private RelayCommand _AddBugCommand;

        private ITrackerService _Service;
        private ProjectViewModel _ActiveProject;
        private IList<BugViewModel> _SelectedBugs;


        public CommandPanelViewModel(IMessenger comm, ITrackerService svc, ProjectViewModel activeProj) 
        {
            _Messenger = comm;
            _Service = svc;
            _ActiveProject = activeProj;

            ListenForMessages();
        }


        private bool IsBugsSelected
        {
            get
            {
                _Messenger.NotifyColleagues(Messages.RequestSelectedBugs);

                if (_SelectedBugs != null && _SelectedBugs.Count > 0)
                    return true;
                else
                    return false;
            }
        }


        private bool IsProjectSelected
        {
            get
            {
                if (_ActiveProject == null)
                    return false;

                return true;
            }
        }


        #region Commands

        public ICommand DeleteSelectedBugsCommand
        {
            get
            {
                if (_DeleteSelectedBugsCommand == null)
                {
                    _DeleteSelectedBugsCommand = new RelayCommand(p => this.DeleteSelectedBugs(), p => IsBugsSelected);
                }

                return _DeleteSelectedBugsCommand;
            }
        }

        public ICommand AddBugCommand
        {
            get
            {
                if (_AddBugCommand == null)
                {
                    _AddBugCommand = new RelayCommand(param => ShowAddBugView(), param => IsProjectSelected);
                }

                return _AddBugCommand;
            }
        }
       
        public ICommand EditBugCommand
        {
            get
            {
                if (_EditBugCommand == null)
                {
                    _EditBugCommand = new RelayCommand(param => ShowEditBugView(), param => IsBugsSelected);
                }

                return _EditBugCommand;
            }
        }

        #endregion Commands


        private void ListenForMessages()
        {
            _Messenger.Register<IList<BugViewModel>>(Messages.SelectedBugsChanged, p => _SelectedBugs = p);
            _Messenger.Register<ProjectViewModel>(Messages.ActiveProjectChanged, p => _ActiveProject = p);
        }


        private void ShowEditBugView()
        {
            _Messenger.NotifyColleagues(Messages.ShowBugViewPanel);
        }


        private void ShowAddBugView()
        {
            _Messenger.NotifyColleagues(Messages.ShowBugAddPanel);
        }


        /// <summary>
        /// Deletes the user selected bugs from the web service and bug table.
        /// </summary>
        private void DeleteSelectedBugs()
        {
            // For each selected bug, remove it from the service and view
            foreach (BugViewModel bug in _SelectedBugs)
            {
                // Delete using web service
                _Service.DeleteBug(bug.ToBugModel());
                // Delete from local bug view
                _Messenger.NotifyColleagues(Messages.SelectedBugDeleted, bug);
            }
        }

    }
}
