using AutoFixture;
using FluentAssertions;
using Overdraft.Domain.Calculators;

namespace Overdraft.UnitTests.Domain.Calculators;

public class BrazilianCreditTaxCalculatorTests
{
    private readonly Fixture _fixture = new();

    [Fact(DisplayName = "Apply daily credit tax of 0.000082%")]
    public void Apply_daily_credit_tax_of_correct_percentage()
    {
        // Arrange
        var principal = _fixture.Create<decimal>();
        const decimal dailyCreditTax = 0.000082m;
        var expected = principal * dailyCreditTax;

        // Act
        var result = BrazilianCreditTaxCalculator.CalculateCreditTax(principal);

        // Assert
        result.Should().Be(expected);
    }

    [Fact(DisplayName = "Apply fixed credit tax of 0.0038m%")]
    public void Apply_fixed_credit_tax_of_correct_percentage()
    {
        // Arrange
        var principal = _fixture.Create<decimal>();
        const decimal dailyCreditTax = 0.0038m;
        var expected = principal * dailyCreditTax;
        const bool hasAlreadyApplied = false;

        // Act
        var result = BrazilianCreditTaxCalculator.CalculateFixedCreditTax(principal, hasAlreadyApplied);

        // Assert
        result.Should().Be(expected);
    }

    [Fact(DisplayName = "Does not apply fixed credit tax when had already applied")]
    public void Does_not_apply_fixed_credit_tax_when_had_already_applied()
    {
        // Arrange
        var principal = _fixture.Create<decimal>();
        const decimal expected = 0m;
        const bool hasAlreadyApplied = true;

        // Act
        var result = BrazilianCreditTaxCalculator.CalculateFixedCreditTax(principal, hasAlreadyApplied);

        // Assert
        result.Should().Be(expected);
    }
}
