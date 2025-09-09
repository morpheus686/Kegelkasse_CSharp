using Kegelkasse.Models;

namespace Kegelkasse.ViewModel
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
