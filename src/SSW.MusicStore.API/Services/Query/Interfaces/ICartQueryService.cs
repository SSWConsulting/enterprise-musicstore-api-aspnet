using System.Threading;
using System.Threading.Tasks;

using SSW.MusicStore.API.Models;
using System.Collections.Generic;

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

		List<Order> GetOrders(string userId, CancellationToken cancellationToken = default(CancellationToken));

	}
}
