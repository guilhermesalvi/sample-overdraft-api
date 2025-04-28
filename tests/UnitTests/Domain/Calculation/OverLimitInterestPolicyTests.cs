using Domain.Calculation;
using Domain.Models;
using FluentAssertions;

namespace UnitTests.Domain.Calculation;

public class OverLimitInterestPolicyTests
{
    [Fact(DisplayName = "Calculate over limit interest when principal amount exceeds approved overdraft limit")]
    public void Calculate_over_limit_interest_when_principal_amount_exceeds_approved_overdraft_limit()
    {
        // Arrange
        const decimal principalAmount = 2000m;
        const decimal approvedOverdraftLimit = 1000m;
        const decimal monthlyOverLimitInterestRate = 0.3m;
        const decimal overLimitAmount = principalAmount - approvedOverdraftLimit;

        var limits = new List<DailyLimitUsageEntry> { new() { PrincipalAmount = principalAmount } };
        var account = new Account { ApprovedOverdraftLimit = approvedOverdraftLimit };
        var contract = new Contract { MonthlyOverLimitInterestRate = monthlyOverLimitInterestRate };

        var expectedInterest = overLimitAmount * contract.DailyOverLimitInterestRate;

        // Act
        var result = OverLimitInterestPolicy.Calculate(limits, account, contract);

        // Assert
        result.Should().Be(expectedInterest);
    }

    [Fact(DisplayName = "Calculate over limit interest when all principal amounts are within approved overdraft limit")]
    public void Calculate_over_limit_interest_when_all_principal_amounts_are_within_approved_overdraft_limit()
    {
        // Arrange
        const decimal principalAmount = 800m;
        const decimal approvedOverdraftLimit = 1000m;
        const decimal monthlyOverLimitInterestRate = 0.3m;
        const decimal expectedInterest = 0m;

        var limits = new List<DailyLimitUsageEntry> { new() { PrincipalAmount = principalAmount } };
        var account = new Account { ApprovedOverdraftLimit = approvedOverdraftLimit };
        var contract = new Contract { MonthlyOverLimitInterestRate = monthlyOverLimitInterestRate };

        // Act
        var result = OverLimitInterestPolicy.Calculate(limits, account, contract);

        // Assert
        result.Should().Be(expectedInterest);
    }

    [Fact(DisplayName = "Calculate over limit interest when limits list is empty")]
    public void Calculate_over_limit_interest_when_limits_list_is_empty()
    {
        // Arrange
        var limits = new List<DailyLimitUsageEntry>();
        var account = new Account { ApprovedOverdraftLimit = 1000m };
        var contract = new Contract { MonthlyOverLimitInterestRate = 0.3m };

        const decimal expectedInterest = 0m;

        // Act
        var result = OverLimitInterestPolicy.Calculate(limits, account, contract);

        // Assert
        result.Should().Be(expectedInterest);
    }

    [Fact(DisplayName =
        "Calculate over limit interest when multiple principal amounts exceed approved overdraft limit")]
    public void Calculate_over_limit_interest_when_multiple_principal_amounts_exceed_approved_overdraft_limit()
    {
        // Arrange
        const decimal firstPrincipalAmount = 1500m;
        const decimal secondPrincipalAmount = 1800m;
        const decimal approvedOverdraftLimit = 1000m;
        const decimal monthlyOverLimitInterestRate = 0.3m;

        const decimal firstOverLimit = firstPrincipalAmount - approvedOverdraftLimit;
        const decimal secondOverLimit = secondPrincipalAmount - approvedOverdraftLimit;

        var limits = new List<DailyLimitUsageEntry>
        {
            new() { PrincipalAmount = firstPrincipalAmount },
            new() { PrincipalAmount = secondPrincipalAmount }
        };

        var account = new Account { ApprovedOverdraftLimit = approvedOverdraftLimit };
        var contract = new Contract { MonthlyOverLimitInterestRate = monthlyOverLimitInterestRate };

        var expectedInterest = (firstOverLimit + secondOverLimit) * contract.DailyOverLimitInterestRate;

        // Act
        var result = OverLimitInterestPolicy.Calculate(limits, account, contract);

        // Assert
        result.Should().Be(expectedInterest);
    }
}
