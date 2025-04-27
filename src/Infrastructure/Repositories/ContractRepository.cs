using Domain.Models;
using Infrastructure.Context;

namespace Infrastructure.Repositories;

public class ContractRepository(
    ApplicationDbContext context) : IContractRepository
{
}
