
using CommunityToolkit.Mvvm.Input;
using Kegelkasse.Base.Models;
using Kegelkasse.Base.Services.Interfaces;
using Kegelkasse.Foundation.Enumerations;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Kegelkasse.Base.ViewModel
{
    public class GamePlayerTabViewModel : LoadableViewModelBase
    {
        private const int TeamId = 1;
        private readonly IDialogService _dialogService;
        private double? toPay;
        private double? paid;
        private int? teamErrors;
        private int? teamFull;
        private StrafenkatalogContext context;
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
                currentGame = value;
                RaisePropertyChanged();

                //NextGameCommand.NotifyCanExecuteChanged();
                //PreviousGameCommand.NotifyCanExecuteChanged();
            }
        }

        public int? TeamResult
        {
            get { return teamResult; }
            private set 
            { 
                teamResult = value;
                RaisePropertyChanged();
            }
        }

        public int? TeamClear
        {
            get { return teamClear; }
            private set 
            { 
                teamClear = value;
                RaisePropertyChanged();
            }
        }

        public int? TeamFull
        {
            get { return teamFull; }
            set 
            { 
                teamFull = value;
                RaisePropertyChanged();
            }
        }

        public int? TeamErrors
        {
            get { return teamErrors; }
            set 
            { 
                teamErrors = value; 
                RaisePropertyChanged();
            }
        }

        public double? ToPay
        {
            get { return toPay; }
            private set 
            { 
                toPay = value; 
                RaisePropertyChanged();
            }
        }

        public double? Paid
        {
            get => paid;
            private set
            {
                paid = value;
                RaisePropertyChanged();
            }
        }

        public ICommand PreviousGameCommand { get; }
        public ICommand NextGameCommand { get; }
        public ICommand ShowEditPlayerDialogCommand { get; }

        public GamePlayerTabViewModel(IDialogService dialogService, StrafenkatalogContext context)
        {
            GridItems = [];
            games = [];
            _dialogService = dialogService;
            this.context = context;
            PreviousGameCommand = new AsyncRelayCommand(ExecutePreviousGameCommand, CanExecutePreviousGameCommand);
            NextGameCommand = new AsyncRelayCommand(ExecuteNextGameCommand, CanExecuteNextGameCommand);
            ShowEditPlayerDialogCommand = new AsyncRelayCommand(ExecuteShowEditPlayerDialogCommand);            
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

            var viewModel = new EditPenaltyViewModel(gamePlayer, playerPenalties);
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
    }
}
