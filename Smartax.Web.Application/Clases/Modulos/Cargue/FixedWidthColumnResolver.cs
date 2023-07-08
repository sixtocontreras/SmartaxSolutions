using Smartax.Web.Application.Clases.Modulos.Cargue.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Smartax.Web.Application.Clases.Modulos.Cargue
{
    public class FixedWidthColumnResolver<T> where T : new()
    {
        private Type _targetType;

        private readonly Dictionary<ColumnLength, PropertyInfo> _columns = new Dictionary<ColumnLength, PropertyInfo>();
        private readonly Dictionary<ColumnLength, IValueConverter> _converters = new Dictionary<ColumnLength, IValueConverter>();

        public FixedWidthColumnResolver()
        {
            _targetType = typeof(T);
        }

        public void Register<M>(int startIndex, int length, Expression<Func<T, M>> member, IValueConverter converter = null)
        {
            var columnRange = new ColumnLength(startIndex, length);
            if (_columns.ContainsKey(columnRange))
            {
                throw new Exception("column range is already registered");
            }

            var memberName = $"{member.Body}".Split('.').LastOrDefault();
            var propertyInfo = _targetType.GetProperty(memberName);
            _columns.Add(columnRange, propertyInfo);
            if (converter != null)
            {
                _converters.Add(columnRange, converter);
            }
        }

        public T Load(string line, out List<string> errors)
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
                    data = line.Substring(position.StartIndex, position.Length);
                    var convertedData = _converters.TryGetValue(position, out converter) ? converter.Convert(data) : Convert.ChangeType(data, property.PropertyType);
                    property.SetValue(target, convertedData, null);
                }
                catch
                {
                    var error = $"Dato inválido: {property.Name}: {data ?? string.Empty}, En la linea '{entireLine}'.";
                    errors.Add(error);
                }
            }

            return target;
        }
    }
}