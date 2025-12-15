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
    public async Task<ActionResult<ApiResponse<Guid>>> AddOption(
        Guid questionId,
        [FromBody] AddOptionToQuestionDto dto,
        CancellationToken cancellationToken)
    {
        if (questionId != dto.QuestionId)
            return BadRequest(ApiResponse<Guid>.FailureResponse("El ID de la pregunta no coincide"));

        var result = await _mediator.Send(new AddOptionToQuestion(dto), cancellationToken);

        if (!result.Success)
            return BadRequest(result);

        return CreatedAtAction(nameof(AddOption), new { questionId, optionId = result.Data }, result);
    }
}
