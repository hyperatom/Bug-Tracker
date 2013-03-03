using System;

namespace Client.Helpers
{
    /// <summary>
    /// Interface defining the operations of a window.
    /// </summary>
    public interface IWindow
    {
        EventHandler RequestClose { get; set; }
    }
}
