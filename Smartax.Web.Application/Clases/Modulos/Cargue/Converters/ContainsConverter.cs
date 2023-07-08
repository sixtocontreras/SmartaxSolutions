using System;
using System.Collections.Generic;

namespace Smartax.Web.Application.Clases.Modulos.Cargue.Converters
{ 
    public class ContainsConverter : IValueConverter
    {
        public ContainsConverter(List<string> values)
        {
            Values = values;
        }

        public IFormatProvider FormatProvider { get; private set; }

        public List<string> Values { get; private set; }

        public object Convert(object value)
        {
            var stringValue = $"{value}";

            if (Values == null)
            {
                return stringValue;
            }

            if (!Values.Contains(stringValue))
            {
                throw new Exception();
            }

            return stringValue;
        }

    }
}