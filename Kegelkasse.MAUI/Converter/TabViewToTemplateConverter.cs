using Kegelkasse.Base.ViewModel;
using System.Globalization;

namespace Kegelkasse.MAUI.Converter
{
    internal class TabViewToTemplateConverter : IValueConverter
    {
        public DataTemplate GamePlayerTabView { get; set; }

        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value == null) return null;

            BindableObject? view = null;

            switch (value)
            {
                case GamePlayerTabViewModel overview:
                    view = this.GamePlayerTabView.CreateContent() as BindableObject;
                    if (view != null) view.BindingContext = overview;
                    break;

                default:
                    return null;
            }

            return view;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
