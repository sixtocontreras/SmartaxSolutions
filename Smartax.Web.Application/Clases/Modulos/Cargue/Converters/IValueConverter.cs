using System;

namespace Smartax.Web.Application.Clases.Modulos.Cargue.Converters
{
    public interface IValueConverter
    {
        IFormatProvider FormatProvider { get; }

        object Convert(object value);
    }
}
