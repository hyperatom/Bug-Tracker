using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Client.Helpers;
using System.Windows.Input;
using Client.ServiceReference;
using System.ServiceModel;
using System.Windows;
using Microsoft.Practices.Unity;
using Client.ViewModels;

namespace Client.ViewModels
{
    /// <summary>
    /// This class is a type of BugPanel which extends its
    /// functionality so adding bugs to the system is possible.
    /// </summary>
    public class BugAddPanelViewModel : BugPanelViewModel
    {

        private RelayCommand _AddBugCommand;

        
        /// <summary>
        /// Initialises the panel object.
        /// </summary>
        /// <param name="comm">Messenger object to communicate with other view models.</param>
        /// <param name="svc">The bug tracker web service.</param>
        /// <param name="activeProj">The currently active project.</param>
        public BugAddPanelViewModel(IMessenger comm, ITrackerService svc, ProjectViewModel activeProj) 
            : base(comm, svc, activeProj) 
        {
            InitialiseBugViewModel();
        }


        #region Commands

        public ICommand AddBugCommand
        {
            get
            {
                if (_AddBugCommand == null)
                {
                    _AddBugCommand = new RelayCommand(p => this.AddBug((BugViewModel)p));
                }

                return _AddBugCommand;
            }
        }

        #endregion Commands


        /// <summary>
        /// Creates a new bug view model and presets the default
        /// priority and status values which reflect on the user
        /// interface.
        /// </summary>
        private void InitialiseBugViewModel()
        {
            EditedBug = new BugViewModel();

            EditedBug.Priority = PriorityList[1];
            EditedBug.Status = StatusList[1];
        }


        /// <summary>
        /// Adds a new bug to the system and reflects the addition
        /// in the table of bugs.
        /// </summary>
        /// <param name="bug">The bug view model to add to system.</param>
        private void AddBug(BugViewModel bug)
        {
            // Performs validation before bug is saved
            if (BugIsValidated())
            {
                try
                {
                    bug.CreatedBy = _Service.GetMyUser();
                    bug.Project   = _ActiveProject.ToProjectModel();

                    // Convert bug view model to bug model service can accept
                    Bug savedBug = _Service.AddBug(bug.ToBugModel());

                    // Notify listeners that bug has been saved
                    _Messenger.NotifyColleagues(Messages.AddPanelSavedBug, new BugViewModel(savedBug));

                    InitialiseBugViewModel();

                    // Close the add panel
                    IsVisible = false;
                }
                catch (FaultException e)
                {
                    MessageBox.Show(e.InnerException.Message);
                }
            }
        }

    }
}
