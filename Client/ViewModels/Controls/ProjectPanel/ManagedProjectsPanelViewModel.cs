using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Client.ServiceReference;
using Client.Helpers;
using Client.Factories;
using System.Collections.ObjectModel;
using Client.ViewModels.Controls.Dialogs;
using System.Windows.Input;
using System.Windows;

namespace Client.ViewModels.Controls.ProjectPanel
{
    public class ManagedProjectsPanelViewModel : ObservableObject, IManagedProjectsPanelViewModel
    {

        private ITrackerService _Service;
        private IMessenger _Messenger;
        private IControlFactory _Factory;
        private User _CurrentUser;

        private ICommand _NewProjectCommand;
        private ICommand _ShowDeleteDialogCommand;
        private ICommand _ViewProjectCommand;
        private ICommand _SelectionChangedCommand;

        private ProjectViewModel _SelectedProject;

        private ObservableCollection<ProjectViewModel> _ManagedProjects;


        public ManagedProjectsPanelViewModel(ITrackerService svc, IMessenger mess, 
                                             IControlFactory ctrlFactory, User currentUser)
        {
            _Service = svc;
            _Messenger = mess;
            _Factory = ctrlFactory;
            _CurrentUser = currentUser;

            ListenForMessages();
        }


        public ProjectViewModel SelectedProject
        {
            get { return _SelectedProject; }
            set 
            { 
                _SelectedProject = value;
                _Messenger.NotifyColleagues(Messages.ProjectSelected, value);
                OnPropertyChanged("SelectedProject");
            }
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


        #region Commands

        public ICommand ViewProjectCommand
        {
            get
            {
                if (_ViewProjectCommand == null)
                {
                    _ViewProjectCommand = new RelayCommand
                        (p => ShowProjectViewPanel((ProjectViewModel)p));
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
                    _NewProjectCommand = new RelayCommand
                        (p => _Messenger.NotifyColleagues(Messages.ShowProjectAddPanel));
                }

                return _NewProjectCommand;
            }
        }

        public ICommand ShowDeleteDialogCommand
        {
            get
            {
                if (_ShowDeleteDialogCommand == null)
                {
                    _ShowDeleteDialogCommand = new RelayCommand(p => _Messenger.NotifyColleagues(Messages.ShowProjectDeleteDialog, (ProjectViewModel)p));
                }

                return _ShowDeleteDialogCommand;
            }
        }

        #endregion Commands


        private void ListenForMessages()
        {
            _Messenger.Register<ProjectViewModel>(Messages.RequestDeleteProject, p => DeleteProject(p));  
            _Messenger.Register<ProjectViewModel>(Messages.SavedProject, p => SaveProjectToList(p));
            _Messenger.Register<ProjectViewModel>(Messages.AddedProject, p => ManagedProjects.Add(p));         
        }


        private void ShowProjectViewPanel(ProjectViewModel project)
        {
            _Messenger.NotifyColleagues(Messages.ShowProjectViewPanel, (ProjectViewModel)project);
            _Messenger.NotifyColleagues(Messages.ProjectSelected, (ProjectViewModel)project);
        }


        private void SaveProjectToList(ProjectViewModel project)
        {
            var proj = ManagedProjects.Where(x => x.Id == project.Id).SingleOrDefault();

            int index = ManagedProjects.IndexOf(proj);

            ManagedProjects.RemoveAt(index);
            ManagedProjects.Insert(index, project);
        }


        private void DeleteProject(ProjectViewModel project)
        {
            try
            {
                _Service.DeleteProject(project.ToProjectModel());
                ManagedProjects.Remove(project);

                _Messenger.NotifyColleagues(Messages.DeletedProject, project);
                _Messenger.NotifyColleagues(Messages.CloseDeleteProjectDialog);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

    }
}
