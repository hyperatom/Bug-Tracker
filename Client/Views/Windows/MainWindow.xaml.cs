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
using Client.ViewModels;
using Client.Views.Controls.Notifications;
using System.ComponentModel;

namespace Client
{

    public interface IMainWindow : IWindow { }

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IMainWindow
    {

        private IGrowlNotifiactions _Notifications;

        private const int NOTIFICATION_OFFSET_TOP = 25;

        private bool _IsShuttingDown = true;

        public MainWindow(IMainWindowViewModel viewModel, IGrowlNotifiactions notify)
        {
            if (viewModel == null)
                throw new ArgumentNullException("View model cannot be null.");

            _Notifications = notify;

            this.DataContext = viewModel;

            viewModel.RequestClose += delegate { _IsShuttingDown = false; this.Close(); };

            InitializeComponent();

            this.LocationChanged += new EventHandler(OnWindowMoved);
            this.Closing += new CancelEventHandler(OnWindowClose);
            this.Closed += OnClosed;
            this.SizeChanged += new SizeChangedEventHandler(OnWindowResized);

            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }


        public void OnWindowResized(object sender, SizeChangedEventArgs e)
        {
            Console.WriteLine("Height: " + e.NewSize.Height);
            Console.WriteLine("Width: " + e.NewSize.Width);
        }


        public void OnWindowMoved(object sender, EventArgs args)
        {
            ((GrowlNotifiactions)_Notifications).Top = this.Top + NOTIFICATION_OFFSET_TOP;
            ((GrowlNotifiactions)_Notifications).Left = this.Left;
        }


        public void OnClosed(object sender, System.EventArgs e)
        {
            if (_IsShuttingDown)
                Application.Current.Shutdown();
        }


        public void OnWindowClose(object sender, EventArgs args)
        {
            ((GrowlNotifiactions)_Notifications).Visibility = System.Windows.Visibility.Collapsed;
        }


    }
}
