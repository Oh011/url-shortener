using FluentValidation;

namespace Project.Application.Features.Authentication.Commands.ResendConfirmationEmail
{
    public class ResendConfirmationEmailCommandValidator : AbstractValidator<ResendConfirmationEmailCommand>
    {


        public ResendConfirmationEmailCommandValidator()
        {



            RuleFor(x => x.Email)
              .NotEmpty().WithMessage("Email is required.")
              .EmailAddress().WithMessage("Invalid email format.");
        }
    }
}
