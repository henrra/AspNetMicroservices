using Ordering.Application.Features.Orders.Commands.Validators;

namespace Ordering.Application.Features.Orders.Commands.UpdateOrder
{
    public class UpdateOrderCommandValidator : OrderCommandBaseValidator<UpdateOrderCommand>
    {
        public UpdateOrderCommandValidator() : base() 
        {
        }
    }
}