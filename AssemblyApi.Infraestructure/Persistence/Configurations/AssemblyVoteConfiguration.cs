using AssemblyApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AssemblyApi.Infraestructure.Persistence.Configurations;

public class AssemblyVoteConfiguration : IEntityTypeConfiguration<AssemblyVote>
{
    public void Configure(EntityTypeBuilder<AssemblyVote> builder)
    {
        builder.ToTable("assembly_votes");

        builder.HasKey(v => v.Id);
        builder.Property(v => v.Id).HasColumnName("id");

        builder.Property(v => v.AssemblyId).HasColumnName("assembly_id").IsRequired();
        builder.Property(v => v.QuestionId).HasColumnName("question_id");
        builder.Property(v => v.OptionId).HasColumnName("option_id");
        builder.Property(v => v.ConfirmedParticipantId).HasColumnName("confirmed_participant_id").IsRequired();
        builder.Property(v => v.VoteTypeId).HasColumnName("vote_type_id").IsRequired();
        builder.Property(v => v.CreatedAt).HasColumnName("created_at").IsRequired();

        builder.HasOne<Assembly>()
            .WithMany()
            .HasForeignKey(v => v.AssemblyId)
            .HasConstraintName("fk_votes_assembly");

        builder.HasOne<AssemblyQuestion>()
            .WithMany()
            .HasForeignKey(v => v.QuestionId)
            .HasConstraintName("fk_votes_question");

        builder.HasOne<AssemblyQuestionOption>()
            .WithMany()
            .HasForeignKey(v => v.OptionId)
            .HasConstraintName("fk_votes_option");

        builder.HasOne<AssemblyConfirmedParticipant>()
            .WithMany()
            .HasForeignKey(v => v.ConfirmedParticipantId)
            .HasConstraintName("fk_votes_confirmed_participant");

        builder.HasOne<VoteType>()
            .WithMany()
            .HasForeignKey(v => v.VoteTypeId)
            .HasConstraintName("fk_votes_type");
    }
}
