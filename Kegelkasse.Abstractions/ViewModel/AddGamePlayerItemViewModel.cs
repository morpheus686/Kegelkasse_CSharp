using Kegelkasse.Base.Models;

namespace Kegelkasse.Base.ViewModel
{
    public class AddGamePlayerItemViewModel : ViewModelBase
    {   
        public AddGamePlayerItemViewModel(Player player, bool isPlaying)
        {
            Player = player;
            IsPlaying = isPlaying;
        }

        public bool IsPlaying { get; set; }
        public Player Player { get; }
        public string PlayerName => Player.Name;

    }
}
