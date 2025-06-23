using System.Data;
using Application.Abstractions.Data;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace Infrastructure.Database;

public sealed class ApplicationDbQuery(IConfiguration configuration) : IApplicationDbQuery
{
    private readonly string? _connectionString = configuration.GetConnectionString("Database");
    private NpgsqlConnection? _connection;

    public async Task<T?> SingleOrDefaultAsync<T>(string sql, object? parameters, CancellationToken cancellationToken)
    {
        _connection ??= new NpgsqlConnection(_connectionString);
        if (_connection.State != ConnectionState.Open)
        {
            await _connection.OpenAsync(cancellationToken);
        }
        CommandDefinition commandDefinition = new(
            commandText: sql,
            parameters: parameters,
            cancellationToken: cancellationToken
        );
        return await _connection.QuerySingleOrDefaultAsync<T>(commandDefinition);
    }

    public async Task<T?> FirstOrDefaultAsync<T>(string sql, object? parameters, CancellationToken cancellationToken)
    {
        _connection ??= new NpgsqlConnection(_connectionString);
        if (_connection.State != ConnectionState.Open)
        {
            await _connection.OpenAsync(cancellationToken);
        }
        CommandDefinition commandDefinition = new(
            commandText: sql,
            parameters: parameters,
            cancellationToken: cancellationToken
        );
        return await _connection.QueryFirstOrDefaultAsync<T>(commandDefinition);
    }

    public async Task<IEnumerable<T>> QueryAsync<T>(string sql, object? parameters, CancellationToken cancellationToken)
    {
        _connection ??= new NpgsqlConnection(_connectionString);
        if (_connection.State != ConnectionState.Open)
        {
            await _connection.OpenAsync(cancellationToken);
        }

        CommandDefinition commandDefinition = new(
            commandText: sql,
            parameters: parameters,
            cancellationToken: cancellationToken
        );

        return await _connection.QueryAsync<T>(commandDefinition);
    }

    public async ValueTask DisposeAsync()
    {
        if (_connection != null)
        {
            await _connection.DisposeAsync();
        }
    }
}
