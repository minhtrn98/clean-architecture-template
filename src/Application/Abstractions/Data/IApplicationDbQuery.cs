namespace Application.Abstractions.Data;

public interface IApplicationDbQuery : IAsyncDisposable
{
    //IDbConnection CreateConnection();

    Task<T?> FirstOrDefaultAsync<T>(string sql, object? parameters, CancellationToken cancellationToken);
    Task<T?> SingleOrDefaultAsync<T>(string sql, object? parameters, CancellationToken cancellationToken);
    Task<IEnumerable<T>> QueryAsync<T>(string sql, object? parameters, CancellationToken cancellationToken);

    Task<IEnumerable<T>> QueryAsync<T>(string sql, CancellationToken cancellationToken)
        => QueryAsync<T>(sql, null, cancellationToken);

    Task<T?> FirstOrDefaultAsync<T>(string sql, CancellationToken cancellationToken)
        => FirstOrDefaultAsync<T>(sql, null, cancellationToken);

    Task<T?> SingleOrDefaultAsync<T>(string sql, CancellationToken cancellationToken)
        => SingleOrDefaultAsync<T>(sql, null, cancellationToken);
}
