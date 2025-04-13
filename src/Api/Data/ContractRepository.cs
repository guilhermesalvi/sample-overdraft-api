using System.Data;
using Dapper;
using Overdraft.Api.Models;

namespace Overdraft.Api.Data;

public class ContractRepository(IDbConnection connection)
{
    public Task CreateAsync(Contract contract, CancellationToken cancellationToken)
    {
        const string sql = @"
            INSERT INTO Contracts (Id,GracePeriodDays,MonthlyInterestRate,MonthlyIofTax,MonthlyOverLimitInterestRate,MonthlyLatePaymentInterestRate,LatePaymentPenaltyRate,IsContractActive,CreatedAt)
            VALUES (@Id,@GracePeriodDays,@MonthlyInterestRate,@MonthlyIofTax,@MonthlyOverLimitInterestRate,@MonthlyLatePaymentInterestRate,@LatePaymentPenaltyRate,@IsContractActive,@CreatedAt)";

        return connection.ExecuteAsync(sql, contract);
    }

    public Task<List<Contract>> GetAsync(Guid? id, CancellationToken cancellationToken)
    {
        const string sql = @"
            SELECT (Id,GracePeriodDays,MonthlyInterestRate,MonthlyIofTax,MonthlyOverLimitInterestRate,MonthlyLatePaymentInterestRate,LatePaymentPenaltyRate,IsContractActive,CreatedAt) FROM Contracts
            WHERE 1=1
            AND IsContractActive = 1
            AND @Id IS NULL OR Id = @Id";

        var contracts = connection.QueryAsync<Contract>(sql, new { Id = id });
        return Task.FromResult(contracts.Result.ToList());
    }

    public Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        const string sql = @"
            UPDATE Contracts
            SET IsContractActive = 0
            WHERE Id = @Id";

        return connection.ExecuteAsync(sql, new { Id = id });
    }
}
