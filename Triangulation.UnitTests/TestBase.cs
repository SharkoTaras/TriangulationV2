using Microsoft.VisualStudio.TestTools.UnitTesting;
using Unity;

namespace Triangulation.UnitTests
{
    public class TestBase
    {
        protected IUnityContainer Container;

        [TestInitialize]
        public virtual void TestInitialize()
        {
            this.Container = new UnityContainer();
        }
    }
}
