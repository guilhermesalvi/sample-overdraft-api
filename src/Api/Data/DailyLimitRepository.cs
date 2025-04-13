using System.Data;
using Dapper;
using Overdraft.Api.Models;

namespace Overdraft.Api.Data;

public class DailyLimitRepository(IDbConnection connection)
{
    public async Task<List<DailyLimit>> GetByAccountIdAsync(Guid accountId, CancellationToken cancellationToken)
    {
        const string sql = @"
            SELECT (Id,AccountId,UsedLimit,ReferenceDate,CreatedAt) FROM DailyLimits
            WHERE 1=1
            AND AccountId = @AccountId";

        var dailyLimits = await connection.QueryAsync<DailyLimit>(sql, new { AccountId = accountId });
        return dailyLimits.ToList();
    }

    public async Task<List<DailyLimit>> GetByAccountIdAndRangeDatesAsync(
        Guid accountId, DateOnly startDate, DateOnly endDate, CancellationToken cancellationToken)
    {
        const string sql = @"
            SELECT (Id,AccountId,UsedLimit,ReferenceDate,CreatedAt) FROM DailyLimits
            WHERE 1=1
            AND AccountId = @AccountId
            AND ReferenceDate >= @StartDate
            AND ReferenceDate <= @EndDate";

        var dailyLimits = await connection.QueryAsync<DailyLimit>(sql,
            new { AccountId = accountId, StartDate = startDate, EndDate = endDate });

        return dailyLimits.ToList();
    }

    public Task<DailyLimit?> GetOneByReferenceDateAsync(
        Guid accountId, DateOnly referenceDate, CancellationToken cancellationToken)
    {
        var startDate = referenceDate;
        var endDate = startDate.AddDays(1);

        const string sql = @"
            SELECT (Id,AccountId,UsedLimit,ReferenceDate,CreatedAt) FROM DailyLimits
            WHERE 1=1
            AND AccountId = @AccountId
            AND ReferenceDate >= @StartDate
            AND ReferenceDate <= @EndDate";

        return connection.QuerySingleOrDefaultAsync<DailyLimit>(sql,
            new { AccountId = accountId, StartDate = startDate, EndDate = endDate });
    }

    public Task CreateAsync(DailyLimit dailyLimit, CancellationToken cancellationToken)
    {
        const string sql = @"
            INSERT INTO DailyLimits (Id,AccountId,UsedLimit,ReferenceDate,CreatedAt)
            VALUES (@Id,@AccountId,@UsedLimit,@ReferenceDate,@CreatedAt)";

        return connection.ExecuteAsync(sql, dailyLimit);
    }

    public Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        const string sql = @"
            DELETE FROM DailyLimits
            WHERE Id = @Id";

        return connection.ExecuteAsync(sql, new { Id = id });
    }
}
