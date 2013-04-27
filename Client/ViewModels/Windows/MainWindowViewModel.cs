using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using Client.Factories;
using Client.Helpers;
using Client.ServiceReference;
using Client.ViewModels.Controls;
using Client.ViewModels.Windows;
using Microsoft.Practices.Unity;

namespace Client.ViewModels
{

    public interface IMainWindowViewModel : IWindow
    {
        IContentPanel ContentPanel { get; set; }
        ICommandPanelViewModel CommandPanel { get; set; }
        ICommand DeBugCommand { get; }

        ObservableCollection<ProjectViewModel> ProjectComboBox { get; set; }
        ProjectViewModel SelectedActiveProject { get; set; }
    }

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
        private IWindowFactory _WindowFactory;

        // Child Panels
        private ICommandPanelViewModel _CommandPanel;
        private IContentPanel          _ContentPanel;
        private IWestSideBarViewModel _WestSideBar;

        private User _CurrentUser;
        private ProjectViewModel _SelectedActiveProject;

        private String _AssignedBugsButtonText;

        private ObservableCollection<ProjectViewModel> _ProjectComboBox;

        private ICommand _DeBugCommand;
        private ICommand _ShowProjectManagerPanelCommand;
        private ICommand _ShowBugTablePanelCommand;
        private ICommand _LogoutCommand;
        private ICommand _ShowAssignedBugsCommand;
        private ICommand _ShowAccountSettingsCommand;


        /// <summary>
        /// Stores references to dependencies and initialises the 
        /// username field by retrieving currents user from service.
        /// </summary>
        /// <param name="comm">Messenger to communciate with other view models.</param>
        /// <param name="svc">Web service allowing remote procedures to be called.</param>
        /// <param name="ctrlfactory">Factory to create controls on the main window.</param>
        public MainWindowViewModel(IMessenger comm, ITrackerService svc,
                                   IWindowFactory winfactory, IControlFactory ctrlfactory)
        {
            _Messenger = comm;
            _Service = svc;
            _ControlFactory = ctrlfactory;
            _WindowFactory = winfactory;

            _CurrentUser = _Service.GetMyUser();

            InitialiseActiveProject();
            ListenForMessages();
        }


        public EventHandler RequestClose { get; set; }


        public IWestSideBarViewModel WestSideBar
        {
            get 
            {
                if (_WestSideBar == null)
                    _WestSideBar = _ControlFactory.CreateWestSideBar(_SelectedActiveProject);

                return _WestSideBar;
            }

            set { _WestSideBar = value; OnPropertyChanged("WestSideBar"); }
        }


