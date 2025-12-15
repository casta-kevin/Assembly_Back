using AssemblyApi.Application.DTOs;
using AssemblyApi.Application.UseCases.Commands;
using AssemblyApi.Application.UseCases.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AssemblyApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AssembliesController : ControllerBase
{
    private readonly IMediator _mediator;

    public AssembliesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<AssemblyDto?>>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetAssemblyById(id), cancellationToken);

        if (!result.Success)
            return NotFound(result);

        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<Guid>>> Create([FromBody] CreateAssemblyDto dto, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new CreateAssembly(dto), cancellationToken);

        if (!result.Success)
            return BadRequest(result);

        return CreatedAtAction(nameof(GetById), new { id = result.Data }, result);
    }

    [HttpPost("{assemblyId}/participants")]
    public async Task<ActionResult<ApiResponse<Guid>>> AddParticipant(Guid assemblyId, [FromBody] AddParticipantToAssemblyDto dto, CancellationToken cancellationToken)
    {
        if (assemblyId != dto.AssemblyId)
            return BadRequest(ApiResponse<Guid>.FailureResponse("El ID de la asamblea no coincide"));

        var result = await _mediator.Send(new AddParticipantToAssembly(dto), cancellationToken);

        if (!result.Success)
            return BadRequest(result);

        return CreatedAtAction(nameof(AddParticipant), new { assemblyId, participantId = result.Data }, result);
    }
}
