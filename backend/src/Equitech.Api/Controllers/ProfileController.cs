using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Equitech.Domain.Common;
using Equitech.Domain.Interfaces;
using Equitech.Domain.Models;

namespace Equitech.Api.Controllers;

[Route("api/v1/profile")]
public class ProfileController : BaseApiController
{
    private static CandidateProfileDto? _activeProfile;
    private static CandidatePreferencesDto _activePreferences = new();

    public ProfileController(ISampleDataProvider sampleData)
    {
        if (_activeProfile == null)
        {
            _activeProfile = sampleData.GetSampleCandidates().FirstOrDefault() ?? new CandidateProfileDto
            {
                FullName = "Default Candidate",
                Headline = "Software Developer"
            };
        }
    }

    [HttpGet]
    [ProducesResponseType(typeof(UserProfileResponse), StatusCodes.Status200OK)]
    public IActionResult GetProfile()
    {
        return Ok(new UserProfileResponse
        {
            Profile = _activeProfile!,
            Preferences = _activePreferences,
            ProfileStatus = "Confirmed",
            ConfirmedAt = DateTime.UtcNow.AddDays(-2)
        });
    }

    [HttpPatch]
    [ProducesResponseType(typeof(UserProfileResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public IActionResult UpdateProfile([FromBody] UpdateProfileRequest request)
    {
        if (request == null)
        {
            return BadRequestProblem("Request body cannot be empty.", ErrorCodes.InvalidPayload);
        }

        if (request.FullName != null)
        {
            if (string.IsNullOrWhiteSpace(request.FullName))
            {
                return BadRequestProblem("Full name cannot be empty.", ErrorCodes.InvalidPayload);
            }
            _activeProfile!.FullName = request.FullName;
        }

        if (request.Headline != null) _activeProfile!.Headline = request.Headline;
        if (request.Location != null) _activeProfile!.Location = request.Location;
        if (request.Summary != null) _activeProfile!.Summary = request.Summary;

        if (request.Preferences != null)
        {
            if (request.Preferences.SalaryMin.HasValue && request.Preferences.SalaryMax.HasValue &&
                request.Preferences.SalaryMin > request.Preferences.SalaryMax)
            {
                return BadRequestProblem("SalaryMin cannot be greater than SalaryMax.", ErrorCodes.InvalidPayload);
            }
            _activePreferences = request.Preferences;
        }

        return Ok(new UserProfileResponse
        {
            Profile = _activeProfile!,
            Preferences = _activePreferences,
            ProfileStatus = "Confirmed",
            ConfirmedAt = DateTime.UtcNow
        });
    }
}

public class UserProfileResponse
{
    [JsonPropertyName("profile")]
    public CandidateProfileDto Profile { get; set; } = new();

    [JsonPropertyName("preferences")]
    public CandidatePreferencesDto Preferences { get; set; } = new();

    [JsonPropertyName("profile_status")]
    public string ProfileStatus { get; set; } = "Confirmed";

    [JsonPropertyName("confirmed_at")]
    public DateTime ConfirmedAt { get; set; }
}

public class UpdateProfileRequest
{
    [JsonPropertyName("full_name")]
    public string? FullName { get; set; }

    [JsonPropertyName("headline")]
    public string? Headline { get; set; }

    [JsonPropertyName("location")]
    public string? Location { get; set; }

    [JsonPropertyName("summary")]
    public string? Summary { get; set; }

    [JsonPropertyName("preferences")]
    public CandidatePreferencesDto? Preferences { get; set; }
}

public class CandidatePreferencesDto
{
    [JsonPropertyName("target_roles")]
    public List<string> TargetRoles { get; set; } = new() { "Software Developer", "Data Analyst" };

    [JsonPropertyName("preferred_locations")]
    public List<string> PreferredLocations { get; set; } = new() { "Mumbai", "Bengaluru", "Remote" };

    [JsonPropertyName("work_modes")]
    public List<string> WorkModes { get; set; } = new() { "Hybrid", "Remote" };

    [JsonPropertyName("salary_min")]
    public decimal? SalaryMin { get; set; } = 800000;

    [JsonPropertyName("salary_max")]
    public decimal? SalaryMax { get; set; } = 1500000;

    [JsonPropertyName("salary_currency")]
    public string SalaryCurrency { get; set; } = "INR";
}
