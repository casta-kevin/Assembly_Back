using AssemblyApi.Application.Repositories;
using AssemblyApi.Application.Services;
using MediatR;

namespace AssemblyApi.Application.UseCases.Commands.Handlers;

public class AddQuestionToAssemblyHandler : IRequestHandler<AddQuestionToAssembly, Guid>
{
    private readonly IAssemblyRepository _assemblyRepository;
    private readonly IQuestionStatusRepository _questionStatusRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IUnitOfWork _unitOfWork;

    public AddQuestionToAssemblyHandler(
        IAssemblyRepository assemblyRepository,
        IQuestionStatusRepository questionStatusRepository,
        ICurrentUserService currentUserService,
        IUnitOfWork unitOfWork)
    {
        _assemblyRepository = assemblyRepository;
        _questionStatusRepository = questionStatusRepository;
        _currentUserService = currentUserService;
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> Handle(AddQuestionToAssembly request, CancellationToken cancellationToken)
    {
        var data = request.Data;
        var createdByUserId = _currentUserService.GetUserId();

        var assembly = await _assemblyRepository.GetByIdAsync(data.AssemblyId, cancellationToken);
        
        if (assembly == null)
            throw new InvalidOperationException("La asamblea no existe");

        var plannedStatusId = await _questionStatusRepository.GetByCodeAsync("PLANNED", cancellationToken);

        assembly.AddQuestion(
            data.Title,
            data.OrderIndex,
            createdByUserId,
            plannedStatusId
        );

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var question = assembly.Questions.First(q => q.OrderIndex == data.OrderIndex);

        if (data.StartDate.HasValue && data.EndDate.HasValue)
        {
            question.SetSchedule(data.StartDate.Value, data.EndDate.Value);
        }

        if (!string.IsNullOrWhiteSpace(data.Description))
        {
            question.UpdateDescription(data.Description);
        }

        foreach (var optionDto in data.Options)
        {
            question.AddOption(optionDto.Text, optionDto.OrderIndex);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return question.Id;
    }
}
