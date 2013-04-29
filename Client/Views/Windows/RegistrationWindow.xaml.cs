using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Client.Views.Windows;
using Client.ViewModels.Windows;

namespace Client
{

    public interface IRegistrationWindow : IWindow { }

    /// <summary>
    /// Interaction logic for RegistrationWindow.xaml
    /// </summary>
    public partial class RegistrationWindow : Window, IRegistrationWindow
    {

        private bool _IsShuttingDown = true;

        public RegistrationWindow(IRegistrationViewModel viewModel)
        {
            if (viewModel == null)
                throw new ArgumentNullException("View model cannot be null.");

            this.DataContext = viewModel;

            viewModel.RequestClose += delegate { _IsShuttingDown = false; this.Close(); };
            this.Closed += OnClosed;

            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;

            InitializeComponent();
        }


        public void OnClosed(object sender, System.EventArgs e)
        {
            if (_IsShuttingDown)
                Application.Current.Shutdown();
        }

    }

}
