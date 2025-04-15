using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Overdraft.Domain.Models;

namespace Overdraft.Infrastructure.Mappings;

public class AccountMap : IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> builder)
    {
        builder.ToTable("Accounts");
        builder.HasKey(x => x.Id);
        builder.HasIndex(x => x.ContractId);
        builder.HasIndex(x => x.IsAccountActive);

        builder.Property(x => x.Id).HasColumnType("uniqueidentifier");
        builder.Property(x => x.ContractId).HasColumnType("uniqueidentifier");
        builder.Property(x => x.PrincipalAmount).HasColumnType("decimal(30,10)");
        builder.Property(x => x.ApprovedOverdraftLimit).HasColumnType("decimal(30,10)");
        builder.Property(x => x.SelfDeclaredLimit).HasColumnType("decimal(30,10)");
        builder.Property(x => x.UsedDaysInCurrentCycle).HasColumnType("int");
        builder.Property(x => x.UsedOverLimit).HasColumnType("decimal(30,10)");
        builder.Property(x => x.IsAccountActive).HasColumnType("bit");
        builder.Property(x => x.CreatedAt).HasColumnType("datetimeoffset");
    }
}
