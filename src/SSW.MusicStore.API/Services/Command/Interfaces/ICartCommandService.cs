using System.Threading;
using System.Threading.Tasks;

using SSW.MusicStore.Data.Entities;

namespace SSW.MusicStore.API.Services.Command.Interfaces
{
    public interface ICartCommandService
    {
        /// <summary>
        /// Empties the cart.
        /// </summary>
        /// <param name="cartId">The cart identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task EmptyCart(string cartId, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Creates the order from cart.
        /// </summary>
        /// <param name="cartId">The cart identifier.</param>
        /// <param name="order">The order.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>OrderId as the confirmation numbe</returns>
        Task<int> CreateOrderFromCart(string cartId, Order order, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Adds album to existing cart.
        /// </summary>
        /// <param name="cartId">The cart identifier.</param>
        /// <param name="album">The album.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task AddToCart(string cartId, Album album, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Removes the cart item.
        /// </summary>
        /// <param name="cartItemId">The cart item identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Number of items left in the cart</returns>
        Task<int> RemoveCartItem(int cartItemId, CancellationToken cancellationToken = default(CancellationToken));
    }
}
