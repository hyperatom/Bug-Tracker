using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Client.Helpers;
using System.Windows.Input;
using Client.Services;
using Client.ServiceReference;
using Microsoft.Practices.Unity;
using Client.ViewModels;

namespace Client.ViewModels
{
    public class CommandPanelViewModel : ObservableObject
    {

        private MainWindowViewModel _Parent;

        private IMessenger _Messenger;

        private RelayCommand _DeleteSelectedBugsCommand;
        private RelayCommand _EditBugCommand;
        private RelayCommand _AddBugCommand;

        private ITrackerService _Service;


        public CommandPanelViewModel(IMessenger comm) 
        {
            _Messenger = comm;

            _Service = IOC.Container.Resolve<ITrackerService>();
        }


        public MainWindowViewModel Parent
        {
            get { return _Parent; }
            set { _Parent = value; }
        }

        private bool IsProjectSelected
        {
            get
            {
                if (Parent.SelectedActiveProject == null)
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
                    _DeleteSelectedBugsCommand = new RelayCommand(param => this.DeleteSelectedBugs(),
                                                                  param => Parent.BugTablePage.IsRowsSelected);
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
                    _AddBugCommand = new RelayCommand(param => ShowNewBugView(), param => IsProjectSelected);
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
                    _EditBugCommand = new RelayCommand(param => ShowBugView(), param => Parent.BugTablePage.IsRowsSelected);
                }

                return _EditBugCommand;
            }
        }

        #endregion Commands


        private void ShowBugView()
        {
            Parent.BugTablePage.SouthViewPanel
                = new BugViewPanelViewModel(_Messenger);

            if (!Parent.BugTablePage.IsBugViewVisible)
                Parent.BugTablePage.ToggleBugView();
        }


        private void ShowNewBugView()
        {
            Parent.BugTablePage.SouthViewPanel
                = new BugAddPanelViewModel(_Messenger, Parent.SelectedActiveProject.ToProjectModel());

            if (!Parent.BugTablePage.IsBugViewVisible)
                Parent.BugTablePage.ToggleBugView();
        }


        /// <summary>
        /// Deletes the user selected bugs from the web service and bug table.
        /// </summary>
        private void DeleteSelectedBugs()
        {
            // Copy and cast selected bugs into a list of bug view models
            List<BugViewModel> bugVmList = Parent.BugTablePage.SelectedBugs.Cast<BugViewModel>().ToList();

            // For each selected bug, remove it from the service and view
            foreach (BugViewModel bug in bugVmList)
            {
                // Delete using web service
                _Service.DeleteBug(bug.ToBugModel());
                // Delete from local bug view
                Parent.BugTablePage.BugList.Remove(bug);
            }

            // If south view panel open then close it
            if (Parent.BugTablePage.IsBugViewVisible) { Parent.BugTablePage.ToggleBugView(); }
        }

    }
}
