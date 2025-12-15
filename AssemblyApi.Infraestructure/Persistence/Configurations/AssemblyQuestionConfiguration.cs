using AssemblyApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AssemblyApi.Infraestructure.Persistence.Configurations;

public class AssemblyQuestionConfiguration : IEntityTypeConfiguration<AssemblyQuestion>
{
    public void Configure(EntityTypeBuilder<AssemblyQuestion> builder)
    {
        builder.ToTable("assembly_questions");

        builder.HasKey(q => q.Id);
        builder.Property(q => q.Id).HasColumnName("id");

        builder.Property(q => q.AssemblyId).HasColumnName("assembly_id").IsRequired();
        builder.Property(q => q.QuestionStatusId)
            .HasColumnName("question_status_id")
            .HasMaxLength(10)
            .IsRequired();
        builder.Property(q => q.QuestionSourceId)
            .HasColumnName("question_source_id")
            .HasMaxLength(10)
            .IsRequired();
        builder.Property(q => q.Title).HasColumnName("title").HasMaxLength(200).IsRequired();
        builder.Property(q => q.Description).HasColumnName("description").HasMaxLength(2000);
        builder.Property(q => q.StartDate).HasColumnName("start_date");
        builder.Property(q => q.EndDate).HasColumnName("end_date");
        builder.Property(q => q.OrderIndex).HasColumnName("order_index").IsRequired();
        builder.Property(q => q.CreatedAt).HasColumnName("created_at").IsRequired();
        builder.Property(q => q.CreatedByUserId).HasColumnName("created_by_user_id").IsRequired();

        builder.HasOne<Assembly>()
            .WithMany()
            .HasForeignKey(q => q.AssemblyId)
            .HasConstraintName("fk_questions_assembly");

        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(q => q.CreatedByUserId)
            .HasConstraintName("fk_questions_created_by_user");

        builder.HasOne<QuestionStatus>()
            .WithMany()
            .HasForeignKey(q => q.QuestionStatusId)
            .HasConstraintName("fk_questions_status");

        builder.HasOne<QuestionSource>()
            .WithMany()
            .HasForeignKey(q => q.QuestionSourceId)
            .HasConstraintName("fk_questions_source");
    }
}
