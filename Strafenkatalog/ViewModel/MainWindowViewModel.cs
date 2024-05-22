using AsyncAwaitBestPractices.MVVM;
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

        public MainWindowViewModel()
        {
            GridItems = [];
            games = [];
            _dialogService = new DialogService();
            this.context = new StrafenkatalogContext();
            this.EditTeamsCommand = new AsyncCommand(EditTeamsExecuted);
            this.games = [.. this.context.Games.Where(g => g.Team == 1)];
            this.CurrentGame = games.LastOrDefault();

            Load();
        }

        private async Task EditTeamsExecuted()
        {
            var vm = new EditTeamsDialogViewModel(this.context);
            await _dialogService.ShowDialog(vm);
        }

        public async Task EditPlayer(SumPerPlayer sumPerPlayer)
        {
            var viewModel = new EditPenaltyViewModel(this.context, sumPerPlayer);
            var result = await _dialogService.ShowDialog(viewModel);

            if (result is DialogResult dialogResult
                && dialogResult == DialogResult.Yes)
            {
                await _dialogService.ShowIndeterminateDialog(SaveChanges);
                Load();
            }
        }

        private async Task SaveChanges(IndeterminateProgressViewModel vm)
        {
            vm.Message = "Ich mache gerade etwas...";
            await Task.Delay(10000);
            vm.Message = "Bin gleich fertig";
            await Task.Delay(2000);
        }

        public void Load()
        {           

            GridItems.Clear();

            foreach (var item in this.context.SumPerPlayers.Where(s => s.GameId == this.CurrentGame.Id))
            {
                GridItems.Add(new MainDataGridItemViewModel(this, item));
            }
        }
    }
}
