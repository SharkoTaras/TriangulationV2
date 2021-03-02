using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;
using Triangulation.Core.Algorithms.Implementation.Algorithms;
using Triangulation.Core.Algorithms.Interfaces.Algorithms;
using Triangulation.Core.Algorithms.Interfaces.Enums;
using Triangulation.Core.Algorithms.Interfaces.Models;
using Triangulation.Core.Algorithms.Interfaces.Models.Dtos;
using Triangulation.Core.Interfaces.Entities;
using Triangulation.Core.Interfaces.Entities.Dtos;
using Triangulation.Core.Interfaces.Extensions;
using Triangulation.Core.Interfaces.Services;
using Triangulation.Core.Interfaces.Utils.Comparers;
using Triangulation.Services;
using Triangulation.UI.Controls;
using Triangulation.UI.ViewModels.Base;
using Triangulation.UI.VisualizingHelpers;

namespace Triangulation.UI.ViewModels
{
    /// <summary>
    /// View model for triangulation process
    /// </summary>
    internal sealed class TriangulationViewModel : ViewModelBase
    {
        #region Private fields
        private Canvas canvas;
        private readonly IHelper canvasHelper;
        private readonly IAlgorithm algorithm;
        private readonly IController controller;
        private readonly CommandInitializer initializer;
        private readonly IService<Region> regionService;
        private ConcurrentDictionary<Edge, Line> linesOnCanvas;
        private ConcurrentDictionary<Vertex, Tuple<Ellipse, TextBlock>> verticesOnCanvas;
        private Region region;
        private uint numberOfPoints;
        private bool triangulated;
        private bool triangulationIsRunning;
        #endregion

        #region Constructor
        public TriangulationViewModel(Canvas canvas, IService<Region> regionService, IController controller)
        {
            this.canvas = canvas;
            this.regionService = regionService;
            this.controller = controller;
            this.region = this.regionService.Initialize();

            this.algorithm = new DelaunayAlgorithm();
            this.linesOnCanvas = new ConcurrentDictionary<Edge, Line>();
            this.verticesOnCanvas = new ConcurrentDictionary<Vertex, Tuple<Ellipse, TextBlock>>();
            this.canvasHelper = new CanvasHelper(this.canvas, this.CurrentZoom, this.verticesOnCanvas, this.linesOnCanvas);
            this.initializer = new CommandInitializer(this);

            //this.Initialize();
            this.initializer.Initialize();
            this.NumberOfPoints = 1;
        }
        #endregion

        #region Properties
        public ushort CurrentZoom { get; set; } = 190;

        public double CanvasWidth
        {
            get
            {
                return 1280;
            }
        }

        public double CanvasHeight
        {
            get
            {
                return 1000;
            }
        }

        public uint NumberOfPoints
        {
            get
            {
                return this.numberOfPoints;
            }

            set
            {
                if (this.numberOfPoints != value)
                {
                    this.numberOfPoints = value;
                    this.OnProtertyChanged();
                    this.Triangulated = false;
                    Task.Run(async () =>
                    {
                        await this.RemoveAll();

                        var canvasRegion = new CanvasRegion() { PrimaryRegion = this.region, Zoom = this.CurrentZoom };
                        await this.canvasHelper.Draw(canvasRegion);

                        if (this.region.InitializingType == RegionInitializingType.Vertex)
                        {
                            await this.canvasHelper.DrawLine((IEnumerable<Vertex>)canvasRegion.Entities);
                        }

                        await this.PrepareAndDrawVertices();
                    });
                }
            }
        }
        #endregion

        #region Commands
        public ICommand DrawVerticesForTriangulationCommand { get; set; }

        public ICommand TriangulateCommand { get; set; }

        public ICommand ShowDataCommand { get; set; }

        public bool TriangulationIsRunning
        {
            get
            {
                return this.triangulationIsRunning;
            }

            private set
            {
                if (this.triangulationIsRunning != value)
                {
                    this.triangulationIsRunning = value;
                    Debug.WriteLine($"{nameof(this.TriangulationIsRunning)} set to: {value}");
                }
            }
        }

        public bool Triangulated
        {
            get
            {
                return this.triangulated;
            }

            private set
            {
                if (this.triangulated != value)
                {
                    this.triangulated = value;
                    Debug.WriteLine($"{nameof(this.Triangulated)} set to: {value}");
                }
            }
        }
        #endregion

        #region Remove
        public async Task RemoveAll()
        {
            await this.canvasHelper.RemoveAll();
            this.verticesOnCanvas = this.canvasHelper.VerticesOnCanvas;
            this.linesOnCanvas = this.canvasHelper.LinesOnCanvas;
        }

        public async Task RemoveVertices()
        {
            await this.canvasHelper.RemoveAllVertices();
            this.verticesOnCanvas = this.canvasHelper.VerticesOnCanvas;
        }

