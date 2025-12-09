using AssemblyApi.Application.Repositories;
using AssemblyApi.Domain.Exceptions;
using MediatR;

namespace AssemblyApi.Application.UseCases.Commands.Handlers;

public class AddOptionToQuestionHandler : IRequestHandler<AddOptionToQuestion, Guid>
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

    public async Task<Guid> Handle(AddOptionToQuestion request, CancellationToken cancellationToken)
    {
        var data = request.Data;

        var question = await _questionRepository.GetByIdAsync(data.QuestionId, cancellationToken);
        
        if (question == null)
            throw new DomainException("La pregunta no existe");

        question.AddOption(data.Text, data.OrderIndex);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var option = question.Options.First(o => o.OrderIndex == data.OrderIndex && o.Text == data.Text);

        return option.Id;
    }
}
