using Domain.Calculation;
using Domain.Models;
using FluentAssertions;

namespace UnitTests.Domain.Calculation;

public class RegularInterestPolicyTests
{
    [Fact(DisplayName = "Calculate regular interest when grace period is not exceeded")]
    public void Calculate_regular_interest_when_grace_period_is_not_exceeded()
    {
        // Arrange
        const decimal principalAmount = 500m;
        const decimal approvedOverdraftLimit = 1000m;
        const int gracePeriodDays = 3;
        const decimal monthlyInterestRate = 0.3m;

        var limits = new List<DailyLimitUsageEntry>
        {
            new() { PrincipalAmount = principalAmount },
            new() { PrincipalAmount = principalAmount }
        };

        var account = new Account { ApprovedOverdraftLimit = approvedOverdraftLimit };

        var contract = new Contract
        {
            GracePeriodDays = gracePeriodDays,
            MonthlyInterestRate = monthlyInterestRate
        };

        const decimal expectedInterest = 0m;

        // Act
        var result = RegularInterestPolicy.Calculate(limits, account, contract);

        // Assert
        result.Should().Be(expectedInterest);
    }

    [Fact(DisplayName =
        "Calculate regular interest when grace period is exceeded and principal amounts are below approved limit")]
    public void
        Calculate_regular_interest_when_grace_period_is_exceeded_and_principal_amounts_are_below_approved_limit()
    {
        // Arrange
        const decimal principalAmount = 800m;
        const decimal approvedOverdraftLimit = 1000m;
        const int gracePeriodDays = 1;
        const decimal monthlyInterestRate = 0.3m;

        var limits = new List<DailyLimitUsageEntry>
        {
            new() { PrincipalAmount = principalAmount },
            new() { PrincipalAmount = principalAmount }
        };

        var account = new Account { ApprovedOverdraftLimit = approvedOverdraftLimit };

        var contract = new Contract
        {
            GracePeriodDays = gracePeriodDays,
            MonthlyInterestRate = monthlyInterestRate
        };

        var dailyInterestRate = contract.DailyInterestRate;
        var expectedInterest = limits.Count * principalAmount * dailyInterestRate;

        // Act
        var result = RegularInterestPolicy.Calculate(limits, account, contract);

        // Assert
        result.Should().Be(expectedInterest);
    }

    [Fact(DisplayName =
        "Calculate regular interest when principal amount exceeds approved limit and grace period is exceeded")]
    public void Calculate_regular_interest_when_principal_amount_exceeds_approved_limit_and_grace_period_is_exceeded()
    {
        // Arrange
        const decimal principalAmount = 2000m;
        const decimal approvedOverdraftLimit = 1000m;
        const int gracePeriodDays = 0;
        const decimal monthlyInterestRate = 0.3m;

        var limits = new List<DailyLimitUsageEntry> { new() { PrincipalAmount = principalAmount } };
        var account = new Account { ApprovedOverdraftLimit = approvedOverdraftLimit };

        var contract = new Contract
        {
            GracePeriodDays = gracePeriodDays,
            MonthlyInterestRate = monthlyInterestRate
        };

        var dailyInterestRate = contract.DailyInterestRate;
        var cappedPrincipal = approvedOverdraftLimit;
        var expectedInterest = cappedPrincipal * dailyInterestRate;

        // Act
        var result = RegularInterestPolicy.Calculate(limits, account, contract);

        // Assert
        result.Should().Be(expectedInterest);
    }

    [Fact(DisplayName = "Calculate regular interest when no positive principal amounts exist")]
    public void Calculate_regular_interest_when_no_positive_principal_amounts_exist()
    {
        // Arrange
        const decimal approvedOverdraftLimit = 1000m;
        const int gracePeriodDays = 0;
        const decimal monthlyInterestRate = 0.3m;

        var limits = new List<DailyLimitUsageEntry>
        {
            new() { PrincipalAmount = 0m },
            new() { PrincipalAmount = -100m }
        };

        var account = new Account { ApprovedOverdraftLimit = approvedOverdraftLimit };

        var contract = new Contract
        {
            GracePeriodDays = gracePeriodDays,
            MonthlyInterestRate = monthlyInterestRate
        };

        const decimal expectedInterest = 0m;

        // Act
        var result = RegularInterestPolicy.Calculate(limits, account, contract);

        // Assert
        result.Should().Be(expectedInterest);
    }

    [Fact(DisplayName = "Calculate regular interest when limits list is empty")]
    public void Calculate_regular_interest_when_limits_list_is_empty()
    {
        // Arrange
        var limits = new List<DailyLimitUsageEntry>();
        var account = new Account { ApprovedOverdraftLimit = 1000m };

        var contract = new Contract
        {
            GracePeriodDays = 2,
            MonthlyInterestRate = 0.3m
        };

        const decimal expectedInterest = 0m;

        // Act
        var result = RegularInterestPolicy.Calculate(limits, account, contract);

        // Assert
        result.Should().Be(expectedInterest);
    }
}
