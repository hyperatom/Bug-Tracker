using System;
using System.Collections.ObjectModel;
using Client.ServiceReference;

namespace Client.ViewModels.Controls.ProjectPanel
{
    public interface IProjectUsersPanelViewModel
    {
        ObservableCollection<User> UserList { get; set; }
        bool IsDeleteButtonVisible { get; set; }
    }
}
