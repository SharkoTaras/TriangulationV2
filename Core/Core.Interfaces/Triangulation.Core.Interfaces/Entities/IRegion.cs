using System.Collections.Generic;
using Triangulation.Core.Interfaces.Entities;

namespace Triangulation.Core.Algorithms.Interfaces.Models
{
    public interface IRegion
    {
        IEnumerable<IEntity> Entities { get; }
    }
}