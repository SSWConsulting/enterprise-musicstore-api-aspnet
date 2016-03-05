using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using SSW.MusicStore.API.ViewModels;
using Microsoft.AspNet.Authorization;

using System;

using Microsoft.Extensions.Logging;

using SSW.MusicStore.API.Helpers;
using SSW.MusicStore.API.Services.Command.Interfaces;
using SSW.MusicStore.API.Services.Query.Interfaces;
using SSW.MusicStore.Data.Entities;

namespace SSW.MusicStore.API.Controllers
{
	[Route("api")]
    public class ShoppingCartController : Controller
    {
		private readonly IServiceProvider _serviceProvider;
		private readonly IAlbumQueryService _albumQueryService;
        private readonly ICartQueryService _cartQueryService;
        private readonly ICartCommandService _cartCommandService;
        private readonly Microsoft.Extensions.Logging.ILogger _logger;

		public ShoppingCartController(
			ILoggerFactory loggerfactory,
			IServiceProvider serviceProvider,
			IAlbumQueryService albumQueryService,
            ICartQueryService cartQueryService,
            ICartCommandService cartCommandService)
		{
			_serviceProvider = serviceProvider;
			_albumQueryService = albumQueryService;
		    _cartQueryService = cartQueryService;
		    _cartCommandService = cartCommandService;
		    _logger = loggerfactory.CreateLogger(nameof(StoreController));
		}

        /// <summary>
        /// Gets the current shopping cart for logged in user.
        /// </summary>
        /// <returns>Cart object including shopping cart items and totals</returns>
        [Authorize(ActiveAuthenticationSchemes = "Bearer")]
        [HttpGet("cart")]
        public async Task<IActionResult> GetCurrentCart()
        {
            _logger.LogInformation("GET request for 'api/cart'");

            // Get current cart for the logged in user
            var viewModel = await GetCart();

            // Return the cart json
            return Json(viewModel);
        }

        /// <summary>
        /// Adds album to cart.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="requestAborted">The request aborted.</param>
        /// <returns>Cart object including shopping cart items and totals</returns>
        [Authorize(ActiveAuthenticationSchemes = "Bearer")]
        [HttpPost("cart/{id}")]
        public async Task<IActionResult> AddToCart(int id, CancellationToken requestAborted)
        {
            _logger.LogInformation("POST request for 'api/{albumId}'", id);

            // Retrieve the album from the database
            var addedAlbum = await _albumQueryService.GetAlbumDetails(id);

            // Add it to the shopping cart
            await _cartCommandService.AddToCart(GetCartId(), addedAlbum, requestAborted);

            // Return the cart json
            var viewModel = await GetCart();
            return Json(viewModel);
        }


		/// <summary>
		/// Gets the current orders for logged in user.
		/// </summary>
		/// <returns>Order object </returns>
		[Authorize(ActiveAuthenticationSchemes = "Bearer")]
		[HttpGet("order")]
		public IActionResult GetOrders(CancellationToken requestAborted)
		{
			_logger.LogInformation("GET request for 'api/order'");

			// Add it to the order
			var viewModel =  _cartQueryService.GetOrders(GetCartId(), requestAborted);

			// Return the order json
			return Json(viewModel);
		}

		/// <summary>Create order from cart.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <param name="requestAborted">The request aborted.</param>
		/// <returns>Cart object including shopping cart items and totals</returns>
		[Authorize(ActiveAuthenticationSchemes = "Bearer")]
		[HttpPost("order")]
		public async Task<IActionResult> CreateOrderFromCart([FromBody] OrderViewModel order, CancellationToken requestAborted)
		{
			_logger.LogInformation("POST request for 'api/order/'");

			var addedOrder = new Order
			{
				Address = order.Address,
				City = order.City,
				Country = order.Country,
				Email = order.Email,
				FirstName = order.FirstName,
				LastName = order.LastName,
				OrderDate = DateTime.Today,
				Phone = order.Phone,
				PostalCode = order.PostalCode,
				State = order.State,
				Username = GetCartId(),
				Total = 0
			};

			// Add it to the order
			var viewModel = await _cartCommandService.CreateOrderFromCart(GetCartId(), addedOrder, requestAborted);

			// Return the order json
			return Json(viewModel);
		}

		/// <summary>
		/// Empties the cart.
		/// </summary>
		/// <param name="requestAborted">The request aborted.</param>
		/// <returns>Cart object including shopping cart items and totals</returns>
		[Authorize(ActiveAuthenticationSchemes = "Bearer")]
        [HttpPost("cart/clear")]
        public async Task<IActionResult> EmptyCart(CancellationToken requestAborted)
        {
            _logger.LogInformation("POST request for 'api/clear'");

            // Clear shopping cart
            await this._cartCommandService.EmptyCart(GetCartId(), requestAborted);

            // Return the cart json
            var viewModel = await GetCart();
            return Json(viewModel);
        }

        /// <summary>
        /// Removes specified item from cart.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="requestAborted">The request aborted.</param>
        /// <returns>Cart object including shopping cart items and totals</returns>
        [Authorize(ActiveAuthenticationSchemes = "Bearer")]
        [HttpDelete("cart/{id}")]
        public async Task<IActionResult> RemoveFromCart(int id, CancellationToken requestAborted)
        {
            _logger.LogInformation("DELETE request for 'api/{cartItemId}'", id);

            // Remove item from the shopping cart
            await _cartCommandService.RemoveCartItem(id, requestAborted);

            // Return the cart json
            var viewModel = await GetCart();
            return Json(viewModel);
        }

        private async Task<ShoppingCartViewModel> GetCart()
        {
            var userId = GetCartId();
            var cart = await this._cartQueryService.GetCart(userId);

            // Set up our ViewModel
            var viewModel = new ShoppingCartViewModel
            {
                CartItems = cart.CartItems.Select(c => new CartItem
                {
                    Album = c.Album,
                    CartId = c.CartId,
                    Count = c.Count,
                    DateCreated = c.DateCreated,
                    CartItemId = c.CartItemId
                }).ToList(),
                CartTotal = cart.GetTotal()
            };
            return viewModel;
        }

		private string GetCartId()
        {
            var userId = this.User.Claims.GetNameIdentifier();
            if (userId.IsNullOrEmpty())
            {
                var message = "Could not find name identifier in user claims";
                _logger.LogError(message);
                throw new ApplicationException(message);
            }

            return userId;
        }
    }
}