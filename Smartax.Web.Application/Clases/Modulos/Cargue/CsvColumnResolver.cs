using Smartax.Web.Application.Clases.Modulos.Cargue.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Smartax.Web.Application.Clases.Modulos.Cargue
{
    public class CsvColumnResolver<T> where T : new()
    {
        private Type _targetType;

        private readonly Dictionary<int, PropertyInfo> _columns = new Dictionary<int, PropertyInfo>();
        private readonly Dictionary<int, IValueConverter> _converters = new Dictionary<int, IValueConverter>();

        public CsvColumnResolver()
        {
            _targetType = typeof(T);
        }

        public void Register<M>(int position, Expression<Func<T, M>> member, IValueConverter converter = null)
        {
            if (_columns.ContainsKey(position))
            {
                throw new Exception("column index is already registered");
            }

            var memberName = $"{member.Body}".Split('.').LastOrDefault();
            var propertyInfo = _targetType.GetProperty(memberName);
            _columns.Add(position, propertyInfo);
            if (converter != null)
            {
                _converters.Add(position, converter);
            }
        }

        public T Load(string[] line, out List<string> errors)
        {
            if (line.Length < 1)
            {
                throw new Exception("empty line cannot be loaded");
            }

            var target = new T();
            var entireLine = string.Join("|", line);
            errors = new List<string>();


            foreach (var column in _columns)
            {
                var position = column.Key;
                var property = column.Value;

                if (!property.CanWrite)
                {
                    continue;
                }

                IValueConverter converter;
                object data = null;

                try
                {
                    data = line[position];
                    var convertedData = _converters.TryGetValue(position, out converter) ? converter.Convert(data) : Convert.ChangeType(data, property.PropertyType);
                    property.SetValue(target, convertedData, null);
                }
                catch
                {
                    var error = $"Dato inválido: {property.Name}: {data ?? string.Empty}, En la linea {entireLine}.";
                    errors.Add(error);
                }
            }

            return target;
        }
    }
}