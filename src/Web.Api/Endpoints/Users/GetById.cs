using Application.Users.GetById;
using MediatR;
using SharedKernel;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Users;

internal sealed class GetByIdEndpoint : EndpointWithoutRequest<UserResponse>
{
    private readonly IMediator _mediator;

    public GetByIdEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Get("users/{userId:guid}");
        Policies(Application.Users.Permissions.UsersAccess);
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        Guid userId = Route<Guid>("userId");
        if (userId == Guid.Empty)
        {
            await SendNotFoundAsync(ct);
            return;
        }

        GetUserByIdQuery query = new(userId);

        Result<UserResponse> result = await _mediator.Send(query, ct);

        if (result.IsSuccess)
        {
            await SendOkAsync(result.Value, ct);
        }
        else
        {
            await SendResultAsync(CustomResults.Problem(result));
        }
    }
}
