using AssemblyApi.Application.DTOs;
using MediatR;

namespace AssemblyApi.Application.UseCases.Commands;

public record AddOptionToQuestion(AddOptionToQuestionDto Data) : IRequest<ApiResponse<Guid>>;
