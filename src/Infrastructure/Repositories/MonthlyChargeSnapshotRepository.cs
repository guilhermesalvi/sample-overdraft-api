using Domain.Models;
using Infrastructure.Context;

namespace Infrastructure.Repositories;

public class MonthlyChargeSnapshotRepository(
    ApplicationDbContext context) : IMonthlyChargeSnapshotRepository
{
}
