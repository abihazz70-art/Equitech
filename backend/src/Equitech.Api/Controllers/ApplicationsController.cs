using System.Security.Cryptography;
using System.Text;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Equitech.Domain.Common;

namespace Equitech.Api.Controllers;

[Route("api/v1")]
public class ApplicationsController : BaseApiController
{
    private static readonly Dictionary<Guid, ApplicationTrackerItem> _applications = new();
    private static readonly Dictionary<string, ApprovalRecord> _approvals = new();

    [HttpPost("applications")]
    [ProducesResponseType(typeof(ApplicationTrackerItem), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public IActionResult CreateApplication([FromBody] CreateApplicationRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.JobId))
        {
            return BadRequestProblem("Field 'job_id' is required.", ErrorCodes.InvalidPayload);
        }

        var app = new ApplicationTrackerItem
        {
            ApplicationId = Guid.NewGuid(),
            JobId = request.JobId,
            Employer = request.Employer ?? "Unknown Employer",
            RoleTitle = request.RoleTitle ?? "Applied Role",
            Status = "Saved",
            AppliedAt = null,
            NextAction = "Generate tailored CV and message in Application Studio",
            NextActionDueAt = DateTime.UtcNow.AddDays(2),
            CreatedAt = DateTime.UtcNow
        };

        _applications[app.ApplicationId] = app;
        return Created($"/api/v1/applications/{app.ApplicationId}", app);
    }

    [HttpGet("applications")]
    [ProducesResponseType(typeof(IReadOnlyList<ApplicationTrackerItem>), StatusCodes.Status200OK)]
    public IActionResult ListApplications()
    {
        return Ok(_applications.Values.OrderByDescending(a => a.CreatedAt).ToList());
    }

