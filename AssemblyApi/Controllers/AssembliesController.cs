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
    public async Task<ActionResult<AssemblyDto>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var query = new GetAssemblyById(id);
        var assembly = await _mediator.Send(query, cancellationToken);

        if (assembly == null)
            return NotFound();

        return Ok(assembly);
    }

    [HttpPost]
    public async Task<ActionResult<Guid>> Create([FromBody] CreateAssemblyDto dto, CancellationToken cancellationToken)
    {
        var command = new CreateAssembly(dto);
        var assemblyId = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = assemblyId }, assemblyId);
    }

    [HttpPost("{assemblyId}/participants")]
    public async Task<ActionResult<Guid>> AddParticipant(Guid assemblyId, [FromBody] AddParticipantToAssemblyDto dto, CancellationToken cancellationToken)
    {
        if (assemblyId != dto.AssemblyId)
            return BadRequest("El ID de la asamblea no coincide");

        var command = new AddParticipantToAssembly(dto);
        var participantId = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(AddParticipant), new { assemblyId, participantId }, participantId);
    }
}
