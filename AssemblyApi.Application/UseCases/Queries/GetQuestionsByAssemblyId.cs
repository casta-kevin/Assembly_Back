using AssemblyApi.Application.DTOs;
using MediatR;

namespace AssemblyApi.Application.UseCases.Queries;

public record GetQuestionsByAssemblyId(Guid AssemblyId) : IRequest<ApiResponse<List<QuestionDto>>>;
