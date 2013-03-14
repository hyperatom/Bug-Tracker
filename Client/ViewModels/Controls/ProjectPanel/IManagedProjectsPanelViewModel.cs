using System;
using System.Windows.Input;
using System.Collections.ObjectModel;
using Client.ViewModels.Controls.Dialogs;

namespace Client.ViewModels.Controls.ProjectPanel
{
    public interface IManagedProjectsPanelViewModel
    {
        ObservableCollection<ProjectViewModel> ManagedProjects { get; set; }
        ICommand NewProjectCommand { get; }
        ICommand ShowDeleteDialogCommand { get; }
        ICommand ViewProjectCommand { get; }
    }
}
