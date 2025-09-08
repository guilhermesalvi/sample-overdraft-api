using CustomerEnrollment.Features.BankAccounts.Domain;

namespace CustomerEnrollment.Features.BankAccounts.Endpoints.CreateBankAccount;

public record CreateBankAccountRequest(Guid CustomerId, CustomerType CustomerType)
{
    public BankAccount ToInactiveBankAccount() => new(CustomerId, CustomerType, IsBankAccountActive: false);
}
