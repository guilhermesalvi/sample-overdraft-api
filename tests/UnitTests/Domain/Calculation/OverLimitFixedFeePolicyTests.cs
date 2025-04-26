using Domain.Calculation;
using Domain.Models;
using FluentAssertions;

namespace UnitTests.Domain.Calculation;

public class OverLimitFixedFeePolicyTests
{
    [Fact(DisplayName = "Calculate over limit fixed fee when principal amount exceeds approved overdraft limit")]
    public void Calculate_over_limit_fixed_fee_when_principal_amount_exceeds_approved_overdraft_limit()
    {
        // Arrange
        const decimal principalAmount = 2000m;
        const decimal approvedOverdraftLimit = 1000m;
        const decimal overLimitFixedFee = 50m;

        var limits = new List<DailyLimitUsageEntry> { new() { PrincipalAmount = principalAmount } };
        var account = new Account { ApprovedOverdraftLimit = approvedOverdraftLimit };
        var contract = new Contract { OverLimitFixedFee = overLimitFixedFee };

        // Act
        var result = OverLimitFixedFeePolicy.Calculate(limits, account, contract);

        // Assert
        result.Should().Be(overLimitFixedFee);
    }

    [Fact(DisplayName =
        "Calculate over limit fixed fee when all principal amounts are within approved overdraft limit")]
    public void Calculate_over_limit_fixed_fee_when_all_principal_amounts_are_within_approved_overdraft_limit()
    {
        // Arrange
        const decimal principalAmount = 800m;
        const decimal approvedOverdraftLimit = 1000m;
        const decimal overLimitFixedFee = 50m;
        const decimal expectedFee = 0m;

        var limits = new List<DailyLimitUsageEntry> { new() { PrincipalAmount = principalAmount } };
        var account = new Account { ApprovedOverdraftLimit = approvedOverdraftLimit };

        var contract = new Contract { OverLimitFixedFee = overLimitFixedFee };

        // Act
        var result = OverLimitFixedFeePolicy.Calculate(limits, account, contract);

        // Assert
        result.Should().Be(expectedFee);
    }

    [Fact(DisplayName = "Calculate over limit fixed fee when limits list is empty")]
    public void Calculate_over_limit_fixed_fee_when_limits_list_is_empty()
    {
        // Arrange
        var limits = new List<DailyLimitUsageEntry>();
        var account = new Account { ApprovedOverdraftLimit = 1000m };
        var contract = new Contract { OverLimitFixedFee = 50m };

        const decimal expectedFee = 0m;

        // Act
        var result = OverLimitFixedFeePolicy.Calculate(limits, account, contract);

        // Assert
        result.Should().Be(expectedFee);
    }
}
