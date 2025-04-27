using Domain.Models;
using Infrastructure.Context;

namespace Infrastructure.Repositories;

public class AccountRepository(
    ApplicationDbContext context) : IAccountRepository
{
}
