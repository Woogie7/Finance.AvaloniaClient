using AutoMapper;
using Avalonia.Data.Converters;
using Avalonia.Media;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finance.AvaloniaClient.Converter
{
    internal class PositiveValueConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if(value is decimal decimalConverter)
            {
                return new SolidColorBrush(decimalConverter > 0 ? Colors.Green : Colors.Red);
            }
            return new SolidColorBrush(Colors.Black);
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
