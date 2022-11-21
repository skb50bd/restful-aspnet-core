using BrotalApiTemplate.Domain.InputModels;
using FluentValidation;

namespace BrotalApiTemplate.Core.Validators;

public class LoginModelValidator : AbstractValidator<LoginModel>
{
    public LoginModelValidator()
    {
        RuleFor(_ => _.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("A valid email is required");

        RuleFor(_ => _.Password)
            .NotEmpty().WithMessage("Password cannot be empty");
    }
}