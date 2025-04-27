using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Mappings;

public class AccountMap : IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> builder)
    {
        builder.ToTable("Accounts");
        builder.HasKey(a => a.Id);
        builder.HasIndex(c => c.Id).IsUnique();
        builder.HasIndex(x => x.IsAccountActive);

        builder.Property(x => x.Id).HasColumnType("uniqueidentifier");
        builder.Property(x => x.CreatedAt).HasColumnType("datetimeoffset");
        builder.Property(x => x.PersonType).HasConversion<string>();
        builder.Property(x => x.IsAccountActive).HasColumnType("bit");

        builder.Property(x => x.CustomerAssetsHeld).HasColumnType("decimal(28, 10)");
        builder.Property(x => x.PrincipalAmount).HasColumnType("decimal(28, 10)");
        builder.Property(x => x.ApprovedOverdraftLimit).HasColumnType("decimal(28, 10)");
        builder.Property(x => x.SelfDeclaredLimit).HasColumnType("decimal(28, 10)");
        builder.Property(x => x.UsedDaysInCurrentCycle).HasColumnType("int");
        builder.Property(x => x.UsedOverLimit).HasColumnType("decimal(28, 10)");

        builder.Property(x => x.RegularInterestDueInCurrentCycle).HasColumnType("decimal(28, 10)");
        builder.Property(x => x.OverLimitInterestDueInCurrentCycle).HasColumnType("decimal(28, 10)");
        builder.Property(x => x.OverLimitFixedFeeDueInCurrentCycle).HasColumnType("decimal(28, 10)");
        builder.Property(x => x.LatePaymentInterestDueInCurrentCycle).HasColumnType("decimal(28, 10)");
        builder.Property(x => x.LatePaymentPenaltyDueInCurrentCycle).HasColumnType("decimal(28, 10)");
        builder.Property(x => x.CreditTaxDueInCurrentCycle).HasColumnType("decimal(28, 10)");
        builder.Property(x => x.FixedCreditTaxDueInCurrentCycle).HasColumnType("decimal(28, 10)");
    }
}
