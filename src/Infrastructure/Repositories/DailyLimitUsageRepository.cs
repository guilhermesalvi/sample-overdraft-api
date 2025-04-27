using Domain.Models;
using Infrastructure.Context;

namespace Infrastructure.Repositories;

public class DailyLimitUsageRepository(
    ApplicationDbContext context) : IDailyLimitUsageRepository
{
}
