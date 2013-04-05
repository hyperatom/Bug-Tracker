using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Client.ViewModels.Controls
{
    public interface IBugTableViewModel : IContentPanel
    {
        string ProjectTitle { get; set; }
        BugViewModel SelectedBug { get; set; }
        List<BugViewModel> SelectedBugs { get; }
        BugPanelViewModel SouthViewPanel { get; set; }
        ICollectionView MyBugList { get; }
    }
}
