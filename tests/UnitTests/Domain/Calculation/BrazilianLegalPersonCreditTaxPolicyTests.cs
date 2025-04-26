using Domain.Calculation;
using Domain.Models;
using FluentAssertions;

namespace UnitTests.Domain.Calculation;

public class BrazilianLegalPersonCreditTaxPolicyTests
{
    [Fact(DisplayName = "Calculate total daily credit tax when principal amounts are positive for legal person")]
    public void Calculate_total_daily_credit_tax_when_principal_amounts_are_positive()
    {
        // Arrange
        const decimal principal1 = 1000m;
        const decimal principal2 = 2000m;
        var limits = new List<DailyLimitUsageEntry>
        {
            new() { PrincipalAmount = principal1 },
            new() { PrincipalAmount = principal2 }
        };

        const decimal expectedTax = (principal1 + principal2) * BrazilianLegalPersonCreditTaxPolicy.DailyTaxRate;

        // Act
        var result = BrazilianLegalPersonCreditTaxPolicy.CalculateTotalDailyCreditTax(limits);

        // Assert
        result.Should().Be(expectedTax);
    }

    [Fact(DisplayName = "Calculate total daily credit tax ignoring non-positive principal amounts for legal person")]
    public void Calculate_total_daily_credit_tax_ignoring_non_positive_principal_amounts()
    {
        // Arrange
        const decimal principal1 = 1000m;
        const decimal principal2 = 0m;
        const decimal principal3 = -500m;
        var limits = new List<DailyLimitUsageEntry>
        {
            new() { PrincipalAmount = principal1 },
            new() { PrincipalAmount = principal2 },
            new() { PrincipalAmount = principal3 }
        };

        const decimal expectedTax = principal1 * BrazilianLegalPersonCreditTaxPolicy.DailyTaxRate;

        // Act
        var result = BrazilianLegalPersonCreditTaxPolicy.CalculateTotalDailyCreditTax(limits);

        // Assert
        result.Should().Be(expectedTax);
    }

    [Fact(DisplayName = "Calculate monthly fixed credit tax when it has already been charged for legal person")]
    public void Calculate_monthly_fixed_credit_tax_when_it_has_already_been_charged()
    {
        // Arrange
        var limits = new List<DailyLimitUsageEntry>
        {
            new() { PrincipalAmount = 1000m }
        };

        // Act
        var result =
            BrazilianLegalPersonCreditTaxPolicy.CalculateMonthlyFixedCreditTax(limits, hasAlreadyBeenCharged: true);

        // Assert
        result.Should().Be(0m);
    }

    [Fact(DisplayName = "Calculate monthly fixed credit tax using maximum positive principal amount for legal person")]
    public void Calculate_monthly_fixed_credit_tax_using_maximum_positive_principal_amount()
    {
        // Arrange
        const decimal principal1 = 1000m;
        const decimal principal2 = 2000m;
        const decimal principal3 = -500m;
        var limits = new List<DailyLimitUsageEntry>
        {
            new() { PrincipalAmount = principal1 },
            new() { PrincipalAmount = principal2 },
            new() { PrincipalAmount = principal3 }
        };

        const decimal expectedTax = principal2 * BrazilianLegalPersonCreditTaxPolicy.FixedTaxRate;

        // Act
        var result =
            BrazilianLegalPersonCreditTaxPolicy.CalculateMonthlyFixedCreditTax(limits, hasAlreadyBeenCharged: false);

        // Assert
        result.Should().Be(expectedTax);
    }

    [Fact(DisplayName = "Calculate monthly fixed credit tax when all principal amounts are non-positive for legal person")]
    public void Calculate_monthly_fixed_credit_tax_when_all_principal_amounts_are_non_positive()
    {
        // Arrange
        var limits = new List<DailyLimitUsageEntry>
        {
            new() { PrincipalAmount = -100m },
            new() { PrincipalAmount = 0m }
        };

        const decimal expectedTax = 0m;

        // Act
        var result =
            BrazilianLegalPersonCreditTaxPolicy.CalculateMonthlyFixedCreditTax(limits, hasAlreadyBeenCharged: false);

        // Assert
        result.Should().Be(expectedTax);
    }
}
