namespace Equitech.Domain.Enums;

public enum CompetencyCategory
{
    Technical,
    DomainKnowledge,
    Behavioral,
    Methodology
}

public enum CompetencyImportance
{
    MustHave,
    Preferred,
    Bonus
}

public enum GapStatus
{
    ConfirmedFit,
    LikelyGap,
    Unknown
}

public enum RiskLevel
{
    Low,
    Medium,
    High
}

public enum QuestionCategory
{
    Technical,
    Behavioral,
    RoleSpecific,
    CaseStudy
}

public enum QuestionDifficulty
{
    Foundational,
    Intermediate,
    Advanced
}

public enum AnswerFrameworkType
{
    STAR,
    CAR,
    TechnicalRootCause,
    ExecutiveSummary
}
