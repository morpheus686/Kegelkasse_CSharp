using Strafenkatalog.ViewModel;

namespace Strafenkatalog.Services.Interfaces
{
    public interface IDialogService
    {
        Task<object?> ShowDialog(ViewModelBase viewModel);

        Task ShowIndeterminateDialog(Func<Task> progressTask);

        Task ShowIndeterminateDialog(Func<Task> progressTask, string message);

        Task ShowIndeterminateDialog(Task worktask);

        Task ShowMessage(string message);
    }
}
