using CommunityToolkit.Maui.Extensions;
using CommunityToolkit.Maui.Views;
using Kegelkasse.Common.Services.Interfaces;
using Kegelkasse.Common.ViewModel;
using Kegelkasse.Foundation.Enumerations;
using Kegelkasse.MAUI.Locators;

namespace Kegelkasse.MAUI.Services
{
    internal class DialogService : IDialogService
    {
        private readonly ViewLocator viewLocator;
        private readonly Page page;
        private readonly UraniumUI.Dialogs.IDialogService uraniumDialogService;

        public DialogService(UraniumUI.Dialogs.IDialogService uraniumDialogService)
        {
            this.viewLocator = new ViewLocator(typeof(App).Assembly);

            this.page = Application.Current?.Windows.FirstOrDefault()?.Page
                ?? throw new TypeLoadException();
            this.uraniumDialogService = uraniumDialogService;
        }

        public async Task<object?> ShowDialog(LoadableViewModelBase viewModel)
        {
            try
            {
                await viewModel.Initialize();
                var view = this.viewLocator.ResolveView(viewModel);

                if (view is Popup popup)
                {
                    popup.BindingContext = viewModel;
                    var result = await page.ShowPopupAsync(popup);

                    if (result == null)
                    {
                        return DialogResult.Abort;
                    }

                    return result;
                }

                return DialogResult.Abort;
            }
            catch (Exception e)
            {
                await this.page.DisplayAlert("Fehler", e.Message, "OK");
                return DialogResult.Abort;
            }
        }

        public Task ShowIndeterminateDialog(Func<IndeterminateProgressViewModel, Task> progressTask)
        {
            throw new NotImplementedException();
        }

        public async Task ShowIndeterminateDialogAsync(Func<Action<string>, object?, Task> progressTask, object? parameter = null)
        {
            var viewModel = new IndeterminateProgressViewModel();
            var progressDialog = await this.uraniumDialogService.DisplayProgressAsync("Speichern", "Bitte warten...");

            try
            {
                await progressTask((message) =>
                {
                    viewModel.Message = message;
                }, parameter);
            }
            finally
            {
                progressDialog.Dispose();
            }
        }

        public Task ShowMessage(string message)
        {
            throw new NotImplementedException();
        }
    }
}
