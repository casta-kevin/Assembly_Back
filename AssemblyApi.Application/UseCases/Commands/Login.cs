using AssemblyApi.Application.DTOs;
using MediatR;

namespace AssemblyApi.Application.UseCases.Commands;

public record Login(LoginDto Data) : IRequest<ApiResponse<LoginResponseDto>>;
