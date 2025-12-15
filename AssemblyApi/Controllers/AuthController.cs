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
    public async Task<ActionResult<ApiResponse<RegisterUserResponseDto>>> Register([FromBody] RegisterUserDto dto, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new RegisterUser(dto), cancellationToken);

        if (!result.Success)
            return BadRequest(result);

        return CreatedAtAction(nameof(Register), new { id = result.Data?.UserId }, result);
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<ActionResult<ApiResponse<LoginResponseDto>>> Login([FromBody] LoginDto dto, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new Login(dto), cancellationToken);

        if (!result.Success)
            return Unauthorized(result);

        return Ok(result);
    }
}
