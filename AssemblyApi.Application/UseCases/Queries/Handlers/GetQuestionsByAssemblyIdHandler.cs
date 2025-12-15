using AssemblyApi.Application.DTOs;
using AssemblyApi.Application.Repositories;
using MediatR;

namespace AssemblyApi.Application.UseCases.Queries.Handlers;

public class GetQuestionsByAssemblyIdHandler : IRequestHandler<GetQuestionsByAssemblyId, ApiResponse<List<QuestionDto>>>
{
    private readonly IQuestionRepository _questionRepository;

    public GetQuestionsByAssemblyIdHandler(IQuestionRepository questionRepository)
    {
        _questionRepository = questionRepository;
    }

    public async Task<ApiResponse<List<QuestionDto>>> Handle(GetQuestionsByAssemblyId request, CancellationToken cancellationToken)
    {
        try
        {
            var questions = await _questionRepository.GetByAssemblyIdAsync(request.AssemblyId, cancellationToken);

            var dto = questions.Select(q => new QuestionDto
            {
                Id = q.Id,
                AssemblyId = q.AssemblyId,
                Title = q.Title,
                Description = q.Description,
                QuestionSourceId = q.QuestionSourceId,
                QuestionStatusId = q.QuestionStatusId,
                OrderIndex = q.OrderIndex,
                StartDate = q.StartDate,
                EndDate = q.EndDate,
                Options = q.Options.Select(o => new QuestionOptionDto
                {
                    Id = o.Id,
                    Text = o.Text
                }).ToList()
            }).OrderBy(q => q.OrderIndex).ToList();

            return ApiResponse<List<QuestionDto>>.SuccessResponse(dto);
        }
        catch (Exception ex)
        {
            return ApiResponse<List<QuestionDto>>.FailureResponse("Error al obtener las preguntas", new[] { ex.Message });
        }
    }
}
