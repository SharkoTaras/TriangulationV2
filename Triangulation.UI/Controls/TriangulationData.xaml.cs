using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Triangulation.Core.Algorithms.Equations.Services;
using Triangulation.Core.Interfaces.Entities;
using Triangulation.Core.Interfaces.Extensions;

namespace Triangulation.UI.Controls
{
    /// <summary>
    /// Interaction logic for TriangulationData.xaml
    /// </summary>
    public partial class TriangulationData : Window
    {
        /// <summary>
        /// Collection of vertices (program data)
        /// </summary>
        private IEnumerable<Vertex> _vertices;
        private List<Triangle> _triangles;
        private ProblemService _problemService;

        public MatrixService MatrixService { get; private set; }

        //private ProblemService _problemService;

        //private IterationSystemService _iterationSystemService;

        /// <summary>
        /// Window constructor, receiving vertices collection
        /// </summary>
        /// <param name="vertices"></param>
        public TriangulationData(IEnumerable<Vertex> vertices, List<Triangle> triangles, Boundary boundary)
        {
            InitializeComponent();
            // this._problemService = new ProblemService(vertices, triangles, boundary);
            DataContext = this;
            _vertices = vertices;
            _triangles = triangles;
            dataGrid.ItemsSource = _vertices;
            _problemService = new ProblemService(_vertices, _triangles, new Boundary((0, 0), (2, 0), (2, 2), (0, 2)));
            MatrixService = new MatrixService(vertices.ToVertexCollection(), triangles);

            InitData();
        }

        /// <summary>
        /// Default window constructor
        /// </summary>
        public TriangulationData() => InitializeComponent();

        public string TriangulaitonData { get; set; }

        private async void InitData()
        {
            await Task.Delay(1);

            Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;

            MatrixService.UpdateSystem();

            var answer = _problemService.SolveProblem(MatrixService.GlobalStiffnessMatrix, MatrixService.GlobalForceVector);

            var sb = new StringBuilder();
            if (_problemService.MatrixA.ColumnCount <= 10)
            {
                sb.AppendLine("Problem matrix:");
                sb.AppendLine(_problemService.MatrixA.ToString());
            }
            if (_problemService.VectorF.Count <= 10)
            {
                sb.AppendLine("Problem vector:");
                sb.AppendLine(_problemService.VectorF.ToString());
            }

            sb.AppendLine("Answer:");
            sb.AppendJoin(" ", answer);
            sb.AppendLine();
            Data.Text = sb.ToString();
        }
    }
}
