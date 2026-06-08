using System.Globalization;
using System.Windows.Data;

namespace NeUrokAdmin.WPF.Views.Converters
{
    [ValueConversion(typeof(DateOnly), typeof(bool))]
    public class DateExpiredConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateOnly date)
                return date <= DateOnly.FromDateTime(DateTime.Now);
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
