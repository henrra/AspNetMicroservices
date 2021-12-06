using Ordering.Application.Features.Orders.Commands.Validators;

namespace Ordering.Application.Features.Orders.Commands.CheckoutOrder
{
    public class CheckoutOrderCommandValidator : OrderCommandBaseValidator<CheckoutOrderCommand>
    {
        public CheckoutOrderCommandValidator() : base()
        {
        }
    }
}