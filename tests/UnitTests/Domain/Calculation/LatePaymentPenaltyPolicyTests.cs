using Domain.Calculation;
using Domain.Models;
using FluentAssertions;

namespace UnitTests.Domain.Calculation;

public class LatePaymentPenaltyPolicyTests
{
    [Fact(DisplayName =
        "Calculate late payment penalty when penalty has not been applied and rollover balance is positive")]
    public void Calculate_late_payment_penalty_when_penalty_has_not_been_applied_and_rollover_balance_is_positive()
    {
        // Arrange
        const bool hasPenaltyBeenApplied = false;
        const decimal rolloverBalance = 500m;
        const decimal latePaymentPenaltyRate = 0.1m;
        const decimal expectedPenalty = rolloverBalance * latePaymentPenaltyRate;

        var contract = new Contract
        {
            LatePaymentPenaltyRate = latePaymentPenaltyRate
        };

        // Act
        var result = LatePaymentPenaltyPolicy.Calculate(hasPenaltyBeenApplied, rolloverBalance, contract);

        // Assert
        result.Should().Be(expectedPenalty);
    }

    [Fact(DisplayName = "Calculate late payment penalty when penalty has already been applied")]
    public void Calculate_late_payment_penalty_when_penalty_has_already_been_applied()
    {
        // Arrange
        const bool hasPenaltyBeenApplied = true;
        const decimal rolloverBalance = 500m;
        const decimal latePaymentPenaltyRate = 0.1m;
        const decimal expectedPenalty = 0m;

        var contract = new Contract
        {
            LatePaymentPenaltyRate = latePaymentPenaltyRate
        };

        // Act
        var result = LatePaymentPenaltyPolicy.Calculate(hasPenaltyBeenApplied, rolloverBalance, contract);

        // Assert
        result.Should().Be(expectedPenalty);
    }

    [Fact(DisplayName = "Calculate late payment penalty when rollover balance is zero")]
    public void Calculate_late_payment_penalty_when_rollover_balance_is_zero()
    {
        // Arrange
        const bool hasPenaltyBeenApplied = false;
        const decimal rolloverBalance = 0m;
        const decimal latePaymentPenaltyRate = 0.1m;
        const decimal expectedPenalty = 0m;

        var contract = new Contract
        {
            LatePaymentPenaltyRate = latePaymentPenaltyRate
        };

        // Act
        var result = LatePaymentPenaltyPolicy.Calculate(hasPenaltyBeenApplied, rolloverBalance, contract);

        // Assert
        result.Should().Be(expectedPenalty);
    }

    [Fact(DisplayName = "Calculate late payment penalty when rollover balance is negative")]
    public void Calculate_late_payment_penalty_when_rollover_balance_is_negative()
    {
        // Arrange
        const bool hasPenaltyBeenApplied = false;
        const decimal rolloverBalance = -100m;
        const decimal latePaymentPenaltyRate = 0.1m;
        const decimal expectedPenalty = 0m;

        var contract = new Contract
        {
            LatePaymentPenaltyRate = latePaymentPenaltyRate
        };

        // Act
        var result = LatePaymentPenaltyPolicy.Calculate(hasPenaltyBeenApplied, rolloverBalance, contract);

        // Assert
        result.Should().Be(expectedPenalty);
    }
}
