using System;
using System.Globalization;
using System.Windows.Controls;

namespace Triangulation.UI
{
    internal class NumberOfPointsRule : ValidationRule
    {
        public NumberOfPointsRule()
        {
        }
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var str = value as string;
            uint number;
            try
            {
                if (str.Length > 0)
                {
                    number = uint.Parse(str);
                }
            }
            catch (Exception e)
            {
                return new ValidationResult(false, $"Illegal character or {e.Message}");
            }

            return ValidationResult.ValidResult;
        }
    }
}
