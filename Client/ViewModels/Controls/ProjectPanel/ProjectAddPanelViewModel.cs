using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Client.Helpers;
using System.Windows.Input;
using Client.ServiceReference;

namespace Client.ViewModels.Controls.ProjectPanel
{
    public class ProjectAddPanelViewModel : ProjectPanelViewModel
    {

        public ProjectAddPanelViewModel(ITrackerService svc, IMessenger mess)
            : base(svc, mess)
        {
            ButtonName = "Add";
        }


        protected override void SaveProject(ProjectViewModel proj)
        {
            if (IsProjectValid)
            {
                Project savedProject = _Service.AddProject(proj.ToProjectModel());

                _Messenger.NotifyColleagues(Messages.AddedProject, new ProjectViewModel(savedProject));

                IsVisible = false;
            }
        }

    }
}
