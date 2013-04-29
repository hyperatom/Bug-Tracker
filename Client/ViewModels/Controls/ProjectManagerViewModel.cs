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

        private IMessenger _Messenger;
        private IControlFactory _Factory;

        private ProjectPanelViewModel _SouthViewPanel;

        private IAssignedProjectsPanelViewModel _AssignedProjectsPanel;
        private IManagedProjectsPanelViewModel _ManagedProjectsPanel;
        private IProjectUsersPanelViewModel _ProjectUsersPanel;
        private IJoinProjectPanelViewModel _QuickJoinPanel;

        private IDeleteProjectDialogViewModel _DeleteDialog;
        
        private User _CurrentUser;


        public ProjectManagerViewModel(IMessenger mess, IControlFactory ctrlFactory, User currentUser)
        {
            _Messenger = mess;
            _Factory = ctrlFactory;
            _CurrentUser = currentUser;

            ListenForMessages();
        }


        public IDeleteProjectDialogViewModel DeleteDialog
        {
            get { return _DeleteDialog; }
            set { _DeleteDialog = value; OnPropertyChanged("DeleteDialog"); }
        }


        public ProjectPanelViewModel SouthViewPanel
        {
            get { return _SouthViewPanel; }
            set { _SouthViewPanel = value; OnPropertyChanged("SouthViewPanel"); }
        }


        public IJoinProjectPanelViewModel QuickJoinPanel
        {
            get 
            {
                if (_QuickJoinPanel == null)
                    _QuickJoinPanel = _Factory.CreateJoinProjectPanel();

                return _QuickJoinPanel;
            }

            set { _QuickJoinPanel = value; OnPropertyChanged("QuickJoinPanel"); }
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


        public IManagedProjectsPanelViewModel ManagedProjectsPanel
        {
            get 
            {
                if (_ManagedProjectsPanel == null)
                    _ManagedProjectsPanel = _Factory.CreateManagedProjectsPanel(_CurrentUser);

                return _ManagedProjectsPanel;
            }

            set { _ManagedProjectsPanel = value; OnPropertyChanged("ManagedProjectsPanel"); }
        }


        public IProjectUsersPanelViewModel ProjectUsersPanel
        {
            get 
            {
                if (_ProjectUsersPanel == null)
                    _ProjectUsersPanel = _Factory.CreateProjectUsersPanel(null);

                return _ProjectUsersPanel;
            }

            set { _ProjectUsersPanel = value; }
        }


        private void ListenForMessages()
        {
            _Messenger.Register(Messages.ShowProjectAddPanel, ShowProjectAddPanel);

            _Messenger.Register<ProjectViewModel>(Messages.ShowProjectViewPanel, 
                            p => ShowProjectViewPanel((ProjectViewModel)p));

            _Messenger.Register(Messages.CloseDeleteProjectDialog,
                            delegate { DeleteDialog.IsVisible = false; DeleteDialog = null; });

            _Messenger.Register<ProjectViewModel>(Messages.ShowProjectDeleteDialog, 
                p => ShowDeleteProjectDialog((ProjectViewModel)p));
            
        }


        private void ShowDeleteProjectDialog(ProjectViewModel proj)
        {
            DeleteDialog = _Factory.CreateDeleteProjectDialog(proj);
        }


        private void ShowProjectViewPanel(ProjectViewModel project)
        {
            SouthViewPanel = _Factory.CreateProjectViewPanel(project);
        }


        private void ShowProjectAddPanel()
        {
            SouthViewPanel = _Factory.CreateProjectAddPanel();
        }

    }
}
