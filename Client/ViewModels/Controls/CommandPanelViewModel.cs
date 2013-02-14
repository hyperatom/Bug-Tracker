using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Client.Commands;
using System.Windows.Input;
using Client.Services;

namespace Client.ViewModels.Controls
{
    public class CommandPanelViewModel
    {

        private MainWindowViewModel _Parent;

        private RelayCommand _DeleteSelectedBugsCommand;
        private RelayCommand _EditBugCommand;


        public MainWindowViewModel Parent
        {
            get { return _Parent; }
            set { _Parent = value; }
        }

        
        public ICommand DeleteSelectedBugsCommand
        {
            get
            {
                if (_DeleteSelectedBugsCommand == null)
                {
                    _DeleteSelectedBugsCommand = new RelayCommand(param => this.DeleteSelectedBugs(),
                                                                  param => Parent.IsRowsSelected);
                }

                return _DeleteSelectedBugsCommand;
            }
        }

       
        public ICommand EditBugCommand
        {
            get
            {
                if (_EditBugCommand == null)
                {
                    _EditBugCommand = new RelayCommand(param => ShowBugView(), param => Parent.IsRowsSelected);
                }

                return _EditBugCommand;
            }
        }


        private void ShowBugView()
        {
            Parent.ToggleBugView();
            Parent.SouthViewPanel.IsSaveButtonVisible = true;
        }


        /// <summary>
        /// Deletes the user selected bugs from the web service and bug table.
        /// </summary>
        private void DeleteSelectedBugs()
        {
            // Copy and cast selected bugs into a list of bug view models
            List<BugViewModel> bugVmList = Parent.SelectedBugs.Cast<BugViewModel>().ToList();

            // For each selected bug, remove it from the service and view
            foreach (BugViewModel bug in bugVmList)
            {
                // Delete using web service
                TrackerService.Service.DeleteBug(bug.ToBugModel());
                // Delete from local bug view
                Parent.BugList.Remove(bug);
            }

            // If south view panel open then close it
            if (Parent.IsBugViewVisible) { Parent.ToggleBugView(); }
        }


    }
}
