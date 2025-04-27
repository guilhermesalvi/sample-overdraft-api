using Domain.Models;
using Infrastructure.Context;

namespace Infrastructure.Repositories;

public class ContractAgreementRepository(
    ApplicationDbContext context) : IContractAgreementRepository
{
}
