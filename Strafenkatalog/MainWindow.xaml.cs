using Strafenkatalog.ViewModel;
using System.Windows;

namespace Strafenkatalog
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (this.DataContext is MainWindowViewModel mwvm)
            {
                mwvm.Load();
            }
        }
    }
}