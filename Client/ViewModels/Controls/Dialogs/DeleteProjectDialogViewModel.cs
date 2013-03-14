using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Client.Helpers;

namespace Client.ViewModels.Controls.Dialogs
{
    public class DeleteProjectDialogViewModel : ObservableObject, IDeleteProjectDialogViewModel
    {

        private IMessenger _Messenger;

        private ICommand _YesDeleteProjectCommand;
        private ICommand _NoDeleteProjectCommand;

        private bool _IsVisible;
        private String _DeleteMessage;
        private ProjectViewModel _ProjectToDelete;


        public DeleteProjectDialogViewModel(IMessenger mess, ProjectViewModel proj)
        {
            _Messenger = mess;
            ProjectToDelete = proj;

            DeleteMessage = "Are you sure you want to delete project " + proj.Name +
                            " and all of it's associated bugs?";

            IsVisible = true;
        }


        public bool IsVisible
        {
            get { return _IsVisible; }
            set { _IsVisible = value; OnPropertyChanged("IsDeleteDialogVisible"); }
        }


        public String DeleteMessage
        {
            get { return _DeleteMessage; }
            set { _DeleteMessage = value; OnPropertyChanged("DeleteMessage"); }
        }


        public ProjectViewModel ProjectToDelete
        {
            get { return _ProjectToDelete; }
            set { _ProjectToDelete = value; OnPropertyChanged("ProjectToDelete"); }
        }


        #region Commands

        public ICommand YesDeleteProjectCommand
        {
            get
            {
                if (_YesDeleteProjectCommand == null)
                {
                    _YesDeleteProjectCommand = new RelayCommand(param => SendDeleteProjectRequest((ProjectViewModel)param));
                }

                return _YesDeleteProjectCommand;
            }
        }

        public ICommand NoDeleteProjectCommand
        {
            get
            {
                if (_NoDeleteProjectCommand == null)
                {
                    _NoDeleteProjectCommand = new RelayCommand(param => _Messenger.NotifyColleagues(Messages.CloseDeleteProjectDialog));
                }

                return _NoDeleteProjectCommand;
            }
        }

        #endregion Commands


        private void SendDeleteProjectRequest(ProjectViewModel project)
        {
            _Messenger.NotifyColleagues(Messages.RequestDeleteProject, project);
        }

    }
}
