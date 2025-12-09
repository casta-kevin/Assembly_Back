using AssemblyApi.Application.DTOs;
using AssemblyApi.Application.UseCases.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AssemblyApi.Controllers;

[ApiController]
[Route("api/questions/{questionId}/[controller]")]
[Authorize]
public class OptionsController : ControllerBase
{
    private readonly IMediator _mediator;

    public OptionsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<ActionResult<Guid>> AddOption(
        Guid questionId, 
        [FromBody] AddOptionToQuestionDto dto, 
        CancellationToken cancellationToken)
    {
        if (questionId != dto.QuestionId)
            return BadRequest("El ID de la pregunta no coincide");

        var command = new AddOptionToQuestion(dto);
        var optionId = await _mediator.Send(command, cancellationToken);
        
        return CreatedAtAction(
            nameof(AddOption), 
            new { questionId, optionId }, 
            optionId);
    }
}
