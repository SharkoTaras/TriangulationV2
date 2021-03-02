namespace Triangulation.Core.Interfaces.Services
{
    public interface IService<TEntity>
    {
        string ServiceEntity { get; }

        public TEntity Initialize();
    }
}
