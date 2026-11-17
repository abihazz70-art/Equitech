using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Equitech.Domain.Common;
using Equitech.Domain.Models;

namespace Equitech.Api.Controllers;

[Route("api/v1/learning-plans")]
public class LearningPlansController : BaseApiController
{
    private static readonly Dictionary<Guid, LearningPlanRecord> _plans = new();

    [HttpPost]
    [ProducesResponseType(typeof(LearningPlanResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public IActionResult GenerateLearningPlan([FromBody] GenerateLearningPlanRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.TargetRole))
        {
            return BadRequestProblem("Field 'target_role' is required.", ErrorCodes.InvalidPayload);
        }

        var planId = Guid.NewGuid();
        var milestones = new List<LearningMilestoneDto>();
        int order = 1;

        var gaps = request.SkillGaps?.Any() == true 
            ? request.SkillGaps 
            : new List<string> { "Advanced SQL", "Reactive Frameworks", "Containerization" };

        foreach (var gap in gaps)
        {
            milestones.Add(new LearningMilestoneDto
            {
                StepOrder = order++,
                SkillGap = gap,
                LearningObjective = $"Acquire hands-on competency in {gap} for {request.TargetRole}.",
                EstimatedHours = 10,
                Priority = "High",
                SuggestedResourceTypes = new List<string> { "Official Documentation", "Guided Labs" },
                RecommendedTopicLinks = new List<TopicResourceLinkDto>
                {
                    new() { Title = $"{gap} Documentation", Provider = "Official Docs", Url = "https://learn.microsoft.com" }
                },
                VerificationTask = $"Build a practical proof of concept demonstrating {gap}."
            });
        }

        var planRecord = new LearningPlanRecord
        {
            Id = planId,
            TargetRole = request.TargetRole,
            JobId = request.JobId,
            TotalHours = milestones.Sum(m => m.EstimatedHours),
            Status = "Active",
            Milestones = milestones,
            CreatedAt = DateTime.UtcNow
        };

        _plans[planId] = planRecord;

        return Created($"/api/v1/learning-plans/{planId}", new LearningPlanResponse
        {
            PlanId = planId,
            TargetRole = planRecord.TargetRole,
            TotalEstimatedHours = planRecord.TotalHours,
            Status = planRecord.Status,
            Milestones = planRecord.Milestones,
            CreatedAt = planRecord.CreatedAt
        });
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(LearningPlanResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public IActionResult GetPlan(Guid id)
    {
        if (!_plans.TryGetValue(id, out var planRecord))
        {
            return NotFoundProblem($"Learning plan '{id}' was not found.", ErrorCodes.ResourceNotFound);
        }

        return Ok(new LearningPlanResponse
        {
            PlanId = planRecord.Id,
            TargetRole = planRecord.TargetRole,
            TotalEstimatedHours = planRecord.TotalHours,
            Status = planRecord.Status,
            Milestones = planRecord.Milestones,
            CreatedAt = planRecord.CreatedAt
        });
    }
}

public class GenerateLearningPlanRequest
{
    [JsonPropertyName("target_role")]
    public string TargetRole { get; set; } = string.Empty;

    [JsonPropertyName("job_id")]
    public string? JobId { get; set; }

    [JsonPropertyName("skill_gaps")]
    public List<string>? SkillGaps { get; set; }
}

public class LearningPlanResponse
{
    [JsonPropertyName("plan_id")]
    public Guid PlanId { get; set; }

    [JsonPropertyName("target_role")]
    public string TargetRole { get; set; } = string.Empty;

    [JsonPropertyName("total_estimated_hours")]
    public double TotalEstimatedHours { get; set; }

    [JsonPropertyName("status")]
    public string Status { get; set; } = "Active";

    [JsonPropertyName("milestones")]
    public List<LearningMilestoneDto> Milestones { get; set; } = new();

    [JsonPropertyName("created_at")]
    public DateTime CreatedAt { get; set; }
}

public class LearningPlanRecord
{
    public Guid Id { get; set; }
    public string TargetRole { get; set; } = string.Empty;
    public string? JobId { get; set; }
    public double TotalHours { get; set; }
    public string Status { get; set; } = "Active";
    public List<LearningMilestoneDto> Milestones { get; set; } = new();
    public DateTime CreatedAt { get; set; }
}
