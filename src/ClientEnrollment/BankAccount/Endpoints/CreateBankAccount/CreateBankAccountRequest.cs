namespace ClientEnrollment.BankAccount.Endpoints.CreateBankAccount;

public record CreateBankAccountRequest(string CustomerId, string CustomerType)
{
    public Account ToInactiveAccount() => new(Guid.Parse(CustomerId), Enum.Parse<CustomerType>(CustomerType), IsAccountActive: false);
}
