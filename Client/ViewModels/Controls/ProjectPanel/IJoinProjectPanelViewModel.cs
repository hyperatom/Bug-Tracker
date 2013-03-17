using System;
using System.Collections.Generic;
using Client.ServiceReference;
using System.Windows.Input;

namespace Client.ViewModels.Controls.ProjectPanel
{
    public interface IJoinProjectPanelViewModel
    {
        String Code { get; set; }
        ICommand JoinProjectCommand { get; }
        IList<Role> RoleList { get; set; }
        Role SelectedRole { get; set; }
    }
}
