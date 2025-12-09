using AssemblyApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AssemblyApi.Infraestructure.Persistence.Configurations;

public class VoteTypeConfiguration : IEntityTypeConfiguration<VoteType>
{
    public void Configure(EntityTypeBuilder<VoteType> builder)
    {
        builder.ToTable("vote_types");

        builder.HasKey(v => v.Id);
        builder.Property(v => v.Id).HasColumnName("id");

        builder.Property(v => v.Code).HasColumnName("code").HasMaxLength(50).IsRequired();
        builder.HasIndex(v => v.Code).IsUnique();

        builder.Property(v => v.Name).HasColumnName("name").HasMaxLength(100).IsRequired();
        builder.Property(v => v.Description).HasColumnName("description").HasMaxLength(255);
    }
}
