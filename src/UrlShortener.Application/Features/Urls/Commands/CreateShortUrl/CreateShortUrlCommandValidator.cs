using FluentValidation;

namespace Project.Application.Features.Urls.Commands.CreateShortUrl
{

    //inherits from IValidator<T>
    internal class CreateShortUrlCommandValidator : AbstractValidator<CreateShortUrlCommand>
    {
        public CreateShortUrlCommandValidator()
        {
            // LongUrl must not be empty and must be a valid URL
            RuleFor(x => x.LongUrl)
                .NotEmpty().WithMessage("LongUrl is required.")
                .Must(BeAValidUrl).WithMessage("LongUrl must be a valid URL.");

            // CustomAlias (if provided) must be alphanumeric and max 20 chars
            RuleFor(x => x.CustomAlias)
                .Matches("^[a-zA-Z0-9_-]*$").WithMessage("CustomAlias can only contain letters, numbers, underscores, or dashes.")
                .MaximumLength(7).WithMessage("CustomAlias cannot exceed 7 characters.")
                .When(x => !string.IsNullOrWhiteSpace(x.CustomAlias));

            // ExpirationDate (if provided) must be in the future
            RuleFor(x => x.ExpirationDate)
                .Must(date => date > DateTime.UtcNow)
                .When(x => x.ExpirationDate.HasValue)
                .WithMessage("ExpirationDate must be in the future.");
        }

        private bool BeAValidUrl(string url)
        {
            return Uri.TryCreate(url, UriKind.Absolute, out var uriResult)
                   && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
        }
    }
}

//Step 1: Uri.TryCreate(...)

//Uri.TryCreate attempts to parse the url string into a Uri object.

//Parameters:

//url → the string being checked.
//UriKind.Absolute → requires a fully qualified URL (must include scheme like http:// or https://, not just google.com).
//out var uriResult → if parsing succeeds, it outputs the Uri object into uriResult.


//(uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps)
//Even if a string is a valid URI, it could be something weird like:

//ftp://example.com
//file://C:/Windows/system32

//So, we add an extra check: the scheme must be either HTTP or HTTPS.