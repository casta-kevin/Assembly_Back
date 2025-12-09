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
    public async Task<ActionResult<List<QuestionDto>>> GetByAssemblyId(Guid assemblyId, CancellationToken cancellationToken)
    {
        var query = new GetQuestionsByAssemblyId(assemblyId);
        var questions = await _mediator.Send(query, cancellationToken);
        return Ok(questions);
    }

    [HttpGet("{questionId}")]
    public async Task<ActionResult<QuestionDto>> GetById(Guid assemblyId, Guid questionId, CancellationToken cancellationToken)
    {
        var query = new GetQuestionById(questionId);
        var question = await _mediator.Send(query, cancellationToken);
        
        if (question == null)
            return NotFound();
        
        if (question.AssemblyId != assemblyId)
            return BadRequest("La pregunta no pertenece a esta asamblea");
        
        return Ok(question);
    }

    [HttpPost]
    public async Task<ActionResult<Guid>> AddQuestion(
        Guid assemblyId, 
        [FromBody] AddQuestionToAssemblyDto dto, 
        CancellationToken cancellationToken)
    {
        if (assemblyId != dto.AssemblyId)
            return BadRequest("El ID de la asamblea no coincide");

        var command = new AddQuestionToAssembly(dto);
        var questionId = await _mediator.Send(command, cancellationToken);
        
        return CreatedAtAction(
            nameof(GetById), 
            new { assemblyId, questionId }, 
            questionId);
    }
}
