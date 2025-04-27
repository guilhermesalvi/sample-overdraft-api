using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Mappings;

public class MonthlyChargeSnapshotMap : IEntityTypeConfiguration<MonthlyChargeSnapshot>
{
    public void Configure(EntityTypeBuilder<MonthlyChargeSnapshot> builder)
    {
        builder.ToTable("MonthlyChargeSnapshots");
        builder.HasKey(m => m.Id);
        builder.HasIndex(m => m.Id).IsUnique();
        builder.HasIndex(m => m.AccountId);
        builder.HasIndex(x => x.ReferenceDate);

        builder.Property(m => m.Id).HasColumnType("uniqueidentifier");
        builder.Property(m => m.CreatedAt).HasColumnType("datetimeoffset");
        builder.Property(m => m.AccountId).HasColumnType("uniqueidentifier");
        builder.Property(m => m.ContractId).HasColumnType("uniqueidentifier");
        builder.Property(m => m.ReferenceDate).HasColumnType("date");

        builder.Property(m => m.ApprovedOverdraftLimit).HasColumnType("decimal(28, 10)");
        builder.Property(m => m.UsedDaysInCurrentCycle).HasColumnType("int");
        builder.Property(m => m.GracePeriodDays).HasColumnType("int");

        builder.Property(m => m.TotalRegularInterestDue).HasColumnType("decimal(28, 10)");
        builder.Property(m => m.TotalOverLimitInterestDue).HasColumnType("decimal(28, 10)");
        builder.Property(m => m.TotalOverLimitFixedFeeDue).HasColumnType("decimal(28, 10)");
        builder.Property(m => m.TotalLatePaymentInterestDue).HasColumnType("decimal(28, 10)");
        builder.Property(m => m.TotalLatePaymentPenaltyDue).HasColumnType("decimal(28, 10)");
        builder.Property(m => m.TotalCreditTaxDue).HasColumnType("decimal(28, 10)");
        builder.Property(m => m.TotalFixedCreditTaxDue).HasColumnType("decimal(28, 10)");

        builder.Property(m => m.TotalDue).HasColumnType("decimal(28, 10)");
        builder.Property(m => m.CapitalizedPrincipal).HasColumnType("decimal(28, 10)");
    }
}
