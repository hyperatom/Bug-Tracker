using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Client.Views.Windows
{
    public interface IWindow
    {
        object DataContext { get; set; }
        void Show();
    }
}
