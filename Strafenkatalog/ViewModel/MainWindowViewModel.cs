﻿using AsyncAwaitBestPractices.MVVM;
using Microsoft.EntityFrameworkCore;
using Strafenkatalog.Components;
using Strafenkatalog.Models;
using Strafenkatalog.Services;
using Strafenkatalog.Services.Interfaces;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Strafenkatalog.ViewModel
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly IDialogService _dialogService;
        private readonly StrafenkatalogContext context;
        private Game? currentGame;
        private List<Game> games;

        public ObservableCollection<MainDataGridItemViewModel> GridItems { get; set; }
        public Game? CurrentGame
        {
            get => currentGame;
            set
            {
                currentGame = value;
                RaisePropertyChanged();
            }
        }

        public ICommand EditTeamsCommand { get; }
        public ICommand PreviousGameCommand { get; }
        public ICommand NextGameCommand { get; }

        public MainWindowViewModel()
        {
            GridItems = [];
            games = [];
            _dialogService = new DialogService();
            this.context = new StrafenkatalogContext();
            this.EditTeamsCommand = new AsyncCommand(EditTeamsExecuted);

            this.PreviousGameCommand = new RelayCommand(ExecutePreviousGameCommand, CanExecutePreviousGameCommand);
            this.NextGameCommand = new RelayCommand(ExecuteNextGameCommand, CanExecuteNextsGameCommand);
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

        private async Task EditTeamsExecuted()
        {
            var vm = new EditTeamsDialogViewModel(this.context);
            await _dialogService.ShowDialog(vm);
        }

        public async Task EditPlayer(SumPerPlayer sumPerPlayer)
        {
            var queryable = this.context.GamePlayers.Where(x => x.Player == sumPerPlayer.PlayerId && x.Game == sumPerPlayer.GameId);
            var gamePlayer = await queryable.FirstAsync();
            var playerPenalties = this.context.PlayerPenalties.Where(pp => pp.GamePlayer == gamePlayer.Id).ToList();

            foreach (var item in playerPenalties)
            {
                item.PenaltyNavigation = this.context.Penalties.Where(p => p.Id == item.Penalty).First();
            }

            var viewModel = new EditPenaltyViewModel(gamePlayer, playerPenalties);
            var result = await _dialogService.ShowDialog(viewModel);

            if (result is DialogResult dialogResult
                && dialogResult == DialogResult.Yes)
            {
                await _dialogService.ShowIndeterminateDialog(async vm =>
                {
                    var hasChanges = this.context.ChangeTracker.HasChanges();
                    int updated = await this.context.SaveChangesAsync();
                    bool hasTransaction = this.context.Database.CurrentTransaction != null;
                    LoadData();
                });
            }
        }

        public void Initialize()
        {
            this.games = [.. this.context.Games.Where(g => g.Team == 1)];
            this.CurrentGame = games.LastOrDefault();

            LoadData();
        }

        private void LoadData()
        {
            GridItems.Clear();

            if (this.currentGame != null)
            {
                foreach (var item in this.context.SumPerPlayers.Where(s => s.GameId == this.CurrentGame.Id))
                {
                    GridItems.Add(new MainDataGridItemViewModel(this, item));
                }
            }
        }
    }
}
