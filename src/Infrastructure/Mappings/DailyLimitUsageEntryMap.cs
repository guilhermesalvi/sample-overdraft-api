using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Overdraft.Domain.Models;

namespace Overdraft.Infrastructure.Mappings;

public class DailyLimitUsageEntryMap : IEntityTypeConfiguration<DailyLimitUsageEntry>
{
    public void Configure(EntityTypeBuilder<DailyLimitUsageEntry> builder)
    {
        builder.ToTable("DailyLimitUsageEntries");
        builder.HasKey(x => x.Id);
        builder.HasIndex(x => x.AccountId);
        builder.HasIndex(x => x.ContractId);
        builder.HasIndex(x => x.ReferenceDate);

        builder.Property(x => x.Id).HasColumnType("uniqueidentifier");
        builder.Property(x => x.AccountId).HasColumnType("uniqueidentifier");
        builder.Property(x => x.ContractId).HasColumnType("uniqueidentifier");
        builder.Property(x => x.ReferenceDate).HasColumnType("date");
        builder.Property(x => x.CreatedAt).HasColumnType("datetimeoffset");

        builder.Property(x => x.PrincipalAmount).HasColumnType("decimal(30, 10)");
        builder.Property(x => x.ApprovedOverdraftLimit).HasColumnType("decimal(30, 10)");
        builder.Property(x => x.SelfDeclaredLimit).HasColumnType("decimal(30, 10)");
        builder.Property(x => x.AccumulatedUsedDays).HasColumnType("int");
        builder.Property(x => x.UsedOverLimit).HasColumnType("decimal(30, 10)");

        builder.Property(x => x.GracePeriodDays).HasColumnType("int");
        builder.Property(x => x.InterestRateApplied).HasColumnType("decimal(30, 10)");
        builder.Property(x => x.CreditTaxRateApplied).HasColumnType("decimal(30, 10)");
        builder.Property(x => x.FixedCreditTaxRateApplied).HasColumnType("decimal(30, 10)");
        builder.Property(x => x.OverLimitRateApplied).HasColumnType("decimal(30, 10)");
        builder.Property(x => x.LatePaymentRateApplied).HasColumnType("decimal(30, 10)");
        builder.Property(x => x.PenaltyRateApplied).HasColumnType("decimal(30, 10)");

        builder.Property(x => x.InterestDue).HasColumnType("decimal(30, 10)");
        builder.Property(x => x.CreditTaxDue).HasColumnType("decimal(30, 10)");
        builder.Property(x => x.FixedCreditTaxDue).HasColumnType("decimal(30, 10)");
        builder.Property(x => x.OverLimitInterestDue).HasColumnType("decimal(30, 10)");
        builder.Property(x => x.LatePaymentInterestDue).HasColumnType("decimal(30, 10)");
        builder.Property(x => x.LatePaymentPenaltyDue).HasColumnType("decimal(30, 10)");
        builder.Property(x => x.TotalDue).HasColumnType("decimal(30, 10)");
    }
}
