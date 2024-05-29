using AsyncAwaitBestPractices.MVVM;
using Strafenkatalog.Components;
using Strafenkatalog.Models;
using Strafenkatalog.Services;
using Strafenkatalog.Services.Interfaces;
using System.Windows.Input;

namespace Strafenkatalog.ViewModel
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly IDialogService dialogService;
        private StrafenkatalogContext context;

        public ICommand OpenSettingsCommand { get; }    
        public ICommand SetGamePlayerTabCommand {  get; }
        public ICommand SetStatisticsTabCommand { get; }
        public ViewModelBase? CurrentTab { private set; get; }

        public MainWindowViewModel()
        {
            this.dialogService = new DialogService();
            this.context = new StrafenkatalogContext();
            this.OpenSettingsCommand = new AsyncCommand(ExecuteOpenSettingsCommand);

            this.SetGamePlayerTabCommand = new RelayCommand(ExecuteSetGamePlayerCommand);
            this.SetStatisticsTabCommand = new RelayCommand(ExecuteSetStatisticsTabCommand);
        }

        private void ExecuteSetStatisticsTabCommand()
        {
            this.CurrentTab = null;
            RaisePropertyChanged(nameof(CurrentTab));
        }

        private void ExecuteSetGamePlayerCommand()
        {
            SetGamePlayerTabViewModel();
        }

        private void SetGamePlayerTabViewModel()
        {
            this.CurrentTab = new GamePlayerTabViewModel(this.dialogService, this.context);
            RaisePropertyChanged(nameof(CurrentTab));
        }

        private async Task ExecuteOpenSettingsCommand()
        {
            var vm = new SettingsViewModel();
            await dialogService.ShowDialog(vm);
        }

        internal void Initialize()
        {
            SetGamePlayerTabViewModel();
        }
    }
}
