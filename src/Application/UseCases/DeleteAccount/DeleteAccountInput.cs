using MediatR;

namespace Overdraft.Application.UseCases.DeleteAccount;

public record DeleteAccountInput(Guid Id) : IRequest;
