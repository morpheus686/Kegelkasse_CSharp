using Microsoft.EntityFrameworkCore;
using Strafenkatalog.Models;

namespace Strafenkatalog.ViewModel
{
    public class EditPenaltyViewModel : LoadableViewModel
    {
        private readonly StrafenkatalogContext context;
        private readonly SumPerPlayer sumPerPlayerOriginal;

        private int? _full;

        public int? Full
        {
            get { return _full; }
            set 
            { 
                _full = value;
                RaisePropertyChanged();
            }
        }


        public EditPenaltyViewModel(StrafenkatalogContext context, SumPerPlayer sumPerPlayer)
        {
            this.context = context;
            this.sumPerPlayerOriginal = sumPerPlayer;
        }

        protected override async Task InitializeInternalAsync()
        {
            var queryable = this.context.GamePlayers.Where(x => x.Player == this.sumPerPlayerOriginal.PlayerId && x.Game == this.sumPerPlayerOriginal.GameId);
            await queryable.LoadAsync();
            var result = await queryable.FirstAsync();

            if (result != null)
            {
                Full = result.Full;
            }
        }
    }
}
