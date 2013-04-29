using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Client.Helpers;
using System.Windows.Input;
using Client.ServiceReference;
using Client.Views.Controls.Notifications;

namespace Client.ViewModels.Controls.ProjectPanel
{
    public class ProjectAddPanelViewModel : ProjectPanelViewModel
    {

        private IGrowlNotifiactions _Notifier;

        public ProjectAddPanelViewModel(ITrackerService svc, IMessenger mess, IGrowlNotifiactions notifier)
            : base(svc, mess)
        {
            _Notifier = notifier;

            ButtonName = "Add & Close";
            PanelTitle = "Add Project";
        }


        protected override void SaveProject(ProjectViewModel proj)
        {
            if (IsProjectValid)
            {
                Project savedProject = _Service.AddProject(proj.ToProjectModel());

                _Notifier.AddNotification(new Notification
                {
                    ImageUrl = Notification.ICON_ADD,
                    Title = "Project Added",
                    Message = "The project " + savedProject.Name + " has been added."
                });

                _Messenger.NotifyColleagues(Messages.AddedProject, new ProjectViewModel(savedProject));

                IsVisible = false;
            }
        }

    }
}
