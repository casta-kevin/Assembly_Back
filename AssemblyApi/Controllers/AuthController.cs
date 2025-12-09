using AssemblyApi.Application.DTOs;
using AssemblyApi.Application.UseCases.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AssemblyApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<ActionResult<RegisterUserResponseDto>> Register([FromBody] RegisterUserDto dto, CancellationToken cancellationToken)
    {
        var command = new RegisterUser(dto);
        var response = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(Register), new { id = response.UserId }, response);
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<ActionResult<LoginResponseDto>> Login([FromBody] LoginDto dto, CancellationToken cancellationToken)
    {
        var command = new Login(dto);
        var response = await _mediator.Send(command, cancellationToken);
        return Ok(response);
    }
}
