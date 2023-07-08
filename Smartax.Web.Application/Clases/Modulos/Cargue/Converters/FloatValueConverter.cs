using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Smartax.Web.Application.Clases.Modulos.Cargue.Converters
{
    public class FloatValueConverter : IValueConverter
    {
        public FloatValueConverter(NumberFormatInfo numberFormat)
        {
            FormatProvider = numberFormat;
        }

        public IFormatProvider FormatProvider { get; private set; }

        public object Convert(object value)
        {
            string data = value as string;

            if (string.IsNullOrEmpty(data))
            {
                return 0F;
            }

            CleanValue(ref data);
            return data.EndsWith(".**") ? 0F : float.Parse(data, FormatProvider);
        }

        private void CleanValue(ref string data)
        {
            data = Regex.Replace(data, "[$ ]", string.Empty);
        }
    }
}
