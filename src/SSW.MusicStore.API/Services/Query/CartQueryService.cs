using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Microsoft.Data.Entity;
using Microsoft.Extensions.DependencyInjection;
using SSW.MusicStore.API.Models;
using SSW.MusicStore.API.Services.Query.Interfaces;

namespace SSW.MusicStore.API.Services.Query
{
	public class CartQueryService : ICartQueryService
	{
		private readonly IDbContextFactory<MusicStoreContext> _dbContextFactory;

		public CartQueryService(IDbContextFactory<MusicStoreContext> dbContextFactory)
		{
			_dbContextFactory = dbContextFactory;
		}

		public async Task<Cart> GetCart(string userId, CancellationToken cancellationToken = default(CancellationToken))
		{
			Serilog.Log.Logger.Debug($"{nameof(GetCart)} for user id '{userId}'");
			using (var dbContext = this._dbContextFactory.Create())
			{
				var cart =
					await
						dbContext.Carts.Include(c => c.CartItems)
							.SingleOrDefaultAsync(c => c.CartId == userId, cancellationToken);
				if (cart == null)
				{
					// cart doesn't exist, create it
					cart = new Cart { CartId = userId };
					dbContext.Carts.Add(cart);
					await dbContext.SaveChangesAsync(cancellationToken);
				}
				else
				{
					// HACK: .Include(c => c.CartItems.Select(ci => ci.Album)) doesnt seem to work
					// using a loop
					foreach (var cartItem in cart.CartItems)
					{
						cartItem.Album =
							await dbContext.Albums.SingleOrDefaultAsync(a => a.AlbumId == cartItem.AlbumId, cancellationToken);
					}
				}

				return cart;
			}
		}

		public List<Order> GetOrders(string userId, CancellationToken cancellationToken = default(CancellationToken))
		{
			using (var dbContext = this._dbContextFactory.Create())
			{
				var orders = dbContext.Orders
						.Where(o => o.Username == userId)
							.ToList();

				return orders;
			}
		}
	}

}
