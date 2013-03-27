using System;
using System.Collections.Generic;
using System.Windows.Input;
using Client.Helpers;
using Client.ServiceReference;
using Client.ViewModels.Controls;

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

        private ITrackerService _Service;
        private ProjectViewModel _ActiveProject;
        private IList<BugViewModel> _SelectedBugs;


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
        private bool IsBugsSelected
        {
            get
            {
                // Request currently selected bugs
                _Messenger.NotifyColleagues(Messages.RequestSelectedBugs);

                if (_SelectedBugs != null && _SelectedBugs.Count > 0)
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
            _Messenger.Register<IList<BugViewModel>>(Messages.SelectedBugsChanged, p => _SelectedBugs = p);
            _Messenger.Register<ProjectViewModel>(Messages.ActiveProjectChanged, p => _ActiveProject = p);
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
                // Notify listeners of delete operation
                _Messenger.NotifyColleagues(Messages.SelectedBugDeleted, bug);
            }
        }

    }
}
