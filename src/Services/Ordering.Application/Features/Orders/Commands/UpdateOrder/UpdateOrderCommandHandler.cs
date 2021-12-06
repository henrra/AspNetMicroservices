using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Contracts.Infrastructure;
using Ordering.Application.Contracts.Persistence;
using Ordering.Application.Exceptions;
using Ordering.Application.Features.Orders.Commands.CheckoutOrder;
using Ordering.Domain.Entities;

namespace Ordering.Application.Features.Orders.Commands.UpdateOrder
{
    public class UpdateOrderCommandHandler : IRequestHandler<UpdateOrderCommand>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateOrderCommandHandler> _logger;
        
        public UpdateOrderCommandHandler(IOrderRepository orderRepository,
            IMapper mapper,
            ILogger<UpdateOrderCommandHandler> logger)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
            _logger = logger;
        }
        
        public async Task<Unit> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
        {
            Order order = await _orderRepository.GetByIdAsync(request.Id);
            if (order == null)
            {
                throw new NotFoundException(nameof(order), request.Id);    
            }
            
            _mapper.Map(request, order);
            await _orderRepository.UpdateAsync(order);
            _logger.LogError($"La commande {order.Id} est mise à jour avec succès.");
            return Unit.Value;
        }
    }
}