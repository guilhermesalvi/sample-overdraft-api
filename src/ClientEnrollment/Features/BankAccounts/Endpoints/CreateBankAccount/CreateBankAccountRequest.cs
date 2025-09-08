using ClientEnrollment.Features.BankAccounts.Domain;

namespace ClientEnrollment.Features.BankAccounts.Endpoints.CreateBankAccount;

public record CreateBankAccountRequest(Guid ClientId, ClientType ClientType)
{
    public BankAccount ToInactiveBankAccount() => new(ClientId, ClientType, IsBankAccountActive: false);
}
