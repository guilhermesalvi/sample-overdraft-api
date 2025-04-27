using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Mappings;

public class ContractAgreementMap : IEntityTypeConfiguration<ContractAgreement>
{
    public void Configure(EntityTypeBuilder<ContractAgreement> builder)
    {
        builder.ToTable("ContractAgreements");
        builder.HasKey(x => x.Id);
        builder.HasIndex(c => c.Id).IsUnique();
        builder.HasIndex(x => x.IsContractAgreementActive);
        builder.HasIndex(x => x.AccountId);

        builder.Property(x => x.Id).HasColumnType("uniqueidentifier");
        builder.Property(x => x.CreatedAt).HasColumnType("datetimeoffset");
        builder.Property(x => x.IsContractAgreementActive).HasColumnType("bit");

        builder.Property(x => x.AccountId).HasColumnType("uniqueidentifier");
        builder.Property(x => x.ContractId).HasColumnType("uniqueidentifier");
        builder.Property(x => x.SignatureDate).HasColumnType("datetimeoffset");
    }
}
