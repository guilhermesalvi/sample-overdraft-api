using MediatR;
using Overdraft.Domain.Accounts;

namespace Overdraft.Application.UseCases.GetAccount;

public record GetAccountInput(Guid? Id) : IRequest<IEnumerable<Account>>;
