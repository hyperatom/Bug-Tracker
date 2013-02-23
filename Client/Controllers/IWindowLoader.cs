using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Client.Helpers;

namespace Client.Controllers
{
    public interface IWindowLoader
    {
        void ShowView(IWindow viewModel);
    }
}
