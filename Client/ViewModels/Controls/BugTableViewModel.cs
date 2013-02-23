using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using Client.ServiceReference;
using Client.Services;
using Microsoft.Practices.Unity;
using Client.Helpers;
using System.Windows;
using Client.ViewModels;

namespace Client.ViewModels
{
    public class BugTableViewModel : ObservableObject
    {

        private String _ProjectTitle;
        private bool _IsBugViewVisible;
        private BugViewModel _SelectedBug;
        private List<BugViewModel> _SelectedBugs;
        private ObservableCollection<BugViewModel> _BugList;
        private BugPanelViewModel _SouthViewPanel;
        private MainWindowViewModel _Parent;
        private IMessenger _Messenger;
        private ITrackerService _Service;


        public BugTableViewModel(IMessenger comm)
        {
            _Messenger = comm;

            _Service = IOC.Container.Resolve<ITrackerService>();

            ListenOnAddedBugs();
        }

        private void ListenOnAddedBugs()
        {
            _Messenger.Register(Messages.AddPanelSavedBug, (Action<BugViewModel>)(p => { _BugList.Add(p); }));
        }


        public MainWindowViewModel Parent
        {
            get { return _Parent; }
            set { _Parent = value; }
        }


        public String ProjectTitle
        {
            get { return _ProjectTitle; }
            set
            {
                _ProjectTitle = value + " Bugs";
                OnPropertyChanged("ProjectTitle");
            }
        }


        public BugPanelViewModel SouthViewPanel
        {
            get
            {
                return _SouthViewPanel;
            }

            set 
            { 
                _SouthViewPanel = value;
                _SouthViewPanel.Parent = this;
                OnPropertyChanged("SouthViewPanel"); 
            }
        }


        /// <summary>
        /// Stores the bug view model which the user has currently selected.
        /// </summary>
        public BugViewModel SelectedBug
        {
            get
            {
                return _SelectedBug;
            }

            set
            {
                _SelectedBug = value;

                if (SouthViewPanel.GetType() == typeof(BugViewPanelViewModel))
                    SouthViewPanel.EditedBug = new BugViewModel(value.ToBugModel());

                OnPropertyChanged("SelectedBug");
            }
        }


        public List<BugViewModel> SelectedBugs
        {
            get
            {
                _SelectedBugs = new List<BugViewModel>();

                foreach (BugViewModel bug in BugList)
                {
                    if (bug.IsSelected)
                        _SelectedBugs.Add(bug);
                }

                return _SelectedBugs;
            }
        }


        public bool IsBugViewVisible
        {
            get { return _IsBugViewVisible; }
            set
            {
                _IsBugViewVisible = value;
                OnPropertyChanged("IsBugViewVisible");
            }
        }


        /// <summary>
        /// The state of row selection. True if more than one rows are selected.
        /// </summary>
        public bool IsRowsSelected
        {
            get
            {
                if (SelectedBugs == null || SelectedBugs.Count <= 0)
                {
                    return false;
                }

                return true;
            }
        }


        /// <summary>
        /// Observable collection of bug view models contained
        /// within the bug table.
        /// </summary>
        public ObservableCollection<BugViewModel> BugList
        {
            get
            {
                if (_BugList == null)
                {
                    _BugList = new ObservableCollection<BugViewModel>();
                }

                return _BugList;
            }

            set
            {
                _BugList = value;
                OnPropertyChanged("BugList");
            }
        }


        /// <summary>
        /// Populates the table of bugs according to currently selected project.
        /// </summary>
        public void PopulateBugTable()
        {
            if (Parent.SelectedActiveProject != null)
            {
                BugList.Clear();
                List<Bug> bugList = _Service.GetBugsByProject(Parent.SelectedActiveProject.ToProjectModel());

                foreach (Bug bug in bugList)
                {
                    BugList.Add(new BugViewModel(bug));
                }
            }
        }


        /// <summary>
        /// Toggles the visibility of the south view panel.
        /// </summary>
        public void ToggleBugView()
        {
            if (!IsBugViewVisible)
            {
                IsBugViewVisible = true;
            }
            else
            {
                IsBugViewVisible = false;
            }
        }

    }
}
