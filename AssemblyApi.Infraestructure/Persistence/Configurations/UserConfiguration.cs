using AssemblyApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AssemblyApi.Infraestructure.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users");

        builder.HasKey(u => u.Id);
        builder.Property(u => u.Id).HasColumnName("id");

        builder.Property(u => u.Username).HasColumnName("username").HasMaxLength(50).IsRequired();
        builder.HasIndex(u => u.Username).IsUnique();

        builder.Property(u => u.Email).HasColumnName("email").HasMaxLength(255);
        builder.HasIndex(u => u.Email).IsUnique();

        builder.Property(u => u.PasswordHash).HasColumnName("password_hash").HasMaxLength(255).IsRequired();
        builder.Property(u => u.PropertyId).HasColumnName("property_id").IsRequired();
        builder.Property(u => u.IsActive).HasColumnName("is_active").IsRequired().HasDefaultValue(true);
        builder.Property(u => u.CreatedAt).HasColumnName("created_at").IsRequired();
        builder.Property(u => u.LastLoginAt).HasColumnName("last_login_at");

        builder.HasOne<Property>()
            .WithMany()
            .HasForeignKey(u => u.PropertyId)
            .HasConstraintName("fk_users_property");
    }
}
