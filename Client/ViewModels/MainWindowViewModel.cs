using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Client.ServiceReference;
using Client.Commands;
using System.Windows.Input;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Collections.Specialized;
using System.Windows.Controls;
using System.Windows.Data;
using System.ServiceModel;
using Client.Services;
using Client.Controllers;
using Client.ViewModels.Controls;

namespace Client.ViewModels
{
    /// <summary>
    /// This class is the view model which controls the main bug
    /// view window.
    /// </summary>
    public class MainWindowViewModel : ViewModel, IWindow
    {

        private bool _IsBugViewVisible;
        private String _Username;
        private String _ProjectTitle;

        private BugViewModel _SelectedBug;

        private List<BugViewModel> _SelectedBugs;
        private ProjectViewModel _SelectedActiveProject;
        private BugPanelViewModel _SouthViewPanel;
        private CommandPanelViewModel _CommandPanel;
        
        private ObservableCollection<BugViewModel> _BugList;
        private ObservableCollection<ProjectViewModel> _ProjectComboBox;

        private RelayCommand _DeBugCommand;


        /// <summary>
        /// Inherits constructor from base class.
        /// </summary>
        public MainWindowViewModel() : base() 
        {
            Username = TrackerService.Service.GetMyUser().FirstName;
        }


        public EventHandler RequestClose { get; set; }
        public IWindowLoader WindowLoader { get; set; }


        public String Username
        {
            get { return _Username; }
            set { _Username = value; }
        }


        public String ProjectTitle
        {
            get { return _ProjectTitle; }
            set 
            { 
                _ProjectTitle = value+" Bugs";
                OnPropertyChanged("ProjectTitle"); 
            }
        }


        public BugPanelViewModel SouthViewPanel
        {
            get 
            {
                if (_SouthViewPanel == null)
                {
                    _SouthViewPanel = new BugPanelViewModel();
                    _SouthViewPanel.Parent = this;
                }

                return _SouthViewPanel;
            }

            set { _SouthViewPanel = value; OnPropertyChanged("BugPanelViewModel"); }
        }


        public CommandPanelViewModel CommandPanel
        {
            get 
            {
                if (_CommandPanel == null)
                {
                    _CommandPanel = new CommandPanelViewModel();
                    _CommandPanel.Parent = this;
                }

                return _CommandPanel;
            }

            set { _CommandPanel = value; OnPropertyChanged("CommandPanelViewModel"); }
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
                SouthViewPanel.EditedBug = value.ToBugModel();
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
        /// The list of user assigned projects to be displayed in combo box.
        /// </summary>
        public ObservableCollection<ProjectViewModel> ProjectComboBox
        {
            get
            {
                if (_ProjectComboBox == null)
                {
                    _ProjectComboBox = ProjectModelToViewModel(TrackerService.Service.GetMyProjects().ToList());
                }

                return _ProjectComboBox;
            }

            set { _ProjectComboBox = value; }
        }


        /// <summary>
        /// Stores the currently selected project from combo box.
        /// </summary>
        public ProjectViewModel SelectedActiveProject
        {
            get { return _SelectedActiveProject; }
            set
            {
                _SelectedActiveProject = value;
                PopulateBugTable();
                ProjectTitle = value.Name;
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



        #region Commands

        public ICommand DeBugCommand
        {
            get
            {
                if (_DeBugCommand == null)
                {
                    _DeBugCommand = new RelayCommand(param => this.Debug());
                }

                return _DeBugCommand;
            }
        }

        #endregion



        private void Debug()
        {
            BugList.Add(new BugViewModel(new Bug { Id = 5, Name = "joe" }));
        }


        /// <summary>
        /// Populates the table of bugs according to currently selected project.
        /// </summary>
        private void PopulateBugTable()
        {
            if (SelectedActiveProject != null)
            {
                BugList.Clear();
                List<Bug> bugList = TrackerService.Service.GetBugsByProject(SelectedActiveProject.ToProjectModel());

                foreach (Bug bug in bugList)
                {
                    BugList.Add(new BugViewModel(bug));
                }
            }
        }



        /// <summary>
        /// Converts a list of project objects to an observable collection of projects.
        /// </summary>
        /// <param name="list">The list of project objects.</param>
        /// <returns>The result of the mappings between project and view model objects.</returns>
        private ObservableCollection<ProjectViewModel> ProjectModelToViewModel(List<Project> list)
        {
            ObservableCollection<ProjectViewModel> obsProjects = new ObservableCollection<ProjectViewModel>();

            foreach (Project proj in list)
            {
                obsProjects.Add(new ProjectViewModel(proj));
            }

            return obsProjects;
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
