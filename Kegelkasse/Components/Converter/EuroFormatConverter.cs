using System.Globalization;
using System.Windows.Data;

namespace Kegelkasse.Components.Converter
{
    public class EuroFormatConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double d)
            {
                return string.Format(culture, "{0:f2} €", d);
            }

            return "-,-- €";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
