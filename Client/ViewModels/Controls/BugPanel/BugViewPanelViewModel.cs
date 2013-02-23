using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Client.Helpers;
using Client.Services;
using System.Windows;
using Client.ServiceReference;
using System.ServiceModel;
using Client.ViewModels;

namespace Client.ViewModels
{
    public class BugViewPanelViewModel : BugPanelViewModel
    {

        private RelayCommand _SaveBugCommand;
        

        public BugViewPanelViewModel(IMessenger comm) : base(comm) { }


        public override BugTableViewModel Parent
        {
            get { return _Parent; }

            set
            {
                _Parent = value;

                EditedBug = new BugViewModel(Parent.SelectedBug.ToBugModel());
            }
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

                    BugViewModel selectedBug = Parent.BugList.Where(p => p.Id == bug.Id).SingleOrDefault();

                    int index = Parent.BugList.IndexOf(selectedBug);

                    Parent.BugList.Remove(selectedBug);
                    Parent.BugList.Insert(index, new BugViewModel(savedBug));
                    Parent.SelectedBug = Parent.BugList.ElementAt(index);

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
