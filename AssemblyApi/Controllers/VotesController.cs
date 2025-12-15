using AssemblyApi.Application.DTOs;
using AssemblyApi.Application.UseCases.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AssemblyApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class VotesController : ControllerBase
{
    private readonly IMediator _mediator;

    public VotesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<Guid>>> RegisterVote([FromBody] RegisterVoteDto dto, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new RegisterVote(dto), cancellationToken);

        if (!result.Success)
            return BadRequest(result);

        return CreatedAtAction(nameof(RegisterVote), new { id = result.Data }, result);
    }
}
