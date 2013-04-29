using System.Windows;
using Client.ViewModels.Windows;
using Client.Views.Windows;
using System;
using Client.ViewModels;

namespace Client
{

    public interface ILoginWindow : IWindow { }

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window, ILoginWindow
    {

        private bool _IsShuttingDown = true;

        public LoginWindow(ILoginViewModel viewModel)
        {
            if (viewModel == null)
                throw new ArgumentNullException("View model cannot be null.");

            this.DataContext = viewModel;

            viewModel.RequestClose += delegate { _IsShuttingDown = false; this.Close(); };

            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            this.Closing += OnClosed;

            InitializeComponent();
        }


        public void OnClosed(object sender, System.EventArgs e)
        {
            if (_IsShuttingDown)
                Application.Current.Shutdown();
        }

    }
}
