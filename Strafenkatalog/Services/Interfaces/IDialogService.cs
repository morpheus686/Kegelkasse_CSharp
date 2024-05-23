using Strafenkatalog.ViewModel;

namespace Strafenkatalog.Services.Interfaces
{
    public interface IDialogService
    {
        Task<object?> ShowDialog(ViewModelBase viewModel);
        Task ShowIndeterminateDialog(Func<IndeterminateProgressViewModel, Task> progressTask);
        Task ShowMessage(string message);
    }
}
