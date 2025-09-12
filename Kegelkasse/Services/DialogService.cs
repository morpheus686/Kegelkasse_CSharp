using Kegelkasse.Common.Services.Interfaces;
using Kegelkasse.Common.ViewModel;
using Kegelkasse.View;
using MaterialDesignThemes.Wpf;

namespace Kegelkasse.Services
{
    public class DialogService : IDialogService
    {
        private const string DialogIdentifier = "DialogHost";

        public Task<object?> ShowDialog(LoadableViewModelBase viewModel)
        {
            viewModel.Initialize();
            return DialogHost.Show(viewModel, DialogIdentifier);
        }

        public Task ShowIndeterminateDialog(Func<IndeterminateProgressViewModel, Task> progressTask)
        {
            return Task.CompletedTask;
        }

        public async Task ShowIndeterminateDialogAsync(Func<Action<string>, object?, Task> progressTask, object? parameter = null)
        {
            var viewModel = new IndeterminateProgressViewModel();
            var showDialogTask = DialogHost.Show(viewModel, DialogIdentifier);

            void updateMessage(string newMessage)
            {
                viewModel.Message = newMessage;
            }

            try
            {
                await progressTask(updateMessage, parameter);
            }
            catch (Exception e)
            {

            }
            finally
            {
                DialogHost.Close(DialogIdentifier, viewModel);
                await showDialogTask;
            }
        }

        public Task ShowMessage(string message)
        {
            return Task.CompletedTask;
        }
    }
}
