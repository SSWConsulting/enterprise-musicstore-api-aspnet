namespace SSW.MusicStore.Data.Interfaces
{
    public interface IRepositoryLocator
    {
        IRepository<TEntity> GetRepository<TEntity>() where TEntity : class;
    }
}
