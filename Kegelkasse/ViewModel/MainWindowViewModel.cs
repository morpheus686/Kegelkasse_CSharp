using CommunityToolkit.Mvvm.Input;
using Kegelkasse.Components;
using Kegelkasse.Models;
using Kegelkasse.Services;
using Kegelkasse.Services.Interfaces;
using System.Windows.Input;

namespace Kegelkasse.ViewModel
{
    public class MainWindowViewModel : LoadableViewModelBase
    {
        private readonly IDialogService dialogService;
        private readonly StrafenkatalogContext context;

        public ICommand OpenSettingsCommand { get; }
        public ICommand SetGamePlayerTabCommand { get; }
        public ICommand SetStatisticsTabCommand { get; }
        public ICommand AddGameCommand { get; }

        public LoadableViewModelBase? CurrentTab { private set; get; }
        public GamePlayerTabViewModel GamePlayerTabViewModel { get; }

        public MainWindowViewModel(GamePlayerTabViewModel gamePlayerTabViewModel, StrafenkatalogContext context)
        {
            dialogService = new DialogService();
            GamePlayerTabViewModel = gamePlayerTabViewModel;
            this.context = context;
            OpenSettingsCommand = new AsyncRelayCommand(ExecuteOpenSettingsCommand);
            SetGamePlayerTabCommand = new RelayCommand(ExecuteSetGamePlayerCommand);
            SetStatisticsTabCommand = new RelayCommand(ExecuteSetStatisticsTabCommand);
            AddGameCommand = new AsyncRelayCommand(ExecuteAddGameCommand);
        }

        private async Task ExecuteAddGameCommand()
        {
            var addgameViewModel = new AddGameDialogViewModel(context);
            var result = await dialogService.ShowDialog(addgameViewModel);

            if (result == null)
            {
                throw new InvalidOperationException("DialogService konnte kein gültiges Ergebnis für den Dialog liefern");
            }

            if ((DialogResult)result == DialogResult.Yes)
            {
                var newGame = new Game
                {
                    Team = addgameViewModel.SelectedTeam.Id,
                    Date = DateOnly.FromDateTime(addgameViewModel.GameDate),
                    Vs = addgameViewModel.Opponent,
                    Gameday = addgameViewModel.GameNumber,
                    Season = addgameViewModel.SelectedSeason
                };

                foreach (var player in addgameViewModel.SelectedPlayers)
                {
                    var newGamePlayer = new GamePlayer
                    {
                        Player = player.Id,
                        Played = 1
                    };

                    foreach (var teamPenalty in context.TeamPenalties.Where(t => t.Team == addgameViewModel.SelectedTeam.Id))
                    {
                        newGamePlayer.PlayerPenalties.Add(new PlayerPenalty
                        {
                            Penalty = teamPenalty.Penalty,
                            Value = 0
                        });
                    }

                    newGame.GamePlayers.Add(newGamePlayer);
                }

                await context.AddAsync(newGame);
                var affected = await context.SaveChangesAsync();
                await GamePlayerTabViewModel.ReloadGames();
            }
        }

        private void ExecuteSetStatisticsTabCommand()
        {
            CurrentTab = null;
            RaisePropertyChanged(nameof(CurrentTab));
        }

        private void ExecuteSetGamePlayerCommand()
        {
            SetGamePlayerTabViewModel();
        }

        private void SetGamePlayerTabViewModel()
        {
            CurrentTab = GamePlayerTabViewModel;
            RaisePropertyChanged(nameof(CurrentTab));
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
