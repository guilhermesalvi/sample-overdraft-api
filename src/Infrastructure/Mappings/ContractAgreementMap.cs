using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Overdraft.Domain.Models;

namespace Overdraft.Infrastructure.Mappings;

public class ContractAgreementMap : IEntityTypeConfiguration<ContractAgreement>
{
    public void Configure(EntityTypeBuilder<ContractAgreement> builder)
    {
        builder.ToTable("Contracts");
        builder.HasKey(x => x.Id);
        builder.HasIndex(x => x.AccountId);
        builder.HasIndex(x => x.ContractId);

        builder.Property(x => x.Id).HasColumnType("uniqueidentifier");
        builder.Property(x => x.AccountId).HasColumnType("uniqueidentifier");
        builder.Property(x => x.ContractId).HasColumnType("uniqueidentifier");
        builder.Property(x => x.SignatureDate).HasColumnType("datetimeoffset");
        builder.Property(x => x.CreatedAt).HasColumnType("datetimeoffset");
    }
}
