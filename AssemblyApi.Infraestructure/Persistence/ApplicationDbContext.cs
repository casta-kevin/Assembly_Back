using AssemblyApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AssemblyApi.Infraestructure.Persistence;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Property> Properties { get; set; }
    public DbSet<Assembly> Assemblies { get; set; }
    public DbSet<AssemblyParticipant> AssemblyParticipants { get; set; }
    public DbSet<AssemblyConfirmedParticipant> AssemblyConfirmedParticipants { get; set; }
    public DbSet<AssemblyQuestion> AssemblyQuestions { get; set; }
    public DbSet<AssemblyQuestionOption> AssemblyQuestionOptions { get; set; }
    public DbSet<AssemblyVote> AssemblyVotes { get; set; }
    public DbSet<AssemblyQuestionResult> AssemblyQuestionResults { get; set; }
    public DbSet<AssemblyStatus> AssemblyStatuses { get; set; }
    public DbSet<QuestionStatus> QuestionStatuses { get; set; }
    public DbSet<VoteType> VoteTypes { get; set; }
    public DbSet<ConfirmationMethod> ConfirmationMethods { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<QuestionSource> QuestionSources { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}
