using MediatR;
using Ordering.Application.Features.Orders.Commands.Models;

namespace Ordering.Application.Features.Orders.Commands.CheckoutOrder
{
    public class CheckoutOrderCommand : OrderCommand, IRequest<int>
    {
    }
}