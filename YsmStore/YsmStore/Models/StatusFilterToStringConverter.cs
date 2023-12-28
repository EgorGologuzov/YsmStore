using System;
using System.Globalization;
using System.Linq;
using Xamarin.Forms;

namespace YsmStore.Models
{
    public class StatusFilterToStringConverter : IValueConverter
    {
        private const string NO_FILTER = "[нет фильтра]";

        private OrderStatusToStringConverter _converter = new OrderStatusToStringConverter();

        public static string[] FilterVariants
        {
            get
            {
                var list = OrderStatusToStringConverter.StatusStrings.ToList();
                list.Insert(0, NO_FILTER);
                return list.ToArray();
            }
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            OrderStatus? filter = (OrderStatus?)value;
            return filter == null ? NO_FILTER : _converter.Convert(filter.Value, targetType, parameter, culture);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string filter = (string)value;
            return filter == NO_FILTER ? null : (OrderStatus?)_converter.ConvertBack(filter, targetType, parameter, culture);
        }
    }
}
