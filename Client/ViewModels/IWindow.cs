using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Client.Controllers;

namespace Client.Helpers
{
    public interface IWindow
    {
        EventHandler RequestClose { get; set; }
    }
}
