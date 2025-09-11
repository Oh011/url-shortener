namespace Project.Application.Features.Urls.Commands.UpdateUrl
{
    using FluentValidation;

    public class UpdateUrlCommandValidator : AbstractValidator<UpdateUrlCommand>
    {
        public UpdateUrlCommandValidator()
        {
            RuleFor(x => x.ShortUrl)
                .NotEmpty().WithMessage("ShortCode is required.")
                .MaximumLength(7).WithMessage("ShortCode cannot exceed 7 characters."); // adjust as needed

            RuleFor(x => x.NewExpiresAt)
                .GreaterThan(DateTime.UtcNow)

                .WithMessage("New expiration date must be in the future.");
        }
    }

}
