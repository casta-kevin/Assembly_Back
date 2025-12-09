using AssemblyApi.Application.Repositories;
using AssemblyApi.Domain.Entities;
using AssemblyApi.Infraestructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AssemblyApi.Infraestructure.Persistence.Repositories;

public class VoteRepository : IVoteRepository
{
    private readonly ApplicationDbContext _context;

    public VoteRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(AssemblyVote vote, CancellationToken cancellationToken = default)
    {
        await _context.AssemblyVotes.AddAsync(vote, cancellationToken);
    }

    public async Task<bool> HasVotedAsync(Guid questionId, Guid userId, CancellationToken cancellationToken = default)
    {
        return await _context.AssemblyVotes
            .Join(_context.AssemblyConfirmedParticipants,
                vote => vote.ConfirmedParticipantId,
                confirmed => confirmed.Id,
                (vote, confirmed) => new { vote, confirmed })
            .Join(_context.AssemblyParticipants,
                combined => combined.confirmed.ParticipantId,
                participant => participant.Id,
                (combined, participant) => new { combined.vote, participant })
            .AnyAsync(x => x.vote.QuestionId == questionId && x.participant.UserId == userId, cancellationToken);
    }

    public async Task<int> GetVoteCountByOptionAsync(Guid questionId, Guid optionId, CancellationToken cancellationToken = default)
    {
        return await _context.AssemblyVotes
            .Where(v => v.QuestionId == questionId && v.OptionId == optionId)
            .CountAsync(cancellationToken);
    }
}
