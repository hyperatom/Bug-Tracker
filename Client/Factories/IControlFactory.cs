using System;
using Client.ViewModels;
using Client.ViewModels.Controls;
using Client.ServiceReference;
using Client.ViewModels.Controls.ProjectPanel;
using Client.ViewModels.Controls.Dialogs;

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
        IDeleteProjectDialogViewModel CreateDeleteProjectDialog(ProjectViewModel vm);
        IAssignedProjectsPanelViewModel CreateAssignedProjectsPanel(User currentUser);
        IManagedProjectsPanelViewModel CreateManagedProjectsPanel(User currentUser);
        IProjectUsersPanelViewModel CreateProjectUsersPanel(ProjectViewModel proj);
    }
}
