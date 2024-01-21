using FluentValidation;
using PicPay.API.Models.Request;

namespace PicPay.API.Validators;

public class CreateTransactionRequestValidator : AbstractValidator<CreateTransactionRequestModel>
{
    public CreateTransactionRequestValidator()
    {
        RuleFor(x => x.Value)
            .NotNull().WithMessage("{PropertyName} is required")
            .GreaterThan(0).WithMessage("Transaction amount must be value greater than zero");

        RuleFor(x => x.PayerId)
            .NotEmpty().WithMessage("{PropertyName} is required");

        RuleFor(x => x.PayeeId)
            .NotEmpty().WithMessage("{PropertyName} is required");
    }
}