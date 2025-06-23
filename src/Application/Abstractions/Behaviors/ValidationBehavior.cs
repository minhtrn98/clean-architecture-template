using FluentValidation;
using FluentValidation.Results;

namespace Application.Abstractions.Behaviors;

internal sealed class ValidationBehavior<TRequest, TResponse>(IValidator<TRequest>? validator = null)
    : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
        where TResponse : Result
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (validator is null)
        {
            return await next(cancellationToken);
        }

        ValidationResult validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (validationResult.IsValid)
        {
            return await next(cancellationToken);
        }

        ValidationError errors = CreateValidationError([.. validationResult.Errors]);

        var result = (TResponse)Activator.CreateInstance(typeof(TResponse), [false, errors])!;

        return result;
    }

    private static ValidationError CreateValidationError(ValidationFailure[] validationFailures) =>
        new([.. validationFailures.Select(f => Error.Problem(f.ErrorCode, f.ErrorMessage))]);
}
