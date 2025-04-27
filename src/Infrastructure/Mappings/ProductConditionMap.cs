using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Mappings;

public class ProductConditionMap : IEntityTypeConfiguration<ProductCondition>
{
    public void Configure(EntityTypeBuilder<ProductCondition> builder)
    {
        builder.ToTable("ProductConditions");
        builder.HasKey(p => p.Id);
        builder.HasIndex(p => p.Id).IsUnique();
        builder.HasIndex(p => p.ContractId);
        builder.HasIndex(p => p.MinAssetsHeld);
        builder.HasIndex(p => p.MaxAssetsHeld);

        builder.Property(p => p.Id).HasColumnType("uniqueidentifier");
        builder.Property(p => p.CreatedAt).HasColumnType("datetimeoffset");
        builder.Property(p => p.ContractId).HasColumnType("uniqueidentifier");

        builder.Property(p => p.MinAssetsHeld).HasColumnType("decimal(28, 10)");
        builder.Property(p => p.MaxAssetsHeld).HasColumnType("decimal(28, 10)");
    }
}
