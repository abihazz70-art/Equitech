using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Equitech.Domain.Common;
using Equitech.Domain.Interfaces;
using Equitech.Domain.Models;

namespace Equitech.Api.Controllers;

[Route("api/v1")]
public class JobsController : BaseApiController
{
    private readonly ISampleDataProvider _dataProvider;
    private static readonly HashSet<string> _savedJobIds = new();

    public JobsController(ISampleDataProvider dataProvider)
    {
        _dataProvider = dataProvider;
    }

    [HttpPost("job-searches")]
    [ProducesResponseType(typeof(JobSearchWorkflowResponse), StatusCodes.Status202Accepted)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public IActionResult CreateJobSearch([FromBody] CreateJobSearchRequest request)
    {
        if (request == null || (request.Keywords == null || !request.Keywords.Any()))
        {
            return BadRequestProblem("Keywords or role family must be provided for job search.", ErrorCodes.InvalidPayload);
        }

        var searchId = Guid.NewGuid();
        return Accepted(new JobSearchWorkflowResponse
        {
            SearchId = searchId,
            Status = "Running",
            QueryTerms = request.Keywords,
            EstimatedCompletionSeconds = 3
        });
    }

    [HttpGet("jobs")]
    [ProducesResponseType(typeof(PaginatedCursorResult<JobSummaryResponse>), StatusCodes.Status200OK)]
    public IActionResult ListJobs(
        [FromQuery] string? cursor = null, 
        [FromQuery] int limit = 10,
        [FromQuery] string? status = null)
    {
        if (limit <= 0 || limit > 50) limit = 10;

        var allJobs = _dataProvider.GetSampleJobs();
        var items = allJobs.Select((j, index) => new JobSummaryResponse
        {
            Id = !string.IsNullOrEmpty(j.SourceJobId) ? j.SourceJobId : $"JOB-{index + 1}",
            Title = j.Title,
            Employer = j.Employer,
            Location = j.Location,
            EmploymentType = j.EmploymentType,
            SalaryMin = j.SalaryMin,
            SalaryMax = j.SalaryMax,
            SalaryCurrency = j.SalaryCurrency,
            Status = j.Status,
            FreshnessTimestamp = j.PostedAt,
            MatchScore = 88 - (index * 6),
            Confidence = "High",
            IsSaved = _savedJobIds.Contains(!string.IsNullOrEmpty(j.SourceJobId) ? j.SourceJobId : $"JOB-{index + 1}")
        }).ToList();

        // Cursor-based pagination logic
        int startIndex = 0;
        if (!string.IsNullOrEmpty(cursor) && int.TryParse(cursor, out var parsedIndex))
        {
            startIndex = Math.Max(0, parsedIndex);
        }

        var pagedItems = items.Skip(startIndex).Take(limit).ToList();
        var nextIndex = startIndex + pagedItems.Count;
        var hasMore = nextIndex < items.Count;

        return Ok(new PaginatedCursorResult<JobSummaryResponse>
        {
            Items = pagedItems,
            NextCursor = hasMore ? nextIndex.ToString() : null,
            HasMore = hasMore,
            Limit = limit
        });
    }

    [HttpGet("jobs/{id}")]
    [ProducesResponseType(typeof(JobDetailResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public IActionResult GetJobDetail(string id)
    {
        var job = _dataProvider.GetSampleJobs().FirstOrDefault(j => 
            j.SourceJobId.Equals(id, StringComparison.OrdinalIgnoreCase));

        if (job == null)
        {
            return NotFoundProblem($"Job with ID '{id}' was not found.", ErrorCodes.ResourceNotFound);
        }

        return Ok(new JobDetailResponse
        {
            Job = job,
            SourceEvidenceUrl = job.CanonicalUrl,
            LastCheckedAt = job.PostedAt,
            FreshnessLabel = "Verified open · checked recently",
            MatchRationale = new MatchRationaleDto
            {
                OverallScore = 88,
                MatchedSkills = new List<string> { "Relational SQL", "Analytical Reporting", "Communication" },
                MissingSkills = new List<string> { "Advanced dbt", "Cloud Warehouses" },
                Caveats = "Score is an algorithmic prioritization aid, not a hiring guarantee."
            }
        });
    }

    [HttpPost("jobs/{id}/save")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public IActionResult SaveOrDismissJob(string id, [FromBody] SaveJobRequest request)
    {
        var job = _dataProvider.GetSampleJobs().FirstOrDefault(j => 
            j.SourceJobId.Equals(id, StringComparison.OrdinalIgnoreCase));

        if (job == null)
        {
            return NotFoundProblem($"Job '{id}' not found.", ErrorCodes.ResourceNotFound);
        }

        if (request.Action.Equals("save", StringComparison.OrdinalIgnoreCase))
        {
            _savedJobIds.Add(id);
        }
        else
        {
            _savedJobIds.Remove(id);
        }

        return Ok(new { message = $"Job '{id}' {request.Action}d successfully.", isSaved = _savedJobIds.Contains(id) });
    }
}

public class CreateJobSearchRequest
{
    [JsonPropertyName("keywords")]
    public List<string> Keywords { get; set; } = new();

    [JsonPropertyName("location")]
    public string? Location { get; set; }
}

public class JobSearchWorkflowResponse
{
    [JsonPropertyName("search_id")]
    public Guid SearchId { get; set; }

    [JsonPropertyName("status")]
    public string Status { get; set; } = "Running";

    [JsonPropertyName("query_terms")]
    public List<string> QueryTerms { get; set; } = new();

    [JsonPropertyName("estimated_completion_seconds")]
    public int EstimatedCompletionSeconds { get; set; } = 3;
}

public class JobSummaryResponse
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("title")]
    public string Title { get; set; } = string.Empty;

    [JsonPropertyName("employer")]
    public string Employer { get; set; } = string.Empty;

    [JsonPropertyName("location")]
    public string Location { get; set; } = string.Empty;

    [JsonPropertyName("employment_type")]
    public string EmploymentType { get; set; } = "Full-time";

    [JsonPropertyName("salary_min")]
    public decimal? SalaryMin { get; set; }

    [JsonPropertyName("salary_max")]
    public decimal? SalaryMax { get; set; }

    [JsonPropertyName("salary_currency")]
    public string SalaryCurrency { get; set; } = "INR";

    [JsonPropertyName("status")]
    public string Status { get; set; } = "Verified open";

    [JsonPropertyName("freshness_timestamp")]
    public string FreshnessTimestamp { get; set; } = string.Empty;

    [JsonPropertyName("match_score")]
    public int MatchScore { get; set; }

    [JsonPropertyName("confidence")]
    public string Confidence { get; set; } = "High";

    [JsonPropertyName("is_saved")]
    public bool IsSaved { get; set; }
}

public class JobDetailResponse
{
    [JsonPropertyName("job")]
    public JobDescriptionDto Job { get; set; } = new();

    [JsonPropertyName("source_evidence_url")]
    public string SourceEvidenceUrl { get; set; } = string.Empty;

    [JsonPropertyName("last_checked_at")]
    public string LastCheckedAt { get; set; } = string.Empty;

    [JsonPropertyName("freshness_label")]
    public string FreshnessLabel { get; set; } = string.Empty;

    [JsonPropertyName("match_rationale")]
    public MatchRationaleDto MatchRationale { get; set; } = new();
}

public class MatchRationaleDto
{
    [JsonPropertyName("overall_score")]
    public int OverallScore { get; set; }

    [JsonPropertyName("matched_skills")]
    public List<string> MatchedSkills { get; set; } = new();

    [JsonPropertyName("missing_skills")]
    public List<string> MissingSkills { get; set; } = new();

    [JsonPropertyName("caveats")]
    public string Caveats { get; set; } = string.Empty;
}

public class SaveJobRequest
{
    [JsonPropertyName("action")]
    public string Action { get; set; } = "save"; // "save" or "dismiss"
}
