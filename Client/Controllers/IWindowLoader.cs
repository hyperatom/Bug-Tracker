using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Client.ViewModels;

namespace Client.Controllers
{
    public interface IWindowLoader
    {
        void ShowView(IWindow viewModel);
    }
}
