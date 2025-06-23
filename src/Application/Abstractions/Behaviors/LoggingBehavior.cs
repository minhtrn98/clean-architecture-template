using Microsoft.Extensions.Logging;
using Serilog.Context;

namespace Application.Abstractions.Behaviors;

internal sealed class LoggingBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
        where TResponse : Result
{
    private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

    public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        string? requestName = typeof(TRequest).FullName ?? typeof(TRequest).Name;

        _logger.LogInformation("Processing request {RequestName}", requestName);

        TResponse response = await next(cancellationToken);

        if (response.IsSuccess)
        {
            _logger.LogInformation("Completed request {RequestName}", requestName);
        }
        else
        {
            using (LogContext.PushProperty("Error", response.Error, true))
            {
                _logger.LogError("Completed request {RequestName} with error", requestName);
            }
        }

        return response;
    }
}
