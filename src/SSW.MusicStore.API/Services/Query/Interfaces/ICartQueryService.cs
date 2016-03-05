using System.Threading;
using System.Threading.Tasks;

using System.Collections.Generic;

using SSW.MusicStore.Data.Entities;

namespace SSW.MusicStore.API.Services.Query.Interfaces
{
    public interface ICartQueryService
    {
        /// <summary>
        /// Gets the shopping cart by user id.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        /// Shopping cart for the specified user
        /// </returns>
        Task<Cart> GetCart(string userId, CancellationToken cancellationToken = default(CancellationToken));

        Task<List<Order>> GetOrders(string userId, CancellationToken cancellationToken = default(CancellationToken));

	}
}
