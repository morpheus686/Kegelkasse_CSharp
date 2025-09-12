using System.Reflection;

namespace Kegelkasse.MAUI.Locators
{
    public class ViewLocator
    {
        public ViewLocator(Assembly viewAssembly)
        {
             this.viewAssembly = viewAssembly;
        }

        private readonly Assembly viewAssembly;

        public object ResolveView(object viewModel)
        {
            if (viewModel == null)
                throw new ArgumentNullException(nameof(viewModel));

            var viewModelType = viewModel.GetType();
            var viewModelName = viewModelType.Name ?? throw new InvalidOperationException("ViewModel has no full name");

            // Annahme: ViewModel-Namespace: MyApp.ViewModels.*
            // Ziel: MyApp.Views.* (gleicher Klassenname, aber ohne "Model" am Ende)
            var viewName = viewModelName
                .Replace("ViewModel", "View");

            var viewFullName = string.Join('.', "Kegelkasse.MAUI.View.DialogView", viewName);

            var viewType = this.viewAssembly.GetType(viewFullName);
            if (viewType == null)
            {
                throw new InvalidOperationException($"View not found: {viewName}");
            }

            // Erstelle eine View-Instanz
            var viewInstance = Activator.CreateInstance(viewType);
            if (viewInstance == null)
                throw new InvalidOperationException($"Could not create instance of {viewName}");

            return viewInstance;
        }
    }
}
