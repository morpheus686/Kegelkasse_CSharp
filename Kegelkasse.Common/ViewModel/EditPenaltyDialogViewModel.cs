using Kegelkasse.Common.Models;
using System.Collections.ObjectModel;

namespace Kegelkasse.Common.ViewModel
{
    public class EditPenaltyDialogViewModel : LoadableViewModelBase
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
            get { return gamePlayer.Full; }
            set
            {
                gamePlayer.Full = value;
                RaisePropertyChanged();

                gamePlayer.Result = value + gamePlayer.Clear;
                RaisePropertyChanged(nameof(Result));
            }
        }

        public int? Clear
        {
            get
            {
                return gamePlayer.Clear;
            }
            set
            {
                gamePlayer.Clear = value;
                RaisePropertyChanged();

                gamePlayer.Result = value + gamePlayer.Full;
                RaisePropertyChanged(nameof(Result));
            }
        }

        public int? Errors
        {
            get { return gamePlayer.Errors; }
            set
            {
                gamePlayer.Errors = value;
                RaisePropertyChanged();

                if (value != null)
                {
                    var errorPenalty = playerPenaltyViewModels.FirstOrDefault(p => p.PlayerPenalty.PenaltyNavigation.GetsValueByParent == 1);

                    if (errorPenalty != null)
                    {
                        errorPenalty.Value = value.Value;
                    }
                    
                }    
            }
        }

        public int? Result { get => gamePlayer.Result; }
        public string Name { get => gamePlayer.PlayerNavigation.Name; }

        public IEnumerable<PlayerPenalty> PlayerPenalties => playerPenalties;

        public EditPenaltyDialogViewModel(GamePlayer gamePlayer, IEnumerable<PlayerPenalty> playerPenalties)
        {
            this.gamePlayer = gamePlayer;
            this.playerPenalties = playerPenalties;
            playerPenaltyViewModels = [];

            foreach (var item in this.playerPenalties)
            {
                playerPenaltyViewModels.Add(new PlayerPenaltyViewModel(item));
            }
        }
    }
}
