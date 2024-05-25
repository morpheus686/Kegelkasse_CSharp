using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Strafenkatalog.Models;

namespace Strafenkatalog.ViewModel
{
    public class PlayerPenaltyViewModel : ViewModelBase
    {
        public PlayerPenaltyViewModel(PlayerPenalty playerPenalty)
        {
            PlayerPenalty = playerPenalty;
        }

        public int Value
        {
            get { return PlayerPenalty.Value; }
            set
            {
                PlayerPenalty.Value = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(this.ToPay));
            }
        }

        public double ToPay
        {
            get { return PlayerPenalty.Value * PlayerPenalty.PenaltyNavigation.Penalty1; }
        }

        public PlayerPenalty PlayerPenalty { get; }
        public string Description => PlayerPenalty.PenaltyNavigation.Description;
    }
}
