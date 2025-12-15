using AssemblyApi.Application.DTOs;
using AssemblyApi.Application.UseCases.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AssemblyApi.Controllers;

[ApiController]
[Route("api/assemblies/{assemblyId}/[controller]")]
[Authorize]
public class AgendaController : ControllerBase
{
    private readonly IMediator _mediator;

    public AgendaController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<List<Guid>>>> Create(
        Guid assemblyId,
        [FromBody] CreateAgendaDto dto,
        CancellationToken cancellationToken)
    {
        if (assemblyId != dto.AssemblyId)
            return BadRequest(ApiResponse<List<Guid>>.FailureResponse("El ID de la asamblea no coincide"));

        var result = await _mediator.Send(new CreateAgenda(dto), cancellationToken);

        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }
}
