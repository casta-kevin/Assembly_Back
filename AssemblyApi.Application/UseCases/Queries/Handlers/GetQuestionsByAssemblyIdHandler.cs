using AssemblyApi.Application.DTOs;
using AssemblyApi.Application.Repositories;
using MediatR;

namespace AssemblyApi.Application.UseCases.Queries.Handlers;

public class GetQuestionsByAssemblyIdHandler : IRequestHandler<GetQuestionsByAssemblyId, List<QuestionDto>>
{
    private readonly IQuestionRepository _questionRepository;

    public GetQuestionsByAssemblyIdHandler(IQuestionRepository questionRepository)
    {
        _questionRepository = questionRepository;
    }

    public async Task<List<QuestionDto>> Handle(GetQuestionsByAssemblyId request, CancellationToken cancellationToken)
    {
        var questions = await _questionRepository.GetByAssemblyIdAsync(request.AssemblyId, cancellationToken);
        
        return questions.Select(q => new QuestionDto
        {
            Id = q.Id,
            AssemblyId = q.AssemblyId,
            Title = q.Title,
            Description = q.Description,
            OrderIndex = q.OrderIndex,
            StartDate = q.StartDate,
            EndDate = q.EndDate,
            Options = q.Options.Select(o => new QuestionOptionDto
            {
                Id = o.Id,
                Text = o.Text,
                OrderIndex = o.OrderIndex
            }).ToList()
        }).ToList();
    }
}
