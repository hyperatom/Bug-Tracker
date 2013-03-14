using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Client.Factories;
using Client.Helpers;
using Client.ServiceReference;
using Client.ViewModels.Controls.ProjectPanel;
using System.Windows;
using System;
using Client.ViewModels.Controls.Dialogs;

namespace Client.ViewModels.Controls
{
    public class ProjectManagerViewModel : ObservableObject, IProjectManagerViewModel
    {

        private ITrackerService _Service;
        private IMessenger _Messenger;
        private IControlFactory _Factory;

        private ICommand _ShowDeleteDialogCommand;
        private ICommand _ViewProjectCommand;
        private ICommand _NewProjectCommand;

        private ObservableCollection<ProjectViewModel> _ManagedProjects;

        private ProjectPanelViewModel _SouthViewPanel;
        private IAssignedProjectsPanelViewModel _AssignedProjectsPanel;
        private IDeleteProjectDialogViewModel _DeleteDialog;

        private User _CurrentUser;


        public ProjectManagerViewModel(ITrackerService svc, IMessenger mess, IControlFactory ctrlFactory, User currentUser)
        {
            _Service = svc;
            _Messenger = mess;
            _Factory = ctrlFactory;
            _CurrentUser = currentUser;

            ListenForMessages();
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


        public IAssignedProjectsPanelViewModel AssignedProjectsPanel
        {
            get 
            {
                if (_AssignedProjectsPanel == null)
                    _AssignedProjectsPanel = _Factory.CreateAssignedProjectsPanel(_CurrentUser);

                return _AssignedProjectsPanel;
            }

            set { _AssignedProjectsPanel = value; OnPropertyChanged("AssignedProjectsPanel"); }
        }


        public IDeleteProjectDialogViewModel DeleteDialog
        {
            get { return _DeleteDialog; }
            set { _DeleteDialog = value; OnPropertyChanged("DeleteDialog"); }
        }


        #region Commands

        public ICommand ShowDeleteDialogCommand
        {
            get
            {
                if (_ShowDeleteDialogCommand == null)
                {
                    _ShowDeleteDialogCommand = new RelayCommand(param => ShowDeleteProjectDialog((ProjectViewModel)param));
                }

                return _ShowDeleteDialogCommand;
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
            _Messenger.Register<ProjectViewModel>(Messages.AddedProject, p => ManagedProjects.Add(p));
            _Messenger.Register<ProjectViewModel>(Messages.RequestDeleteProject, p => DeleteProject(p));

            _Messenger.Register(Messages.CloseDeleteProjectDialog, 
                delegate { DeleteDialog.IsVisible = false; DeleteDialog = null; });
        }


        private void SaveProjectToList(ProjectViewModel project)
        {
            var proj = ManagedProjects.Where(x => x.Id == project.Id).SingleOrDefault();

            int index = ManagedProjects.IndexOf(proj);

            ManagedProjects.RemoveAt(index);
            ManagedProjects.Insert(index, project);
        }


        private void ShowDeleteProjectDialog(ProjectViewModel proj)
        {
            DeleteDialog = _Factory.CreateDeleteProjectDialog(proj);
        }


        private void DeleteProject(ProjectViewModel project)
        {
            DeleteDialog.IsVisible = false;
            DeleteDialog = null;

            try
            {
                _Service.DeleteProject(project.ToProjectModel());
                ManagedProjects.Remove(project);

                _Messenger.NotifyColleagues(Messages.DeletedProject, project);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

    }
}
