using System;
using System.Windows.Input;
using System.Collections.ObjectModel;
using Client.ViewModels.Controls.DTOs;

namespace Client.ViewModels.Controls.ProjectPanel
{
    public interface IProjectUsersPanelViewModel
    {
        ICommand AcceptUserCommand { get; }
        ObservableCollection<UserViewModel> AssignedUsersList { get; set; }
        bool IsAssignedUserButtonsVisible { get; set; }
        bool IsPendingUserButtonsVisible { get; set; }
        ObservableCollection<UserViewModel> PendingUsersList { get; set; }
        ObservableCollection<UserViewModel> ProjectManagersList { get; set; }
        ICommand RejectUserCommand { get; }
        ICommand RemoveUserCommand { get; }
    }
}
