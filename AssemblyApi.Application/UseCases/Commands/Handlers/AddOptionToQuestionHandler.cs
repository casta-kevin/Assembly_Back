using AssemblyApi.Application.DTOs;
using AssemblyApi.Application.Repositories;
using MediatR;

namespace AssemblyApi.Application.UseCases.Commands.Handlers;

public class AddOptionToQuestionHandler : IRequestHandler<AddOptionToQuestion, ApiResponse<Guid>>
{
    private readonly IQuestionRepository _questionRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AddOptionToQuestionHandler(
        IQuestionRepository questionRepository,
        IUnitOfWork unitOfWork)
    {
        _questionRepository = questionRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ApiResponse<Guid>> Handle(AddOptionToQuestion request, CancellationToken cancellationToken)
    {
        try
        {
            var data = request.Data;

            var question = await _questionRepository.GetByIdAsync(data.QuestionId, cancellationToken);

            if (question == null)
                return ApiResponse<Guid>.FailureResponse("La pregunta no existe");

            var option = question.AddOption(data.Text);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return ApiResponse<Guid>.SuccessResponse(option.Id, "Opcion agregada correctamente");
        }
        catch (Exception ex)
        {
            return ApiResponse<Guid>.FailureResponse(ex.Message);
        }
    }
}
