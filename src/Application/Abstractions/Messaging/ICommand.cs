namespace Application.Abstractions.Messaging;

public interface ICommand<TResponse> : IRequest<Result<TResponse>>;
