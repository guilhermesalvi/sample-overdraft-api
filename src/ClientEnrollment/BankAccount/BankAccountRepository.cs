using System.Data;
using CSharpFunctionalExtensions;
using Dapper;

namespace ClientEnrollment.BankAccount;

public class BankAccountRepository(
    IDbConnection db,
    ILogger<BankAccountRepository> logger)
{
    public async Task<Result<Guid>> CreateAccountAsync(Account account, CancellationToken ct)
    {
        const string sql = """
                           INSERT INTO dbo.Accounts (Id, CustomerId, CustomerType, IsAccountActive, CreatedAt)
                           VALUES (@Id, @CustomerId, @CustomerType, @IsAccountActive, @CreatedAt)
                           """;

        var p = new DynamicParameters();
        p.Add("@Id", account.Id, DbType.Guid);
        p.Add("@CustomerId", account.CustomerId, DbType.Guid);
        p.Add("@CustomerType", (short)account.CustomerType, DbType.Int16);
        p.Add("@IsAccountActive", account.IsAccountActive, DbType.Boolean);
        p.Add("@CreatedAt", account.CreatedAt, DbType.DateTimeOffset);

        var cmd = new CommandDefinition(sql, p, commandTimeout: 5, cancellationToken: ct);

        try
        {
            var rows = await db.ExecuteAsync(cmd);
            return rows == 1
                ? Result.Success(account.Id)
                : Result.Failure<Guid>("Insert failed, no row affected.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error inserting account {AccountId}", account.Id);
            return Result.Failure<Guid>("Unexpected error while creating account.");
        }
    }

    public async Task<Result<bool>> CheckCustomerIdExistsAsync(Guid customerId, CancellationToken ct)
    {
        const string sql = "SELECT COUNT(1) FROM dbo.Accounts WHERE CustomerId = @CustomerId";

        var p = new DynamicParameters();
        p.Add("@CustomerId", customerId, DbType.Guid);

        var cmd = new CommandDefinition(sql, p, commandTimeout: 5, cancellationToken: ct);

        try
        {
            var count = await db.ExecuteScalarAsync<int>(cmd);
            return Result.Success(count > 0);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error checking existence of customer {AccountId}", customerId);
            return Result.Failure<bool>("Unexpected error while checking customer existence.");
        }
    }
}
