using CommunityToolkit.Mvvm.Input;
using Kegelkasse.Common.Models;
using Microsoft.EntityFrameworkCore;
using System.Windows.Input;

namespace Kegelkasse.Common.ViewModel
{
    public class AddGameDialogViewModel : DialogViewModelBase
    {
        private const int DefaultTeamId = 1;
        private const int DefaultSeasonId = 2;
        private readonly KegelkasseContext _context;

        private string _opponent;

        public string Opponent
        {
            get { return _opponent; }
            set
            {
                SetProperty(ref _opponent, value);
                ValidateOpponent();
                OnPropertyChanged(nameof(HasErrors));
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

        public AddGameStep CurrentStep { get; private set; } = AddGameStep.GameData;

        public ICommand NextCommand { get; }
        public ICommand BackCommand { get; }


        public AddGameDialogViewModel(KegelkasseContext context)
        {
            _context = context;
            _opponent = string.Empty;
            Players = [];
            Teams = [];
            Seasons = [];
            GameDate = DateTime.Now;
            GameNumber = 1;

            NextCommand = new RelayCommand(GoNext, () => CanGoNext);
            BackCommand = new RelayCommand(GoBack, () => CurrentStep == AddGameStep.Players);
        }

        private void GoNext()
        {
            if (CurrentStep == AddGameStep.GameData)
            {
                CurrentStep = AddGameStep.Players;
            }
        }

        private void GoBack()
        {
            if (CurrentStep == AddGameStep.Players)
            {
                CurrentStep = AddGameStep.GameData;
            }
        }

        public bool CanGoNext => CurrentStep == AddGameStep.GameData && !HasErrors;

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
