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

    public interface IMainWindow : IWindow { }

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IMainWindow
    {

        public MainWindow(IMainWindowViewModel viewModel)
        {
            if (viewModel == null)
                throw new ArgumentNullException("View model cannot be null.");

            this.DataContext = viewModel;

            viewModel.RequestClose += delegate { this.Close(); };

            InitializeComponent();
        }

    }
}
