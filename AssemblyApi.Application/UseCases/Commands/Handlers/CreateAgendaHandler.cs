using AssemblyApi.Application.Constants;
using AssemblyApi.Application.DTOs;
using AssemblyApi.Application.Repositories;
using AssemblyApi.Application.Services;
using MediatR;

namespace AssemblyApi.Application.UseCases.Commands.Handlers;

public class CreateAgendaHandler : IRequestHandler<CreateAgenda, ApiResponse<List<Guid>>>
{
    private readonly IAssemblyRepository _assemblyRepository;
    private readonly IQuestionStatusRepository _questionStatusRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IUnitOfWork _unitOfWork;

    public CreateAgendaHandler(
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

    public async Task<ApiResponse<List<Guid>>> Handle(CreateAgenda request, CancellationToken cancellationToken)
    {
        try
        {
            var data = request.Data;

            if (data.Questions is null || data.Questions.Count == 0)
                return ApiResponse<List<Guid>>.FailureResponse("Debe registrar al menos una pregunta");

            var assembly = await _assemblyRepository.GetByIdAsync(data.AssemblyId, cancellationToken);

            if (assembly == null)
                return ApiResponse<List<Guid>>.FailureResponse("La asamblea no existe");

            var plannedStatus = await _questionStatusRepository.GetByIdAsync(QuestionStatusIds.Planned, cancellationToken);
            if (plannedStatus is null)
                return ApiResponse<List<Guid>>.FailureResponse("El estado 'PLND' no esta configurado");

            var createdByUserId = _currentUserService.GetUserId();
            var createdQuestions = new List<Guid>();

            foreach (var questionDto in data.Questions.OrderBy(q => q.OrderIndex))
            {
                var question = assembly.AddQuestion(
                    questionDto.Title,
                    createdByUserId,
                    plannedStatus.Id,
                    QuestionSourceIds.Agenda,
                    questionDto.OrderIndex);

                if (!string.IsNullOrWhiteSpace(questionDto.Description))
                {
                    question.UpdateDescription(questionDto.Description);
                }

                if (questionDto.StartDate.HasValue && questionDto.EndDate.HasValue)
                {
                    question.SetSchedule(questionDto.StartDate.Value, questionDto.EndDate.Value);
                }

                foreach (var optionDto in questionDto.Options)
                {
                    question.AddOption(optionDto.Text);
                }

                createdQuestions.Add(question.Id);
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return ApiResponse<List<Guid>>.SuccessResponse(createdQuestions, "Agenda creada correctamente");
        }
        catch (Exception ex)
        {
            return ApiResponse<List<Guid>>.FailureResponse(ex.Message);
        }
    }
}
