using AssemblyApi.Application.DTOs;
using MediatR;

namespace AssemblyApi.Application.UseCases.Commands;

public record RegisterUser(RegisterUserDto Data) : IRequest<ApiResponse<RegisterUserResponseDto>>;
