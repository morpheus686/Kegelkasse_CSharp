using Microsoft.EntityFrameworkCore;
using Strafenkatalog.Models;

namespace Strafenkatalog.ViewModel
{
    public class EditPenaltyViewModel : LoadableViewModel
    {
        private readonly StrafenkatalogContext context;
        private readonly SumPerPlayer sumPerPlayer;

        private int? full;

        public int? Full
        {
            get { return full; }
            set 
            { 
                full = value;
                RaisePropertyChanged();
            }
        }


        public EditPenaltyViewModel(StrafenkatalogContext context, SumPerPlayer sumPerPlayer)
        {
            this.context = context;
            this.sumPerPlayer = sumPerPlayer;
        }

        protected override async Task InitializeInternalAsync()
        {
            var queryable = this.context.GamePlayers.Where(x => x.Player == this.sumPerPlayer.PlayerId && x.Game == this.sumPerPlayer.GameId);
            await queryable.LoadAsync();
            var result = await queryable.FirstAsync();

            if (result != null)
            {
                Full = result.Full;
            }
        }
    }
}
