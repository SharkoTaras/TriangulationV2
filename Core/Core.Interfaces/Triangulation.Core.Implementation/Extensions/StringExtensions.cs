using System;
using Triangulation.Core.Implementation.Utils;

namespace Triangulation.Core.Implementation.Extensions
{
    public static class StringExtensions
    {
        public static double ToDouble(this string str)
        {
            Util.Check.NotNullOrEmpty(str);

            return Convert.ToDouble(str);
        }
    }
}
