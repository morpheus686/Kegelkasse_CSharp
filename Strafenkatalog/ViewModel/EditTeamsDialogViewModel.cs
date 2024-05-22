using Strafenkatalog.Models;

namespace Strafenkatalog.ViewModel
{
    public class EditTeamsDialogViewModel : LoadableViewModel
    {
        private readonly StrafenkatalogContext context;

        public EditTeamsDialogViewModel(StrafenkatalogContext context)
        {
            this.context = context;
        }
    }
}
