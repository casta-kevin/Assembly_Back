using AssemblyApi.Application.DTOs;
using MediatR;

namespace AssemblyApi.Application.UseCases.Commands;

public record AddQuestionToAssembly(AddQuestionToAssemblyDto Data) : IRequest<ApiResponse<Guid>>;
