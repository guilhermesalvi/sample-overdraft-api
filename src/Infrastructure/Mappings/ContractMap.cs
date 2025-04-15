using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Overdraft.Domain.Models;

namespace Overdraft.Infrastructure.Mappings;

public class ContractMap : IEntityTypeConfiguration<Contract>
{
    public void Configure(EntityTypeBuilder<Contract> builder)
    {
        builder.ToTable("Contracts");
        builder.HasKey(x => x.Id);
        builder.HasIndex(x => x.IsContractActive);

        builder.Property(x => x.Id).HasColumnType("uniqueidentifier");
        builder.Property(x => x.GracePeriodDays).HasColumnType("int");
        builder.Property(x => x.MonthlyInterestRate).HasColumnType("decimal(30, 10)");
        builder.Property(x => x.MonthlyOverLimitInterestRate).HasColumnType("decimal(30, 10)");
        builder.Property(x => x.MonthlyLatePaymentInterestRate).HasColumnType("decimal(30, 10)");
        builder.Property(x => x.LatePaymentPenaltyRate).HasColumnType("decimal(30, 10)");
        builder.Property(x => x.IsContractActive).HasColumnType("bit");
        builder.Property(x => x.CreatedAt).HasColumnType("datetimeoffset");
    }
}
