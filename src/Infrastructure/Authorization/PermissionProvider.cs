using Microsoft.AspNetCore.Http;

namespace Infrastructure.Authorization;

internal sealed class PermissionProvider
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public PermissionProvider(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    // TODO: Here you'll implement your logic to fetch permissions. Maybe get from cache
    public Task<HashSet<string>> GetForUserIdAsync(Guid userId)
    {
        List<string> permissionsValue = _httpContextAccessor.HttpContext?.User.Claims
            .Where(c => c.Type == "permission")
            .Select(c => c.Value)
            .ToList() ?? [];

        HashSet<string> permissionsSet = [.. permissionsValue];

        return Task.FromResult(permissionsSet);
    }
}
