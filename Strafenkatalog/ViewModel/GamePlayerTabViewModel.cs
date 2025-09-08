using AsyncAwaitBestPractices.MVVM;
using Microsoft.EntityFrameworkCore;
using Strafenkatalog.Components;
using Strafenkatalog.Models;
using Strafenkatalog.Services.Interfaces;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Strafenkatalog.ViewModel
{
    public class GamePlayerTabViewModel : LoadableViewModel
    {
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
            this.PreviousGameCommand = new RelayCommand(ExecutePreviousGameCommand, CanExecutePreviousGameCommand);
            this.NextGameCommand = new RelayCommand(ExecuteNextGameCommand, CanExecuteNextsGameCommand);
            this.ShowEditPlayerDialogCommand = new AsyncCommand(ExecuteShowEditPlayerDialogCommand);            
        }

        private Task ExecuteShowEditPlayerDialogCommand()
        {
            if (SelectedGridItem == null)
            {
                throw new InvalidOperationException("Die Eigenschaft 'SelectedGridItem' darf nicht null sein!");
            }

            return EditPlayer(SelectedGridItem.SumPerPlayer);
        }

        private bool CanExecuteNextsGameCommand()
        {
            return this.CurrentGame != null
                && this.CurrentGame != this.games.Last();
        }

        private void ExecuteNextGameCommand()
        {
            if (this.CurrentGame != null)
            {
                var nextIndex = games.IndexOf(this.CurrentGame) + 1;
                this.CurrentGame = this.games[nextIndex];
                LoadData();
            }
        }

        private bool CanExecutePreviousGameCommand()
        {
            return this.CurrentGame != null
                && this.CurrentGame != this.games.First();
        }

        private void ExecutePreviousGameCommand()
        {
            if (this.CurrentGame != null)
            {
                var previousIndex = games.IndexOf(this.CurrentGame) - 1;
                this.CurrentGame = this.games[previousIndex];
                LoadData();
            }
        }

        public async Task EditPlayer(SumPerPlayer? sumPerPlayer)
        {
            if (sumPerPlayer == null)
            {
                throw new ArgumentException("Argument sumPerPlayer darf nicht null sein!");
            }

            var gamePlayer = await this.context.GamePlayers.FirstAsync(x => x.Player == sumPerPlayer.PlayerId && x.Game == sumPerPlayer.GameId);
            var playerPenalties = this.context.PlayerPenalties.Where(pp => pp.GamePlayer == gamePlayer.Id).ToList();

            foreach (var item in playerPenalties)
            {
                item.PenaltyNavigation = this.context.Penalties.Where(p => p.Id == item.Penalty).First();
            }

            gamePlayer.PlayerNavigation = await this.context.Players.FirstAsync(p => p.Id == gamePlayer.Player);

            var viewModel = new EditPenaltyViewModel(gamePlayer, playerPenalties);
            var result = await _dialogService.ShowDialog(viewModel);

            if (result is DialogResult dialogResult
                && dialogResult == DialogResult.Yes)
            {
                await _dialogService.ShowIndeterminateDialog(async vm =>
                {
                    int updated = await this.context.SaveChangesAsync();
                });
            }
            else
            {
                await this.context.DisposeAsync();
                this.context = new StrafenkatalogContext();
            }

            LoadData();
        }

        protected override void InitializeInternal()
        {
            this.games = [.. this.context.Games.Where(g => g.Team == 1)];
            this.CurrentGame = games.LastOrDefault();

            LoadData();
        }

        private void LoadData()
        {
            GridItems.Clear();

            if (this.CurrentGame != null)
            {
                foreach (var item in this.context.SumPerPlayers.Where(s => s.GameId == this.CurrentGame.Id))
                {
                    GridItems.Add(new MainDataGridItemViewModel(this, item));
                }

                var result = this.context.ResultOfGames.First(r => r.Id == this.CurrentGame.Id);
                this.TeamResult = result.TotalResult;
                this.TeamClear = result.TotalClear;
                this.TeamFull = result.TotalFull;
                this.TeamErrors = result.TotalErrors;

                var gamesum = this.context.SumPerGames.First(s => s.GameId == this.CurrentGame.Id);
                this.ToPay = gamesum.PenaltySum;

                //this.Paid = this.context.PaidPerGames.First(p => p.Game == this.CurrentGame.Id).Paid;
            }
        }
    }
}
