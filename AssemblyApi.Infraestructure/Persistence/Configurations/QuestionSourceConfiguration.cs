using AssemblyApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AssemblyApi.Infraestructure.Persistence.Configurations;

public class QuestionSourceConfiguration : IEntityTypeConfiguration<QuestionSource>
{
    public void Configure(EntityTypeBuilder<QuestionSource> builder)
    {
        builder.ToTable("question_sources");

        builder.HasKey(q => q.Id);
        builder.Property(q => q.Id)
            .HasColumnName("id")
            .HasMaxLength(10)
            .IsRequired();

        builder.Property(q => q.Name)
            .HasColumnName("name")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(q => q.Description)
            .HasColumnName("description")
            .HasMaxLength(255);
    }
}
