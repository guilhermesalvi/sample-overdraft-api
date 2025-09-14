using CustomerEnrollment.OverdraftAccounts.Aggregates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CustomerEnrollment.CrossCutting.Database.Mappings;

public class OverdraftAccountMap : IEntityTypeConfiguration<OverdraftAccount>
{
    public void Configure(EntityTypeBuilder<OverdraftAccount> builder)
    {
        builder.ToTable("OverdraftAccounts").HasKey(x => x.Id);

        builder.HasIndex(x => x.Id);
        builder.HasIndex(x => x.IsOverdraftAccountActive);

        builder.Property(x => x.Id).HasColumnType("uniqueidentifier");
        builder.Property(x => x.CustomerId).HasColumnType("uniqueidentifier");
        builder.Property(x => x.CustomerType).HasColumnType("int");
        builder.Property(x => x.IsOverdraftAccountActive).HasColumnType("bit");
        builder.Property(x => x.CreatedAt).HasColumnType("datetimeoffset");
    }
}
