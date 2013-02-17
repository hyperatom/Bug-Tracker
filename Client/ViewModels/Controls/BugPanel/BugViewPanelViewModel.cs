using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Client.Commands;
using Client.Services;
using System.Windows;
using Client.ServiceReference;
using System.ServiceModel;

namespace Client.ViewModels.Controls
{
    public class BugViewPanelViewModel : BugPanelViewModel
    {

        private RelayCommand _SaveBugCommand;
        

        public BugViewPanelViewModel() : base() { }


        public override BugTableViewModel Parent
        {
            get { return _Parent; }

            set
            {
                _Parent = value;

                EditedBug = Parent.SelectedBug.ToBugModel();
            }
        }


        #region Commands

        public ICommand SaveBugCommand
        {
            get
            {
                if (_SaveBugCommand == null)
                {
                    _SaveBugCommand = new RelayCommand(p => this.SaveBug((Bug)p));
                }

                return _SaveBugCommand;
            }
        }

        #endregion


        /// <summary>
        /// Saves a bug which has been editied.
        /// </summary>
        /// <param name="bug">The object which has been edited.</param>
        private void SaveBug(Bug bug)
        {
            try
            {
                TrackerService.Service.SaveBug(bug);
                
                BugViewModel selectedBug = Parent.BugList.Where(p => p.Id == bug.Id).SingleOrDefault();

                int index = Parent.BugList.IndexOf(selectedBug);

                Parent.BugList.Remove(selectedBug);
                Parent.BugList.Insert(index, new BugViewModel(bug));

                Parent.SelectedBug = Parent.BugList.ElementAt(index);
            }
            catch (FaultException e)
            {
                MessageBox.Show(e.Message);
            }
        }

    }
}
