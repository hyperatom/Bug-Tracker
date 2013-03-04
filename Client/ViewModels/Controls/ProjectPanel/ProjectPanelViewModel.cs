using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Client.ServiceReference;
using Client.Helpers;
using System.Windows.Input;

namespace Client.ViewModels.Controls.ProjectPanel
{
    public abstract class ProjectPanelViewModel : ObservableObject
    {

        private ProjectViewModel _Project;

        private String _ButtonName;
        private bool _IsVisible = true;

        protected IMessenger _Messenger;
        protected ITrackerService _Service;

        private ICommand _SaveProjectCommand;


        public ProjectPanelViewModel(ITrackerService svc, IMessenger mess)
        {
            _Messenger = mess;
            _Service = svc;

            Project = new ProjectViewModel();
        }


        #region Commands

        public ICommand SaveProjectCommand
        {
            get
            {
                if (_SaveProjectCommand == null)
                {
                    _SaveProjectCommand = new RelayCommand(param => SaveProject((ProjectViewModel)param));
                }

                return _SaveProjectCommand;
            }
        }

        #endregion Commands


        public bool IsVisible
        {
            get { return _IsVisible; }
            set { _IsVisible = value; OnPropertyChanged("IsVisible"); }
        }


        public ProjectViewModel Project
        {
            get { return _Project; }
            set { _Project = value; OnPropertyChanged("Project"); }
        }


        public String ButtonName
        {
            get { return _ButtonName; }
            set { _ButtonName = value; OnPropertyChanged("ButtonName"); }
        }


        protected bool IsProjectValid
        {
            get
            {
                Project.IsValidating = true;

                OnPropertyChanged("Project");

                Project.IsValidating = false;

                return (Project.Errors.Count == 0);
            }
        }


        protected abstract void SaveProject(ProjectViewModel project);

    }
}
