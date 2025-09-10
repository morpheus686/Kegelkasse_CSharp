using Kegelkasse.Base.Models;

namespace Kegelkasse.Base.ViewModel
{
    public class EditTeamsDialogViewModel : LoadableViewModelBase
    {
        private readonly StrafenkatalogContext context;

        public EditTeamsDialogViewModel(StrafenkatalogContext context)
        {
            this.context = context;
        }
    }
}