        public async Task RemoveLines()
        {
            await this.canvasHelper.RemoveAllLines();
            this.linesOnCanvas = this.canvasHelper.LinesOnCanvas;
        }
        #endregion

        public async Task PrepareAndDrawVertices()
        {
            var vertices = this.controller.PrepareVertices(this.NumberOfPoints).Result;

            Debug.WriteLine(new string('-', 20));
            Debug.WriteLine(nameof(PrepareAndDrawVertices));
            Debug.WriteLine($"Vertices count: {vertices.Count()}");
            vertices = vertices.DistinctVertex();

            Debug.WriteLine($"Vertices count after distinct: {vertices.Count()}");

            await this.canvasHelper.Draw(vertices);
            this.verticesOnCanvas = this.canvasHelper.VerticesOnCanvas;
            Debug.WriteLine($"Vertices on canvas: {this.verticesOnCanvas.Count}");
            Debug.WriteLine(new string('-', 20));
        }

        public async void Triangulate()
        {
            Debug.WriteLine(new string('-', 20));
            Debug.WriteLine($"Start {nameof(Triangulate)}");
            if (this.Triangulated == false)
            {
                Debug.WriteLine($"{nameof(this.Triangulated)}: {this.Triangulated}");
                if (this.TriangulationIsRunning == false)
                {
                    Debug.WriteLine($"{nameof(this.TriangulationIsRunning)}: {this.TriangulationIsRunning}");
                    this.TriangulationIsRunning = true;

                    var lines = await Task.Run(() =>
                    {
                        Debug.WriteLine($"Start triangulation algorithm");
                        var sw = new Stopwatch();
                        this.algorithm.Triangles = null;
                        sw.Start();
                        var result = this.algorithm.Triangulate(this.verticesOnCanvas.Keys.DistinctVertex().ToVertexCollection());
                        sw.Stop();
                        Debug.WriteLine($"Triangulate time: {sw.ElapsedMilliseconds}");
                        Debug.WriteLine($"Triangles count: {this.algorithm.Triangles.Count}");
                        return result;
                    });

                    var triangles = this.algorithm.Triangles;
                    Debug.WriteLine($"Start optimization algorithm");
                    var sw = new Stopwatch();
                    sw.Start();
                    var newTriangles = await TriangulationOptimization.Optimize(new Boundary((0, 0), (2, 0), (2, 2), (0, 2)), triangles);
                    sw.Stop();

                    Debug.WriteLine($"Optimization time: {sw.ElapsedMilliseconds}");

                    this.Triangulated = true;

                    Debug.WriteLine($"Start gettin all edges from triangles");
                    var edges = new List<Edge>();

                    sw.Restart();
                    sw.Start();
                    foreach (var t in newTriangles)
                    {
                        if (!edges.Contains(t.First, new EdgeComparer()))
                        {
                            edges.Add(t.First);
                        }
                        if (!edges.Contains(t.Second, new EdgeComparer()))
                        {
                            edges.Add(t.Second);
                        }
                        if (!edges.Contains(t.Third, new EdgeComparer()))
                        {
                            edges.Add(t.Third);
                        }
                    }

                    sw.Stop();

                    Debug.WriteLine($"Getting edges time: {sw.ElapsedMilliseconds}");

                    foreach (var line in edges)
                    {
                        await this.canvasHelper.DrawLine(new CanvasEdge(line, this.CurrentZoom));
                    }
                    this.TriangulationIsRunning = false;
                }
                else
                {
                    Debug.WriteLine($"{nameof(this.TriangulationIsRunning)}: {this.TriangulationIsRunning}");
                    Debug.WriteLine($"SKIP TRIANGULATE COMMAND");
                }
            }
            else
            {
                Debug.WriteLine($"{nameof(this.Triangulated)}: {this.Triangulated}");
                Debug.WriteLine($"SKIP TRIANGULATE COMMAND");
            }
            Debug.WriteLine($"End {nameof(Triangulate)}");
            Debug.WriteLine(new string('-', 20));
        }

        public async Task ShowData()
        {
            var wnd = new TriangulationData(this.verticesOnCanvas.Keys, this.algorithm.Triangles, new Boundary((0, 0), (2, 0), (2, 2), (0, 2)));
            wnd.Show();
        }

        #region Private Methods
        private void Initialize()
        {
            this.region = this.regionService.Initialize();
            var canvasRegion = new CanvasRegion() { PrimaryRegion = this.region, Zoom = this.CurrentZoom };

            this.canvasHelper.Draw(canvasRegion);

            if (this.region.InitializingType == RegionInitializingType.Vertex)
            {
                this.canvasHelper.DrawLine((IEnumerable<Vertex>)canvasRegion.Entities);
            }
        }
        #endregion
    }
}
