using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Contracts.Infrastructure;
using Ordering.Application.Contracts.Models;
using Ordering.Application.Contracts.Persistence;
using Ordering.Domain.Common;
using Ordering.Domain.Entities;

namespace Ordering.Application.Features.Orders.Commands.CheckoutOrder
{
    public class CheckoutIOrderCommandHadler : IRequestHandler<CheckoutOrderCommand, int>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;
        private readonly ILogger<CheckoutIOrderCommandHadler> _logger;
    
        public CheckoutIOrderCommandHadler(IOrderRepository orderRepository,
            IMapper mapper,
            IEmailService emailService,
            ILogger<CheckoutIOrderCommandHadler> logger)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
            _emailService = emailService;
            _logger = logger;
        }
    
        public async Task<int> Handle(CheckoutOrderCommand request, CancellationToken cancellationToken)
        {
            var orderEntity = _mapper.Map<Order>(request);
            Order newOrder = await _orderRepository.AddAsync(orderEntity);
            _logger.LogInformation($"Création commande {newOrder.Id} avec succès.");
            await SendMail(newOrder);
            return newOrder.Id;
        }
    
        private async Task SendMail(EntityBase newOrder)
        {
            var email = new Email
            {
                To = "radohenintsoa.rasatamanana@gmail.com",
                Body = "Commande créée.",
                Subject = $"Commande N° {newOrder.Id}"
            };
    
            try
            {
                await _emailService.SendMail(email);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Commande {newOrder.Id} - erreur envoi mail : {ex.Message}");
            }
        }
    }
}