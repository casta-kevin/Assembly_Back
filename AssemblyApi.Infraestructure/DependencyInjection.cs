using AssemblyApi.Application.Repositories;
using AssemblyApi.Application.Services;
using AssemblyApi.Infraestructure.Persistence;
using AssemblyApi.Infraestructure.Persistence.Repositories;
using AssemblyApi.Infraestructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AssemblyApi.Infraestructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IAssemblyRepository, AssemblyRepository>();
        services.AddScoped<IAssemblyStatusRepository, AssemblyStatusRepository>();
        services.AddScoped<IQuestionStatusRepository, QuestionStatusRepository>();
        services.AddScoped<IParticipantRepository, ParticipantRepository>();
        services.AddScoped<IVoteRepository, VoteRepository>();
        services.AddScoped<IQuestionRepository, QuestionRepository>();
        services.AddScoped<IConfirmedParticipantRepository, ConfirmedParticipantRepository>();
        services.AddScoped<IVoteTypeRepository, VoteTypeRepository>();
        services.AddScoped<IUserRepository, UserRepository>();

        services.AddScoped<IJwtTokenService, JwtTokenService>();
        services.AddScoped<IPasswordHashService, PasswordHashService>();
        services.AddScoped<ICurrentUserService, CurrentUserService>();

        return services;
    }
}
