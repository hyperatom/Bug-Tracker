using System;
using Client.ViewModels;
using Client.ViewModels.Controls;
using Client.ServiceReference;
using Client.ViewModels.Controls.ProjectPanel;

namespace Client.Factories
{
    public interface IControlFactory
    {
        BugPanelViewModel CreateBugAddPanel(ProjectViewModel project);
        BugPanelViewModel CreateBugViewPanel(ProjectViewModel project, BugViewModel bug);
        IBugTableViewModel CreateBugTablePanel(ProjectViewModel project);
        ICommandPanelViewModel CreateCommandPanel(ProjectViewModel project);
        IProjectManagerViewModel CreateProjectManagerPanel(User currentUser);
        ProjectPanelViewModel CreateProjectAddPanel();
        ProjectPanelViewModel CreateProjectViewPanel(ProjectViewModel vm);
    }
}
