using System;
using System.Collections.ObjectModel;
using Client.ServiceReference;
using Client.ViewModels.Controls.DTOs;

namespace Client.ViewModels.Controls.ProjectPanel
{
    public interface IProjectUsersPanelViewModel
    {
        ObservableCollection<UserViewModel> UsersInProjectList { get; set; }
        bool IsDeleteButtonVisible { get; set; }
    }
}
