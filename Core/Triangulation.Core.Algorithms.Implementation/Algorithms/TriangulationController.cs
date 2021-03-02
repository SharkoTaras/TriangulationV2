using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Triangulation.Core.Algorithms.Interfaces.Algorithms;
using Triangulation.Core.Algorithms.Interfaces.Models;
using Triangulation.Core.Interfaces.Entities;
using Triangulation.Core.Interfaces.Utils.Comparers;
using Unity;

namespace Triangulation.Core.Algorithms.Implementation.Algorithms
{
    public class TriangulationController : IController
    {
        [Dependency]
        public IRegion Region { get; set; }

        public IEnumerable<Vertex> Vertices { get; private set; }

        public async Task<IEnumerable<Vertex>> PrepareVertices(uint count)
        {
            var minX = 0;
            var maxX = 2;

            var minY = 0;
            var maxY = 2;

            double dx;
            double dy;

            if (count == 0)
            {
                dx = 0d;
                dy = 0d;
            }
            else
            {
                dx = (double)(maxX - minX) / (count + 1);
                dy = (double)(maxY - minY) / (count + 1);
            }

            Debug.WriteLine(new string('-', 20));
            Debug.WriteLine(nameof(PrepareVertices));
            Debug.WriteLine("Start creating vertices");
            Debug.WriteLine($"Need {count} vertices");
            var result = new ConcurrentBag<Vertex>();
            var entities = this.Region.Entities as IEnumerable<Vertex>;

            var sw = new Stopwatch();
            for (var i = 0; i < count + 2; i++)
            {
                for (var j = 0; j < count + 2; j++)
                {
                    var vertex = new Vertex(minX + (i * dx), minY + (j * dy));
                    if (!entities.Contains(vertex, new VertexComparer()))
                    {
                        result.Add(vertex);
                    }
                }
            }
            sw.Stop();
            Debug.WriteLine($"Preparing duration: {sw.ElapsedMilliseconds}");
            Debug.WriteLine($"Vertices count: {result.Count}");
            Debug.WriteLine(new string('-', 20));
            return await Task.FromResult(result);
            #region Todo
            // TODO: Make it parallel

            //var tcs = new TaskCompletionSource<IEnumerable<Vertex>>();
            //Parallel.For<ConcurrentBag<Vertex>>(0, count + 2, () => new ConcurrentBag<Vertex>(), (i, loop, bag) =>
            //{
            //    for (var j = 0; j < count + 2; j++)
            //    {
            //        var vertex = new Vertex(minX + (i * dx), minY + (j * dy));
            //        if (!entities.Contains(vertex, new VertexComparer()))
            //        {
            //            bag.Add(vertex);
            //        }
            //    }
            //    return bag;
            //},
            //(x) =>
            //{
            //    result = new ConcurrentBag<Vertex>(x);
            //});

            //tcs.SetResult(result);
            //var task = tcs.Task; 
            #endregion
        }
    }
}
