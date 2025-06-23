using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Users;

namespace Application.Users.GetById;

internal sealed class GetUserByIdQueryHandler(IApplicationDbQuery dbQuery, IUserContext userContext)
    : IQueryHandler<GetUserByIdQuery, UserResponse>
{
    public async Task<Result<UserResponse>> Handle(GetUserByIdQuery query, CancellationToken cancellationToken)
    {
        if (query.UserId != userContext.UserId)
        {
            return Result.Failure<UserResponse>(UserErrors.Unauthorized());
        }

        const string sql = """
                select
                    u.id as "Id",
                    u.first_name as "FirstName",
                    u.last_name as "LastName",
                    u.email as "Email"
                from
                    users u
                    where u.id = @userId
                """;
        object param = new
        {
            userId = query.UserId
        };
        UserResponse? user = await dbQuery.SingleOrDefaultAsync<UserResponse>(sql, param, cancellationToken);

        if (user is null)
        {
            return Result.Failure<UserResponse>(UserErrors.NotFound(query.UserId));
        }

        return user;
    }
}
