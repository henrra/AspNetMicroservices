using FluentValidation;
using FluentValidation.Results;
using Ordering.Application.Features.Orders.Commands.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Ordering.Application.Features.Orders.Commands.Validators
{
    public class OrderCommandBaseValidator<T> : AbstractValidator<T> where T : OrderCommand
    {
        protected OrderCommandBaseValidator()
        {
            RuleFor(p => p.UserName)
                .NotEmpty().WithMessage("{UserName} est requis.")
                .NotNull()
                .MaximumLength(50).WithMessage("{UserName} ne doit pas dépasser 50 caractères.");

            RuleFor(p => p.EmailAddress)
                .NotEmpty().WithMessage("{EmailAddress} est requis.");

            RuleFor(p => p.TotalPrice)
                .NotEmpty().WithMessage("{TotalPrice} est requis.")                
                .GreaterThan(0).WithMessage("{TotalPrice} doit être supérieur à 0.");
        }
    }
}
