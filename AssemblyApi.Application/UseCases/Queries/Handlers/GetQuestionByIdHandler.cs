using AssemblyApi.Application.DTOs;
using AssemblyApi.Application.Repositories;
using MediatR;

namespace AssemblyApi.Application.UseCases.Queries.Handlers;

public class GetQuestionByIdHandler : IRequestHandler<GetQuestionById, QuestionDto?>
{
    private readonly IQuestionRepository _questionRepository;

    public GetQuestionByIdHandler(IQuestionRepository questionRepository)
    {
        _questionRepository = questionRepository;
    }

    public async Task<QuestionDto?> Handle(GetQuestionById request, CancellationToken cancellationToken)
    {
        var question = await _questionRepository.GetByIdAsync(request.Id, cancellationToken);
        
        if (question == null)
            return null;

        return new QuestionDto
        {
            Id = question.Id,
            AssemblyId = question.AssemblyId,
            Title = question.Title,
            Description = question.Description,
            OrderIndex = question.OrderIndex,
            StartDate = question.StartDate,
            EndDate = question.EndDate,
            Options = question.Options.Select(o => new QuestionOptionDto
            {
                Id = o.Id,
                Text = o.Text,
                OrderIndex = o.OrderIndex
            }).ToList()
        };
    }
}
