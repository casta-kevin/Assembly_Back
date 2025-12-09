using AssemblyApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AssemblyApi.Infraestructure.Persistence.Configurations;

public class ConfirmationMethodConfiguration : IEntityTypeConfiguration<ConfirmationMethod>
{
    public void Configure(EntityTypeBuilder<ConfirmationMethod> builder)
    {
        builder.ToTable("confirmation_methods");

        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id).HasColumnName("id");

        builder.Property(c => c.Code).HasColumnName("code").HasMaxLength(50).IsRequired();
        builder.HasIndex(c => c.Code).IsUnique();

        builder.Property(c => c.Name).HasColumnName("name").HasMaxLength(100).IsRequired();
        builder.Property(c => c.Description).HasColumnName("description").HasMaxLength(255);
    }
}
