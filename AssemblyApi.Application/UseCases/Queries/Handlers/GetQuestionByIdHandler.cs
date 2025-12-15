using AssemblyApi.Application.DTOs;
using AssemblyApi.Application.Repositories;
using MediatR;

namespace AssemblyApi.Application.UseCases.Queries.Handlers;

public class GetQuestionByIdHandler : IRequestHandler<GetQuestionById, ApiResponse<QuestionDto?>>
{
    private readonly IQuestionRepository _questionRepository;

    public GetQuestionByIdHandler(IQuestionRepository questionRepository)
    {
        _questionRepository = questionRepository;
    }

    public async Task<ApiResponse<QuestionDto?>> Handle(GetQuestionById request, CancellationToken cancellationToken)
    {
        try
        {
            var question = await _questionRepository.GetByIdAsync(request.Id, cancellationToken);

            if (question == null)
                return ApiResponse<QuestionDto?>.FailureResponse("La pregunta no existe");

            var dto = new QuestionDto
            {
                Id = question.Id,
                AssemblyId = question.AssemblyId,
                Title = question.Title,
                Description = question.Description,
                QuestionSourceId = question.QuestionSourceId,
                QuestionStatusId = question.QuestionStatusId,
                OrderIndex = question.OrderIndex,
                StartDate = question.StartDate,
                EndDate = question.EndDate,
                Options = question.Options.Select(o => new QuestionOptionDto
                {
                    Id = o.Id,
                    Text = o.Text
                }).ToList()
            };

            return ApiResponse<QuestionDto?>.SuccessResponse(dto);
        }
        catch (Exception ex)
        {
            return ApiResponse<QuestionDto?>.FailureResponse("Error al obtener la pregunta", new[] { ex.Message });
        }
    }
}
