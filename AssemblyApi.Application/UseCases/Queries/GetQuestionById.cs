using AssemblyApi.Application.DTOs;
using MediatR;

namespace AssemblyApi.Application.UseCases.Queries;

public record GetQuestionById(Guid Id) : IRequest<ApiResponse<QuestionDto?>>;
