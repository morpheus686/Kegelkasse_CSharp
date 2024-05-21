using Microsoft.EntityFrameworkCore;
using Strafenkatalog.Models;
using Strafenkatalog.Services;
using Strafenkatalog.Services.Interfaces;
using System.Collections.ObjectModel;

namespace Strafenkatalog.ViewModel
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly IDialogService _dialogService;
        private readonly StrafenkatalogContext context;

        public ObservableCollection<MainDataGridItemViewModel>? GridItems { get; set; }

        public MainWindowViewModel()
        {
            GridItems = [];
            _dialogService = new DialogService();
            this.context = new StrafenkatalogContext();

            foreach (var item in this.context.SumPerPlayers)
            {
                GridItems.Add(new MainDataGridItemViewModel(this, item));
            }
        }

        public async Task EditPlayer(SumPerPlayer sumPerPlayer)
        {
            var viewModel = new EditPenaltyViewModel(this.context, sumPerPlayer);
            var dialogResult = await _dialogService.ShowDialog(viewModel);
        }
    }
}
