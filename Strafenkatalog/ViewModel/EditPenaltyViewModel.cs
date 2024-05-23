using Strafenkatalog.Models;

namespace Strafenkatalog.ViewModel
{
    public class EditPenaltyViewModel : LoadableViewModel
    {
        private int? _full;
        private GamePlayer? gamePlayer;
        private readonly IEnumerable<PlayerPenalty> playerPenalties;

        public int? Full
        {
            get { return _full; }
            set 
            { 
                _full = value;
                RaisePropertyChanged();
            }
        }

        public GamePlayer? GamePlayer
        {
            get => gamePlayer;
            private set
            {
                gamePlayer = value;
                RaisePropertyChanged();
            }
        }

        public IEnumerable<PlayerPenalty> PlayerPenalties => playerPenalties;

        public EditPenaltyViewModel(GamePlayer gamePlayer, IEnumerable<PlayerPenalty> playerPenalties)
        {
            this.gamePlayer = gamePlayer;
            this.playerPenalties = playerPenalties;
        }
    }
}
