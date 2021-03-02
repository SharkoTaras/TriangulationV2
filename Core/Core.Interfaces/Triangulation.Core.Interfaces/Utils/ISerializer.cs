namespace Triangulation.Core.Interfaces.Utils
{
    public interface ISerializer<TObject>
        where TObject : ISerializable, new()
    {
        string Serialize(TObject obj);
        TObject Deserialize(string str);
    }
}
