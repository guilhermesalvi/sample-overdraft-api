using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Mappings;

public class DailyLimitUsageEntryMap: IEntityTypeConfiguration<DailyLimitUsageEntry>
{
    public void Configure(EntityTypeBuilder<DailyLimitUsageEntry> builder)
    {
        builder.ToTable("DailyLimitUsageEntries");
        builder.HasKey(x => x.Id);
        builder.HasIndex(c => c.Id).IsUnique();
        builder.HasIndex(x => x.AccountId);
        builder.HasIndex(x => x.ReferenceDate);

        builder.Property(x=>x.Id).HasColumnType("uniqueidentifier");
        builder.Property(x=>x.CreatedAt).HasColumnType("datetimeoffset");
        builder.Property(x=>x.AccountId).HasColumnType("uniqueidentifier");
        builder.Property(x=>x.ContractId).HasColumnType("uniqueidentifier");
        builder.Property(x=>x.ReferenceDate).HasColumnType("date");

        builder.Property(x => x.PrincipalAmount).HasColumnType("decimal(28,10)");
        builder.Property(x => x.ApprovedOverdraftLimit).HasColumnType("decimal(28,10)");
        builder.Property(x => x.UsedOverLimit).HasColumnType("decimal(28,10)");
    }
}
