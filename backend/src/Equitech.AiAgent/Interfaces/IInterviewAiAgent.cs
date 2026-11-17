using Equitech.Domain.Models;

namespace Equitech.AiAgent.Interfaces;

public interface IInterviewAiAgent
{
    Task<InterviewPackDto> GenerateInterviewPackAsync(
        JobDescriptionDto job, 
        CandidateProfileDto candidate, 
        CancellationToken ct = default);

    Task<ExerciseEvaluationResultDto> EvaluateExerciseAnswerAsync(
        PracticalExerciseDto exercise, 
        string candidateSubmission, 
        CancellationToken ct = default);
}

public class ExerciseEvaluationResultDto
{
    public int OverallScore { get; set; }
    public string SummaryFeedback { get; set; } = string.Empty;
    public List<RubricCriterionScoreDto> CriterionScores { get; set; } = new();
}

public class RubricCriterionScoreDto
{
    public string Criteria { get; set; } = string.Empty;
    public string LevelAchieved { get; set; } = "Competent"; // Unsatisfactory, Competent, Exemplary
    public int ScoreAwarded { get; set; }
    public string Feedback { get; set; } = string.Empty;
}
