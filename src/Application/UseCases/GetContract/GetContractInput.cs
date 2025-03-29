using MediatR;
using Overdraft.Domain.Accounts;

namespace Overdraft.Application.UseCases.GetContract;

public record GetContractInput(Guid? Id) : IRequest<IEnumerable<Contract>>;
