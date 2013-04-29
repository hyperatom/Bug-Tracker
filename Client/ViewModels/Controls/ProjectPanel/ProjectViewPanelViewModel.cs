using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Client.Helpers;
using Client.ServiceReference;
using Client.Views.Controls.Notifications;

namespace Client.ViewModels.Controls.ProjectPanel
{
    public class ProjectViewPanelViewModel : ProjectPanelViewModel
    {

        private IGrowlNotifiactions _Notifier;

        public ProjectViewPanelViewModel(ITrackerService svc, IMessenger mess, ProjectViewModel selectedProj, IGrowlNotifiactions notifier)
            : base(svc, mess)
        {
            Project = selectedProj.Clone();

            _Notifier = notifier;

            ButtonName = "Save & Close";
            PanelTitle = "Edit Project";
        }


        protected override void SaveProject(ProjectViewModel project)
        {
            if (IsProjectValid)
            {
                ProjectViewModel savedProject = new ProjectViewModel(_Service.SaveProject(project.ToProjectModel()));

                _Notifier.AddNotification(new Notification
                {
                    ImageUrl = Notification.ICON_EDIT,
                    Title = "Project Saved",
                    Message = "The project " + savedProject.Name + " has been saved."
                });

                _Messenger.NotifyColleagues(Messages.SavedProject, savedProject);

                IsVisible = false;
            }
        }

    }
}
