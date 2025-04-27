using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Mappings;

public class ContractMap : IEntityTypeConfiguration<Contract>
{
    public void Configure(EntityTypeBuilder<Contract> builder)
    {
        builder.ToTable("Contracts");
        builder.HasKey(c => c.Id);
        builder.HasIndex(c => c.Id).IsUnique();
        builder.HasIndex(c => c.IsContractActive);

        builder.Property(c => c.Id).HasColumnType("uniqueidentifier");
        builder.Property(c => c.CreatedAt).HasColumnType("datetimeoffset");
        builder.Property(c => c.IsContractActive).HasColumnType("bit");

        builder.Property(c => c.GracePeriodDays).HasColumnType("int");
        builder.Property(c => c.MonthlyInterestRate).HasColumnType("decimal(28, 10)");
        builder.Property(c => c.MonthlyOverLimitInterestRate).HasColumnType("decimal(28, 10)");
        builder.Property(c => c.OverLimitFixedFee).HasColumnType("decimal(28, 10)");
        builder.Property(c => c.MonthlyLatePaymentInterestRate).HasColumnType("decimal(28, 10)");
        builder.Property(c => c.LatePaymentPenaltyRate).HasColumnType("decimal(28, 10)");
    }
}
