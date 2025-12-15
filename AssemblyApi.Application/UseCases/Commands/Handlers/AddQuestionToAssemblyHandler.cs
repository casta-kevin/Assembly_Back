using AssemblyApi.Application.Constants;
using AssemblyApi.Application.DTOs;
using AssemblyApi.Application.Repositories;
using AssemblyApi.Application.Services;
using MediatR;

namespace AssemblyApi.Application.UseCases.Commands.Handlers;

public class AddQuestionToAssemblyHandler : IRequestHandler<AddQuestionToAssembly, ApiResponse<Guid>>
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

    public async Task<ApiResponse<Guid>> Handle(AddQuestionToAssembly request, CancellationToken cancellationToken)
    {
        try
        {
            var data = request.Data;
            var createdByUserId = _currentUserService.GetUserId();

            var assembly = await _assemblyRepository.GetByIdAsync(data.AssemblyId, cancellationToken);

            if (assembly == null)
                return ApiResponse<Guid>.FailureResponse("La asamblea no existe");

            var plannedStatus = await _questionStatusRepository.GetByIdAsync(QuestionStatusIds.Planned, cancellationToken);
            if (plannedStatus is null)
                return ApiResponse<Guid>.FailureResponse("El estado 'PLND' no esta configurado");

            var sourceId = string.IsNullOrWhiteSpace(data.QuestionSourceId)
                ? QuestionSourceIds.Agenda
                : data.QuestionSourceId.Trim().ToUpperInvariant();

            var question = assembly.AddQuestion(data.Title, createdByUserId, plannedStatus.Id, sourceId, data.OrderIndex);

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
                question.AddOption(optionDto.Text);
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return ApiResponse<Guid>.SuccessResponse(question.Id, "Pregunta agregada correctamente");
        }
        catch (Exception ex)
        {
            return ApiResponse<Guid>.FailureResponse(ex.Message);
        }
    }
}
