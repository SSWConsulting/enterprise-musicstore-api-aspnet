namespace SSW.MusicStore.Data.Interfaces
{
    public interface ISoftDeletableEntity
    {
        bool IsDeleted { get; set; }
    }
}
