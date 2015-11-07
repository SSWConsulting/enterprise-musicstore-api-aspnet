using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNet.Antiforgery;
using Microsoft.AspNet.Mvc;
using Microsoft.Data.Entity;
using Microsoft.Framework.Primitives;
using SSW.MusicStore.API.Models;
using SSW.MusicStore.API.ViewModels;
using Microsoft.AspNet.Authorization;
using SSW.MusicStore.API.Services.Query;
using System;
using Microsoft.Framework.Logging;
using System.Data.Common;
using System.Net;
using Serilog;

using SSW.MusicStore.API.Helpers;
using SSW.MusicStore.API.Services.Command.Interfaces;
using SSW.MusicStore.API.Services.Query.Interfaces;

namespace SSW.MusicStore.API.Controllers
{
    [Route("api")]
    public class ShoppingCartController : Controller
    {
		[FromServices]
		public MusicStoreContext DbContext { get; set; }

		private readonly IServiceProvider _serviceProvider;
		private readonly IAlbumQueryService _albumQueryService;
        private readonly ICartQueryService _cartQueryService;
        private readonly ICartCommandService _cartCommandService;
        private readonly Microsoft.Framework.Logging.ILogger _logger;

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