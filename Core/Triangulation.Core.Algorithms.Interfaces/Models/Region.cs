using System.Collections.Generic;
using Triangulation.Core.Algorithms.Interfaces.Enums;
using Triangulation.Core.Interfaces.Entities;

namespace Triangulation.Core.Algorithms.Interfaces.Models
{
    public class Region : IRegion
    {
        #region Constructors
        public Region()
        {
        }

        public Region(IEnumerable<IEntity> entities, RegionInitializingType initializingType)
        {
            this.Entities = entities;
            this.InitializingType = initializingType;
        }
        #endregion

        #region Properties
        public readonly RegionInitializingType InitializingType;

        public IEnumerable<IEntity> Entities { get; }
        #endregion
    }
}
