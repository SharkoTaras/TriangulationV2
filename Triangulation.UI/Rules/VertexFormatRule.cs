using System;
using System.Globalization;
using System.Windows.Controls;
using Triangulation.Core.Implementation.Serializers;

namespace Triangulation.UI
{
    internal class VertexFormatRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var strVertex = value as string;
            var serializer = new VertexSerializer();
            try
            {
                serializer.Deserialize(strVertex);
            }
            catch (Exception e)
            {
                return new ValidationResult(false, $"Illegas character or {e.Message}.\nFormat: (x,y)");
            }

            return ValidationResult.ValidResult;
        }
    }
}
