namespace Triangulation.Core.Interfaces.Services
{
    public interface IReader<TEntity>
    {
        string EntityName { get; }

        TEntity Read();
    }
}
