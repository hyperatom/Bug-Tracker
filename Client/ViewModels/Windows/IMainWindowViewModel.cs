using System.Collections.ObjectModel;
using System.Windows.Input;
using Client.Helpers;
using Client.ViewModels.Controls;

namespace Client.ViewModels.Windows
{
    public interface IMainWindowViewModel : IWindow
    {
        IBugTableViewModel BugTablePanel { get; set; }
        ICommandPanelViewModel CommandPanel { get; set; }
        ICommand DeBugCommand { get; }

        ObservableCollection<ProjectViewModel> ProjectComboBox { get; set; }
        ProjectViewModel SelectedActiveProject { get; set; }
        string Username { get; set; }
    }
}
