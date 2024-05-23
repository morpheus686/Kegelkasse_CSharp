using MaterialDesignThemes.Wpf;
using Strafenkatalog.Services.Interfaces;
using Strafenkatalog.View;
using Strafenkatalog.ViewModel;

namespace Strafenkatalog.Services
{
    public class DialogService : IDialogService
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

        public async Task ShowIndeterminateDialog(Func<IndeterminateProgressViewModel, Task> progressTask)
        {
            var vm = new IndeterminateProgressViewModel();

            var view = new IndeterminateProgressDialogView
            {
                DataContext = vm
            };

            _dialogHost.Value.DialogContent = view;                        
            _dialogHost.Value.IsOpen = true;            
            await progressTask(vm);
            _dialogHost.Value.IsOpen = false;
            _dialogHost.Value.DialogContent = null;
        }

        public Task ShowMessage(string message)
        {
            return Task.CompletedTask;
        }

        private DialogHost LoadDialogHost()
        {
            if (App.Current.MainWindow is not MainWindow mainWindow)
            {
                throw new InvalidOperationException("MainWindow wurde nicht gefunden!");
            }

            return mainWindow.DialogHost;
        }
    }
}
