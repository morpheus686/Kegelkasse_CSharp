using AsyncAwaitBestPractices.MVVM;
using Strafenkatalog.Models;
using System.Windows.Input;

namespace Strafenkatalog.ViewModel
{
    public class MainDataGridItemViewModel
    {
        public ICommand EditCommand { get; }

        private readonly MainWindowViewModel parent;
        private readonly SumPerPlayer sumPerPlayer;

        public DateOnly? Date { get => sumPerPlayer.Date; }
        public string? TeamName { get => sumPerPlayer.TeamName; }
        public string? Name { get => sumPerPlayer.Name; }
        public double? PenaltySum { get => sumPerPlayer.PenaltySum; }
        public int? Full { get => sumPerPlayer.Full; }
        public int? Clear { get => sumPerPlayer.Clear; }
        public int? Errors { get => sumPerPlayer.Errors; }
        public int? HasPlayed { get => sumPerPlayer.Played; }
        public int? Result { get => sumPerPlayer.Result; }

        public MainDataGridItemViewModel(MainWindowViewModel parent, 
            SumPerPlayer sumPerPlayer)
        {
            this.parent = parent;
            this.sumPerPlayer = sumPerPlayer;
            EditCommand = new AsyncCommand(ExecuteEdit);
        }

        private Task ExecuteEdit()
        {
            return this.parent.EditPlayer(this.sumPerPlayer);
        }
    }
}
