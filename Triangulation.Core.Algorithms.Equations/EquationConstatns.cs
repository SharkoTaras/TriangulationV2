using System;

namespace Triangulation.Core.Algorithms.Equations
{
    public class EquationConstatns
    {
        //static EquationConstatns()
        //{
        //    f = (x, y) => { return 1; };
        //}

        //public const int A11 = 7;

        //public const int A22 = 3;

        //public const int d = 0;

        //public static Func<double, double, double> f;

        public const double a11 = 1;
        public const double a22 = 1;
        public const double d = 0;
        public const double bv = 0;
        public const double f = 0;
        //public static Func<double, double, double> f = (x, y) => 2 * (x - 2) * x + 2 * (y - 2) * y;
        public static Func<double, double, double> u = (x, y) => (x - 2) * (y - 2) * x * y;
    }
}
