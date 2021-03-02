using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Windows.Controls;
using Triangulation.Core.Interfaces.Entities;
using System.Windows.Shapes;
using System.Windows;
using System.Windows.Media;
using System;
using Triangulation.Core.Interfaces.Entities.Dtos;
using Triangulation.Core.Algorithms.Interfaces.Enums;
using System.Diagnostics;
using System.Collections.Generic;
using Triangulation.Core.Algorithms.Interfaces.Models.Dtos;
using System.Windows.Media.Animation;
using System.Linq;

namespace Triangulation.UI.VisualizingHelpers
{
    public class CanvasHelper : IHelper
    {
        #region Constructor
        public CanvasHelper(
            Canvas canvas,
            uint zoom,
            ConcurrentDictionary<Vertex, Tuple<Ellipse, TextBlock>> verticesOnCanvas,
            ConcurrentDictionary<Edge, Line> linesOnCanvas)
        {
            this.Canvas = canvas;
            this.Zoom = zoom;
            this.VerticesOnCanvas = verticesOnCanvas;
            this.LinesOnCanvas = linesOnCanvas;
        }
        #endregion

        #region Properties
        public ConcurrentDictionary<Vertex, Tuple<Ellipse, TextBlock>> VerticesOnCanvas { get; set; }
        public ConcurrentDictionary<Edge, Line> LinesOnCanvas { get; set; }
        public readonly uint Zoom;
        public Canvas Canvas;

        public double EllipseSize { get; private set; } = 5;
        public double AnimationDuration { get; private set; } = 10;
        #endregion

        public async Task Draw(CanvasRegion region)
        {
            await Application.Current.Dispatcher.InvokeAsync(async () =>
            {
                switch (region.InitializingType)
                {
                    case RegionInitializingType.Vertex:
                        {
                            foreach (var entity in region.Entities)
                            {
                                var v = entity as Vertex;
                                await this.Draw(new CanvasVertex(v, region.Zoom));
                            }
                        }
                        break;
                    case RegionInitializingType.Edge:
                        throw new NotImplementedException();
                        break;
                    case RegionInitializingType.Triangle:
                        throw new NotImplementedException();
                        break;

                }
            });
        }

        public async Task Draw(CanvasVertex canvasVertex)
        {
            await Application.Current.Dispatcher.InvokeAsync(() =>
           {
               var ellipse = new Ellipse
               {
                   Width = EllipseSize,
                   Height = EllipseSize,
                   Fill = Brushes.Red
               };

               var tb = new TextBlock
               {
                   Text = canvasVertex.Id.ToString(),
                   Background = Brushes.Transparent,
                   FontSize = 15,
               };

               this.SetOnCanvas(ellipse, canvasVertex);
               this.SetOnCanvas(tb, canvasVertex);

               this.VerticesOnCanvas.TryAdd(canvasVertex.PrimaryVertex, new Tuple<Ellipse, TextBlock>(ellipse, tb));

               this.Canvas.Children.Add(ellipse);
               this.Canvas.Children.Add(tb);
           });
        }

        public async Task Draw(IEnumerable<Vertex> canvasVertex)
        {
            if (canvasVertex is null)
            {
                return;
            }

            foreach (var v in canvasVertex)
            {
                await this.Draw(new CanvasVertex(v, this.Zoom));
            }

        }
        public async Task DrawLine(IEnumerable<Vertex> vertices)
        {
            var v = vertices.ToList();

            var first = v.FirstOrDefault();
            var last = v.LastOrDefault();
            for (var i = 0; i < v.Count - 1; i++)
            {
                var canvasEdge = new CanvasEdge(new Edge(v[i], v[i + 1]), this.Zoom);
                await this.DrawLine(canvasEdge);
            }
            await this.DrawLine(new CanvasEdge(new Edge(last, first), this.Zoom));
        }

        public async Task DrawLine(CanvasEdge canvasEdge)
        {
            await Application.Current.Dispatcher.InvokeAsync(() =>
            {
                var line = new Line()
                {
                    X1 = canvasEdge.Start.X + (this.Canvas.Width / 2),
                    Y1 = canvasEdge.Start.Y + (this.Canvas.Height / 2),
                    X2 = canvasEdge.End.X + (this.Canvas.Width / 2),
                    Y2 = canvasEdge.End.Y + (this.Canvas.Height / 2),
                    Stroke = Brushes.DarkViolet,
                };

                this.LinesOnCanvas.TryAdd(canvasEdge.PrimaryEdge, line);

                var lineAnimationX = new DoubleAnimation
                {
                    From = line.X1,
                    To = line.X2,
                    Duration = TimeSpan.FromMilliseconds(this.AnimationDuration)
                };
                var lineAnimationY = new DoubleAnimation
                {
                    From = line.Y1,
                    To = line.Y2,
                    Duration = TimeSpan.FromMilliseconds(this.AnimationDuration),
                };

                this.Canvas.Children.Add(line);
                line.BeginAnimation(Line.X2Property, lineAnimationX);
                line.BeginAnimation(Line.Y2Property, lineAnimationY);
            });
        }

        #region Remove
        public async Task Remove(IEnumerable<Tuple<Ellipse, TextBlock>> entities)
        {
            await Application.Current.Dispatcher.InvokeAsync(() =>
            {
                foreach (var v in entities)
                {
                    this.Canvas.Children.Remove(v.Item1);
                    this.Canvas.Children.Remove(v.Item2);
                }
            });
        }

        public async Task Remove(IEnumerable<Line> lines)
        {
            await Application.Current.Dispatcher.InvokeAsync(() =>
            {
                foreach (var l in lines)
                {
                    this.Canvas.Children.Remove(l);
                }
            });
        }

        public async Task RemoveAllVertices()
        {
            await Application.Current.Dispatcher.InvokeAsync(() =>
            {
                foreach (var v in this.VerticesOnCanvas)
                {
                    this.Canvas.Children.Remove(v.Value.Item1);
                    this.Canvas.Children.Remove(v.Value.Item2);
                }
                this.VerticesOnCanvas.Clear();
            });
        }

        public async Task RemoveAllLines()
        {
            await Application.Current.Dispatcher.InvokeAsync(() =>
            {
                foreach (var l in this.LinesOnCanvas)
                {
                    this.Canvas.Children.Remove(l.Value);
                }
                this.LinesOnCanvas.Clear();
            });
        }

        public async Task RemoveAll()
        {
            await Application.Current.Dispatcher.InvokeAsync(() =>
            {
                this.Canvas.Children.Clear();
                this.LinesOnCanvas.Clear();
                this.VerticesOnCanvas.Clear();
            });
        }
        #endregion

        #region Helper Methods
        private void SetOnCanvas(FrameworkElement element, CanvasVertex canvasVertex)
        {
            Application.Current.Dispatcher.InvokeAsync(() =>
            {
                Canvas.SetLeft(element, (this.Canvas.Width / 2) + canvasVertex.X - (element.Height / 2));
                Canvas.SetTop(element, (this.Canvas.Height / 2) + canvasVertex.Y - (element.Height / 2));
            });
        }
        #endregion
    }
}
