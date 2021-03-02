using System;

namespace Triangulation.Core.Algorithms.Equations
{
    public class EquationConstatns
    {
        public const double a11 = 7;
        public const double a22 = 6;
        public const double d = 1;
        public const double bv = 0;
        public const double f = 5;

        //public static Func<double, double, double> f = (x, y) => 2 * (x - 2) * x + 2 * (y - 2) * y;
        public static Func<double, double, double> u = (x, y) => (x - 2) * (y - 2) * x * y;
    }
}
