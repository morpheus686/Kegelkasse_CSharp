using Kegelkasse.Common.Models;

namespace Kegelkasse.Common.ViewModel
{
    public class EditTeamsDialogViewModel : LoadableViewModelBase
    {
        private readonly KegelkasseContext context;

        public EditTeamsDialogViewModel(KegelkasseContext context)
        {
            this.context = context;
        }
    }
}
