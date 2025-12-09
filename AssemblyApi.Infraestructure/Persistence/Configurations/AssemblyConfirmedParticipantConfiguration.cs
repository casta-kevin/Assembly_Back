using AssemblyApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AssemblyApi.Infraestructure.Persistence.Configurations;

public class AssemblyConfirmedParticipantConfiguration : IEntityTypeConfiguration<AssemblyConfirmedParticipant>
{
    public void Configure(EntityTypeBuilder<AssemblyConfirmedParticipant> builder)
    {
        builder.ToTable("assembly_confirmed_participants");

        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id).HasColumnName("id");

        builder.Property(c => c.AssemblyId).HasColumnName("assembly_id").IsRequired();
        builder.Property(c => c.ParticipantId).HasColumnName("participant_id").IsRequired();
        builder.Property(c => c.ConfirmedAt).HasColumnName("confirmed_at").IsRequired();
        builder.Property(c => c.ConfirmedByUserId).HasColumnName("confirmed_by_user_id");
        builder.Property(c => c.ConfirmationMethodId).HasColumnName("confirmation_method_id").IsRequired();

        builder.HasIndex(c => new { c.AssemblyId, c.ParticipantId }).IsUnique().HasDatabaseName("uq_confirmed_assembly_participant");

        builder.HasOne<Assembly>()
            .WithMany()
            .HasForeignKey(c => c.AssemblyId)
            .HasConstraintName("fk_confirmed_assembly");

        builder.HasOne<AssemblyParticipant>()
            .WithMany()
            .HasForeignKey(c => c.ParticipantId)
            .HasConstraintName("fk_confirmed_participant");

        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(c => c.ConfirmedByUserId)
            .HasConstraintName("fk_confirmed_by_user");

        builder.HasOne<ConfirmationMethod>()
            .WithMany()
            .HasForeignKey(c => c.ConfirmationMethodId)
            .HasConstraintName("fk_confirmed_method");
    }
}
