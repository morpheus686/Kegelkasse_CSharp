using CommunityToolkit.Mvvm.Input;
using Kegelkasse.Common.Models;
using Kegelkasse.Common.Services.Interfaces;
using Kegelkasse.Foundation.Enumerations;
using System.Windows.Input;

namespace Kegelkasse.Common.ViewModel
{
    public class MainWindowViewModel : LoadableViewModelBase
    {
        private readonly IDialogService dialogService;
        private readonly KegelkasseContext context;

        public ICommand OpenSettingsCommand { get; }
        public ICommand SetGamePlayerTabCommand { get; }
        public ICommand SetStatisticsTabCommand { get; }
        public ICommand AddGameCommand { get; }

        public LoadableViewModelBase? CurrentTab { private set; get; }
        public GamePlayerTabViewModel GamePlayerTabViewModel { get; }

        public MainWindowViewModel(GamePlayerTabViewModel gamePlayerTabViewModel, KegelkasseContext context, IDialogService dialogService)
        {
            this.dialogService = dialogService;
            GamePlayerTabViewModel = gamePlayerTabViewModel;
            this.context = context;
            OpenSettingsCommand = new AsyncRelayCommand(ExecuteOpenSettingsCommand);
            SetGamePlayerTabCommand = new RelayCommand(ExecuteSetGamePlayerCommand);
            SetStatisticsTabCommand = new RelayCommand(ExecuteSetStatisticsTabCommand);
            AddGameCommand = new AsyncRelayCommand(this.GamePlayerTabViewModel.ExecuteAddGameCommandAsync);
        }

        private void ExecuteSetStatisticsTabCommand()
        {
            CurrentTab = null;
            OnPropertyChanged(nameof(CurrentTab));
        }

        private void ExecuteSetGamePlayerCommand()
        {
            SetGamePlayerTabViewModel();
        }

        private void SetGamePlayerTabViewModel()
        {
            CurrentTab = GamePlayerTabViewModel;
            OnPropertyChanged(nameof(CurrentTab));
        }

        private async Task ExecuteOpenSettingsCommand()
        {
            var vm = new SettingsViewModel();
            await dialogService.ShowDialog(vm);
        }

        protected override void InitializeInternal()
        {
            SetGamePlayerTabViewModel();
        }

        protected async override Task InitializeInternalAsync()
        {
            SetGamePlayerTabViewModel();

            if (CurrentTab != null)
            {
                await CurrentTab.Initialize();
            }
        }
    }
}
