namespace Kegelkasse.Base.ViewModel
{
    public abstract class LoadableViewModelBase : ViewModelBase
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
