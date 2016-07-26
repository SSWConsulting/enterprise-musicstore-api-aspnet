using SSW.DataOnion.Core;
using SSW.DataOnion.Interfaces;
using SSW.MusicStore.Data.Entities;

namespace SSW.MusicStore.Data.Repositories
{
    public class AlbumRepository : BaseRepository<Album, MusicStoreContext>
    {
        public AlbumRepository(IAmbientDbContextLocator dbContextScopeLocator)
            : base(dbContextScopeLocator)
        {
        }
    }

    public class ArtistRepository : BaseRepository<Artist, MusicStoreContext>
    {
        public ArtistRepository(IAmbientDbContextLocator dbContextScopeLocator)
            : base(dbContextScopeLocator)
        {
        }
    }

    public class CartRepository : BaseRepository<Cart, MusicStoreContext>
    {
        public CartRepository(IAmbientDbContextLocator dbContextScopeLocator)
            : base(dbContextScopeLocator)
        {
        }
    }

    public class CartItemRepository : BaseRepository<CartItem, MusicStoreContext>
    {
        public CartItemRepository(IAmbientDbContextLocator dbContextScopeLocator)
            : base(dbContextScopeLocator)
        {
        }
    }

    public class GenreRepository : BaseRepository<Genre, MusicStoreContext>
    {
        public GenreRepository(IAmbientDbContextLocator dbContextScopeLocator)
            : base(dbContextScopeLocator)
        {
        }
    }

    public class OrderRepository : BaseRepository<Order, MusicStoreContext>
    {
        public OrderRepository(IAmbientDbContextLocator dbContextScopeLocator)
            : base(dbContextScopeLocator)
        {
        }
    }

    public class OrderDetailRepository : BaseRepository<OrderDetail, MusicStoreContext>
    {
        public OrderDetailRepository(IAmbientDbContextLocator dbContextScopeLocator)
            : base(dbContextScopeLocator)
        {
        }
    }
}
