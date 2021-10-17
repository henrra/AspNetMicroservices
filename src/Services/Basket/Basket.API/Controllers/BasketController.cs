using System;
using System.Net;
using System.Threading.Tasks;
using Basket.API.Entities;
using Basket.API.Repositories;
using Basket.API.Services;
using Discount.Grpc.Protos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Basket.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class BasketController : ControllerBase
    {
        private readonly ILogger<BasketController> _logger;
        private readonly IBasketRepository _basketRepository;
        private readonly DiscountGrpcService _discountGrpcService;

        public BasketController(ILogger<BasketController> logger, IBasketRepository basketRepository,
            DiscountGrpcService discountGrpcService)
        {
            _logger = logger;
            _basketRepository = basketRepository ?? throw new ArgumentNullException(nameof(basketRepository));
            _discountGrpcService = discountGrpcService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(ShoppingCart), (int)HttpStatusCode.OK)]
        [Route("[action]/{userName}")]
        public async Task<IActionResult> GetBasket(string userName)
        {
            ShoppingCart basket = await _basketRepository.GetBasket(userName);
            return Ok(basket ?? new ShoppingCart(userName));
        }

        [HttpPost]
        [ProducesResponseType(typeof(ShoppingCart), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateBasket([FromBody] ShoppingCart basket)
        {
            foreach (ShoppingCartItem basketItem in basket.Items)
            {
                CouponModel coupon = await _discountGrpcService.GetDiscount(basketItem.ProductName);
                basketItem.Price -= coupon.Amount;
            }

            ShoppingCart updatedBasket = await _basketRepository.UpdateBasket(basket);
            return Ok(updatedBasket ?? basket);
        }

        [HttpDelete]
        [Route("[action]/{userName}")]
        [ProducesResponseType(typeof(ShoppingCart), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteBasket(string userName)
        {
            await _basketRepository.DeleteBasket(userName);
            return Ok();
        }
    }
}