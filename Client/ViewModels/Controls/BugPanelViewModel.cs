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
    public class BugPanelViewModel : ViewModel
    {

        private bool _IsAddButtonVisible = false;
        private bool _IsSaveButtonVisible = false;

        private Bug _EditedBug;

        private List<String> _PriorityList;
        private List<User>   _UsersInActiveProject;
        private List<String> _StatusList;

        private RelayCommand _SaveBugCommand;
        private RelayCommand _AddBugCommand;

        private MainWindowViewModel _Parent;


        public BugPanelViewModel() : base() 
        { 
        }


        public MainWindowViewModel Parent
        {
            get { return _Parent; }
           
            set 
            {
                _Parent = value;
            }
        }


        public Bug EditedBug
        {
            get { return _EditedBug; }
            set { _EditedBug = value; OnPropertyChanged("EditedBug"); }
        }


        public List<String> StatusList
        {
            get
            {
                if (_StatusList == null)
                    _StatusList = TrackerService.Service.GetBugStatusList();

                return _StatusList; 
            }
            
            set { _StatusList = value; }
        }


        public List<User> UsersInActiveProject
        {
            get 
            {
                if (_UsersInActiveProject == null && Parent.SelectedActiveProject != null)
                {
                    _UsersInActiveProject = TrackerService.Service.GetUsersByProject
                        (Parent.SelectedActiveProject.ToProjectModel());
                }

                return _UsersInActiveProject;
            }

            set { _UsersInActiveProject = value; }
        }


        public List<String> PriorityList
        {
            get 
            {
                if (_PriorityList == null)
                    _PriorityList = TrackerService.Service.GetBugPriorityList();

                return _PriorityList;
            }

            set { _PriorityList = value; }
        }


        public bool IsAddButtonVisible
        {
            get { return _IsAddButtonVisible; }
            set { _IsAddButtonVisible = value; OnPropertyChanged("IsAddButtonVisible");  }
        }


        public bool IsSaveButtonVisible
        {
            get { return _IsSaveButtonVisible; }
            set { _IsSaveButtonVisible = value; OnPropertyChanged("IsSaveButtonVisible");  }
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
                bug.LastModified = DateTime.Now;
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



        private void AddBug(BugViewModel bug)
        {
            TrackerService.Service.AddBug(bug.ToBugModel());
            Parent.BugList.Add(bug);
        }

    }
}
