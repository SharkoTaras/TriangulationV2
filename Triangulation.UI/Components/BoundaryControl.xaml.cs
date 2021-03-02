using System.Windows.Controls;
using Triangulation.Core.Interfaces.Entities;

namespace Triangulation.UI
{
    /// <summary>
    /// Interaction logic for Boundary.xaml
    /// </summary>
    public partial class BoundaryControl : UserControl
    {
        public BoundaryControl()
        {
            this.InitializeComponent();
            this.DataContext = this;
        }

        public Vertex TopLeft { get; set; }

        public Vertex TopRight { get; set; }

        public Vertex BottomRight { get; set; }

        public Vertex BottomLeft { get; set; }

        public Boundary Boundary
        {
            get
            {
                return new Boundary(this.BottomLeft, this.BottomRight, this.TopRight, this.TopLeft);
            }
        }
    }
}
