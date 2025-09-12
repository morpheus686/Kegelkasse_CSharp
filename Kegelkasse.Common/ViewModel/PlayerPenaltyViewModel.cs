using Kegelkasse.Common.Models;

namespace Kegelkasse.Common.ViewModel
{
    public class PlayerPenaltyViewModel(PlayerPenalty playerPenalty) : ViewModelBase
    {
        public PlayerPenalty PlayerPenalty { get; } = playerPenalty;

        public int Value
        {
            get { return PlayerPenalty.Value; }
            set
            {
                PlayerPenalty.Value = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(ToPay));
            }
        }

        public double ToPay => PlayerPenalty.Value * PlayerPenalty.PenaltyNavigation.Penalty1;
        public bool IsNotErrorPenalty => PlayerPenalty.PenaltyNavigation.GetsValueByParent == 0;
        public string Description => PlayerPenalty.PenaltyNavigation.Description;
    }
}
