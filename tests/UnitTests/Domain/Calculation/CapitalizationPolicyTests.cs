using Domain.Calculation;
using FluentAssertions;

namespace UnitTests.Domain.Calculation;

public class CapitalizationPolicyTests
{
    [Fact(DisplayName = "Capitalization returns sum when principal and total charges are positive")]
    public void Capitalization_returns_sum_when_principal_and_total_charges_are_positive()
    {
        // Arrange
        const decimal principal = 100m;
        const decimal totalCharges = 50m;
        const decimal expected = 150m;

        // Act
        var result = CapitalizationPolicy.Calculate(principal, totalCharges);

        // Assert
        result.Should().Be(expected);
    }

    [Fact(DisplayName = "Capitalization returns sum when principal is zero")]
    public void Capitalization_returns_sum_when_principal_is_zero()
    {
        // Arrange
        const decimal principal = 0m;
        const decimal totalCharges = 75m;
        const decimal expected = 75m;

        // Act
        var result = CapitalizationPolicy.Calculate(principal, totalCharges);

        // Assert
        result.Should().Be(expected);
    }

    [Fact(DisplayName = "Capitalization returns sum when total charges is zero")]
    public void Capitalization_returns_sum_when_total_charges_is_zero()
    {
        // Arrange
        const decimal principal = 200m;
        const decimal totalCharges = 0m;
        const decimal expected = 200m;

        // Act
        var result = CapitalizationPolicy.Calculate(principal, totalCharges);

        // Assert
        result.Should().Be(expected);
    }

    [Fact(DisplayName = "Capitalization returns sum when both principal and total charges are zero")]
    public void Capitalization_returns_sum_when_both_principal_and_total_charges_are_zero()
    {
        // Arrange
        const decimal principal = 0m;
        const decimal totalCharges = 0m;
        const decimal expected = 0m;

        // Act
        var result = CapitalizationPolicy.Calculate(principal, totalCharges);

        // Assert
        result.Should().Be(expected);
    }

    [Fact(DisplayName = "Capitalization returns sum when principal and total charges are negative")]
    public void Capitalization_returns_sum_when_principal_and_total_charges_are_negative()
    {
        // Arrange
        const decimal principal = -100m;
        const decimal totalCharges = -50m;
        const decimal expected = -150m;

        // Act
        var result = CapitalizationPolicy.Calculate(principal, totalCharges);

        // Assert
        result.Should().Be(expected);
    }
}
