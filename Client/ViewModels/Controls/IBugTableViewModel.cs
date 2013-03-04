using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Client.ViewModels.Controls
{
    public interface IBugTableViewModel : IContentPanel
    {
        ObservableCollection<BugViewModel> BugList { get; set; }
        void PopulateBugTable();
        string ProjectTitle { get; set; }
        BugViewModel SelectedBug { get; set; }
        List<BugViewModel> SelectedBugs { get; }
        BugPanelViewModel SouthViewPanel { get; set; }
    }
}
