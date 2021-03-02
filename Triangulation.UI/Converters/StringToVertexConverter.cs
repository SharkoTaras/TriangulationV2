using System;
using System.Globalization;
using System.Windows.Data;
using Triangulation.Core.Implementation.Serializers;
using Triangulation.Core.Interfaces.Entities;

namespace Triangulation.UI
{
    [ValueConversion(typeof(string), typeof(Vertex))]
    public class StringToVertexConverter : IValueConverter
    {
        private VertexSerializer VertexSerializer = new VertexSerializer();

        public static StringToVertexConverter Instance = new StringToVertexConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return null;
            }
            return value.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return this.VertexSerializer.Deserialize(value as string);
        }
    }
}