using CustomerEnrollment.CrossCutting.Database.Context;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace CustomerEnrollment.OverdraftAccounts.Endpoints.CreateOverdraftAccount;

public class CreateOverdraftAccountRequestValidator : AbstractValidator<CreateOverdraftAccountRequest>
{
    public CreateOverdraftAccountRequestValidator(CustomerEnrollmentDbContext context)
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(x => x.CustomerId)
            .NotEmpty().WithMessage("CustomerId must be provided.")
            .CustomAsync(async (customerId, ctx, ct) =>
            {
                var exists = await context.OverdraftAccounts.AnyAsync(x => x.CustomerId == customerId, ct);

                if (exists)
                    ctx.AddFailure(nameof(CreateOverdraftAccountRequest.CustomerId),
                        "CustomerId already has an associated overdraft account.");
            });

        RuleFor(x => x.CustomerType)
            .IsInEnum().WithMessage("CustomerType must be a valid enum value.");
    }
}
