using AssemblyApi.Application.DTOs;
using AssemblyApi.Application.UseCases.Commands;
using AssemblyApi.Application.UseCases.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AssemblyApi.Controllers;

[ApiController]
[Route("api/assemblies/{assemblyId}/[controller]")]
[Authorize]
public class QuestionsController : ControllerBase
{
    private readonly IMediator _mediator;

    public QuestionsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<QuestionDto>>>> GetByAssemblyId(Guid assemblyId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetQuestionsByAssemblyId(assemblyId), cancellationToken);

        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }

    [HttpGet("{questionId}")]
    public async Task<ActionResult<ApiResponse<QuestionDto?>>> GetById(Guid assemblyId, Guid questionId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetQuestionById(questionId), cancellationToken);

        if (!result.Success)
            return NotFound(result);

        if (result.Data?.AssemblyId != assemblyId)
            return BadRequest(ApiResponse<QuestionDto?>.FailureResponse("La pregunta no pertenece a esta asamblea"));

        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<Guid>>> AddQuestion(
        Guid assemblyId,
        [FromBody] AddQuestionToAssemblyDto dto,
        CancellationToken cancellationToken)
    {
        if (assemblyId != dto.AssemblyId)
            return BadRequest(ApiResponse<Guid>.FailureResponse("El ID de la asamblea no coincide"));

        var result = await _mediator.Send(new AddQuestionToAssembly(dto), cancellationToken);

        if (!result.Success)
            return BadRequest(result);

        return CreatedAtAction(nameof(GetById), new { assemblyId, questionId = result.Data }, result);
    }
}
