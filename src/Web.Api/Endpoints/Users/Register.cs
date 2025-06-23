using Application.Users.Register;
using MediatR;
using SharedKernel;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Users;

internal sealed class RegisterEndpoint : Endpoint<RegisterEndpoint.Request>
{
    public sealed record Request(string Email, string FirstName, string LastName, string Password);

    private readonly IMediator _mediator;

    public RegisterEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Post("users/register");
        AllowAnonymous();
    }

    public override async Task HandleAsync(Request request, CancellationToken ct)
    {
        RegisterUserCommand command = new(
                request.Email,
                request.FirstName,
                request.LastName,
                request.Password);

        Result<Guid> result = await _mediator.Send(command, ct);

        if (result.IsSuccess)
        {
            await SendOkAsync(ct);
        }
        else
        {
            await SendResultAsync(CustomResults.Problem(result));
        }
    }
}
