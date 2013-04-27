using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Client.Helpers;
using Client.ServiceReference;

namespace Client.ViewModels.Controls.ProjectPanel
{
    public class ProjectViewPanelViewModel : ProjectPanelViewModel
    {

        public ProjectViewPanelViewModel(ITrackerService svc, IMessenger mess, ProjectViewModel selectedProj)
            : base(svc, mess)
        {
            Project = selectedProj.Clone();

            ButtonName = "Save & Close";
            PanelTitle = "Edit Project";
        }


        protected override void SaveProject(ProjectViewModel project)
        {
            if (IsProjectValid)
            {
                ProjectViewModel savedProject = new ProjectViewModel(_Service.SaveProject(project.ToProjectModel()));

                _Messenger.NotifyColleagues(Messages.SavedProject, savedProject);

                IsVisible = false;
            }
        }

    }
}
