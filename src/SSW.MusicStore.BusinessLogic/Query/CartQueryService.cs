using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Autofac.Features.OwnedInstances;
using Microsoft.Data.Entity;
using SSW.DataOnion.Interfaces;
using SSW.MusicStore.BusinessLogic.Interfaces.Query;
using SSW.MusicStore.Data.Entities;

namespace SSW.MusicStore.BusinessLogic.Query
{
	public class CartQueryService : ICartQueryService
	{
        private readonly Func<Owned<IUnitOfWork>> unitOfWorkFunc;

        public CartQueryService(Func<Owned<IUnitOfWork>> unitOfWorkFunc)
        {
            this.unitOfWorkFunc = unitOfWorkFunc;
        }

        public async Task<Cart> GetCart(string userId, CancellationToken cancellationToken = default(CancellationToken))
		{
			Serilog.Log.Logger.Debug($"{nameof(GetCart)} for user id '{userId}'");
            using (var unitOfWork = this.unitOfWorkFunc())
            {
                var cart =
                    await
                        unitOfWork.Value.Repository<Cart>()
                            .Get(c => c.CartId == userId, c => c.CartItems)
                            .SingleOrDefaultAsync(cancellationToken);
                if (cart == null)
                {
                    // cart doesn't exist, create it
                    cart = new Cart { CartId = userId };
                    unitOfWork.Value.Repository<Cart>().Add(cart);
                    await unitOfWork.Value.SaveChangesAsync(cancellationToken);
                }
                else
                {
                    // HACK: .Include(c => c.CartItems.Select(ci => ci.Album)) EF7 doesn't support lazy loading yet and nested includes
                    // using a loop
                    foreach (var cartItem in cart.CartItems)
                    {
                        cartItem.Album =
                            await
                                unitOfWork.Value.Repository<Album>()
                                    .Get()
                                    .SingleOrDefaultAsync(a => a.AlbumId == cartItem.AlbumId, cancellationToken);
                    }
                }

                return cart;
            }
		}

		public async Task<List<Order>> GetOrders(string userId, CancellationToken cancellationToken = default(CancellationToken))
		{
            using (var unitOfWork = this.unitOfWorkFunc())
            {
                var orders = await unitOfWork.Value.Repository<Order>().Get(o => o.Username == userId).ToListAsync(cancellationToken);
				return orders;
			}
		}
	}

}
