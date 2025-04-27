using Domain.Models;
using Infrastructure.Context;

namespace Infrastructure.Repositories;

public class ProductConditionRepository(
    ApplicationDbContext context) : IProductConditionRepository
{
}
