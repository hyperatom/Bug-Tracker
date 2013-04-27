using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Windows;
using System.Windows.Input;
using Client.Helpers;
using Client.ServiceReference;
using Client.ViewModels;
using Client.ViewModels.Controls.DTOs;

namespace Client.ViewModels
{
    /// <summary>
    /// Extends the behaviour of a bug view panel by adding
    /// operations to edit an existing bug.
    /// </summary>
    public class BugViewPanelViewModel : BugPanelViewModel
    {

        private RelayCommand _SaveBugCommand;
        

        /// <summary>
        /// Initialises the bug view panel and de-references the selected bug to
        /// ensure updates to the edited bug do not reflect in the bug table.
        /// Also initialises a message listener to monitor changes in selected bug.
        /// </summary>
        /// <param name="comm">The mediator object for communication between view models.</param>
        /// <param name="svc">The bug tracker web service.</param>
        /// <param name="activeProj">The currently active project.</param>
        /// <param name="selectedBug">The currently selected bug</param>
        public BugViewPanelViewModel(IMessenger comm, ITrackerService svc, ProjectViewModel activeProj, BugViewModel selectedBug)
            : base(comm, svc, activeProj) 
        {
            if (selectedBug == null)
                throw new ArgumentNullException("The selected bug cannot be null.");

            UpdateBugView(selectedBug);

            ListenForMessages();
        }


        /// <summary>
        /// Listens for messages which change object dependencies.
        /// </summary>
        private void ListenForMessages()
        {
            _Messenger.Register<BugViewModel>(Messages.SelectedBugChanged, p => UpdateBugView(p));
        }


        #region Commands

        public ICommand SaveBugCommand
        {
            get
            {
                if (_SaveBugCommand == null)
                {
                    _SaveBugCommand = new RelayCommand(p => this.SaveBug((BugViewModel)p));
                }

                return _SaveBugCommand;
            }
        }

        #endregion


        private void UpdateBugView(BugViewModel bug)
        {
            if (bug != null)
            {
                EditedBug = bug.Clone();

                if (EditedBug.AssignedUser != null)
                    AssignedUser = UsersInActiveProject.Where(p => p.Id == EditedBug.AssignedUser.Id).SingleOrDefault();
                else
                    AssignedUser = UsersInActiveProject.Where(p => p.Id == 0).SingleOrDefault();
            }
        }


        /// <summary>
        /// Saves a bug which has been editied.
        /// </summary>
        /// <param name="bug">The object which has been edited.</param>
        private void SaveBug(BugViewModel bug)
        {
            if (BugIsValidated())
            {
                try
                {
                    Bug savedBug = _Service.SaveBug(bug.ToBugModel());

                    _Messenger.NotifyColleagues(Messages.SelectedBugSaved, new BugViewModel(savedBug));

                    EditedBug = new BugViewModel(savedBug);

                    IsVisible = false;
                }
                catch (FaultException e)
                {
                    MessageBox.Show(e.Message);
                }
            }
        }

    }
}
