
using CommunityToolkit.Mvvm.Input;
using Kegelkasse.Common.Models;
using Kegelkasse.Common.Services.Interfaces;
using Kegelkasse.Foundation.Enumerations;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;

namespace Kegelkasse.Common.ViewModel
{
    public class GamePlayerTabViewModel : LoadableViewModelBase
    {
        private const int TeamId = 1;
        private readonly IDialogService _dialogService;
        private double? toPay;
        private double? paid;
        private int? teamErrors;
        private int? teamFull;
        private KegelkasseContext context;
        private Game? currentGame;
        private List<Game> games;
        private int? teamResult;
        private int? teamClear;

        public ObservableCollection<MainDataGridItemViewModel> GridItems { get; set; }
        public MainDataGridItemViewModel? SelectedGridItem { private get; set; }

        public Game? CurrentGame
        {
            get => currentGame;
            set
            {
                SetProperty(ref currentGame, value);

                NextGameCommand.NotifyCanExecuteChanged();
                PreviousGameCommand.NotifyCanExecuteChanged();
            }
        }

        public int? TeamResult
        {
            get { return teamResult; }
            private set 
            { 
                teamResult = value;
                OnPropertyChanged();
            }
        }

        public int? TeamClear
        {
            get { return teamClear; }
            private set 
            { 
                teamClear = value;
                OnPropertyChanged();
            }
        }

        public int? TeamFull
        {
            get { return teamFull; }
            set 
            { 
                teamFull = value;
                OnPropertyChanged();
            }
        }

        public int? TeamErrors
        {
            get { return teamErrors; }
            set 
            { 
                teamErrors = value; 
                OnPropertyChanged();
            }
        }

        public double? ToPay
        {
            get { return toPay; }
            private set 
            { 
                toPay = value; 
                OnPropertyChanged();
            }
        }

        public double? Paid
        {
            get => paid;
            private set
            {
                paid = value;
                OnPropertyChanged();
            }
        }

        public IAsyncRelayCommand PreviousGameCommand { get; }
        public IAsyncRelayCommand NextGameCommand { get; }
        public IAsyncRelayCommand ShowEditPlayerDialogCommand { get; }
        public IAsyncRelayCommand AddGameCommand { get; }

        public GamePlayerTabViewModel(IDialogService dialogService, KegelkasseContext context)
        {
            GridItems = [];
            games = [];
            _dialogService = dialogService;
            this.context = context;
            PreviousGameCommand = new AsyncRelayCommand(ExecutePreviousGameCommand, CanExecutePreviousGameCommand);
            NextGameCommand = new AsyncRelayCommand(ExecuteNextGameCommand, CanExecuteNextGameCommand);
            ShowEditPlayerDialogCommand = new AsyncRelayCommand(ExecuteShowEditPlayerDialogCommand);
            AddGameCommand = new AsyncRelayCommand(ExecuteAddGameCommandAsync);
        }

        private Task ExecuteShowEditPlayerDialogCommand()
        {
            if (SelectedGridItem == null)
            {
                throw new InvalidOperationException("Die Eigenschaft 'SelectedGridItem' darf nicht null sein!");
            }

            return EditPlayer(SelectedGridItem.SumPerPlayer);
        }

        private bool CanExecuteNextGameCommand()
        {
            return CurrentGame != null
                && CurrentGame != games.Last();
        }

        private async Task ExecuteNextGameCommand()
        {
            if (CurrentGame != null)
            {
                var nextIndex = games.IndexOf(CurrentGame) + 1;
                CurrentGame = games[nextIndex];
                await LoadDataAsync();
            }
        }

        private bool CanExecutePreviousGameCommand()
        {
            bool canExecute = CurrentGame != null
                && CurrentGame != games.First();
            return canExecute;
        }

        private async Task ExecutePreviousGameCommand()
        {
            if (CurrentGame != null)
            {
                var previousIndex = games.IndexOf(CurrentGame) - 1;
                CurrentGame = games[previousIndex];
                await LoadDataAsync();
            }
        }

        public async Task EditPlayer(SumPerPlayer? sumPerPlayer)
        {
            if (sumPerPlayer == null)
            {
                throw new ArgumentException("Argument sumPerPlayer darf nicht null sein!");
            }

            var gamePlayer = await context.GamePlayers.FirstAsync(x => x.Player == sumPerPlayer.PlayerId && x.Game == sumPerPlayer.GameId);
            var playerPenalties = context.PlayerPenalties.Where(pp => pp.GamePlayer == gamePlayer.Id).ToList();

            foreach (var item in playerPenalties)
            {
                item.PenaltyNavigation = context.Penalties.Where(p => p.Id == item.Penalty).First();
            }

            gamePlayer.PlayerNavigation = await context.Players.FirstAsync(p => p.Id == gamePlayer.Player);

            var viewModel = new EditPenaltyDialogViewModel(gamePlayer, playerPenalties);
            var result = await _dialogService.ShowDialog(viewModel);

            if (result is DialogResult dialogResult
                && dialogResult == DialogResult.Yes)
            {
                await _dialogService.ShowIndeterminateDialogAsync(async (message, parameter) =>
                {
                    int updated = await context.SaveChangesAsync();
                },
                null);
            }

            await LoadDataAsync();
        }

        protected override Task InitializeInternalAsync()
        {
            return ReloadGames();
        }

        public async Task ReloadGames()
        {
            LoadGames();
            await LoadDataAsync();
        }

        private void LoadGames()
        {
            games = [.. context.Games.Where(g => g.Team == TeamId).OrderBy(g => g.Date)];
            CurrentGame = games.LastOrDefault();
        }

        private async Task LoadDataAsync()
        {
            GridItems.Clear();

            if (CurrentGame != null)
            {
                foreach (var item in context.SumPerPlayers.Where(s => s.GameId == CurrentGame.Id))
                {
                    GridItems.Add(new MainDataGridItemViewModel(this, item));
                }

                var result = await context.ResultOfGames.FirstAsync(r => r.Id == CurrentGame.Id);
                TeamResult = result.TotalResult;
                TeamClear = result.TotalClear;
                TeamFull = result.TotalFull;
                TeamErrors = result.TotalErrors;

                var gamesum = await context.SumPerGames.FirstAsync(s => s.GameId == CurrentGame.Id);
                ToPay = gamesum.PenaltySum;

                //this.Paid = this.context.PaidPerGames.First(p => p.Game == this.CurrentGame.Id).Paid;
            }
        }

        public async Task ExecuteAddGameCommandAsync()
        {
            var addgameViewModel = new AddGameDialogViewModel(context);
            var result = await this._dialogService.ShowDialog(addgameViewModel);

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
                await ReloadGames();
            }
        }
    }
}
