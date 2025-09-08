using CustomerEnrollment.Features.BankAccounts.Domain;
using CSharpFunctionalExtensions;
using Dapper;
using Microsoft.Data.SqlClient;

namespace CustomerEnrollment.Features.BankAccounts.Repositories;

public class BankAccountRepository(
    IConfiguration configuration,
    ILogger<BankAccountRepository> logger)
{
    public async Task<Result> CreateBankAccountAsync(BankAccount account, CancellationToken ct)
    {
        const string sql = """
                           INSERT INTO dbo.Accounts (Id, CustomerId, CustomerType, IsBankAccountActive, CreatedAt)
                           VALUES (@Id, @CustomerId, @CustomerType, @IsBankAccountActive, @CreatedAt)
                           """;

        var p = new DynamicParameters();
        p.Add("Id", account.Id);
        p.Add("CustomerId", account.CustomerId);
        p.Add("CustomerType", (short)account.CustomerType);
        p.Add("IsBankAccountActive", account.IsBankAccountActive);
        p.Add("CreatedAt", account.CreatedAt);

        var cmd = new CommandDefinition(sql, p, commandTimeout: 5, cancellationToken: ct);

        try
        {
            await using var conn = new SqlConnection(configuration.GetConnectionString("customer-enrollment-db"));
            var rows = await conn.ExecuteAsync(cmd);

            if (rows == 1) return Result.Success();

            logger.LogWarning("Failed to create bank account for customer {CustomerId}. Rows affected: {Rows}",
                account.CustomerId, rows);
            return Result.Failure("Failed to create bank account");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to create bank account for customer {CustomerId}", account.CustomerId);
            return Result.Failure("Failed to create bank account");
        }
    }

    public async Task<Result<bool>> CheckCustomerIdExistsAsync(Guid customerId, CancellationToken ct)
    {
        const string sql = """
                           SELECT COUNT(1)
                           FROM dbo.Accounts
                           WHERE CustomerId = @CustomerId
                           """;

        var p = new DynamicParameters();
        p.Add("CustomerId", customerId);

        var cmd = new CommandDefinition(sql, p, commandTimeout: 5, cancellationToken: ct);

        try
        {
            await using var conn = new SqlConnection(configuration.GetConnectionString("customer-enrollment-db"));
            var count = await conn.ExecuteScalarAsync<int>(cmd);

            return Result.Success(count > 0);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to check existence of bank account for customer {CustomerId}", customerId);
            return Result.Failure<bool>("Failed to check existence of bank account");
        }
    }
}
