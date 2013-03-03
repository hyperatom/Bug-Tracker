using System;
using Client.ViewModels;
using Client.ViewModels.Controls;

namespace Client.Factories
{
    public interface IControlFactory
    {
        BugPanelViewModel CreateBugAddPanel(ProjectViewModel project);
        BugPanelViewModel CreateBugViewPanel(ProjectViewModel project, BugViewModel bug);
        IBugTableViewModel CreateBugTablePanel(ProjectViewModel project);
        ICommandPanelViewModel CreateCommandPanel(ProjectViewModel project);
    }
}
