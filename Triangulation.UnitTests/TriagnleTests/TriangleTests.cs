using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Triangulation.Core.Interfaces.Entities;

namespace Triangulation.UnitTests.TriagnleTests
{
    [TestClass]
    public class TriangleTests
    {
        [TestMethod]
        public void UpdateCircleTest()
        {
            var triangle = new Triangle((0, 0), (1, 1), (1, 0));
            var watch = new Stopwatch();
            watch.Start();
            triangle.UpdateCircumcircleDefault();
            Debug.WriteLine($"Default time: {watch.ElapsedMilliseconds}");
            watch.Stop();
            watch.Reset();

            watch.Start();
            triangle.UpdateCircumcircleVector();
            Debug.WriteLine($"New time time: {watch.ElapsedMilliseconds}");
            watch.Stop();
        }
    }
}
