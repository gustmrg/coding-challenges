using System.Data;
using FluentValidation;
using PicPay.API.Models.Request;
using static PicPay.API.Helpers.StringHelper;

namespace PicPay.API.Validators;

public class CreateUserRequestValidator : AbstractValidator<CreateUserRequestModel>
{
    public CreateUserRequestValidator()
    {
        RuleFor(x => x.FullName).NotEmpty()
            .WithMessage("Full name is required")
            .MinimumLength(2).WithMessage("Your full name length must be at least 2 characters long.")
            .MaximumLength(250).WithMessage("Your password length must be at most 250 characters.");

        RuleFor(x => x.Email).NotEmpty()
            .WithMessage("Email is required")
            .EmailAddress().WithMessage("Email is invalid");
        
        RuleFor(x => x.Password).NotEmpty()
            .WithMessage("Your password cannot be empty")
            .MinimumLength(8).WithMessage("Your password length must be at least 8.")
            .Matches(@"[A-Z]+").WithMessage("Your password must contain at least one uppercase letter.")
            .Matches(@"[a-z]+").WithMessage("Your password must contain at least one lowercase letter.")
            .Matches(@"[0-9]+").WithMessage("Your password must contain at least one number.")
            .Matches(@"[\!\?\*\.]+").WithMessage("Your password must contain at least one (!? *.).");

        RuleFor(x => x.DocumentNumber).NotEmpty()
            .Must(IsValidDocumentNumber).WithMessage("Document number must be in a valid CPF or CNPJ format");
    }

    private static bool IsValidDocumentNumber(string documentNumber)
    {
        documentNumber = RemoveSpecialCharactersAndLetters(documentNumber);
        
        return documentNumber.Length is 11 or 14;
    }
}