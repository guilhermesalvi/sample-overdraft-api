using System.Data;
using Dapper;
using Overdraft.Api.Models;

namespace Overdraft.Api.Data;

public class AccountRepository(IDbConnection connection)
{
    public Task CreateAsync(Account account, CancellationToken _)
    {
        const string sql = @"
            INSERT INTO Account (Id, ContractId, UsedLimit, OverdraftLimit, SelectedLimit, UsedDays, IsAccountActive, CreatedAt)
            VALUES (@Id, @ContractId, @UsedLimit, @OverdraftLimit, @SelectedLimit, @UsedDays, @IsAccountActive, @CreatedAt)";

        return connection.ExecuteAsync(sql, account);
    }

    public async Task<List<Account>> GetAsync(Guid? id, CancellationToken cancellationToken)
    {
        const string sql = @"
            SELECT (Id, ContractId, UsedLimit, OverdraftLimit, SelectedLimit, UsedDays, IsAccountActive, CreatedAt) FROM Account
            WHERE 1=1
            AND IsAccountActive = 1
            AND @Id IS NULL OR Id = @Id";

        var accounts = await connection.QueryAsync<Account>(sql, new { Id = id });
        return accounts.ToList();
    }

    public Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        const string sql = @"
            UPDATE Account
            SET IsAccountActive = 0
            WHERE Id = @Id";

        return connection.ExecuteAsync(sql, new { Id = id });
    }
}
