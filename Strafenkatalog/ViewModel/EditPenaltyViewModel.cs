using Strafenkatalog.Models;
using System.Collections.ObjectModel;

namespace Strafenkatalog.ViewModel
{
    public class EditPenaltyViewModel : LoadableViewModel
    {

        private GamePlayer gamePlayer;
        private readonly IEnumerable<PlayerPenalty> playerPenalties;
        private ObservableCollection<PlayerPenaltyViewModel> playerPenaltyViewModels;

        public IEnumerable<PlayerPenaltyViewModel> PlayerPenaltyViewModels
        {
            get { return playerPenaltyViewModels; }
        }

        public int? Full
        {
            get { return this.gamePlayer.Full; }
            set
            {
                this.gamePlayer.Full = value;
                RaisePropertyChanged();

                this.gamePlayer.Result = value + this.gamePlayer.Clear;
                RaisePropertyChanged(nameof(this.Result));
            }
        }

        public int? Clear
        {
            get
            {
                return this.gamePlayer.Clear;
            }
            set
            {
                this.gamePlayer.Clear = value;
                RaisePropertyChanged();

                this.gamePlayer.Result = value + this.gamePlayer.Full;
                RaisePropertyChanged(nameof(this.Result));
            }
        }

        public int? Errors
        {
            get { return this.gamePlayer.Errors; }
            set
            {
                this.gamePlayer.Errors = value;
                RaisePropertyChanged();

                if (value != null)
                {
                    var errorPenalty = this.playerPenaltyViewModels.FirstOrDefault(p => p.PlayerPenalty.PenaltyNavigation.GetsValueByParent == 1);

                    if (errorPenalty != null)
                    {
                        errorPenalty.Value = value.Value;
                    }
                    
                }    
            }
        }

        public int? Result { get => this.gamePlayer.Result; }
        public string Name { get => this.gamePlayer.PlayerNavigation.Name; }

        public IEnumerable<PlayerPenalty> PlayerPenalties => playerPenalties;

        public EditPenaltyViewModel(GamePlayer gamePlayer, IEnumerable<PlayerPenalty> playerPenalties)
        {
            this.gamePlayer = gamePlayer;
            this.playerPenalties = playerPenalties;
            this.playerPenaltyViewModels = [];

            foreach (var item in this.playerPenalties)
            {
                this.playerPenaltyViewModels.Add(new PlayerPenaltyViewModel(item));
            }
        }
    }
}
