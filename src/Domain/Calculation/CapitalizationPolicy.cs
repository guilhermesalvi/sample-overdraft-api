namespace Domain.Calculation;

public static class CapitalizationPolicy
{
    public static decimal Calculate(decimal principal, decimal totalCharges) =>
        principal + totalCharges;
}
