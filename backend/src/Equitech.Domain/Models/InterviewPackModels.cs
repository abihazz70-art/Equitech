using System.Text.Json.Serialization;
using Equitech.Domain.Enums;

namespace Equitech.Domain.Models;

public class JobDescriptionDto
{
    [JsonPropertyName("source_id")]
    public string SourceId { get; set; } = string.Empty;

    [JsonPropertyName("source_job_id")]
    public string SourceJobId { get; set; } = string.Empty;

    [JsonPropertyName("title")]
    public string Title { get; set; } = string.Empty;

    [JsonPropertyName("employer")]
    public string Employer { get; set; } = string.Empty;

    [JsonPropertyName("canonical_url")]
    public string CanonicalUrl { get; set; } = string.Empty;

    [JsonPropertyName("location")]
    public string Location { get; set; } = string.Empty;

    [JsonPropertyName("country_code")]
    public string CountryCode { get; set; } = "IND";

    [JsonPropertyName("employment_type")]
    public string EmploymentType { get; set; } = "Full-time";

    [JsonPropertyName("salary_min")]
    public decimal? SalaryMin { get; set; }

    [JsonPropertyName("salary_max")]
    public decimal? SalaryMax { get; set; }

    [JsonPropertyName("salary_currency")]
    public string SalaryCurrency { get; set; } = "INR";

    [JsonPropertyName("posted_at")]
    public string PostedAt { get; set; } = DateTime.UtcNow.ToString("o");

    [JsonPropertyName("status")]
    public string Status { get; set; } = "Verified open";

    [JsonPropertyName("description")]
    public string Description { get; set; } = string.Empty;
}

public class CandidateProfileDto
{
    [JsonPropertyName("profile_id")]
    public string ProfileId { get; set; } = string.Empty;

    [JsonPropertyName("full_name")]
    public string FullName { get; set; } = string.Empty;

    [JsonPropertyName("headline")]
    public string Headline { get; set; } = string.Empty;

    [JsonPropertyName("location")]
    public string Location { get; set; } = string.Empty;

    [JsonPropertyName("summary")]
    public string Summary { get; set; } = string.Empty;

    [JsonPropertyName("employment_history")]
    public List<CandidateEmploymentDto> EmploymentHistory { get; set; } = new();

    [JsonPropertyName("education")]
    public List<CandidateEducationDto> Education { get; set; } = new();

    [JsonPropertyName("skills")]
    public List<CandidateSkillDto> Skills { get; set; } = new();

    [JsonPropertyName("certifications")]
    public List<string> Certifications { get; set; } = new();

    [JsonPropertyName("achievements")]
    public List<string> Achievements { get; set; } = new();
}

public class CandidateEmploymentDto
{
    [JsonPropertyName("company")]
    public string Company { get; set; } = string.Empty;

    [JsonPropertyName("role")]
    public string Role { get; set; } = string.Empty;

    [JsonPropertyName("start_date")]
    public string StartDate { get; set; } = string.Empty;

    [JsonPropertyName("end_date")]
    public string? EndDate { get; set; }

    [JsonPropertyName("is_current")]
    public bool IsCurrent { get; set; }

    [JsonPropertyName("responsibilities")]
    public List<string> Responsibilities { get; set; } = new();
}

public class CandidateEducationDto
{
    [JsonPropertyName("institution")]
    public string Institution { get; set; } = string.Empty;

    [JsonPropertyName("degree")]
    public string Degree { get; set; } = string.Empty;

    [JsonPropertyName("graduation_year")]
    public int GraduationYear { get; set; }
}

public class CandidateSkillDto
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("proficiency")]
    public string Proficiency { get; set; } = string.Empty;

    [JsonPropertyName("evidence")]
    public string Evidence { get; set; } = string.Empty;
}

public class InterviewPackDto
{
    [JsonPropertyName("pack_id")]
    public Guid PackId { get; set; } = Guid.NewGuid();

    [JsonPropertyName("job_id")]
    public string JobId { get; set; } = string.Empty;

    [JsonPropertyName("profile_id")]
    public string ProfileId { get; set; } = string.Empty;

    [JsonPropertyName("generated_at")]
    public string GeneratedAt { get; set; } = DateTime.UtcNow.ToString("o");

    [JsonPropertyName("source_basis")]
    public SourceBasisDto SourceBasis { get; set; } = new();

    [JsonPropertyName("disclaimer")]
    public string Disclaimer { get; set; } = "Notice: These questions and practical exercises are AI-synthesized predictions generated to assist in self-directed interview preparation based on public job requirements. They do not represent official, disclosed, or confidential questions from the employer.";

    [JsonPropertyName("role_competencies")]
    public List<RoleCompetencyDto> RoleCompetencies { get; set; } = new();

    [JsonPropertyName("candidate_gap_analysis")]
    public List<CandidateGapItemDto> CandidateGapAnalysis { get; set; } = new();

    [JsonPropertyName("questions")]
    public List<InterviewQuestionDto> Questions { get; set; } = new();

    [JsonPropertyName("practical_exercises")]
    public List<PracticalExerciseDto> PracticalExercises { get; set; } = new();

    [JsonPropertyName("learning_plan")]
    public LearningPlanDto LearningPlan { get; set; } = new();
}

public class SourceBasisDto
{
    [JsonPropertyName("job_title")]
    public string JobTitle { get; set; } = string.Empty;

    [JsonPropertyName("employer")]
    public string Employer { get; set; } = string.Empty;

    [JsonPropertyName("candidate_name")]
    public string CandidateName { get; set; } = string.Empty;

    [JsonPropertyName("profile_version")]
    public int ProfileVersion { get; set; } = 1;
}

