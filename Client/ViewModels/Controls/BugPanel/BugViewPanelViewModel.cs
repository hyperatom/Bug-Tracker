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

namespace Client.ViewModels
{
    public class BugViewPanelViewModel : BugPanelViewModel
    {

        private RelayCommand _SaveBugCommand;
        

        public BugViewPanelViewModel(IMessenger comm, ITrackerService svc, ProjectViewModel proj, BugViewModel SelectedBug)
            : base(comm, svc, proj) 
        {
            ListenForMessages();
            EditedBug = new BugViewModel(SelectedBug.ToBugModel());
        }

        private void ListenForMessages()
        {
            _Messenger.Register<BugViewModel>(Messages.SelectedBugChanged, p => EditedBug = new BugViewModel(p.ToBugModel()));
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
                }
                catch (FaultException e)
                {
                    MessageBox.Show(e.Message);
                }
            }
        }

    }
}
