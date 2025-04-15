namespace Overdraft.Domain.Calculators;

public static class BrazilianCreditTaxCalculator
{
    public const decimal DailyTaxRate = 0.000082m;
    public const decimal FixedTaxRate = 0.0038m;

    public static decimal CalculateCreditTax(decimal principal)
    {
        if (principal <= 0) return 0m;
        return principal * DailyTaxRate;
    }

    public static decimal CalculateFixedCreditTax(decimal principal, bool hasAlreadyApplied)
    {
        if (hasAlreadyApplied || principal <= 0) return 0m;
        return principal * FixedTaxRate;
    }

    public static decimal CalculateTotalCreditTax(decimal dailyCreditTax, decimal fixedCreditTax)
        => dailyCreditTax + fixedCreditTax;
}
