using System;
using System.Globalization;

namespace Smartax.Web.Application.Clases.Modulos.Cargue.Converters
{
    public class DateTimeValueConverter : IValueConverter
    {
        public DateTimeValueConverter(string format, CultureInfo cultureInfo)
        {
            FormatProvider = cultureInfo;
            Format = format;
        }
        public IFormatProvider FormatProvider { get; private set; }

        public string Format { get; private set; }

        public object Convert(object value)
        {
            DateTime dateTime;

            if (DateTime.TryParseExact($"{value}", Format, FormatProvider, DateTimeStyles.None, out dateTime))
            {
                return dateTime;
            }
            else
            {
                throw new Exception();
            }
        }
    }
}
