using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Client.Services;
using Client.Commands;
using System.Windows.Input;
using Client.ServiceReference;
using System.ServiceModel;
using System.Windows;

namespace Client.ViewModels.Controls
{
    public class BugAddPanelViewModel : BugPanelViewModel
    {
        
        private RelayCommand _AddBugCommand;


        public BugAddPanelViewModel() : base() 
        {
            EditedBug = new Bug();
        }


        #region Commands

        public ICommand AddBugCommand
        {
            get
            {
                if (_AddBugCommand == null)
                {
                    _AddBugCommand = new RelayCommand(p => this.AddBug((Bug)p));
                }

                return _AddBugCommand;
            }
        }

        #endregion Commands


        private void AddBug(Bug bug)
        {
            try
            {
                bug.LastModified = DateTime.Now;
                bug.DateFound = DateTime.Now;
                bug.CreatedBy = TrackerService.Service.GetMyUser();
                bug.Project = Parent.Parent.SelectedActiveProject.ToProjectModel();
                Bug savedBug = TrackerService.Service.AddBug(bug);
                Parent.BugList.Add(new BugViewModel(savedBug));
            }
            catch (FaultException e)
            {
                MessageBox.Show(e.InnerException.Message);
            }
        }
    }
}
