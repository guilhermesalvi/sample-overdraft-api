using Domain.Calculation;
using Domain.Models;
using FluentAssertions;

namespace UnitTests.Domain.Calculation;

public class LatePaymentInterestPolicyTests
{
    [Fact(DisplayName = "Calculate late payment interest when all principal amounts are positive")]
    public void Calculate_late_payment_interest_when_all_principal_amounts_are_positive()
    {
        // Arrange
        const decimal principalAmount1 = 100m;
        const decimal principalAmount2 = 200m;
        const decimal monthlyLatePaymentInterestRate = 0.06m;
        const decimal expectedInterest = (principalAmount1 + principalAmount2) * (monthlyLatePaymentInterestRate / 30);

        var lateDays = new List<DailyLimitUsageEntry>
        {
            new() { PrincipalAmount = principalAmount1 },
            new() { PrincipalAmount = principalAmount2 }
        };

        var contract = new Contract
        {
            MonthlyLatePaymentInterestRate = monthlyLatePaymentInterestRate
        };

        // Act
        var result = LatePaymentInterestPolicy.Calculate(lateDays, contract);

        // Assert
        result.Should().Be(expectedInterest);
    }

    [Fact(DisplayName = "Calculate late payment interest when principal amounts include zero and negative values")]
    public void Calculate_late_payment_interest_when_principal_amounts_include_zero_and_negative_values()
    {
        // Arrange
        const decimal principalAmountPositive = 150m;
        const decimal principalAmountNegative = -50m;
        const decimal principalAmountZero = 0m;
        const decimal monthlyLatePaymentInterestRate = 0.03m;
        const decimal expectedInterest = principalAmountPositive * (monthlyLatePaymentInterestRate / 30);

        var lateDays = new List<DailyLimitUsageEntry>
        {
            new() { PrincipalAmount = principalAmountPositive },
            new() { PrincipalAmount = principalAmountNegative },
            new() { PrincipalAmount = principalAmountZero }
        };

        var contract = new Contract
        {
            MonthlyLatePaymentInterestRate = monthlyLatePaymentInterestRate
        };

        // Act
        var result = LatePaymentInterestPolicy.Calculate(lateDays, contract);

        // Assert
        result.Should().Be(expectedInterest);
    }

    [Fact(DisplayName = "Calculate late payment interest when there are no late days")]
    public void Calculate_late_payment_interest_when_there_are_no_late_days()
    {
        // Arrange
        var lateDays = new List<DailyLimitUsageEntry>();

        var contract = new Contract
        {
            MonthlyLatePaymentInterestRate = 0.05m
        };

        const decimal expectedInterest = 0m;

        // Act
        var result = LatePaymentInterestPolicy.Calculate(lateDays, contract);

        // Assert
        result.Should().Be(expectedInterest);
    }

    [Fact(DisplayName = "Calculate late payment interest when all principal amounts are zero or negative")]
    public void Calculate_late_payment_interest_when_all_principal_amounts_are_zero_or_negative()
    {
        // Arrange
        var lateDays = new List<DailyLimitUsageEntry>
        {
            new() { PrincipalAmount = 0m },
            new() { PrincipalAmount = -100m }
        };

        var contract = new Contract
        {
            MonthlyLatePaymentInterestRate = 0.04m
        };

        const decimal expectedInterest = 0m;

        // Act
        var result = LatePaymentInterestPolicy.Calculate(lateDays, contract);

        // Assert
        result.Should().Be(expectedInterest);
    }
}
