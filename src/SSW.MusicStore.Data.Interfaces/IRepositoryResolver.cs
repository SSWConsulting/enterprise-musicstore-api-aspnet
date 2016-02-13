namespace SSW.MusicStore.Data.Interfaces
{
    public interface IRepositoryResolver
    {
        IRepository<TEntity> Resolve<TEntity>() where TEntity : class;
    }
}
