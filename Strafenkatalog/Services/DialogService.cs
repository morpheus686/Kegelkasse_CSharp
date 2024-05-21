using MaterialDesignThemes.Wpf;
using Strafenkatalog.Services.Interfaces;
using Strafenkatalog.ViewModel;

namespace Strafenkatalog.Services
{
    internal class DialogService : IDialogService
    {
        private const string DialogIdentifier = "DialogHost";
        private readonly Lazy<DialogHost> _dialogHost;

        public DialogService()
        {
            _dialogHost = new Lazy<DialogHost>(LoadDialogHost);
        }

        public Task<object?> ShowDialog(ViewModelBase viewModel)
        {
            return DialogHost.Show(viewModel, DialogIdentifier);
        }

        public Task ShowIndeterminateDialog(Func<Task> progressTask)
        {
            return ShowIndeterminateDialog(progressTask, null);
        }

        public async Task ShowIndeterminateDialog(Task worktask)
        {
            try
            {
                OpenIndeterminateProgressDialog(null);
                await worktask;
            }
            finally
            {
                CloseIndeterminateProgressDialog();
            }
        }

        public async Task ShowIndeterminateDialog(Func<Task> progressTask, string message)
        {
            try
            {
                OpenIndeterminateProgressDialog(message);
                await progressTask();
            }
            finally
            {
                CloseIndeterminateProgressDialog();
            }
        }

        public Task ShowMessage(string message)
        {
            return Task.CompletedTask;
        }

        private void CloseIndeterminateProgressDialog()
        {
            _dialogHost.Value.IsOpen = false;
            _dialogHost.Value.DialogContent = null;
        }

        private DialogHost LoadDialogHost()
        {
            if (App.Current.MainWindow is not MainWindow mainWindow)
            {
                throw new InvalidOperationException("MainWindow wurde nicht gefunden!");
            }

            return mainWindow.DialogHost;
        }

        private void OpenIndeterminateProgressDialog(string message)
        {    
            _dialogHost.Value.IsOpen = true;
        }
    }
}
