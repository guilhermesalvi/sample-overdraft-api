using MediatR;

namespace Overdraft.Application.UseCases.DeleteContract;

public record DeleteContractInput(Guid Id) : IRequest;
