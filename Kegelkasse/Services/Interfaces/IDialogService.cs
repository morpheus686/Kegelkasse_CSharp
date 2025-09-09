using Kegelkasse.ViewModel;

namespace Kegelkasse.Services.Interfaces
{
    public interface IDialogService
    {
        Task<object?> ShowDialog(LoadableViewModelBase viewModel);
        Task ShowIndeterminateDialog(Func<IndeterminateProgressViewModel, Task> progressTask);
        Task ShowMessage(string message);
    }
}
