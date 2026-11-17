using Microsoft.AspNetCore.Mvc;
using Equitech.AiAgent.Interfaces;
using Equitech.Domain.Common;
using Equitech.Domain.Interfaces;
using Equitech.Domain.Models;

namespace Equitech.Api.Controllers;

[Route("api/v1/interview-packs")]
public class InterviewPacksController : BaseApiController
{
    private readonly IInterviewAiAgent _aiAgent;
    private readonly IInterviewPackRepository _repository;
    private readonly ILogger<InterviewPacksController> _logger;

    public InterviewPacksController(
        IInterviewAiAgent aiAgent, 
        IInterviewPackRepository repository,
        ILogger<InterviewPacksController> logger)
    {
        _aiAgent = aiAgent;
        _repository = repository;
        _logger = logger;
    }

    [HttpPost]
    [HttpPost("generate")]
    [ProducesResponseType(typeof(InterviewPackDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GeneratePack([FromBody] GeneratePackRequest request, CancellationToken ct)
    {
        if (request.JobDescription == null || string.IsNullOrWhiteSpace(request.JobDescription.Title))
        {
            return BadRequestProblem("Job description with a valid title is required.", ErrorCodes.InvalidPayload);
        }

        if (request.CandidateProfile == null || string.IsNullOrWhiteSpace(request.CandidateProfile.FullName))
        {
            return BadRequestProblem("Candidate profile with a valid full name is required.", ErrorCodes.InvalidPayload);
        }

        _logger.LogInformation("Generating interview pack for {Candidate} applying for {JobTitle}", 
            request.CandidateProfile.FullName, request.JobDescription.Title);

        var pack = await _aiAgent.GenerateInterviewPackAsync(request.JobDescription, request.CandidateProfile, ct);
        var savedPack = await _repository.SavePackAsync(pack, ct);

        return Ok(savedPack);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(InterviewPackDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetPackById(Guid id, CancellationToken ct)
    {
        var pack = await _repository.GetPackByIdAsync(id, ct);
        if (pack == null)
        {
            return NotFoundProblem($"Interview pack '{id}' was not found.", ErrorCodes.ResourceNotFound);
        }

        return Ok(pack);
    }

    [HttpGet("recent")]
    [ProducesResponseType(typeof(IEnumerable<InterviewPackDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetRecentPacks([FromQuery] int count = 10, CancellationToken ct = default)
    {
        var packs = await _repository.GetRecentPacksAsync(count, ct);
        return Ok(packs);
    }
}

public class GeneratePackRequest
{
    public JobDescriptionDto JobDescription { get; set; } = new();
    public CandidateProfileDto CandidateProfile { get; set; } = new();
}
