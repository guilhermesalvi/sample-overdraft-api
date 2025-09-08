using ClientEnrollment.Features.BankAccounts.Domain;
using CSharpFunctionalExtensions;
using Dapper;
using Microsoft.Data.SqlClient;

namespace ClientEnrollment.Features.BankAccounts.Repositories;

public class BankAccountRepository(
    IConfiguration configuration,
    ILogger<BankAccountRepository> logger)
{
    public async Task<Result> CreateBankAccountAsync(BankAccount account, CancellationToken ct)
    {
        const string sql = """
                           INSERT INTO dbo.Accounts (Id, ClientId, ClientType, IsBankAccountActive, CreatedAt)
                           VALUES (@Id, @ClientId, @ClientType, @IsBankAccountActive, @CreatedAt)
                           """;

        var p = new DynamicParameters();
        p.Add("Id", account.Id);
        p.Add("ClientId", account.ClientId);
        p.Add("ClientType", (short)account.ClientType);
        p.Add("IsBankAccountActive", account.IsBankAccountActive);
        p.Add("CreatedAt", account.CreatedAt);

        var cmd = new CommandDefinition(sql, p, commandTimeout: 5, cancellationToken: ct);

        try
        {
            await using var conn = new SqlConnection(configuration.GetConnectionString("client-enrollment-db"));
            var rows = await conn.ExecuteAsync(cmd);

            if (rows == 1) return Result.Success();

            logger.LogWarning("Failed to create bank account for client {ClientId}. Rows affected: {Rows}",
                account.ClientId, rows);
            return Result.Failure("Failed to create bank account");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to create bank account for client {ClientId}", account.ClientId);
            return Result.Failure("Failed to create bank account");
        }
    }

    public async Task<Result<bool>> CheckCustomerIdExistsAsync(Guid clientId, CancellationToken ct)
    {
        const string sql = """
                           SELECT COUNT(1)
                           FROM dbo.Accounts
                           WHERE ClientId = @ClientId
                           """;

        var p = new DynamicParameters();
        p.Add("ClientId", clientId);

        var cmd = new CommandDefinition(sql, p, commandTimeout: 5, cancellationToken: ct);

        try
        {
            await using var conn = new SqlConnection(configuration.GetConnectionString("client-enrollment-db"));
            var count = await conn.ExecuteScalarAsync<int>(cmd);

            return Result.Success(count > 0);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to check existence of bank account for client {ClientId}", clientId);
            return Result.Failure<bool>("Failed to check existence of bank account");
        }
    }
}
