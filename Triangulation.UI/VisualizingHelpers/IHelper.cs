using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Shapes;
using Triangulation.Core.Algorithms.Interfaces.Models.Dtos;
using Triangulation.Core.Interfaces.Entities;
using Triangulation.Core.Interfaces.Entities.Dtos;

namespace Triangulation.UI.VisualizingHelpers
{
    public interface IHelper
    {
        ConcurrentDictionary<Vertex, Tuple<Ellipse, TextBlock>> VerticesOnCanvas { get; set; }
        ConcurrentDictionary<Edge, Line> LinesOnCanvas { get; set; }

        Task Draw(CanvasRegion region);
        Task Draw(CanvasVertex canvasVertex);
        Task Draw(IEnumerable<Vertex> vertices);
        Task DrawLine(IEnumerable<Vertex> vertices);
        Task DrawLine(CanvasEdge canvasEdge);
        Task Remove(IEnumerable<Tuple<Ellipse, TextBlock>> vertices);
        Task Remove(IEnumerable<Line> lines);
        Task RemoveAllVertices();
        Task RemoveAllLines();
        Task RemoveAll();
    }
}
