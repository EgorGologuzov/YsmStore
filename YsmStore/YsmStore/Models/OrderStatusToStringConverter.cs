using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Xamarin.Forms;

namespace YsmStore.Models
{
    public class OrderStatusToStringConverter : IValueConverter
    {
        public static readonly Dictionary<OrderStatus, string> StatusDictionary = new Dictionary<OrderStatus, string>()
        {
            { OrderStatus.Processed, "Оформлен" },
            { OrderStatus.InDelivary, "В доставке" },
            { OrderStatus.AwaitPickUp, "Ожидает получения" },
            { OrderStatus.Received, "Получен" },
            { OrderStatus.Cancaled, "Отменен" }
        };

        public static string[] StatusStrings { get => StatusDictionary.Values.ToArray(); }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return StatusDictionary[(OrderStatus)value];
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return StatusDictionary.Where(pare => pare.Value == (string)value).FirstOrDefault().Key;
        }
    }
}
