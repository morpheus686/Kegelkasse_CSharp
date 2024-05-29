using AsyncAwaitBestPractices.MVVM;
using Strafenkatalog.Models;
using System.Windows.Input;

namespace Strafenkatalog.ViewModel
{
    public class MainDataGridItemViewModel
    {
        public ICommand EditCommand { get; }

        private readonly GamePlayerTabViewModel parent;
        public SumPerPlayer SumPerPlayer { get; }
        public DateOnly? Date { get => SumPerPlayer.Date; }
        public string? TeamName { get => SumPerPlayer.TeamName; }
        public string? Name { get => SumPerPlayer.Name; }
        public double? PenaltySum { get => SumPerPlayer.PenaltySum; }
        public int? Full { get => SumPerPlayer.Full; }
        public int? Clear { get => SumPerPlayer.Clear; }
        public int? Errors { get => SumPerPlayer.Errors; }
        public int? HasPlayed { get => SumPerPlayer.Played; }
        public int? Result { get => SumPerPlayer.Result; }

        public MainDataGridItemViewModel(GamePlayerTabViewModel parent,
            SumPerPlayer sumPerPlayer)
        {
            this.parent = parent;
            this.SumPerPlayer = sumPerPlayer;
            EditCommand = new AsyncCommand(ExecuteEdit);
        }

        private Task ExecuteEdit()
        {
            return this.parent.EditPlayer(this.SumPerPlayer);
        }
    }
}
