using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Users;

namespace Application.Users.GetByEmail;

internal sealed class GetUserByEmailQueryHandler(IApplicationDbQuery dbQuery)
    : IQueryHandler<GetUserByEmailQuery, UserResponse>
{
    public async Task<Result<UserResponse>> Handle(GetUserByEmailQuery query, CancellationToken cancellationToken)
    {
        const string sql = """
                select
                    u.id as "Id",
                    u.first_name as "FirstName",
                    u.last_name as "LastName",
                    u.email as "Email"
                from
                    users u
                    where u.email = @email
                """;
        object param = new
        {
            email = query.Email
        };
        UserResponse? user = await dbQuery.SingleOrDefaultAsync<UserResponse>(sql, param, cancellationToken);

        if (user is null)
        {
            return Result.Failure<UserResponse>(UserErrors.NotFoundByEmail);
        }

        return user;
    }
}
