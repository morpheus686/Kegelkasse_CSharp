using Kegelkasse.Base.ViewModel;
using UraniumUI.Pages;

namespace Kegelkasse.MAUI
{
    public partial class MainPage : UraniumContentPage
    {
        private MainWindowViewModel ViewModel { get; }


        public MainPage(MainWindowViewModel mainWindowViewModel)
        {
            InitializeComponent();
            this.ViewModel = mainWindowViewModel;
            this.BindingContext = this.ViewModel;

            this.Loaded += MainWindow_Loaded;
        }

        private async void MainWindow_Loaded(object? sender, EventArgs e)
        {
            await this.ViewModel.Initialize();
        }
    }
}
