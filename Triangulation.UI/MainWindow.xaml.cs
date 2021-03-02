using System;
using System.IO;
using System.Windows;
using Triangulation.Core.Algorithms.Implementation.Algorithms;
using Triangulation.Core.Algorithms.Interfaces.Algorithms;
using Triangulation.Core.Algorithms.Interfaces.Models;
using Triangulation.Core.Implementation.Services;
using Triangulation.Core.Interfaces;
using Triangulation.Core.Interfaces.Services;
using Triangulation.UI.ViewModels;
using Unity;

namespace Triangulation.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private IUnityContainer container;

        public MainWindow()
        {
            this.InitializeComponent();
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            this.container = new UnityContainer();
            var initializer = new VertexRegionInitializer(new StreamReader(GlobalConstants.FilesPath.Region));
            initializer.Initialize(this.container);
            this.container.RegisterType<IService<Region>, RegionService>();
            this.container.RegisterFactory<IRegion>((c) =>
            {
                return c.Resolve<IService<Region>>().Initialize();
            });
            this.container.RegisterType<IController, TriangulationController>();
        }
        protected override void OnContentRendered(EventArgs e)
        {
            base.OnContentRendered(e);
            this.DataContext = new TriangulationViewModel(
                 this.Canvas,
                 this.container.Resolve<IService<Region>>(),
                 this.container.Resolve<IController>());
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
        }
    }
}
