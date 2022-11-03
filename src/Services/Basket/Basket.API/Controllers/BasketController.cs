using System;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Basket.API.Entities;
using Basket.API.Repositories;
using Basket.API.Services;
using Discount.Grpc.Protos;
using EventBus.Messages.Events;
using MassTransit;
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
        private readonly IMapper _mapper;
        private readonly IPublishEndpoint _publishEndpoint;

        public BasketController(ILogger<BasketController> logger,
            IBasketRepository basketRepository,
            DiscountGrpcService discountGrpcService,
            IPublishEndpoint publishEndpoint,
            IMapper mapper)
        {
            _logger = logger;
            _basketRepository = basketRepository ?? throw new ArgumentNullException(nameof(basketRepository));
            _discountGrpcService = discountGrpcService;
            _mapper = mapper;
            _publishEndpoint = publishEndpoint;
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

        [Route("[action]")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Accepted)]
        [ProducesResponseType((int)HttpStatusCode.BadGateway)]
        public async Task<IActionResult> Checkout([FromBody] BasketCheckout basketCheckout)
        {
            // On récupère s'il existe.
            var basket = await _basketRepository.GetBasket(basketCheckout.UserName);
            if (basket == null)
            {
                return BadRequest();
            }

            // Envoi vers RabbitMq
            var eventMessage = _mapper.Map<BasketCheckoutEvent>(basketCheckout);
            eventMessage.TotalPrice = basket.GetTotalPrice();
            await _publishEndpoint.Publish(eventMessage);

            // Suppression du panier
            await _basketRepository.DeleteBasket(basketCheckout.UserName);
            return Accepted();
        }

        [ApiExplorerSettings(IgnoreApi = true)] // Pour ignorer dans Swagger
        [Route("[action]")]
        public Task<string> Ping()
        {
            return Task.FromResult("Pong");
        }
    }
}