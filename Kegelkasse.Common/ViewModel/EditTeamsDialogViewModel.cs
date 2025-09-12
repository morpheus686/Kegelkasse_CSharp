using Kegelkasse.Common.Models;

namespace Kegelkasse.Common.ViewModel
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
