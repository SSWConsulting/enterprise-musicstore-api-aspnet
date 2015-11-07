using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Data.Entity;

using SSW.MusicStore.API.Models;
using SSW.MusicStore.API.Services.Command.Interfaces;

namespace SSW.MusicStore.API.Services.Command
{
    public class CartCommandService : ICartCommandService
    {
        private readonly IDbContextFactory<MusicStoreContext> _dbContextFactory;

        public CartCommandService(IDbContextFactory<MusicStoreContext> dbContextFactory)
        {
            this._dbContextFactory = dbContextFactory;
        }

        public async Task EmptyCart(string cartId, CancellationToken cancellationToken = new CancellationToken())
        {
            Serilog.Log.Logger.Debug($"{nameof(this.EmptyCart)} for cart id '{cartId}'");
            using (var dbContext = this._dbContextFactory.Create())
            {
                var cartItems = await dbContext.CartItems.Where(cart => cart.CartId == cartId).ToArrayAsync(cancellationToken);
                dbContext.CartItems.RemoveRange(cartItems);
                await dbContext.SaveChangesAsync(cancellationToken);
            }
        }

        public async Task SaveCart(string cartId, CancellationToken cancellationToken = new CancellationToken())
        {
            Serilog.Log.Logger.Debug($"{nameof(this.EmptyCart)} for cart id '{cartId}'");
            using (var dbContext = this._dbContextFactory.Create())
            {
                var cartItems = await dbContext.CartItems.Where(cart => cart.CartId == cartId).ToArrayAsync(cancellationToken);
                dbContext.CartItems.RemoveRange(cartItems);
                await dbContext.SaveChangesAsync(cancellationToken);
            }
        }

        public async Task<int> CreateOrderFromCart(string cartId, Order order, CancellationToken cancellationToken = new CancellationToken())
        {
            Serilog.Log.Logger.Debug($"{nameof(this.CreateOrderFromCart)} for cart id '{cartId}' and order dated {order.OrderDate.ToShortDateString()}");
            using (var dbContext = this._dbContextFactory.Create())
            {
                decimal orderTotal = 0;

                var cartItems =
                    await dbContext.CartItems.Where(cart => cart.CartId == cartId).Include(c => c.Album).ToListAsync(cancellationToken);

                // Iterate over the items in the cart, adding the order details for each
                foreach (var item in cartItems)
                {
                    //var album = _db.Albums.Find(item.AlbumId);
                    var album = await dbContext.Albums.SingleAsync(a => a.AlbumId == item.AlbumId, cancellationToken);

                    var orderDetail = new OrderDetail
                    {
                        AlbumId = item.AlbumId,
                        OrderId = order.OrderId,
                        UnitPrice = album.Price,
                        Quantity = item.Count,
                    };

                    // Set the order total of the shopping cart
                    orderTotal += (item.Count * album.Price);
                    dbContext.OrderDetails.Add(orderDetail);
                }

                // Set the order's total to the orderTotal count
                order.Total = orderTotal;

                // Empty the shopping cart
                var cartItemsToClear = await dbContext.CartItems.Where(cart => cart.CartId == cartId).ToArrayAsync(cancellationToken);
                dbContext.CartItems.RemoveRange(cartItemsToClear);

                // Save all the changes 
                await dbContext.SaveChangesAsync(cancellationToken);

                // Return the OrderId as the confirmation number
                return order.OrderId;
            }
        }

        public async Task AddToCart(string cartId, Album album, CancellationToken cancellationToken = default(CancellationToken))
        {
            Serilog.Log.Logger.Debug($"{nameof(this.AddToCart)} album '{album.Title}' for cart with id '{cartId}'");
            using (var dbContext = this._dbContextFactory.Create())
            {
                // Get the matching cart and album instances
                var cartItem =
                    await
                        dbContext.CartItems.SingleOrDefaultAsync(
                            c => c.CartId == cartId && c.AlbumId == album.AlbumId,
                            cancellationToken);

                if (cartItem == null)
                {
                    // Create a new cart item if no cart item exists
                    cartItem = new CartItem
                    {
                        AlbumId = album.AlbumId,
                        CartId = cartId,
                        Count = 1,
                        DateCreated = DateTime.Now
                    };

                    dbContext.CartItems.Add(cartItem);
                }
                else
                {
                    // If the item does exist in the cart, then add one to the quantity
                    cartItem.Count++;
                }

                await dbContext.SaveChangesAsync(cancellationToken);
            }
        }

        public async Task<int> RemoveCartItem(int cartItemId, CancellationToken cancellationToken = new CancellationToken())
        {
            Serilog.Log.Logger.Debug($"{nameof(this.RemoveCartItem)} for cart item with id '{cartItemId}'");
            using (var dbContext = this._dbContextFactory.Create())
            {
                // Get the cart
                var cartItem = await dbContext.CartItems.SingleOrDefaultAsync(c => c.CartItemId == cartItemId, cancellationToken);

                if (cartItem == null)
                {
                    var message = $"Cart item with id {cartItemId} could not be found.";
                    Serilog.Log.Logger.Error(message);
                    throw new ApplicationException(message);
                }

                var itemCount = 0;

                if (cartItem.Count > 1)
                {
                    cartItem.Count--;
                    itemCount = cartItem.Count;
                }
                else
                {
                    dbContext.CartItems.Remove(cartItem);
                }

                await dbContext.SaveChangesAsync(cancellationToken);
                return itemCount;
            }
        }
    }

}
