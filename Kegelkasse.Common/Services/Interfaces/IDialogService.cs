using Kegelkasse.Common.ViewModel;

namespace Kegelkasse.Common.Services.Interfaces
{
    public interface IDialogService
    {
        Task<object?> ShowDialog(LoadableViewModelBase viewModel);
        Task ShowIndeterminateDialogAsync(Func<Action<string>, object?, Task> progressTask, object? parameter = null);
        Task ShowMessage(string message);
    }
}
