using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Client.Factories;
using Client.Helpers;
using Client.ServiceReference;
using Client.ViewModels.Controls.ProjectPanel;

namespace Client.ViewModels.Controls
{
    public class ProjectManagerViewModel : ObservableObject, IProjectManagerViewModel
    {

        private ITrackerService _Service;
        private IMessenger _Messenger;
        private IControlFactory _Factory;

        private ICommand _DeleteProjectCommand;
        private ICommand _ViewProjectCommand;
        private ICommand _NewProjectCommand;

        private ObservableCollection<ProjectViewModel> _ManagedProjects;
        private ObservableCollection<ProjectViewModel> _AssigedProjects;

        private ProjectPanelViewModel _SouthViewPanel;

        private User _CurrentUser;


        public ProjectManagerViewModel(ITrackerService svc, IMessenger mess, IControlFactory ctrlFactory, User currentUser)
        {
            _Service = svc;
            _Messenger = mess;
            _Factory = ctrlFactory;
            _CurrentUser = currentUser;

            ListenForMessages();
        }


        public ObservableCollection<ProjectViewModel> AssigedProjects
        {
            get 
            {
                if (_AssigedProjects == null)
                {
                    _AssigedProjects = new ObservableCollection<ProjectViewModel>();

                    _Service.GetProjectsAssignedTo(_CurrentUser).ForEach(p => _AssigedProjects.Add(new ProjectViewModel(p)));
                }

                return _AssigedProjects; 
            }

            set { _AssigedProjects = value; OnPropertyChanged("AssignedProjects"); }
        }


        public ObservableCollection<ProjectViewModel> ManagedProjects
        {
            get 
            {
                if (_ManagedProjects == null)
                {
                    _ManagedProjects = new ObservableCollection<ProjectViewModel>();

                    _Service.GetProjectsManagedBy(_CurrentUser).ForEach(p => _ManagedProjects.Add(new ProjectViewModel(p)));
                }

                return _ManagedProjects;
            }

            set { _ManagedProjects = value; OnPropertyChanged("ManagedProjects"); }
        }


        public ProjectPanelViewModel SouthViewPanel
        {
            get { return _SouthViewPanel; }
            set { _SouthViewPanel = value; OnPropertyChanged("SouthViewPanel"); }
        }


        #region Commands

        public ICommand DeleteProjectCommand
        {
            get
            {
                if (_DeleteProjectCommand == null)
                {
                    _DeleteProjectCommand = new RelayCommand(param => DeleteProject((ProjectViewModel)param));
                }

                return _DeleteProjectCommand;
            }
        }

        public ICommand ViewProjectCommand
        {
            get
            {
                if (_ViewProjectCommand == null)
                {
                    _ViewProjectCommand = new RelayCommand
                        (param => SouthViewPanel = _Factory.CreateProjectViewPanel((ProjectViewModel)param));
                }

                return _ViewProjectCommand;
            }
        }

        public ICommand NewProjectCommand
        {
            get
            {
                if (_NewProjectCommand == null)
                {
                    _NewProjectCommand = new RelayCommand(param => SouthViewPanel = _Factory.CreateProjectAddPanel());
                }

                return _NewProjectCommand;
            }
        }

        #endregion Commands


        private void ListenForMessages()
        {
            _Messenger.Register<ProjectViewModel>(Messages.SavedProject, p => SaveProjectToList(p));
            _Messenger.Register<ProjectViewModel>(Messages.AddedProject, p => AddProjectToList(p));
        }


        private void SaveProjectToList(ProjectViewModel project)
        {
            var proj = ManagedProjects.Where(x => x.Id == project.Id).SingleOrDefault();

            int index = ManagedProjects.IndexOf(proj);

            ManagedProjects.RemoveAt(index);
            ManagedProjects.Insert(index, project);
        }


        private void AddProjectToList(ProjectViewModel project)
        {
            ManagedProjects.Add(project);
        }


        private void DeleteProject(ProjectViewModel proj)
        {
            SouthViewPanel = _Factory.CreateProjectAddPanel();
        }

    }
}