        /// <summary>
        /// The panel which displays the table of bugs
        /// </summary>
        public IContentPanel ContentPanel
        {
            get
            {
                if (_ContentPanel == null)
                {
                    _ContentPanel = _ControlFactory.CreateBugTablePanel(_SelectedActiveProject);
                }

                return _ContentPanel; 
            }

            set { _ContentPanel = value; OnPropertyChanged("ContentPanel"); }
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
                    _CommandPanel = _ControlFactory.CreateCommandPanel(SelectedActiveProject);
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
                    
                    foreach (Project proj in _Service.GetAllProjectsByUser(_Service.GetMyUser()))
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


        public String AssignedBugsButtonText
        {
            get 
            {
                _AssignedBugsButtonText = "Bugs Assigned To Me (" + GetNumberOfBugsAssignedToUser() + ")";

                return _AssignedBugsButtonText;
            }
            set { _AssignedBugsButtonText = value; OnPropertyChanged("AssignedBugsButtonText"); }
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

        public ICommand LogoutCommand
        {
            get
            {
                if (_LogoutCommand == null)
                {
                    _LogoutCommand = new RelayCommand(param => Logout());
                }

                return _LogoutCommand;
            }
        }

        public ICommand ShowProjectManagerPanelCommand
        {
            get
            {
                if (_ShowProjectManagerPanelCommand == null)
                {
                    _ShowProjectManagerPanelCommand = new RelayCommand(param => this.ShowProjectManagerPanel());
                }

                return _ShowProjectManagerPanelCommand;
            }
        }

        public ICommand ShowBugTablePanelCommand
        {
            get
            {
                if (_ShowBugTablePanelCommand == null)
                {
                    _ShowBugTablePanelCommand = new RelayCommand(param => this.ShowBugTablePanel());
                }

                return _ShowBugTablePanelCommand;
            }
        }

        public ICommand ShowAssignedBugsCommand
        {
            get
            {
                if (_ShowAssignedBugsCommand == null)
                {
                    _ShowAssignedBugsCommand = new RelayCommand(param => this.ShowAssignedBugs());
                }

                return _ShowAssignedBugsCommand;
            }
        }

        public ICommand ShowAccountSettingsCommand
        {
            get
            {
                if (_ShowAccountSettingsCommand == null)
                {
                    _ShowAccountSettingsCommand = new RelayCommand(param => this.ShowAccountSettingsPanel());
                }

                return _ShowAccountSettingsCommand;
            }
        }

        #endregion


        private void ListenForMessages()
        {
            _Messenger.Register<ProjectViewModel>(Messages.AddedProject, p => ProjectComboBox.Add(p));
            _Messenger.Register<ProjectViewModel>(Messages.SavedProject, p => ProjectSaved(p));
            _Messenger.Register<ProjectViewModel>(Messages.DeletedProject, p => ProjectDeletedAction(p));

            _Messenger.Register<BugViewModel>(Messages.SelectedBugSaved, delegate { OnPropertyChanged("AssignedBugsButtonText"); });
            _Messenger.Register<BugViewModel>(Messages.SelectedBugDeleted, delegate { OnPropertyChanged("AssignedBugsButtonText"); });
        }


        private void ProjectSaved(ProjectViewModel project)
        {
            if (project.Id == SelectedActiveProject.Id)
                UpdateProjectInComboBox(project);
        }


        private void ShowAssignedBugs()
        {
            if (_ContentPanel.GetType() != typeof(IBugTableViewModel))
                ShowBugTablePanel();

            _Messenger.NotifyColleagues(Messages.ShowAssignedBugs);
        }


        private int GetNumberOfBugsAssignedToUser()
        {
            if (SelectedActiveProject == null)
                return 0;

            return _Service.GetNumberOfBugsAssignedToUserInProject
                (SelectedActiveProject.ToProjectModel(), _CurrentUser);
        }


        private void ProjectDeletedAction(ProjectViewModel project)
        {
            if (ActiveProjectUpdated(project))
            {
                if (ProjectComboBox.IndexOf(SelectedActiveProject) != 0)
                    SelectedActiveProject = ProjectComboBox[0];
                else
                    SelectedActiveProject = ProjectComboBox[1];
            }

            ProjectComboBox.Remove(ProjectComboBox.Where(c => c.Id == project.Id).SingleOrDefault());
        }


        private bool ActiveProjectUpdated(ProjectViewModel project)
        {
            return SelectedActiveProject.Id == project.Id && ProjectComboBox.Count > 1;
        }


        private void UpdateProjectInComboBox(ProjectViewModel proj)
        {
            var project = ProjectComboBox.Where(p => p.Id == proj.Id).SingleOrDefault();

            int index = ProjectComboBox.IndexOf(project);

            ProjectComboBox[index] = proj;
            SelectedActiveProject = ProjectComboBox[index];
        }


        private void ShowProjectManagerPanel()
        {
            ContentPanel = _ControlFactory.CreateProjectManagerPanel(_CurrentUser);

            _Messenger.NotifyColleagues(Messages.BugTableDisplaying, false);
        }


        private void ShowBugTablePanel()
        {
            ContentPanel = null;
            ContentPanel = _ControlFactory.CreateBugTablePanel(_SelectedActiveProject);

            _Messenger.NotifyColleagues(Messages.BugTableDisplaying, true);
        }


        private void ShowAccountSettingsPanel()
        {
            ContentPanel = _ControlFactory.CreateAccountSettingsPanel();

            _Messenger.NotifyColleagues(Messages.BugTableDisplaying, false);
        }


        private void InitialiseActiveProject()
        {
            List<Project> projects = _Service.GetAllProjectsByUser(_CurrentUser);
            
            if (projects.Count > 0 && projects != null)
                SelectedActiveProject = new ProjectViewModel(projects[0]);
        }


        private void Logout()
        {
            _WindowFactory.CreateLoginWindow().Show();
            RequestClose.Invoke(this, null);
        }


        private void Debug()
        {
            MessageBox.Show("d");
        }

    }
}