using Triangulation.UI.ViewModels;
using Triangulation.UI.ViewModels.Base;

namespace Triangulation.Services
{
    internal class CommandInitializer
    {
        public CommandInitializer(TriangulationViewModel model)
        {
            this.Model = model;
        }

        public TriangulationViewModel Model { get; }

        public void Initialize()
        {
            this.Model.DrawVerticesForTriangulationCommand = new RelayCommand(() =>
            {
                this.Model.PrepareAndDrawVertices();
            });

            this.Model.TriangulateCommand = new RelayCommand(() =>
            {
                this.Model.Triangulate();
            });

            this.Model.ShowDataCommand = new RelayCommand(() =>
            {
                this.Model.ShowData();
            });
        }
    }
}
