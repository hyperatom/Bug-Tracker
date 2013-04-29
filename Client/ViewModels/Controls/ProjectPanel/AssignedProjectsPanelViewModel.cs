using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using Client.Helpers;
using Client.ServiceReference;
using System.Windows.Input;
using System.Windows;

namespace Client.ViewModels.Controls.ProjectPanel
{
    public class AssignedProjectsPanelViewModel : ObservableObject, IAssignedProjectsPanelViewModel
    {

        private ITrackerService _Service;
        private IMessenger _Messenger;
        
        private User _CurrentUser;
        private ProjectViewModel _SelectedProject;

        private bool _IsVisible;

        private ObservableCollection<ProjectViewModel> _AssignedProjects;

        private ICommand _LeaveProjectCommand;


        public AssignedProjectsPanelViewModel(IMessenger mess, ITrackerService svc, User currentUser)
        {
            _Service = svc;
            _Messenger = mess;
            _CurrentUser = currentUser;

            ListenForMessages();

            IsVisible = true;
        }


        #region Commands

        public ICommand LeaveProjectCommand
        {
            get
            {
                if (_LeaveProjectCommand == null)
                {
                    _LeaveProjectCommand = new RelayCommand(p => LeaveProject(), p => SelectedProject != null);
                }

                return _LeaveProjectCommand;
            }
        }

        #endregion Commands


        public bool IsVisible
        {
            get { return _IsVisible; }
            set { _IsVisible = value; OnPropertyChanged("IsVisible"); }
        }


        public ProjectViewModel SelectedProject
        {
            get { return _SelectedProject; }
            set 
            { 
                _SelectedProject = value;

                if (value != null)
                {
                    _Messenger.NotifyColleagues(Messages.AssignedProjectSelected, value);
                }

                OnPropertyChanged("SelectedProject"); 
            }
        }


        public ObservableCollection<ProjectViewModel> AssignedProjects
        {
            get
            {
                if (_AssignedProjects == null)
                {
                    _AssignedProjects = new ObservableCollection<ProjectViewModel>();

                    _Service.GetProjectsAssignedTo(_CurrentUser).ForEach(p => _AssignedProjects.Add(new ProjectViewModel(p)));
                }

                return _AssignedProjects;
            }

            set { _AssignedProjects = value; OnPropertyChanged("AssignedProjects"); }
        }


        private void ListenForMessages()
        {
            _Messenger.Register<ProjectViewModel>(Messages.ManagedProjectSelected, p => SelectedProject = null);
            _Messenger.Register<ProjectViewModel>(Messages.AddedProject, p => AssignedProjects.Add(p));
            _Messenger.Register<ProjectViewModel>(Messages.DeletedProject, p => RemoveProjectFromList(p));
        }


        private void RemoveProjectFromList(ProjectViewModel project)
        {
            AssignedProjects.Remove(AssignedProjects.Where(p => p.Id == project.Id).SingleOrDefault());
        }


        private void LeaveProject()
        {
            try
            {
                _Service.LeaveProject(SelectedProject.ToProjectModel(), _CurrentUser);

                _Messenger.NotifyColleagues(Messages.DeletedProject, SelectedProject);

                AssignedProjects.Remove(SelectedProject);
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

    }
}
