using System;
using System.Windows.Input;

namespace Client.ViewModels
{
    public interface ICommandPanelViewModel
    {
        ICommand AddBugCommand { get; }
        ICommand DeleteSelectedBugsCommand { get; }
        ICommand EditBugCommand { get; }
    }
}
