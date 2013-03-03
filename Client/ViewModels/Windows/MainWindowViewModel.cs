using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Client.Helpers;
using Client.ServiceReference;
using Client.ViewModels.Controls;
using Client.ViewModels.Windows;
using Client.Factories;
using System.Collections.Generic;

namespace Client.ViewModels
{
    /// <summary>
    /// This class is the view model which controls the main bug
    /// view window. This class is concerned with controlling
    /// child components which compose of the main window.
    /// </summary>
    public class MainWindowViewModel : ObservableObject, IMainWindowViewModel
    {

        // Dependencies
        private IMessenger _Messenger;
        private ITrackerService _Service;
        private IControlFactory _ControlFactory;

        // Child Panels
        private ICommandPanelViewModel _CommandPanel;
        private IBugTableViewModel     _BugTablePanel;

        private String _Username;
        private ProjectViewModel _SelectedActiveProject;

        private ObservableCollection<ProjectViewModel> _ProjectComboBox;

        private RelayCommand _DeBugCommand;


        /// <summary>
        /// Stores references to dependencies and initialises the 
        /// username field by retrieving currents user from service.
        /// </summary>
        /// <param name="comm">Messenger to communciate with other view models.</param>
        /// <param name="svc">Web service allowing remote procedures to be called.</param>
        /// <param name="ctrlfactory">Factory to create controls on the main window.</param>
        public MainWindowViewModel(IMessenger comm, ITrackerService svc, IControlFactory ctrlfactory)
        {
            if (comm == null)
                throw new ArgumentNullException("The messenger cannot be null.");

            if (svc == null)
                throw new ArgumentNullException("The service factory cannot be null.");

            if (ctrlfactory == null)
                throw new ArgumentNullException("The control factory cannot be null.");

            _Messenger = comm;
            _Service = svc;
            _ControlFactory = ctrlfactory;

            Username = _Service.GetMyUser().FirstName;

            InitialiseActiveProject();
        }


        public EventHandler RequestClose { get; set; }


        public String Username
        {
            get { return _Username; }
            set { _Username = value; }
        }


        /// <summary>
        /// The panel which displays the table of bugs
        /// </summary>
        public IBugTableViewModel BugTablePanel
        {
            get
            {
                if (_BugTablePanel == null)
                {
                    _BugTablePanel = _ControlFactory.CreateBugTablePanel(_SelectedActiveProject);
                }

                return _BugTablePanel; 
            }

            set { _BugTablePanel = value; OnPropertyChanged("BugTablePanel"); }
        }


        /// <summary>
        /// Command panel which provides buttons to manipulate bugs
        /// </summary>
        public ICommandPanelViewModel CommandPanel
        {
            get 
            {
                if (_CommandPanel == null)
                {
                    _CommandPanel = _ControlFactory.CreateCommandPanel(_SelectedActiveProject);
                }

                return _CommandPanel;
            }

            set { _CommandPanel = value; OnPropertyChanged("CommandPanel"); }
        }


        /// <summary>
        /// The list of user assigned projects to be displayed in combo box.
        /// Associated projects are retrieved from the service and initialses
        /// the currently active project.
        /// </summary>
        public ObservableCollection<ProjectViewModel> ProjectComboBox
        {
            get
            {
                if (_ProjectComboBox == null)
                {
                    _ProjectComboBox = new ObservableCollection<ProjectViewModel>();
                    
                    foreach (Project proj in _Service.GetMyProjects())
                    {
                        _ProjectComboBox.Add(new ProjectViewModel(proj));
                    }
                }

                return _ProjectComboBox;
            }

            set { _ProjectComboBox = value; OnPropertyChanged("ProjectComboBox"); }
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

                // Notify listeners of active project change
                _Messenger.NotifyColleagues(Messages.ActiveProjectChanged, value);

                OnPropertyChanged("SelectedActiveProject");
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


        private void InitialiseActiveProject()
        {
            List<Project> projects = _Service.GetMyProjects();
            
            if (projects.Count > 0 && projects != null)
                SelectedActiveProject = new ProjectViewModel(projects[0]);
        }


        private void Debug()
        {
            
        }

    }
}
