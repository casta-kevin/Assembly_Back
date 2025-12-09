using AssemblyApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AssemblyApi.Infraestructure.Persistence.Configurations;

public class QuestionStatusConfiguration : IEntityTypeConfiguration<QuestionStatus>
{
    public void Configure(EntityTypeBuilder<QuestionStatus> builder)
    {
        builder.ToTable("question_statuses");

        builder.HasKey(s => s.Id);
        builder.Property(s => s.Id).HasColumnName("id");

        builder.Property(s => s.Code).HasColumnName("code").HasMaxLength(50).IsRequired();
        builder.HasIndex(s => s.Code).IsUnique();

        builder.Property(s => s.Name).HasColumnName("name").HasMaxLength(100).IsRequired();
        builder.Property(s => s.Description).HasColumnName("description").HasMaxLength(255);
    }
}
