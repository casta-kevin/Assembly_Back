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
        builder.Property(v => v.Id)
            .HasColumnName("id")
            .HasMaxLength(10)
            .IsRequired();

        builder.Property(v => v.Name)
            .HasColumnName("name")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(v => v.Description)
            .HasColumnName("description")
            .HasMaxLength(255);
    }
}