public class RoleCompetencyDto
{
    [JsonPropertyName("competency_id")]
    public string CompetencyId { get; set; } = string.Empty;

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("category")]
    public string Category { get; set; } = "Technical";

    [JsonPropertyName("importance")]
    public string Importance { get; set; } = "Must-Have";

    [JsonPropertyName("description")]
    public string Description { get; set; } = string.Empty;
}

public class CandidateGapItemDto
{
    [JsonPropertyName("competency_id")]
    public string CompetencyId { get; set; } = string.Empty;

    [JsonPropertyName("competency_name")]
    public string CompetencyName { get; set; } = string.Empty;

    [JsonPropertyName("status")]
    public string Status { get; set; } = "Confirmed Fit"; // "Confirmed Fit", "Likely Gap", "Unknown / Insufficient Data"

    [JsonPropertyName("candidate_evidence")]
    public string? CandidateEvidence { get; set; }

    [JsonPropertyName("gap_summary")]
    public string GapSummary { get; set; } = string.Empty;

    [JsonPropertyName("risk_level")]
    public string RiskLevel { get; set; } = "Low"; // "Low", "Medium", "High"
}

public class InterviewQuestionDto
{
    [JsonPropertyName("question_id")]
    public string QuestionId { get; set; } = string.Empty;

    [JsonPropertyName("category")]
    public string Category { get; set; } = "Technical"; // "Technical", "Behavioral", "Role-Specific", "Case-Study"

    [JsonPropertyName("difficulty")]
    public string Difficulty { get; set; } = "Intermediate"; // "Foundational", "Intermediate", "Advanced"

    [JsonPropertyName("question_text")]
    public string QuestionText { get; set; } = string.Empty;

    [JsonPropertyName("rationale")]
    public string Rationale { get; set; } = string.Empty;

    [JsonPropertyName("target_competency_ids")]
    public List<string> TargetCompetencyIds { get; set; } = new();

    [JsonPropertyName("answer_framework")]
    public AnswerFrameworkDto AnswerFramework { get; set; } = new();
}

public class AnswerFrameworkDto
{
    [JsonPropertyName("model_structure")]
    public string ModelStructure { get; set; } = "STAR"; // "STAR", "CAR", "Technical Root-Cause", "Executive Summary"

    [JsonPropertyName("recommended_approach")]
    public string RecommendedApproach { get; set; } = string.Empty;

    [JsonPropertyName("candidate_talking_points")]
    public List<string> CandidateTalkingPoints { get; set; } = new();

    [JsonPropertyName("pitfalls_to_avoid")]
    public List<string> PitfallsToAvoid { get; set; } = new();
}

public class PracticalExerciseDto
{
    [JsonPropertyName("exercise_id")]
    public string ExerciseId { get; set; } = string.Empty;

    [JsonPropertyName("title")]
    public string Title { get; set; } = string.Empty;

    [JsonPropertyName("scenario")]
    public string Scenario { get; set; } = string.Empty;

    [JsonPropertyName("timebox_minutes")]
    public int TimeboxMinutes { get; set; } = 45;

    [JsonPropertyName("instructions")]
    public List<string> Instructions { get; set; } = new();

    [JsonPropertyName("expected_deliverable")]
    public string ExpectedDeliverable { get; set; } = string.Empty;

    [JsonPropertyName("scoring_rubric")]
    public List<ScoringRubricItemDto> ScoringRubric { get; set; } = new();

    [JsonPropertyName("model_solution_outline")]
    public string ModelSolutionOutline { get; set; } = string.Empty;
}

public class ScoringRubricItemDto
{
    [JsonPropertyName("criteria")]
    public string Criteria { get; set; } = string.Empty;

    [JsonPropertyName("weight_percent")]
    public int WeightPercent { get; set; } = 30;

    [JsonPropertyName("levels")]
    public RubricLevelsDto Levels { get; set; } = new();
}

public class RubricLevelsDto
{
    [JsonPropertyName("unsatisfactory")]
    public string Unsatisfactory { get; set; } = string.Empty;

    [JsonPropertyName("competent")]
    public string Competent { get; set; } = string.Empty;

    [JsonPropertyName("exemplary")]
    public string Exemplary { get; set; } = string.Empty;
}

public class LearningPlanDto
{
    [JsonPropertyName("target_role")]
    public string TargetRole { get; set; } = string.Empty;

    [JsonPropertyName("total_estimated_hours")]
    public double TotalEstimatedHours { get; set; }

    [JsonPropertyName("milestones")]
    public List<LearningMilestoneDto> Milestones { get; set; } = new();
}

public class LearningMilestoneDto
{
    [JsonPropertyName("step_order")]
    public int StepOrder { get; set; }

    [JsonPropertyName("skill_gap")]
    public string SkillGap { get; set; } = string.Empty;

    [JsonPropertyName("learning_objective")]
    public string LearningObjective { get; set; } = string.Empty;

    [JsonPropertyName("estimated_hours")]
    public double EstimatedHours { get; set; }

    [JsonPropertyName("priority")]
    public string Priority { get; set; } = "High"; // "High", "Medium", "Low"

    [JsonPropertyName("suggested_resource_types")]
    public List<string> SuggestedResourceTypes { get; set; } = new();

    [JsonPropertyName("recommended_topic_links")]
    public List<TopicResourceLinkDto> RecommendedTopicLinks { get; set; } = new();

    [JsonPropertyName("verification_task")]
    public string VerificationTask { get; set; } = string.Empty;
}

public class TopicResourceLinkDto
{
    [JsonPropertyName("title")]
    public string Title { get; set; } = string.Empty;

    [JsonPropertyName("provider")]
    public string Provider { get; set; } = string.Empty;

    [JsonPropertyName("url")]
    public string Url { get; set; } = string.Empty;
}
