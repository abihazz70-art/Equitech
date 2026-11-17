using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Equitech.Domain.Common;

namespace Equitech.Api.Controllers;

[Route("api/v1/admin")]
public class AdminController : BaseApiController
{
    [HttpGet("source-health")]
    [ProducesResponseType(typeof(SourceHealthReportResponse), StatusCodes.Status200OK)]
    public IActionResult GetSourceHealth()
    {
        var report = new SourceHealthReportResponse
        {
            GeneratedAt = DateTime.UtcNow,
            OverallSystemStatus = "Healthy",
            Connectors = new List<SourceConnectorStatusDto>
            {
                new()
                {
                    SourceId = "src_greenhouse_01",
                    Name = "Greenhouse Job Board API",
                    Type = "ATS API",
                    HealthStatus = "Operational",
                    LatencyMs = 120,
                    RecordsSyncedLast24h = 450,
                    ErrorRatePercent = 0.2,
                    LastSuccessAt = DateTime.UtcNow.AddMinutes(-5)
                },
                new()
                {
                    SourceId = "src_lever_02",
                    Name = "Lever Postings API",
                    Type = "ATS API",
                    HealthStatus = "Operational",
                    LatencyMs = 185,
                    RecordsSyncedLast24h = 320,
                    ErrorRatePercent = 0.5,
                    LastSuccessAt = DateTime.UtcNow.AddMinutes(-12)
                },
                new()
                {
                    SourceId = "src_adzuna_03",
                    Name = "Adzuna Aggregator API",
                    Type = "Aggregator",
                    HealthStatus = "Operational",
                    LatencyMs = 310,
                    RecordsSyncedLast24h = 1240,
                    ErrorRatePercent = 1.1,
                    LastSuccessAt = DateTime.UtcNow.AddMinutes(-2)
                }
            },
            OperationalMetrics = new OperationalMetricsDto
            {
                ActiveSearchesInFlight = 2,
                PendingCvParsingJobs = 0,
                AverageAiLatencySeconds = 2.4,
                MonthlyAiBudgetConsumptionPercent = 14.8
            }
        };

        return Ok(report);
    }
}

public class SourceHealthReportResponse
{
    [JsonPropertyName("generated_at")]
    public DateTime GeneratedAt { get; set; }

    [JsonPropertyName("overall_system_status")]
    public string OverallSystemStatus { get; set; } = "Healthy";

    [JsonPropertyName("connectors")]
    public List<SourceConnectorStatusDto> Connectors { get; set; } = new();

    [JsonPropertyName("operational_metrics")]
    public OperationalMetricsDto OperationalMetrics { get; set; } = new();
}

public class SourceConnectorStatusDto
{
    [JsonPropertyName("source_id")]
    public string SourceId { get; set; } = string.Empty;

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;

    [JsonPropertyName("health_status")]
    public string HealthStatus { get; set; } = "Operational";

    [JsonPropertyName("latency_ms")]
    public int LatencyMs { get; set; }

    [JsonPropertyName("records_synced_last_24h")]
    public int RecordsSyncedLast24h { get; set; }

    [JsonPropertyName("error_rate_percent")]
    public double ErrorRatePercent { get; set; }

    [JsonPropertyName("last_success_at")]
    public DateTime LastSuccessAt { get; set; }
}

public class OperationalMetricsDto
{
    [JsonPropertyName("active_searches_in_flight")]
    public int ActiveSearchesInFlight { get; set; }

    [JsonPropertyName("pending_cv_parsing_jobs")]
    public int PendingCvParsingJobs { get; set; }

    [JsonPropertyName("average_ai_latency_seconds")]
    public double AverageAiLatencySeconds { get; set; }

    [JsonPropertyName("monthly_ai_budget_consumption_percent")]
    public double MonthlyAiBudgetConsumptionPercent { get; set; }
}
