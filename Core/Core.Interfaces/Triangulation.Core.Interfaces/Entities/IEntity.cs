using Triangulation.Core.Interfaces.Utils;

namespace Triangulation.Core.Interfaces.Entities
{
    public interface IEntity : ISerializable
    {
        uint Id { get; }
    }
}
