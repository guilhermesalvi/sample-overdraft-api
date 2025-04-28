using Domain.Calculation;
using Domain.Models;
using FluentAssertions;

namespace UnitTests.Domain.Calculation;

public class LatePaymentInterestPolicyTests
{
    [Fact(DisplayName = "Calculate interest using contract daily late-payment rate derived from monthly rate")]
    public void Calculate_interest_using_contract_daily_late_payment_rate_derived_from_monthly_rate()
    {
        // Arrange
        const decimal monthlyLateRate = 0.09m;
        var contract = new Contract { MonthlyLatePaymentInterestRate = monthlyLateRate };
        var entryDate1 = new DateOnly(2025, 4, 25);
        var entryDate2 = new DateOnly(2025, 4, 28);
        const decimal principal1 = 100m;
        const decimal principal2 = 200m;
        var limits = new List<DailyLimitUsageEntry>
        {
            new() { ReferenceDate = entryDate1, PrincipalAmount = principal1 },
            new() { ReferenceDate = entryDate2, PrincipalAmount = principal2 }
        };
        const decimal expectedRate = monthlyLateRate / 30;
        const decimal expected = principal2 * expectedRate;

        // Act
        var result = LatePaymentInterestPolicy.Calculate(limits, contract);

        // Assert
        result.Should().Be(expected);
    }

    [Theory(DisplayName = "Calculate interest returns zero when last principal amount is non-positive")]
    [InlineData(0)]
    [InlineData(-150.25)]
    public void Calculate_interest_returns_zero_when_last_principal_amount_is_non_positive(decimal lastPrincipal)
    {
        // Arrange
        const decimal monthlyLateRate = 0.05m;
        var contract = new Contract { MonthlyLatePaymentInterestRate = monthlyLateRate };
        var limits = new List<DailyLimitUsageEntry>
        {
            new() { ReferenceDate = new DateOnly(2025, 4, 20), PrincipalAmount = 100m },
            new() { ReferenceDate = new DateOnly(2025, 4, 21), PrincipalAmount = lastPrincipal }
        };

        // Act
        var result = LatePaymentInterestPolicy.Calculate(limits, contract);

        // Assert
        result.Should().Be(0m);
    }

    [Fact(DisplayName = "Calculate interest considers only the entry with the latest reference date")]
    public void Calculate_interest_considers_only_latest_reference_date()
    {
        // Arrange
        const decimal monthlyLateRate = 0.07m;
        var contract = new Contract { MonthlyLatePaymentInterestRate = monthlyLateRate };
        var oldest = new DateOnly(2025, 1, 1);
        var middle = new DateOnly(2025, 2, 1);
        var newest = new DateOnly(2025, 3, 1);
        var limits = new List<DailyLimitUsageEntry>
        {
            new() { ReferenceDate = middle, PrincipalAmount = 150m },
            new() { ReferenceDate = newest, PrincipalAmount = 50m },
            new() { ReferenceDate = oldest, PrincipalAmount = 300m }
        };
        const decimal expected = 50m * (monthlyLateRate / 30);

        // Act
        var result = LatePaymentInterestPolicy.Calculate(limits, contract);

        // Assert
        result.Should().Be(expected);
    }

    [Fact(DisplayName = "Calculate interest throws when limits list is empty")]
    public void Calculate_interest_throws_when_limits_list_is_empty()
    {
        // Arrange
        var contract = new Contract { MonthlyLatePaymentInterestRate = 0.04m };
        var limits = new List<DailyLimitUsageEntry>();

        // Act
        Action act = () => LatePaymentInterestPolicy.Calculate(limits, contract);

        // Assert
        act.Should().Throw<InvalidOperationException>()
            .WithMessage("*Sequence contains no elements*");
    }
}
