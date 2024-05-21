using Strafenkatalog.Services.Interfaces;

namespace Strafenkatalog.ViewModel
{
    public class LoadableViewModel : ViewModelBase
    { 
        public Task Initialize()
        {
            InitializeInternal();
            return InitializeInternalAsync();
        }

        protected virtual void InitializeInternal()
        {
        }

        protected virtual Task InitializeInternalAsync()
        {
            return Task.CompletedTask;
        }
    }
}
