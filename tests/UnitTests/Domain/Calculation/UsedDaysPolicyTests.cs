using Domain.Calculation;
using Domain.Models;
using FluentAssertions;

namespace UnitTests.Domain.Calculation;

public class UsedDaysPolicyTests
{
    [Fact(DisplayName = "Calculate used days when all principal amounts are positive")]
    public void Calculate_used_days_when_all_principal_amounts_are_positive()
    {
        // Arrange
        const decimal principal1 = 100m;
        const decimal principal2 = 200m;
        var limits = new List<DailyLimitUsageEntry>
        {
            new() { PrincipalAmount = principal1 },
            new() { PrincipalAmount = principal2 }
        };
        const int expected = 2;

        // Act
        var result = UsedDaysPolicy.Calculate(limits);

        // Assert
        result.Should().Be(expected);
    }

    [Fact(DisplayName = "Calculate used days ignoring non-positive principal amounts")]
    public void Calculate_used_days_ignoring_non_positive_principal_amounts()
    {
        // Arrange
        const decimal positiveAmount = 100m;
        const decimal zeroAmount = 0m;
        const decimal negativeAmount = -50m;
        var limits = new List<DailyLimitUsageEntry>
        {
            new() { PrincipalAmount = positiveAmount },
            new() { PrincipalAmount = zeroAmount },
            new() { PrincipalAmount = negativeAmount },
            new() { PrincipalAmount = positiveAmount }
        };
        const int expected = 2;

        // Act
        var result = UsedDaysPolicy.Calculate(limits);

        // Assert
        result.Should().Be(expected);
    }

    [Fact(DisplayName = "Calculate used days when no limits are provided")]
    public void Calculate_used_days_when_no_limits_are_provided()
    {
        // Arrange
        var limits = new List<DailyLimitUsageEntry>();
        const int expected = 0;

        // Act
        var result = UsedDaysPolicy.Calculate(limits);

        // Assert
        result.Should().Be(expected);
    }
}
