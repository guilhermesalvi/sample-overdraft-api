using System.Data;
using System.Text;
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

    public async Task<Result<IReadOnlyList<BankAccount>>> GetBankAccountsAsync(
        Guid? accountId,
        Guid? customerId,
        CustomerType? customerType,
        bool? isBankAccountActive,
        int offset,
        int limit,
        CancellationToken ct)
    {
        offset = Math.Max(0, offset);
        limit = limit is <= 0 or > 100 ? 100 : limit;

        var builder = new SqlBuilder();
        var parameters = new DynamicParameters();

        parameters.Add("Offset", offset);
        parameters.Add("Limit", limit);

        if (accountId is { } aid)
        {
            parameters.Add("AccountId", aid);
            builder.Where("Id = @AccountId");
        }

        if (customerId is { } cid)
        {
            parameters.Add("CustomerId", cid);
            builder.Where("CustomerId = @CustomerId");
        }

        if (customerType is { } ctType)
        {
            parameters.Add("CustomerType", ctType);
            builder.Where("CustomerType = @CustomerType");
        }

        if (isBankAccountActive is { } active)
        {
            parameters.Add("IsActive", active);
            builder.Where("IsBankAccountActive = @IsActive");
        }

        var template = builder.AddTemplate("""
                                           SELECT
                                               Id, CustomerId, CustomerType, IsBankAccountActive, CreatedAt
                                           FROM dbo.Accounts WITH (READCOMMITTEDLOCK)
                                           /**where**/
                                           ORDER BY CreatedAt DESC, Id DESC
                                           OFFSET @Offset ROWS FETCH NEXT @Limit ROWS ONLY;
                                           """, parameters);

        var cmd = new CommandDefinition(
            template.RawSql,
            template.Parameters,
            commandType: CommandType.Text,
            commandTimeout: 5,
            cancellationToken: ct);

        try
        {
            await using var conn = new SqlConnection(configuration.GetConnectionString("customer-enrollment-db"));
            var rows = await conn.QueryAsync<BankAccount>(cmd);
            return Result.Success<IReadOnlyList<BankAccount>>(rows.AsList());
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to get bank accounts");
            return Result.Failure<IReadOnlyList<BankAccount>>("Failed to get bank accounts");
        }
    }
}
