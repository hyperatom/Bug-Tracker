using System;
using System.Windows.Input;

namespace Client.ViewModels.Controls.Dialogs
{
    public interface IDeleteProjectDialogViewModel
    {
        string DeleteMessage { get; set; }
        bool IsVisible { get; set; }
        ICommand NoDeleteProjectCommand { get; }
        ProjectViewModel ProjectToDelete { get; set; }
        ICommand YesDeleteProjectCommand { get; }
    }
}
