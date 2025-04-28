using Domain.Calculation;
using Domain.Models;
using FluentAssertions;

namespace UnitTests.Domain.Calculation;

public class LatePaymentPenaltyPolicyTests
{
    [Fact(DisplayName = "Calculate penalty when rollover balance positive and no previous penalty")]
    public void Calculate_penalty_when_rollover_balance_positive_and_no_previous_penalty()
    {
        // Arrange
        const decimal monthlyPenaltyRate = 0.10m;
        var contract = new Contract { LatePaymentPenaltyRate = monthlyPenaltyRate };
        const decimal principal = 200m;
        const decimal overdraft = 100m;
        var limits = new List<DailyLimitUsageEntry>
        {
            new() { ReferenceDate = new DateOnly(2025, 4, 27), PrincipalAmount = principal }
        };
        var account = new Account { ApprovedOverdraftLimit = overdraft };
        const decimal rolloverBalance = principal - overdraft;
        const decimal expected = rolloverBalance * monthlyPenaltyRate;

        // Act
        var result = LatePaymentPenaltyPolicy.Calculate(
            hasPenaltyBeenApplied: false,
            limits,
            contract,
            account);

        // Assert
        result.Should().Be(expected);
    }

    [Theory(DisplayName = "Calculate penalty returns zero when rollover balance is non-positive")]
    [InlineData(100, 100)]
    [InlineData(50, 100)]
    public void Calculate_penalty_returns_zero_when_rollover_balance_is_non_positive(decimal principal,
        decimal overdraft)
    {
        // Arrange
        const decimal monthlyPenaltyRate = 0.08m;
        var contract = new Contract { LatePaymentPenaltyRate = monthlyPenaltyRate };
        var limits = new List<DailyLimitUsageEntry>
        {
            new() { ReferenceDate = new DateOnly(2025, 4, 26), PrincipalAmount = principal }
        };
        var account = new Account { ApprovedOverdraftLimit = overdraft };

        // Act
        var result = LatePaymentPenaltyPolicy.Calculate(
            hasPenaltyBeenApplied: false,
            limits,
            contract,
            account);

        // Assert
        result.Should().Be(0m);
    }

    [Fact(DisplayName = "Calculate penalty returns zero when penalty has already been applied")]
    public void Calculate_penalty_returns_zero_when_penalty_has_already_been_applied()
    {
        // Arrange
        const decimal monthlyPenaltyRate = 0.12m;
        var contract = new Contract { LatePaymentPenaltyRate = monthlyPenaltyRate };
        const decimal principal = 300m;
        const decimal overdraft = 150m;
        var limits = new List<DailyLimitUsageEntry>
        {
            new() { ReferenceDate = new DateOnly(2025, 4, 25), PrincipalAmount = principal }
        };
        var account = new Account { ApprovedOverdraftLimit = overdraft };

        // Act
        var result = LatePaymentPenaltyPolicy.Calculate(
            hasPenaltyBeenApplied: true, limits, contract, account);

        // Assert
        result.Should().Be(0m);
    }

    [Fact(DisplayName = "Calculate penalty considers only the entry with the latest reference date")]
    public void Calculate_penalty_considers_only_latest_reference_date()
    {
        // Arrange
        const decimal monthlyPenaltyRate = 0.15m;
        var contract = new Contract { LatePaymentPenaltyRate = monthlyPenaltyRate };
        var oldest = new DateOnly(2025, 1, 1);
        var middle = new DateOnly(2025, 2, 1);
        var newest = new DateOnly(2025, 3, 1);
        var limits = new List<DailyLimitUsageEntry>
        {
            new() { ReferenceDate = middle, PrincipalAmount = 500m },
            new() { ReferenceDate = oldest, PrincipalAmount = 800m },
            new() { ReferenceDate = newest, PrincipalAmount = 200m }
        };
        var account = new Account { ApprovedOverdraftLimit = 100m };
        const decimal rolloverBalance = 200m - 100m;
        const decimal expected = rolloverBalance * monthlyPenaltyRate;

        // Act
        var result = LatePaymentPenaltyPolicy.Calculate(
            hasPenaltyBeenApplied: false, limits, contract, account);

        // Assert
        result.Should().Be(expected);
    }

    [Fact(DisplayName = "Calculate penalty throws when limits list is empty")]
    public void Calculate_penalty_throws_when_limits_list_is_empty()
    {
        // Arrange
        var contract = new Contract { LatePaymentPenaltyRate = 0.05m };
        var limits = new List<DailyLimitUsageEntry>();
        var account = new Account { ApprovedOverdraftLimit = 100m };

        // Act
        Action act = () => LatePaymentPenaltyPolicy.Calculate(
            hasPenaltyBeenApplied: false, limits, contract, account);

        // Assert
        act.Should().Throw<InvalidOperationException>()
            .WithMessage("*Sequence contains no elements*");
    }
}
