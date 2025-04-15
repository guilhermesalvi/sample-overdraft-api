using AutoFixture;
using FluentAssertions;
using Overdraft.Domain.Calculators;
using Overdraft.Domain.Models;

namespace Overdraft.UnitTests.Domain.Calculators;

public class InterestCalculatorTests
{
    private readonly Fixture _fixture = new();

    [Theory(DisplayName = "Apply interest after grace period")]
    [InlineData(100, 4, 5)]
    public void Apply_interest_after_grace_period(
        decimal principal, int gracePeriodDays, int accumulatedUsedDays)
    {
        // Arrange
        var contract = _fixture
            .Build<Contract>()
            .With(x => x.GracePeriodDays, gracePeriodDays)
            .Create();
        var expected = principal * contract.DailyInterestRate;

        // Act
        var result = InterestCalculator.CalculatePrincipalInterest(
            principal, accumulatedUsedDays, contract);

        // Assert
        result.Should().Be(expected);
    }

    [Theory(DisplayName = "Does not apply interest within grace period")]
    [InlineData(100, 4, 4)]
    [InlineData(100, 4, 3)]
    public void Does_not_apply_interest_within_grace_period(
        decimal principal, int gracePeriodDays, int accumulatedUsedDays)
    {
        // Arrange
        var contract = _fixture
            .Build<Contract>()
            .With(x => x.GracePeriodDays, gracePeriodDays)
            .Create();
        const decimal expected = 0m;

        // Act
        var result = InterestCalculator.CalculatePrincipalInterest(
            principal, accumulatedUsedDays, contract);

        // Assert
        result.Should().Be(expected);
    }

    [Theory(DisplayName = "Does not apply interest without principal")]
    [InlineData(0, 4, 5)]
    [InlineData(-1, 4, 5)]
    public void Does_not_apply_interest_without_principal(
        decimal principal, int gracePeriodDays, int accumulatedUsedDays)
    {
        // Arrange
        var contract = _fixture
            .Build<Contract>()
            .With(x => x.GracePeriodDays, gracePeriodDays)
            .Create();
        const decimal expected = 0m;

        // Act
        var result = InterestCalculator.CalculatePrincipalInterest(
            principal, accumulatedUsedDays, contract);

        // Assert
        result.Should().Be(expected);
    }

    [Theory(DisplayName = "Calculate over limit interest to excess")]
    [InlineData(101, 100)]
    [InlineData(200, 100)]
    public void Calculate_over_limit_interest_to_excess(
        decimal principal, decimal approvedOverdraftLimit)
    {
        // Arrange
        var contract = _fixture.Create<Contract>();
        var expected = (principal - approvedOverdraftLimit) * contract.DailyOverLimitInterestRate;

        // Act
        var result = InterestCalculator.CalculateOverLimitInterest(
            principal, approvedOverdraftLimit, contract);

        // Assert
        result.Should().Be(expected);
    }

    [Theory(DisplayName = "Does not apply over limit interest without excess")]
    [InlineData(100, 100)]
    [InlineData(100, 200)]
    public void Does_not_apply_over_limit_interest_without_excess(
        decimal principal, decimal approvedOverdraftLimit)
    {
        // Arrange
        var contract = _fixture.Create<Contract>();
        const decimal expected = 0m;

        // Act
        var result = InterestCalculator.CalculateOverLimitInterest(
            principal, approvedOverdraftLimit, contract);

        // Assert
        result.Should().Be(expected);
    }

    [Theory(DisplayName = "Apply late payment interest")]
    [InlineData(100)]
    public void Apply_late_payment_interest(decimal rolloverPrincipal)
    {
        // Arrange
        var contract = _fixture.Create<Contract>();
        var expected = rolloverPrincipal * contract.DailyLatePaymentInterestRate;

        // Act
        var result = InterestCalculator.CalculateLatePaymentInterest(
            rolloverPrincipal, contract);

        // Assert
        result.Should().Be(expected);
    }

    [Theory(DisplayName = "Does not apply late payment interest without rollover principal")]
    [InlineData(0)]
    [InlineData(-1)]
    public void Does_not_apply_late_payment_without_rollover_principal(decimal rolloverPrincipal)
    {
        // Arrange
        var contract = _fixture.Create<Contract>();
        const decimal expected = 0m;

        // Act
        var result = InterestCalculator.CalculateLatePaymentInterest(
            rolloverPrincipal, contract);

        // Assert
        result.Should().Be(expected);
    }

    [Theory(DisplayName = "Apply late payment penalty to rollover principal")]
    [InlineData(100)]
    public void Apply_late_payment_penalty_to_rollover_principal(decimal rolloverPrincipal)
    {
        // Arrange
        var contract = _fixture.Create<Contract>();
        const bool hasAlreadyApplied = false;
        var expected = rolloverPrincipal * contract.LatePaymentPenaltyRate;

        // Act
        var result = InterestCalculator.CalculateLatePaymentPenalty(
            rolloverPrincipal, contract, hasAlreadyApplied);

        // Assert
        result.Should().Be(expected);
    }

    [Theory(DisplayName = "Does not apply late payment penalty without rollover principal")]
    [InlineData(0)]
    [InlineData(-1)]
    public void Does_not_apply_late_payment_penalty_without_rollover_principal(
        decimal rolloverPrincipal)
    {
        // Arrange
        var contract = _fixture.Create<Contract>();
        const bool hasAlreadyApplied = false;
        const decimal expected = 0m;

        // Act
        var result = InterestCalculator.CalculateLatePaymentPenalty(
            rolloverPrincipal, contract, hasAlreadyApplied);

        // Assert
        result.Should().Be(expected);
    }

    [Theory(DisplayName = "Does not apply late payment penalty when had already applied")]
    [InlineData(0, true)]
    public void Does_not_apply_late_payment_penalty_to_had_already_applied(
        decimal rolloverPrincipal, bool hasAlreadyApplied)
    {
        // Arrange
        var contract = _fixture.Create<Contract>();
        const decimal expected = 0m;

        // Act
        var result = InterestCalculator.CalculateLatePaymentPenalty(
            rolloverPrincipal, contract, hasAlreadyApplied);

        // Assert
        result.Should().Be(expected);
    }

    [Fact(DisplayName = "Calculate total interest")]
    public void Calculate_total_interest()
    {
        // Arrange
        var principalInterest = _fixture.Create<decimal>();
        var overLimitInterest = _fixture.Create<decimal>();
        var latePaymentInterest = _fixture.Create<decimal>();
        var latePaymentPenalty = _fixture.Create<decimal>();
        var expected = principalInterest + overLimitInterest + latePaymentInterest + latePaymentPenalty;

        // Act
        var result = InterestCalculator.CalculateTotalInterest(
            principalInterest,
            overLimitInterest,
            latePaymentInterest,
            latePaymentPenalty);

        // Assert
        result.Should().Be(expected);
    }

    [Fact(DisplayName = "Apply capitalized monthly interest")]
    public void Apply_capitalized_monthly_interest()
    {
        // Arrange
        var principal = _fixture.Create<decimal>();
        var totalInterest = _fixture.Create<decimal>();
        var expected = principal + totalInterest;

        // Act
        var result = InterestCalculator.CapitalizeMonthlyInterest(principal, totalInterest);

        // Assert
        result.Should().Be(expected);
    }
}
