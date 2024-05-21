using Strafenkatalog.ViewModel;
using System.Windows.Controls;

namespace Strafenkatalog.Components
{
    public class LoadableView : ContentControl
    {
        public LoadableView()
        {
            Loaded += View_Loaded;
        }

        private async void View_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            if (DataContext is LoadableViewModel lvm)
            {
                Task loadTask = lvm.Initialize();

                if (loadTask.IsCompleted)
                {
                    return;
                }

                await loadTask;
            }
        }
    }
}
