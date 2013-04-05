using System;
using System.Collections.Generic;
using System.Windows.Input;
using Client.Helpers;
using Client.ServiceReference;
using Client.ViewModels.Controls;
using System.Windows;

namespace Client.ViewModels
{
    /// <summary>
    /// This class provides methods which invoke data manipulation
    /// operations on bug object.
    /// </summary>
    public class CommandPanelViewModel : ObservableObject, ICommandPanelViewModel
    {

        private IMessenger _Messenger;

        private RelayCommand _DeleteSelectedBugsCommand;
        private RelayCommand _EditBugCommand;
        private RelayCommand _AddBugCommand;

        private bool _IsBugTableDisplaying = true;

        private ITrackerService _Service;
        private ProjectViewModel _ActiveProject;
        private BugViewModel _SelectedBug;


        /// <summary>
        /// Stores references to dependencies and listens for incoming messages.
        /// </summary>
        /// <param name="comm">The mediator which allows communication between view models.</param>
        /// <param name="svc">The bug tracker web service.</param>
        /// <param name="activeProj"></param>
        public CommandPanelViewModel(IMessenger comm, ITrackerService svc, ProjectViewModel activeProj) 
        {
            if (comm == null)
                throw new ArgumentNullException("The messenger cannot be null.");

            if (svc == null)
                throw new ArgumentNullException("Web service cannot be null.");

            _Messenger = comm;
            _Service = svc;

            if (!IsProjectNull(activeProj))
                _ActiveProject = activeProj;

            ListenForMessages();
        }


        /// <summary>
        /// Requests a collection of currently selected bugs and
        /// returns true or false depending if any are selected.
        /// </summary>
        private bool IsBugSelected
        {
            get
            {
                if (_SelectedBug != null && _IsBugTableDisplaying)
                    return true;
                else
                    return false;
            }
        }


        /// <summary>
        /// Tracks whether an active project is selected.
        /// </summary>
        private bool IsProjectSelected
        {
            get
            {
                if (_ActiveProject == null || !_IsBugTableDisplaying)
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
                    _DeleteSelectedBugsCommand = new RelayCommand(p => this.DeleteSelectedBugs(), p => IsBugSelected);
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
                    _EditBugCommand = new RelayCommand(param => ShowEditBugView(), p => IsBugSelected);
                }

                return _EditBugCommand;
            }
        }

        #endregion Commands


        private bool IsProjectNull(ProjectViewModel proj)
        {
            if (proj == null || proj.Id == 0)
                return true;

            return false;
        }


        /// <summary>
        /// Subscribes to incoming messages concerned with data in this object.
        /// </summary>
        private void ListenForMessages()
        {
            _Messenger.Register<BugViewModel>(Messages.SelectedBugChanged, p => _SelectedBug = p);
            _Messenger.Register<ProjectViewModel>(Messages.ActiveProjectChanged, p => _ActiveProject = p);
            _Messenger.Register<bool>(Messages.BugTableDisplaying, p => _IsBugTableDisplaying = p);
        }


        /// <summary>
        /// Sends a message requesting to show the bug view panel.
        /// </summary>
        private void ShowEditBugView()
        {
            _Messenger.NotifyColleagues(Messages.ShowBugViewPanel);
        }


        /// <summary>
        /// Sends a messages requesting to show the bug add panel.
        /// </summary>
        private void ShowAddBugView()
        {
            _Messenger.NotifyColleagues(Messages.ShowBugAddPanel);
        }


        private void DeleteSelectedBugs()
        {
            _Messenger.NotifyColleagues(Messages.DeleteSelectedBugs);
        }

    }
}
