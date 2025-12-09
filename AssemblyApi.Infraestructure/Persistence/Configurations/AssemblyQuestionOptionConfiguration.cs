using AssemblyApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AssemblyApi.Infraestructure.Persistence.Configurations;

public class AssemblyQuestionOptionConfiguration : IEntityTypeConfiguration<AssemblyQuestionOption>
{
    public void Configure(EntityTypeBuilder<AssemblyQuestionOption> builder)
    {
        builder.ToTable("assembly_question_options");

        builder.HasKey(o => o.Id);
        builder.Property(o => o.Id).HasColumnName("id");

        builder.Property(o => o.QuestionId).HasColumnName("question_id").IsRequired();
        builder.Property(o => o.Text).HasColumnName("text").HasMaxLength(200).IsRequired();
        builder.Property(o => o.Value).HasColumnName("value").HasMaxLength(50);
        builder.Property(o => o.OrderIndex).HasColumnName("order_index").IsRequired();
        builder.Property(o => o.IsActive).HasColumnName("is_active").IsRequired().HasDefaultValue(true);

        builder.HasOne<AssemblyQuestion>()
            .WithMany()
            .HasForeignKey(o => o.QuestionId)
            .HasConstraintName("fk_options_question");
    }
}
