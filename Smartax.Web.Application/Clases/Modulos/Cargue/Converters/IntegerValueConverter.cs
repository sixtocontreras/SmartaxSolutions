using System;
using System.Globalization;

namespace Smartax.Web.Application.Clases.Modulos.Cargue.Converters
{
    public class IntegerValueConverter : IValueConverter
    {
        public IntegerValueConverter(NumberFormatInfo numberFormat)
        {
            FormatProvider = numberFormat;
        }

        public IFormatProvider FormatProvider { get; private set; }

        public object Convert(object value)
        {
            return (int)double.Parse($"{value}", FormatProvider);
        }
    }
}
