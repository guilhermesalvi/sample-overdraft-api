namespace Overdraft.Domain.Accounts;

public static class CreditTaxCalculator
{
    public static decimal CalculateCreditTax(decimal principal, decimal rate)
        => principal * rate;

    public static decimal CalculateFixedCreditTax(decimal principal, decimal fixedRate, bool hasAlreadyApplied)
        => hasAlreadyApplied ? 0m : principal * fixedRate;
}
