using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using Client.Controllers;
using Client.Helpers;
using Client.ServiceReference;
using Client.ViewModels;

namespace Client.ViewModels
{
    /// <summary>
    /// This class is the view model which controls the main bug
    /// view window.
    /// </summary>
    public class MainWindowViewModel : ObservableObject, IWindow
    {

        private IMessenger _Messenger;
        private ITrackerService _Service;

        private String                _Username;
        private ProjectViewModel      _SelectedActiveProject;
        private CommandPanelViewModel _CommandPanel;
        private BugTableViewModel     _BugTablePage;

        private ObservableCollection<ProjectViewModel> _ProjectComboBox;

        private RelayCommand _DeBugCommand;


        /// <summary>
        /// Inherits constructor from base class.
        /// </summary>
        public MainWindowViewModel(IMessenger comm, ITrackerService svc)
        {
            _Messenger = comm;
            _Service = svc;

            Username = _Service.GetMyUser().FirstName;
            
        }


        public EventHandler RequestClose { get; set; }


        public String Username
        {
            get { return _Username; }
            set { _Username = value; }
        }


        public BugTableViewModel BugTablePage
        {
            get
            {
                if (_BugTablePage == null)
                {
                    _BugTablePage = new BugTableViewModel(_Messenger, _Service, _SelectedActiveProject);
                }

                return _BugTablePage; 
            }

            set { _BugTablePage = value; }
        }


        public CommandPanelViewModel CommandPanel
        {
            get 
            {
                if (_CommandPanel == null)
                {
                    _CommandPanel = new CommandPanelViewModel(_Messenger, _Service, _SelectedActiveProject);
                }

                return _CommandPanel;
            }

            set { _CommandPanel = value; OnPropertyChanged("CommandPanelViewModel"); }
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
                    _ProjectComboBox = ProjectModelToViewModel(_Service.GetMyProjects().ToList());
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
                _Messenger.NotifyColleagues(Messages.ActiveProjectChanged, value);
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

    }
}
