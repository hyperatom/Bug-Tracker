using System;
using System.Collections.ObjectModel;

namespace Client.ViewModels.Controls
{
    public interface IProjectManagerViewModel : IContentPanel
    {
        ObservableCollection<ProjectViewModel> ManagedProjects { get; set; }
    }
}
