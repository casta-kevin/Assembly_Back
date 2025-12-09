using AssemblyApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AssemblyApi.Infraestructure.Persistence.Configurations;

public class AssemblyQuestionResultConfiguration : IEntityTypeConfiguration<AssemblyQuestionResult>
{
    public void Configure(EntityTypeBuilder<AssemblyQuestionResult> builder)
    {
        builder.ToTable("assembly_question_results");

        builder.HasKey(r => r.Id);
        builder.Property(r => r.Id).HasColumnName("id");

        builder.Property(r => r.AssemblyId).HasColumnName("assembly_id").IsRequired();
        builder.Property(r => r.QuestionId).HasColumnName("question_id").IsRequired();
        builder.Property(r => r.OptionId).HasColumnName("option_id").IsRequired();
        builder.Property(r => r.VotesCount).HasColumnName("votes_count").IsRequired();
        builder.Property(r => r.IsWinningOption).HasColumnName("is_winning_option").IsRequired().HasDefaultValue(false);
        builder.Property(r => r.IsTie).HasColumnName("is_tie").IsRequired().HasDefaultValue(false);
        builder.Property(r => r.CalculatedAt).HasColumnName("calculated_at").IsRequired();

        builder.HasOne<Assembly>()
            .WithMany()
            .HasForeignKey(r => r.AssemblyId)
            .HasConstraintName("fk_results_assembly");

        builder.HasOne<AssemblyQuestion>()
            .WithMany()
            .HasForeignKey(r => r.QuestionId)
            .HasConstraintName("fk_results_question");

        builder.HasOne<AssemblyQuestionOption>()
            .WithMany()
            .HasForeignKey(r => r.OptionId)
            .HasConstraintName("fk_results_option");
    }
}
