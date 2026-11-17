using Microsoft.AspNetCore.Mvc;
using Equitech.AiAgent.Interfaces;
using Equitech.Domain.Common;
using Equitech.Domain.Models;

namespace Equitech.Api.Controllers;

[Route("api/v1/practice-sessions")]
public class PracticeSessionsController : BaseApiController
{
    private readonly IInterviewAiAgent _aiAgent;

    public PracticeSessionsController(IInterviewAiAgent aiAgent)
    {
        _aiAgent = aiAgent;
    }

    [HttpPost]
    [HttpPost("evaluate")]
    [ProducesResponseType(typeof(ExerciseEvaluationResultDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Evaluate([FromBody] EvaluateAnswerRequest request, CancellationToken ct)
    {
        if (request == null || request.Exercise == null)
        {
            return BadRequestProblem("Exercise details with rubric are required.", ErrorCodes.InvalidPayload);
        }

        var evaluation = await _aiAgent.EvaluateExerciseAnswerAsync(request.Exercise, request.CandidateSubmission ?? string.Empty, ct);
        return Ok(evaluation);
    }
}

public class EvaluateAnswerRequest
{
    public PracticalExerciseDto Exercise { get; set; } = new();
    public string? CandidateSubmission { get; set; }
}
