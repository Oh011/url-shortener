using FluentValidation;
using MediatR;

using ValidationException = Project.Application.Exceptions.ValidationException;

namespace Project.Application.Common.Behaviors
{

    //--> is means it’s a pipeline behavior that runs whenever _mediator.Send() is called
    internal class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
    {


        private readonly IEnumerable<IValidator<TRequest>> _Validators;



        //DI automatically injects all registered validators for the request type.
        //Example: if you have CreateShortUrlCommandValidator : AbstractValidator<CreateShortUrlCommand>, it will be included here.
        //_validators holds all validators for the incoming TRequest.

        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            _Validators = validators;
        }


        //next = delegate pointing to the next behavior or the actual handler if this is the last step.
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {


            if (_Validators.Any())
            {

                var context = new ValidationContext<TRequest>(request);

                var validationResults = await Task.WhenAll(

                    _Validators.Select(v => v.ValidateAsync(context, cancellationToken))

                    );


                //validationResult Type has List called errors


                var errors = validationResults.SelectMany(result => result.Errors)
                    .Where(errors => errors != null).ToList();



                if (errors.Count > 0)
                {
                    var errorDict = errors
                        .GroupBy(f => f.PropertyName)
                        .ToDictionary(
                            g => g.Key,
                            g => g.Select(e => e.ErrorMessage).ToList()
                        );

                    throw new ValidationException(errorDict);
                }



            }

            return await next();


        }
    }
}
