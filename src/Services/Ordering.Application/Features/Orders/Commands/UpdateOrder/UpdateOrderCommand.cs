using MediatR;
using Ordering.Application.Features.Orders.Commands.Models;

namespace Ordering.Application.Features.Orders.Commands.UpdateOrder
{
    public class UpdateOrderCommand : OrderCommand, IRequest
    {
    }
}