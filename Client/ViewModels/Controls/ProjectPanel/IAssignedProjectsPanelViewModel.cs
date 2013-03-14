using System;
using System.Collections.ObjectModel;

namespace Client.ViewModels.Controls.ProjectPanel
{
    public interface IAssignedProjectsPanelViewModel
    {
        ObservableCollection<ProjectViewModel> AssignedProjects { get; set; }
        bool IsVisible { get; set; }
    }
}
