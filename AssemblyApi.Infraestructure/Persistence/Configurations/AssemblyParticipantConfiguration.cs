using AssemblyApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AssemblyApi.Infraestructure.Persistence.Configurations;

public class AssemblyParticipantConfiguration : IEntityTypeConfiguration<AssemblyParticipant>
{
    public void Configure(EntityTypeBuilder<AssemblyParticipant> builder)
    {
        builder.ToTable("assembly_participants");

        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id).HasColumnName("id");

        builder.Property(p => p.AssemblyId).HasColumnName("assembly_id").IsRequired();
        builder.Property(p => p.UserId).HasColumnName("user_id").IsRequired();
        builder.Property(p => p.IsVotingMember).HasColumnName("is_voting_member").IsRequired().HasDefaultValue(true);
        builder.Property(p => p.CanVoteToStartAssembly).HasColumnName("can_vote_to_start_assembly").IsRequired().HasDefaultValue(false);
        builder.Property(p => p.IsAdministrator).HasColumnName("is_administrator").IsRequired().HasDefaultValue(false);
        builder.Property(p => p.IsActive).HasColumnName("is_active").IsRequired().HasDefaultValue(true);
        builder.Property(p => p.CreatedAt).HasColumnName("created_at").IsRequired();

        builder.HasIndex(p => new { p.AssemblyId, p.UserId }).IsUnique().HasDatabaseName("uq_participants_assembly_user");

        builder.HasOne<Assembly>()
            .WithMany()
            .HasForeignKey(p => p.AssemblyId)
            .HasConstraintName("fk_participants_assembly");

        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(p => p.UserId)
            .HasConstraintName("fk_participants_user");
    }
}
