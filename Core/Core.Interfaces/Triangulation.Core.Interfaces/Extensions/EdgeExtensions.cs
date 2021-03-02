using Triangulation.Core.Implementation.Utils;
using Triangulation.Core.Interfaces.Entities;

namespace Triangulation.Core.Interfaces.Extensions
{
    public static class EdgeExtensions
    { /// <summary>
      /// Check if the segment intersects another one
      /// </summary>
      /// <param name="target">Target segment</param>
      /// <param name="other">Segment to check with</param>
      /// <returns><see cref="bool"/> result of intersection</returns>
        public static bool IsIntersect(this Edge target, Edge other)
        {
            Util.Check.NotNull(target, "Parameter is null");
            Util.Check.NotNull(other, "Parameter is null");

            var s1 = target.Start;
            var s2 = other.Start;
            var e1 = target.End;
            var e2 = other.End;

            var numerator1 = (e2.X - s2.X) * (s1.Y - s2.Y) - (e2.Y - s2.Y) * (s1.X - s2.X);
            var numerator2 = (e1.X - s1.X) * (s1.Y - s2.Y) - (e1.Y - s1.Y) * (s1.X - s2.X);
            var denominator = (e2.Y - s2.Y) * (e1.X - s1.X) - (e2.X - s2.X) * (e1.Y - s1.Y);

            if (denominator == 0)
            {
                return false;
            }

            var ta = numerator1 / denominator;
            var tb = numerator2 / denominator;

            return ta.InRange(0, 1) && tb.InRange(0, 1);
        }
    }
}
