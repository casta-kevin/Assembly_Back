using AssemblyApi.Application.Repositories;
using AssemblyApi.Application.Services;
using AssemblyApi.Domain.Entities;
using MediatR;

namespace AssemblyApi.Application.UseCases.Commands.Handlers;

public class CreateAssemblyHandler : IRequestHandler<CreateAssembly, Guid>
{
    private readonly IAssemblyRepository _assemblyRepository;
    private readonly IAssemblyStatusRepository _statusRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IUnitOfWork _unitOfWork;

    public CreateAssemblyHandler(
        IAssemblyRepository assemblyRepository,
        IAssemblyStatusRepository statusRepository,
        ICurrentUserService currentUserService,
        IUnitOfWork unitOfWork)
    {
        _assemblyRepository = assemblyRepository;
        _statusRepository = statusRepository;
        _currentUserService = currentUserService;
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> Handle(CreateAssembly request, CancellationToken cancellationToken)
    {
        var data = request.Data;
        var propertyId = _currentUserService.GetPropertyId();
        var createdByUserId = _currentUserService.GetUserId();

        var scheduledStatusId = await _statusRepository.GetByCodeAsync("SCHEDULED", cancellationToken);

        var assembly = new Assembly(propertyId, scheduledStatusId, data.Title, createdByUserId);

        if (!string.IsNullOrWhiteSpace(data.Description) || !string.IsNullOrWhiteSpace(data.Rules))
        {
            assembly.UpdateDetails(data.Title, data.Description, data.Rules);
        }

        if (data.StartDatePlanned.HasValue && data.EndDatePlanned.HasValue)
        {
            assembly.ScheduleDates(data.StartDatePlanned.Value, data.EndDatePlanned.Value);
        }

        await _assemblyRepository.AddAsync(assembly, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return assembly.Id;
    }
}