    [HttpPost("applications/{id:guid}/artifacts")]
    [ProducesResponseType(typeof(ApplicationArtifactResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public IActionResult GenerateArtifacts(Guid id, [FromBody] GenerateArtifactsRequest request)
    {
        if (!_applications.TryGetValue(id, out var app))
        {
            return NotFoundProblem($"Application with ID '{id}' was not found.", ErrorCodes.ResourceNotFound);
        }

        var artifactType = request.ArtifactType ?? "TailoredResume";
        var content = $"Tailored {artifactType} for {app.RoleTitle} at {app.Employer}. Evidence grounded strictly in candidate profile.";
        var hash = Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(content)));

        var artifact = new ApplicationArtifactResponse
        {
            ArtifactId = Guid.NewGuid(),
            ApplicationId = id,
            ArtifactType = artifactType,
            Content = content,
            ContentHash = hash,
            RequiresApproval = true,
            GeneratedAt = DateTime.UtcNow
        };

        app.Status = "DraftReady";
        app.NextAction = "Review and explicitly approve tailored materials";

        return Ok(artifact);
    }

    [HttpPost("approvals")]
    [ProducesResponseType(typeof(ApprovalResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public IActionResult CreateApproval([FromBody] CreateApprovalRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.ContentHash))
        {
            return BadRequestProblem("Field 'content_hash' is required.", ErrorCodes.InvalidPayload);
        }

        if (string.IsNullOrWhiteSpace(request.ActionType))
        {
            return BadRequestProblem("Field 'action_type' is required.", ErrorCodes.InvalidPayload);
        }

        var token = Guid.NewGuid().ToString("N");
        var record = new ApprovalRecord
        {
            ApprovalToken = token,
            ActionType = request.ActionType,
            Destination = request.Destination ?? "Employer Career Portal",
            ContentHash = request.ContentHash,
            ApprovedAt = DateTime.UtcNow,
            ExpiresAt = DateTime.UtcNow.AddHours(2),
            IsConsumed = false
        };

        _approvals[token] = record;

        return Created($"/api/v1/approvals/{token}", new ApprovalResponse
        {
            ApprovalToken = token,
            ApprovedAt = record.ApprovedAt,
            ExpiresAt = record.ExpiresAt,
            Status = "Valid"
        });
    }

    [HttpPost("applications/{id:guid}/submit")]
    [ProducesResponseType(typeof(SubmissionResultResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public IActionResult SubmitApplication(Guid id, [FromBody] SubmitApplicationRequest request)
    {
        if (!_applications.TryGetValue(id, out var app))
        {
            return NotFoundProblem($"Application '{id}' not found.", ErrorCodes.ResourceNotFound);
        }

        if (string.IsNullOrWhiteSpace(request.ApprovalToken))
        {
            return BadRequestProblem("An unconsumed approval token is required to submit an application (Section 10.4 Approval Security Model).", ErrorCodes.ApprovalRequired);
        }

        if (!_approvals.TryGetValue(request.ApprovalToken, out var approval))
        {
            return BadRequestProblem("Approval token is invalid or expired.", ErrorCodes.ApprovalExpired);
        }

        if (approval.IsConsumed)
        {
            return BadRequestProblem("Approval token has already been consumed.", ErrorCodes.ApprovalExpired);
        }

        approval.IsConsumed = true;
        app.Status = "Submitted";
        app.AppliedAt = DateTime.UtcNow;
        app.NextAction = "Follow up in 5 business days";
        app.NextActionDueAt = DateTime.UtcNow.AddDays(5);

        return Ok(new SubmissionResultResponse
        {
            ApplicationId = id,
            Status = "Submitted",
            SubmissionChannel = "OfficialCareerSiteHandoff",
            HandoffUrl = $"https://careers.{app.Employer.ToLower().Replace(" ", "")}.example.com/apply/{app.JobId}",
            SubmittedAt = DateTime.UtcNow,
            NextFollowUpProposal = "Drafted reminder scheduled for review"
        });
    }
}

public class CreateApplicationRequest
{
    [JsonPropertyName("job_id")]
    public string JobId { get; set; } = string.Empty;

    [JsonPropertyName("employer")]
    public string? Employer { get; set; }

    [JsonPropertyName("role_title")]
    public string? RoleTitle { get; set; }
}

public class ApplicationTrackerItem
{
    [JsonPropertyName("application_id")]
    public Guid ApplicationId { get; set; }

    [JsonPropertyName("job_id")]
    public string JobId { get; set; } = string.Empty;

    [JsonPropertyName("employer")]
    public string Employer { get; set; } = string.Empty;

    [JsonPropertyName("role_title")]
    public string RoleTitle { get; set; } = string.Empty;

    [JsonPropertyName("status")]
    public string Status { get; set; } = "Saved";

    [JsonPropertyName("applied_at")]
    public DateTime? AppliedAt { get; set; }

    [JsonPropertyName("next_action")]
    public string NextAction { get; set; } = string.Empty;

    [JsonPropertyName("next_action_due_at")]
    public DateTime NextActionDueAt { get; set; }

    [JsonPropertyName("created_at")]
    public DateTime CreatedAt { get; set; }
}

public class GenerateArtifactsRequest
{
    [JsonPropertyName("artifact_type")]
    public string? ArtifactType { get; set; } = "TailoredResume";
}

public class ApplicationArtifactResponse
{
    [JsonPropertyName("artifact_id")]
    public Guid ArtifactId { get; set; }

    [JsonPropertyName("application_id")]
    public Guid ApplicationId { get; set; }

    [JsonPropertyName("artifact_type")]
    public string ArtifactType { get; set; } = string.Empty;

    [JsonPropertyName("content")]
    public string Content { get; set; } = string.Empty;

    [JsonPropertyName("content_hash")]
    public string ContentHash { get; set; } = string.Empty;

    [JsonPropertyName("requires_approval")]
    public bool RequiresApproval { get; set; } = true;

    [JsonPropertyName("generated_at")]
    public DateTime GeneratedAt { get; set; }
}

public class CreateApprovalRequest
{
    [JsonPropertyName("action_type")]
    public string ActionType { get; set; } = "SubmitApplication";

    [JsonPropertyName("destination")]
    public string? Destination { get; set; }

    [JsonPropertyName("content_hash")]
    public string ContentHash { get; set; } = string.Empty;
}

public class ApprovalRecord
{
    public string ApprovalToken { get; set; } = string.Empty;
    public string ActionType { get; set; } = string.Empty;
    public string Destination { get; set; } = string.Empty;
    public string ContentHash { get; set; } = string.Empty;
    public DateTime ApprovedAt { get; set; }
    public DateTime ExpiresAt { get; set; }
    public bool IsConsumed { get; set; }
}

public class ApprovalResponse
{
    [JsonPropertyName("approval_token")]
    public string ApprovalToken { get; set; } = string.Empty;

    [JsonPropertyName("approved_at")]
    public DateTime ApprovedAt { get; set; }

    [JsonPropertyName("expires_at")]
    public DateTime ExpiresAt { get; set; }

    [JsonPropertyName("status")]
    public string Status { get; set; } = "Valid";
}

public class SubmitApplicationRequest
{
    [JsonPropertyName("approval_token")]
    public string ApprovalToken { get; set; } = string.Empty;
}

public class SubmissionResultResponse
{
    [JsonPropertyName("application_id")]
    public Guid ApplicationId { get; set; }

    [JsonPropertyName("status")]
    public string Status { get; set; } = "Submitted";

    [JsonPropertyName("submission_channel")]
    public string SubmissionChannel { get; set; } = string.Empty;

    [JsonPropertyName("handoff_url")]
    public string HandoffUrl { get; set; } = string.Empty;

    [JsonPropertyName("submitted_at")]
    public DateTime SubmittedAt { get; set; }

    [JsonPropertyName("next_follow_up_proposal")]
    public string NextFollowUpProposal { get; set; } = string.Empty;
}
