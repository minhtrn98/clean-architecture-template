using Application.Users.Login;
using MediatR;
using SharedKernel;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Users;

internal sealed class LoginEndpoint : Endpoint<LoginEndpoint.Request, object>
{
    public sealed record Request(string Email, string Password);

    private readonly IMediator _mediator;

    public LoginEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Post("users/login");
        AllowAnonymous();
    }

    public override async Task HandleAsync(Request request, CancellationToken ct)
    {
        LoginUserCommand command = new(request.Email, request.Password);

        Result<string> result = await _mediator.Send(command, ct);

        if (result.IsSuccess)
        {
            await SendOkAsync(new
            {
                token = result.Value
            }, ct);
        }
        else
        {
            await SendResultAsync(CustomResults.Problem(result));
        }
    }
}
