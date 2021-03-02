using System;

namespace Triangulation.Core.Interfaces.Extensions
{
    public static class NumberExtensions
    {
        /// <summary>
        /// Same <see cref="Math.Pow(double, double)"/> but directly from the target number
        /// </summary>
        /// <param name="number">Target number</param>
        /// <param name="pow">Degree indicator value</param>
        /// <returns>Number, raised to the power</returns>
        public static double ToPow(this double number, double pow)
        {
            return Math.Pow(number, pow);
        }

        /// <summary>
        /// Check if the number is in passed range (range boundary may be included or not)
        /// </summary>
        /// <param name="number">Target value to check</param>
        /// <param name="from">Range start</param>
        /// <param name="to">Range end</param>
        /// <param name="includeBoundaries">Defines whether to include boundaries of range or not</param>
        /// <returns><see cref="true"/> if number is in passed range and <see cref="false"/> otherwise</returns>
        public static bool InRange(this double number, double from, double to, bool includeBoundaries = false)
        {
            if (includeBoundaries)
            {
                return from <= number && number <= to;
            }
            else
            {
                return from < number && number < to;
            }
        }
    }
}
