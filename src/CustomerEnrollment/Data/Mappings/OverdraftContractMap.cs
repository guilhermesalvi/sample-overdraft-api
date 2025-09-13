using CustomerEnrollment.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CustomerEnrollment.Data.Mappings;

public class OverdraftContractMap : IEntityTypeConfiguration<OverdraftContract>
{
    public void Configure(EntityTypeBuilder<OverdraftContract> builder)
    {
        builder.ToTable("OverdraftContracts").HasKey(x => x.Id);

        builder.HasIndex(x => x.Id);
        builder.HasIndex(x => x.IsOverdraftContractActive);
        builder.HasIndex(x => x.OverdraftAccountId).IsUnique();

        builder.Property(x => x.Id).HasColumnType("uniqueidentifier");
        builder.Property(x => x.OverdraftAccountId).HasColumnType("uniqueidentifier");
        builder.Property(x => x.GracePeriodDays).HasColumnType("int");
        builder.Property(x => x.MonthlyInterestRate).HasColumnType("decimal(19,8)");
        builder.Property(x => x.MonthlyOverLimitInterestRate).HasColumnType("decimal(19,8)");
        builder.Property(x => x.OverLimitFixedFee).HasColumnType("decimal(19,8)");
        builder.Property(x => x.MonthlyLatePaymentInterestRate).HasColumnType("decimal(19,8)");
        builder.Property(x => x.LatePaymentPenaltyRate).HasColumnType("decimal(19,8)");
        builder.Property(x => x.CreatedAt).HasColumnType("datetimeoffset");
        builder.Property(x => x.IsOverdraftContractActive).HasColumnType("bit");
        builder.Property(x => x.SignatureDate).HasColumnType("datetimeoffset");
        builder.Property(x => x.CanceledAt).HasColumnType("datetimeoffset");

        builder
            .HasOne<OverdraftAccount>()
            .WithOne()
            .HasForeignKey<OverdraftContract>(x => x.OverdraftAccountId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
