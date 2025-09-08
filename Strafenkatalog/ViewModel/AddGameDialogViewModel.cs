using Kegelkasse.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Kegelkasse.ViewModel
{
    public class AddGameDialogViewModel : DialogViewModelBase
    {
        private const int DefaultTeamId = 1;
        private const int DefaultSeasonId = 2;
        private readonly StrafenkatalogContext _context;

        private string _opponent;

        public string Opponent
        {
            get { return _opponent; }
            set
            {
                _opponent = value;
                ValidateOpponent();
                RaisePropertyChanged(nameof(HasErrors));
            }
        }

        public List<AddGamePlayerItemViewModel> Players { get; }
        public override bool HasErrors => base.HasErrors;
        public DateTime GameDate { get; set; }
        public int GameNumber { get; set; }

        public List<Team> Teams { get; }
        public Team SelectedTeam { get; set; } = null!;
        public IEnumerable<Player> SelectedPlayers => Players.Where(item => item.IsPlaying).Select(item => item.Player);

        public List<Season> Seasons { get; }
        public Season SelectedSeason { get; set; } = null!;


        public AddGameDialogViewModel(StrafenkatalogContext context)
        {
            _context = context;
            _opponent = string.Empty;
            Players = [];
            Teams = [];
            Seasons = [];
            GameDate = DateTime.Now;
            GameNumber = 1;
        }

        private void ValidateOpponent()
        {
            const string propertyName = nameof(Opponent);

            if (string.IsNullOrWhiteSpace(Opponent))
            {
                AddError(propertyName, "Die gegnerische Mannschaft darf nicht leer sein!");
            }
            else
            {
                ClearErrors(propertyName);
            }
        }

        protected async override Task InitializeInternalAsync()
        {
            var players = _context.Players.OrderBy(e => e.Name).ToList();
            var defaultPlayers = _context.DefaultPlayers.Where(e => e.TeamId == DefaultTeamId).ToList();

            foreach (var player in players)
            {
                var isPlayingByDefault = defaultPlayers.Exists(dp => dp.PlayerId == player.Id);
                Players.Add(new AddGamePlayerItemViewModel(player, isPlayingByDefault));
            }

            var defaultTeam = await _context.Teams.FirstAsync(t => t.Id == DefaultTeamId);
            Teams.Add(defaultTeam);
            SelectedTeam = defaultTeam;

            var defaultSeason = await _context.Seasons.FirstAsync(s =>  s.Id == DefaultSeasonId);
            Seasons.Add(defaultSeason);
            SelectedSeason = defaultSeason;

            ValidateOpponent();
        }
    }
}
