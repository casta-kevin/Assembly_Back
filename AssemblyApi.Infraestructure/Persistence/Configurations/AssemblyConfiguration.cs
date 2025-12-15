using AssemblyApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AssemblyApi.Infraestructure.Persistence.Configurations;

public class AssemblyConfiguration : IEntityTypeConfiguration<Assembly>
{
    public void Configure(EntityTypeBuilder<Assembly> builder)
    {
        builder.ToTable("assemblies");

        builder.HasKey(a => a.Id);
        builder.Property(a => a.Id).HasColumnName("id");

        builder.Property(a => a.PropertyId).HasColumnName("property_id").IsRequired();
        builder.Property(a => a.AssemblyStatusId)
            .HasColumnName("assembly_status_id")
            .HasMaxLength(10)
            .IsRequired();
        builder.Property(a => a.Title).HasColumnName("title").HasMaxLength(200).IsRequired();
        builder.Property(a => a.Description).HasColumnName("description").HasMaxLength(2000);
        builder.Property(a => a.Rules).HasColumnName("rules").HasMaxLength(2000);
        builder.Property(a => a.StartDatePlanned).HasColumnName("start_date_planned");
        builder.Property(a => a.EndDatePlanned).HasColumnName("end_date_planned");
        builder.Property(a => a.StartDateActual).HasColumnName("start_date_actual");
        builder.Property(a => a.EndDateActual).HasColumnName("end_date_actual");
        builder.Property(a => a.CreatedByUserId).HasColumnName("created_by_user_id").IsRequired();
        builder.Property(a => a.CreatedAt).HasColumnName("created_at").IsRequired();

        builder.HasOne<Property>()
            .WithMany()
            .HasForeignKey(a => a.PropertyId)
            .HasConstraintName("fk_assemblies_property");

        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(a => a.CreatedByUserId)
            .HasConstraintName("fk_assemblies_created_by_user");

        builder.HasOne<AssemblyStatus>()
            .WithMany()
            .HasForeignKey(a => a.AssemblyStatusId)
            .HasConstraintName("fk_assemblies_status");
    }
}
