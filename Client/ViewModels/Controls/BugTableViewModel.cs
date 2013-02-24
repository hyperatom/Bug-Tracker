using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using Client.Helpers;
using Client.ServiceReference;
using Client.ViewModels;
using Microsoft.Practices.Unity;

namespace Client.ViewModels
{
    public class BugTableViewModel : ObservableObject
    {

        private String _ProjectTitle;

        private BugViewModel _SelectedBug;
        private List<BugViewModel> _SelectedBugs;

        private ObservableCollection<BugViewModel> _BugList;
        private BugPanelViewModel _SouthViewPanel;

        private ProjectViewModel _ActiveProject;
        private IMessenger _Messenger;
        private ITrackerService _Service;


        public BugTableViewModel(IMessenger comm, ITrackerService svc, ProjectViewModel activeProj)
        {
            _Messenger = comm;
            _Service = svc;
            _ActiveProject = activeProj;

            _SelectedBugs = new List<BugViewModel>();

            ListenForMessages();
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

                _Messenger.NotifyColleagues(Messages.SelectedBugChanged, value);

                OnPropertyChanged("SelectedBug");
            }
        }


        public List<BugViewModel> SelectedBugs
        {
            get
            {
                _SelectedBugs.Clear();

                foreach (BugViewModel bug in BugList)
                {
                    if (bug.IsSelected)
                        _SelectedBugs.Add(bug);
                }

                return _SelectedBugs;
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
            if (_ActiveProject != null)
            {
                BugList.Clear();
                IList<Bug> bugList = _Service.GetBugsByProject(_ActiveProject.ToProjectModel());

                foreach (Bug bug in bugList)
                {
                    BugList.Add(new BugViewModel(bug));
                }
            }
        }


        private void ListenForMessages()
        {            
            _Messenger.Register<ProjectViewModel>(Messages.ActiveProjectChanged, ActiveProjectChanged);
            _Messenger.Register<BugViewModel>(Messages.AddPanelSavedBug, p => _BugList.Add(p));
            _Messenger.Register<BugViewModel>(Messages.SelectedBugDeleted, SelectedBugDeleted);
            _Messenger.Register<BugViewModel>(Messages.SelectedBugSaved, SaveBugToTable);

            _Messenger.Register(Messages.ShowBugViewPanel, ShowBugViewPanel);
            _Messenger.Register(Messages.ShowBugAddPanel, ShowBugAddPanel);
            _Messenger.Register(Messages.RequestSelectedBugs, SendSelectedBugs);
        }


        private void SelectedBugDeleted(BugViewModel bug)
        {
            _BugList.Remove(bug);

            if (SouthViewPanel != null && SouthViewPanel.IsVisible)
                SouthViewPanel.IsVisible = false;
        }


        private void SendSelectedBugs()
        {
            _Messenger.NotifyColleagues(Messages.SelectedBugsChanged, SelectedBugs);
        }


        private void SaveBugToTable(BugViewModel bug)
        {
            BugViewModel selectedBug = BugList.Where(p => p.Id == bug.Id).SingleOrDefault();

            int index = BugList.IndexOf(selectedBug);

            BugList.Remove(selectedBug);
            BugList.Insert(index, bug);
            SelectedBug = BugList.ElementAt(index);
        }


        private void ShowBugViewPanel()
        {
            SouthViewPanel = new BugViewPanelViewModel(_Messenger, _Service, _ActiveProject, _SelectedBug);
            SouthViewPanel.IsVisible = true;
        }


        private void ShowBugAddPanel()
        {
            SouthViewPanel = new BugAddPanelViewModel(_Messenger, _Service, _ActiveProject);
            SouthViewPanel.IsVisible = true;
        }


        private void ActiveProjectChanged(ProjectViewModel proj)
        {
            _ActiveProject = proj;
            PopulateBugTable();
            ProjectTitle = proj.Name;
        }

    }
}
