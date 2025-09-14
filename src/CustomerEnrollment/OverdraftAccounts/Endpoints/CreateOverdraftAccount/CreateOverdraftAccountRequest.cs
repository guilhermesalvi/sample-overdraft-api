using CustomerEnrollment.OverdraftAccounts.Aggregates;

namespace CustomerEnrollment.OverdraftAccounts.Endpoints.CreateOverdraftAccount;

public record CreateOverdraftAccountRequest(Guid CustomerId, CustomerType CustomerType)
{
    public OverdraftAccount ToInactiveOverdraftAccount() => new()
    {
        CustomerId = CustomerId,
        CustomerType = CustomerType,
        IsOverdraftAccountActive = false
    };
}
