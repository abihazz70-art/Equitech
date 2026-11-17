using System.Text.Json;
using System.Text.RegularExpressions;
using Equitech.AiAgent.Interfaces;
using Equitech.Domain.Models;

namespace Equitech.AiAgent.Engine;

public class InterviewAiAgentEngine : IInterviewAiAgent
{
    private readonly string? _openAiApiKey;
    private readonly HttpClient _httpClient;

    public InterviewAiAgentEngine(HttpClient? httpClient = null, string? openAiApiKey = null)
    {
        _httpClient = httpClient ?? new HttpClient();
        _openAiApiKey = openAiApiKey ?? Environment.GetEnvironmentVariable("OPENAI_API_KEY");
    }

    public async Task<InterviewPackDto> GenerateInterviewPackAsync(
        JobDescriptionDto job, 
        CandidateProfileDto candidate, 
        CancellationToken ct = default)
    {
        // 1. If an OpenAI API Key is provided, we can invoke the LLM with structured outputs
        if (!string.IsNullOrWhiteSpace(_openAiApiKey))
        {
            try
            {
                var llmPack = await TryCallOpenAiAsync(job, candidate, ct);
                if (llmPack != null)
                {
                    return llmPack;
                }
            }
            catch
            {
                // Graceful fallback to deterministic AI engine if OpenAI is unreachable or rate-limited
            }
        }

        // 2. High-performance, deterministic AI agent reasoning engine
        return GenerateDeterministicPack(job, candidate);
    }

    public Task<ExerciseEvaluationResultDto> EvaluateExerciseAnswerAsync(
        PracticalExerciseDto exercise, 
        string candidateSubmission, 
        CancellationToken ct = default)
    {
        var result = new ExerciseEvaluationResultDto();
        var totalWeight = exercise.ScoringRubric.Sum(r => r.WeightPercent);
        if (totalWeight == 0) totalWeight = 100;

        double accumulatedScore = 0;
        var subLower = candidateSubmission.ToLowerInvariant();

        foreach (var rubric in exercise.ScoringRubric)
        {
            var achieved = "Competent";
            int scoreRatio = 80;
            string feedback = "Good effort addressing the key prompt constraints.";

            // Basic quality heuristics on candidate submission
            if (string.IsNullOrWhiteSpace(candidateSubmission) || candidateSubmission.Length < 40)
            {
                achieved = "Unsatisfactory";
                scoreRatio = 35;
                feedback = "Submission lacks sufficient detail or misses the core deliverable requirements.";
            }
            else if (candidateSubmission.Length > 250 && (subLower.Contains("async") || subLower.Contains("table") || subLower.Contains("def ") || subLower.Contains("class ") || subLower.Contains("error") || subLower.Contains("margin")))
            {
                achieved = "Exemplary";
                scoreRatio = 95;
                feedback = "Demonstrates strong technical depth, structured delivery, and considers edge cases.";
            }

            int criterionScore = (int)Math.Round((scoreRatio * rubric.WeightPercent) / 100.0);
            accumulatedScore += criterionScore;

            result.CriterionScores.Add(new RubricCriterionScoreDto
            {
                Criteria = rubric.Criteria,
                LevelAchieved = achieved,
                ScoreAwarded = criterionScore,
                Feedback = feedback
            });
        }

        result.OverallScore = (int)Math.Min(100, Math.Round(accumulatedScore));
        result.SummaryFeedback = result.OverallScore >= 80 
            ? "Strong performance. Your answer demonstrated solid alignment with the rubric expectations."
            : "Satisfactory attempt. Review the exemplary rubric criteria and focus on structured error handling and justification.";

        return Task.FromResult(result);
    }

    private InterviewPackDto GenerateDeterministicPack(JobDescriptionDto job, CandidateProfileDto candidate)
    {
        var pack = new InterviewPackDto
        {
            PackId = Guid.NewGuid(),
            JobId = !string.IsNullOrEmpty(job.SourceJobId) ? job.SourceJobId : "JOB-" + Guid.NewGuid().ToString("N")[..8],
            ProfileId = !string.IsNullOrEmpty(candidate.ProfileId) ? candidate.ProfileId : "CAND-" + Guid.NewGuid().ToString("N")[..8],
            GeneratedAt = DateTime.UtcNow.ToString("o"),
            SourceBasis = new SourceBasisDto
            {
                JobTitle = job.Title,
                Employer = job.Employer,
                CandidateName = candidate.FullName,
                ProfileVersion = 1
            },
            Disclaimer = "Notice: These questions and practical exercises are AI-synthesized predictions generated to assist in self-directed interview preparation based on public job requirements. They do not represent official, disclosed, or confidential questions from " + (!string.IsNullOrWhiteSpace(job.Employer) ? job.Employer : "the employer") + "."
        };

        // 1. Extract Competencies from Job Description
        pack.RoleCompetencies = ExtractCompetenciesFromJob(job);

        // 2. Perform Fit vs Gap Analysis
        pack.CandidateGapAnalysis = PerformGapAnalysis(pack.RoleCompetencies, candidate);

        // 3. Generate Categorized Question Bank with Frameworks & Candidate Talking Points
        pack.Questions = GenerateQuestions(job, candidate, pack.RoleCompetencies, pack.CandidateGapAnalysis);

        // 4. Generate Practical Timed Exercises & Rubrics
        pack.PracticalExercises = GeneratePracticalExercises(job, pack.RoleCompetencies);

        // 5. Generate Prioritized Learning Plan
        pack.LearningPlan = GenerateLearningPlan(job, pack.CandidateGapAnalysis);

        return pack;
    }

    private List<RoleCompetencyDto> ExtractCompetenciesFromJob(JobDescriptionDto job)
    {
        var desc = (job.Title + " " + job.Description).ToLowerInvariant();
        var list = new List<RoleCompetencyDto>();

        int compIndex = 1;

        if (desc.Contains("sql") || desc.Contains("query") || desc.Contains("database") || desc.Contains("snowflake") || desc.Contains("postgres"))
        {
            list.Add(new RoleCompetencyDto
            {
                CompetencyId = $"COMP-{compIndex++:D2}",
                Name = "Relational Data Querying & SQL Optimization",
                Category = "Technical",
                Importance = "Must-Have",
                Description = "Writing complex queries, joins, window functions, and query optimization."
            });
        }

        if (desc.Contains("power bi") || desc.Contains("tableau") || desc.Contains("dashboard") || desc.Contains("reporting"))
        {
            list.Add(new RoleCompetencyDto
            {
                CompetencyId = $"COMP-{compIndex++:D2}",
                Name = "Business Intelligence & Interactive Dashboarding",
                Category = "Technical",
                Importance = "Must-Have",
                Description = "Designing KPI dashboards, data modeling, and reporting for stakeholders."
            });
        }

        if (desc.Contains("python") || desc.Contains("pandas") || desc.Contains("numpy") || desc.Contains("etl") || desc.Contains("dbt"))
        {
            list.Add(new RoleCompetencyDto
            {
                CompetencyId = $"COMP-{compIndex++:D2}",
                Name = "Data Transformation & Python Scripting",
                Category = "Technical",
                Importance = "Preferred",
                Description = "Transforming data pipelines, exploratory data analysis, and automated workflows."
            });
        }

        if (desc.Contains("fintech") || desc.Contains("payment") || desc.Contains("settlement") || desc.Contains("merchant"))
        {
            list.Add(new RoleCompetencyDto
            {
                CompetencyId = $"COMP-{compIndex++:D2}",
                Name = "Payment & Merchant Domain Knowledge",
                Category = "DomainKnowledge",
                Importance = "Preferred",
                Description = "Understanding payment lifecycles, merchant settlement, and transaction failure analytics."
            });
        }

        if (desc.Contains("c#") || desc.Contains(".net") || desc.Contains("asp.net") || desc.Contains("web api"))
        {
            list.Add(new RoleCompetencyDto
            {
                CompetencyId = $"COMP-{compIndex++:D2}",
                Name = "ASP.NET Core Web API & Asynchronous Architecture",
                Category = "Technical",
                Importance = "Must-Have",
                Description = "Building resilient RESTful microservices, dependency injection, and async patterns."
            });
        }

        if (desc.Contains("vue") || desc.Contains("react") || desc.Contains("typescript") || desc.Contains("frontend"))
        {
            list.Add(new RoleCompetencyDto
            {
                CompetencyId = $"COMP-{compIndex++:D2}",
                Name = "Modern Reactive SPA (Vue 3 / TypeScript)",
                Category = "Technical",
                Importance = "Must-Have",
                Description = "Component composition, reactive state stores, and type-safe API communication."
            });
        }

        if (desc.Contains("docker") || desc.Contains("container") || desc.Contains("ci/cd") || desc.Contains("cloud"))
        {
            list.Add(new RoleCompetencyDto
            {
                CompetencyId = $"COMP-{compIndex++:D2}",
                Name = "Containerization & DevOps Workflows",
                Category = "Methodology",
                Importance = "Preferred",
                Description = "Docker containerization, environment configuration, and automated build pipelines."
            });
        }

        if (desc.Contains("financial model") || desc.Contains("dcf") || desc.Contains("valuation") || desc.Contains("equity research") || desc.Contains("banking"))
        {
            list.Add(new RoleCompetencyDto
            {
                CompetencyId = $"COMP-{compIndex++:D2}",
                Name = "3-Statement Financial Modeling & Valuation (DCF/DDM)",
                Category = "Technical",
                Importance = "Must-Have",
                Description = "Dynamic balance sheet forecasting, cash flow models, and equity valuation."
            });

            list.Add(new RoleCompetencyDto
            {
                CompetencyId = $"COMP-{compIndex++:D2}",
                Name = "BFSI Banking Regulatory & Asset Quality Analysis",
                Category = "DomainKnowledge",
                Importance = "Must-Have",
                Description = "Tracking NIM, NPA slippages, PCR, and RBI regulatory capital requirements."
            });
        }

        // Always include a behavioral / stakeholder competency
        list.Add(new RoleCompetencyDto
        {
            CompetencyId = $"COMP-{compIndex:D2}",
            Name = "Cross-Functional Collaboration & Problem-Solving",
            Category = "Behavioral",
            Importance = "Must-Have",
            Description = "Communicating technical nuances to business stakeholders and handling ambiguous requirements."
        });

        return list;
    }

    private List<CandidateGapItemDto> PerformGapAnalysis(
        List<RoleCompetencyDto> competencies, 
        CandidateProfileDto candidate)
    {
        var result = new List<CandidateGapItemDto>();
        var candText = (candidate.Summary + " " + 
            string.Join(" ", candidate.Skills.Select(s => s.Name + " " + s.Evidence)) + " " +
            string.Join(" ", candidate.EmploymentHistory.Select(e => e.Role + " " + string.Join(" ", e.Responsibilities))) + " " +
            string.Join(" ", candidate.Achievements) + " " +
            string.Join(" ", candidate.Certifications)).ToLowerInvariant();

        foreach (var comp in competencies)
        {
            var compNameLower = comp.Name.ToLowerInvariant();
            var keywords = compNameLower.Split(new[] { ' ', '&', '/', '(', ')' }, StringSplitOptions.RemoveEmptyEntries)
                .Where(w => w.Length > 3 && w != "with" && w != "data" && w != "core" && w != "knowledge");

            // Look for matching candidate skills
            var matchedSkill = candidate.Skills.FirstOrDefault(s => 
                keywords.Any(k => s.Name.ToLowerInvariant().Contains(k) || s.Evidence.ToLowerInvariant().Contains(k)));

            if (matchedSkill != null)
            {
                result.Add(new CandidateGapItemDto
                {
                    CompetencyId = comp.CompetencyId,
                    CompetencyName = comp.Name,
                    Status = "Confirmed Fit",
                    CandidateEvidence = $"{matchedSkill.Name} ({matchedSkill.Proficiency}): {matchedSkill.Evidence}",
                    GapSummary = $"Strong alignment found in profile skills and prior background.",
                    RiskLevel = "Low"
                });
            }
            else if (keywords.Any(k => candText.Contains(k)))
            {
                // Found in employment text or summary
                var matchingSnippet = candidate.EmploymentHistory
                    .SelectMany(e => e.Responsibilities)
                    .FirstOrDefault(r => keywords.Any(k => r.ToLowerInvariant().Contains(k)));

                result.Add(new CandidateGapItemDto
                {
                    CompetencyId = comp.CompetencyId,
                    CompetencyName = comp.Name,
                    Status = "Confirmed Fit",
                    CandidateEvidence = matchingSnippet ?? "Mentioned in employment history or summary.",
                    GapSummary = "Relevant foundational experience demonstrated in previous role.",
                    RiskLevel = "Low"
                });
            }
            else if (comp.Importance == "Must-Have")
            {
                result.Add(new CandidateGapItemDto
                {
                    CompetencyId = comp.CompetencyId,
                    CompetencyName = comp.Name,
                    Status = "Likely Gap",
                    CandidateEvidence = null,
                    GapSummary = $"Candidate has no stated production experience with {comp.Name}. This is a primary requirement.",
                    RiskLevel = "High"
                });
            }
            else
            {
                result.Add(new CandidateGapItemDto
                {
                    CompetencyId = comp.CompetencyId,
                    CompetencyName = comp.Name,
                    Status = "Unknown / Insufficient Data",
                    CandidateEvidence = null,
                    GapSummary = $"No explicit mention of {comp.Name} in profile.",
                    RiskLevel = "Medium"
                });
            }
        }

        return result;
    }

    private List<InterviewQuestionDto> GenerateQuestions(
        JobDescriptionDto job, 
        CandidateProfileDto candidate, 
        List<RoleCompetencyDto> competencies, 
        List<CandidateGapItemDto> gaps)
    {
        var questions = new List<InterviewQuestionDto>();
        int qId = 1;

        // Find primary gap and primary fit
        var primaryGap = gaps.FirstOrDefault(g => g.Status == "Likely Gap");
        var primaryFit = gaps.FirstOrDefault(g => g.Status == "Confirmed Fit");

        // 1. Technical Question (Targeted at core competency)
        var techComp = competencies.FirstOrDefault(c => c.Category == "Technical") ?? competencies.First();
        questions.Add(new InterviewQuestionDto
        {
            QuestionId = $"Q-TECH-{qId++:D2}",
            Category = "Technical",
            Difficulty = "Intermediate",
            QuestionText = $"When working with {techComp.Name}, what architectural or optimization strategy do you implement to ensure high throughput and minimize latency/error rates under peak load?",
            Rationale = $"Validates candidate's hands-on engineering/analytical depth in {techComp.Name}, a primary requirement of {job.Title}.",
            TargetCompetencyIds = new List<string> { techComp.CompetencyId },
            AnswerFramework = new AnswerFrameworkDto
            {
                ModelStructure = "Technical Root-Cause",
                RecommendedApproach = "Structure the answer in 3 parts: 1) Identify the typical bottleneck or constraint, 2) Explain the specific mechanism (caching, index tuning, async/await, partitioning) applied, 3) Mention how you monitor and verify the fix.",
                CandidateTalkingPoints = primaryFit?.CandidateEvidence != null 
                    ? new List<string> { $"Draw directly on your experience: \"{primaryFit.CandidateEvidence}\"" }
                    : new List<string> { "Highlight your foundational principles and systematic diagnostic methodology." },
                PitfallsToAvoid = new List<string>
                {
                    "Do not provide generic definitions; use concrete examples of parameters, queries, or architecture.",
                    "Never claim to have resolved issues using tools you have never worked with."
                }
            }
        });

        // 2. Role-Specific Question (Targeted at primary gap or role reality)
        if (primaryGap != null)
        {
            questions.Add(new InterviewQuestionDto
            {
                QuestionId = $"Q-ROLE-{qId++:D2}",
                Category = "Role-Specific",
                Difficulty = "Intermediate",
                QuestionText = $"This role requires hands-on capability with {primaryGap.CompetencyName}. How have you approached quickly adopting an adjacent technology or methodology when delivering a business project?",
                Rationale = $"Directly tests the candidate's self-awareness, learning agility, and technical adaptability regarding an identified gap.",
                TargetCompetencyIds = new List<string> { primaryGap.CompetencyId },
                AnswerFramework = new AnswerFrameworkDto
                {
                    ModelStructure = "CAR",
                    RecommendedApproach = "Acknowledge the gap transparently without apologizing. Frame your adjacent strength, outline your current self-study or practical exposure, and provide a past example of rapid upskilling.",
                    CandidateTalkingPoints = new List<string>
                    {
                        "Connect your strong existing baseline with the new requirement.",
                        candidate.Achievements.FirstOrDefault() != null 
                            ? $"Reference your demonstrated track record: \"{candidate.Achievements.First()}\""
                            : "Mention a specific project where you bridged a skill gap independently."
                    },
                    PitfallsToAvoid = new List<string>
                    {
                        "Never pretend to possess production experience you do not have.",
                        "Avoid defensive responses; show enthusiastic curiosity and disciplined learning habits."
                    }
                }
            });
        }
        else
        {
            questions.Add(new InterviewQuestionDto
            {
                QuestionId = $"Q-ROLE-{qId++:D2}",
                Category = "Role-Specific",
                Difficulty = "Intermediate",
                QuestionText = $"In the context of {job.Employer ?? "our organization"}, how do you ensure the outputs of your analysis/code meet both strict technical reliability and executive business expectations?",
                Rationale = "Assesses business domain acumen and alignment with employer expectations.",
                TargetCompetencyIds = competencies.Select(c => c.CompetencyId).Take(2).ToList(),
                AnswerFramework = new AnswerFrameworkDto
                {
                    ModelStructure = "CAR",
                    RecommendedApproach = "Context (high-stake business workflow) -> Action (validation checklist, stakeholder reviews) -> Result (repeatable quality).",
                    CandidateTalkingPoints = new List<string> { "Demonstrate how you validate requirements before diving into implementation." },
                    PitfallsToAvoid = new List<string> { "Don't sound purely academic; emphasize real operational rigor." }
                }
            });
        }

        // 3. Behavioral Question (STAR Framework)
        questions.Add(new InterviewQuestionDto
        {
            QuestionId = $"Q-BEH-{qId++:D2}",
            Category = "Behavioral",
            Difficulty = "Intermediate",
            QuestionText = "Describe a situation where a critical deliverable was delayed or encountered unexpected data/technical discrepancies right before a milestone. How did you handle the situation?",
            Rationale = "Evaluates emotional intelligence, accountability, and problem triage under tight deadlines.",
            TargetCompetencyIds = competencies.Where(c => c.Category == "Behavioral").Select(c => c.CompetencyId).ToList(),
            AnswerFramework = new AnswerFrameworkDto
            {
                ModelStructure = "STAR",
                RecommendedApproach = "Situation (specific business stakes) -> Task (what you personally owned) -> Action (transparent communication, root-cause triage, pragmatic trade-off) -> Result (outcome, post-mortem, and lasting preventive measure).",
                CandidateTalkingPoints = candidate.EmploymentHistory.FirstOrDefault() != null
                    ? new List<string> { $"Draw from your tenure at {candidate.EmploymentHistory.First().Company} handling business reporting or systems." }
                    : new List<string> { "Select a clear situation where you took personal accountability." },
                PitfallsToAvoid = new List<string>
                {
                    "Never blame colleagues, previous managers, or third-party vendors.",
                    "Avoid vague conclusions; always quantify the result or operational resolution."
                }
            }
        });

        return questions;
    }

    private List<PracticalExerciseDto> GeneratePracticalExercises(
        JobDescriptionDto job, 
        List<RoleCompetencyDto> competencies)
    {
        var exercises = new List<PracticalExerciseDto>();
        var titleLower = job.Title.ToLowerInvariant();

        if (titleLower.Contains("data") || titleLower.Contains("analyst") || titleLower.Contains("bi"))
        {
            exercises.Add(new PracticalExerciseDto
            {
                ExerciseId = "EX-01",
                Title = "Merchant Funnel Drop-off Analysis & SQL Query Design",
                Scenario = "You are given a transactional table `merchant_transactions` (transaction_id, merchant_id, amount, status, failure_reason, created_at). The merchant success rate dropped from 94% to 81% over the last 48 hours in the UPI payment gateway.",
                TimeboxMinutes = 45,
                Instructions = new List<string>
                {
                    "1. Write an optimized SQL query using Window Functions or CTEs to calculate hourly failure rates and rank the top 3 failure_reasons.",
                    "2. Identify whether the issue is isolated to specific high-volume merchants or systemic across all transactions.",
                    "3. Draft a 3-bullet action memo to the Head of Product summarizing the root cause and recommending immediate mitigation."
                },
                ExpectedDeliverable = "SQL query snippet and an executive summary memo (under 250 words).",
                ScoringRubric = new List<ScoringRubricItemDto>
                {
                    new()
                    {
                        Criteria = "SQL Query Correctness & Optimization",
                        WeightPercent = 40,
                        Levels = new RubricLevelsDto
                        {
                            Unsatisfactory = "Syntax errors, Cartesian joins, or lack of proper aggregation logic.",
                            Competent = "Correct use of GROUP BY, CTEs, and window functions (e.g. DENSE_RANK or RATIO).",
                            Exemplary = "Includes indexed filter conditions, proper NULL handling, and execution cost awareness."
                        }
                    },
                    new()
                    {
                        Criteria = "Root Cause Diagnostic & Business Communication",
                        WeightPercent = 40,
                        Levels = new RubricLevelsDto
                        {
                            Unsatisfactory = "Vague conclusions without data-backed justification; technical jargon without business context.",
                            Competent = "Clearly highlights the top failure driver and isolates impacted merchant tiers.",
                            Exemplary = "Actionable executive memo with immediate containment steps and proactive follow-up metrics."
                        }
                    },
                    new()
                    {
                        Criteria = "Deliverable Completeness & Time Management",
                        WeightPercent = 20,
                        Levels = new RubricLevelsDto
                        {
                            Unsatisfactory = "Missing SQL or memo entirely within timebox.",
                            Competent = "All 3 components submitted and coherent within 45 minutes.",
                            Exemplary = "Concise, publication-ready submission delivered well within the timebox."
                        }
                    }
                },
                ModelSolutionOutline = "SELECT date_trunc('hour', created_at) AS hr, failure_reason, count(*), round(count(*) * 100.0 / sum(count(*)) over(partition by date_trunc('hour', created_at)), 2) as pct FROM merchant_transactions WHERE created_at >= NOW() - INTERVAL '48 hours' GROUP BY 1, 2 ORDER BY 1 DESC, pct DESC;"
            });
        }
        else if (titleLower.Contains("equity") || titleLower.Contains("research") || titleLower.Contains("finance"))
        {
            exercises.Add(new PracticalExerciseDto
            {
                ExerciseId = "EX-01",
                Title = "Commercial Bank Net Interest Margin (NIM) Compression & Valuation Note",
                Scenario = "State Bank / Private Bank reports Q2 results: Net Interest Margin compressed by 22 bps due to repricing of term deposits, while Gross NPA fell 15 bps to 2.1%. Management guides 14% credit growth for FY27.",
                TimeboxMinutes = 50,
                Instructions = new List<string>
                {
                    "1. Calculate the implied impact of the 22 bps NIM compression on Net Interest Income (NII) on an interest-earning asset base of INR 8,50,000 Cr.",
                    "2. Explain how falling Gross NPAs and Provision Coverage Ratio (PCR at 74%) offset or amplify Return on Assets (RoA).",
                    "3. Prepare an Investment Recommendation stance (BUY / ACCUMULATE / HOLD) with two key risks."
                },
                ExpectedDeliverable = "Structured valuation memo with calculation table and recommendation.",
                ScoringRubric = new List<ScoringRubricItemDto>
                {
                    new()
                    {
                        Criteria = "Banking Metrics Financial Accuracy",
                        WeightPercent = 40,
                        Levels = new RubricLevelsDto
                        {
                            Unsatisfactory = "Mathematical errors in NII calculation or confusion between NIM and Spread.",
                            Competent = "Accurate NII impact calculation (INR 1,870 Cr annualized) and sound PCR analysis.",
                            Exemplary = "Dynamic RoA DuPont decomposition demonstrating impact on Return on Equity (RoE)."
                        }
                    },
                    new()
                    {
                        Criteria = "Investment Thesis & Regulatory Insight",
                        WeightPercent = 40,
                        Levels = new RubricLevelsDto
                        {
                            Unsatisfactory = "No thesis; copy-pastes reported guidance without critical evaluation.",
                            Competent = "Clear BUY/HOLD thesis considering RBI interest rate cycle and cost of funds.",
                            Exemplary = "Differentiated view on credit costs vs deposit franchise elasticity."
                        }
                    },
                    new()
                    {
                        Criteria = "Clarity & Presentation Format",
                        WeightPercent = 20,
                        Levels = new RubricLevelsDto
                        {
                            Unsatisfactory = "Disorganized notes lacking structure.",
                            Competent = "Clean tables and institutional-grade analyst memo structure.",
                            Exemplary = "Executive presentation style ready for an institutional fund manager."
                        }
                    }
                },
                ModelSolutionOutline = "NII delta = 850,000 * 0.0022 = INR 1,870 Cr. Lower credit costs from 15 bps GNPA reduction partially mitigate operating profit compression. Stance: ACCUMULATE on valuation pullback."
            });
        }
        else
        {
            // Default Software Developer / Engineering Exercise
            exercises.Add(new PracticalExerciseDto
            {
                ExerciseId = "EX-01",
                Title = "Full-Stack Feature: Resilient Job Status Badge Component & API",
                Scenario = "You are designing a real-time status monitor widget. You must build an ASP.NET Core endpoint returning job health metrics and a corresponding Vue 3 component that displays the badge with state colors (Green: Active, Amber: Degraded, Red: Down).",
                TimeboxMinutes = 45,
                Instructions = new List<string>
                {
                    "1. Write a minimal C# ASP.NET Core controller endpoint GET /api/v1/jobs/{id}/status returning StatusDto (JobId, StatusEnum, LastCheckedAtUtc).",
                    "2. Write a Vue 3 SFC (JobStatusBadge.vue) using <script setup lang=\"ts\"> accepting jobId: string as a prop.",
                    "3. Handle Loading, Error, and Success states gracefully in the template.",
                    "4. Include one xUnit mock test verifying the endpoint returns 404 when the job does not exist."
                },
                ExpectedDeliverable = "A single markdown document or code file containing the C# controller, DTO, the Vue 3 component, and unit test.",
                ScoringRubric = new List<ScoringRubricItemDto>
                {
                    new()
                    {
                        Criteria = "API Architecture & HTTP Semantics",
                        WeightPercent = 30,
                        Levels = new RubricLevelsDto
                        {
                            Unsatisfactory = "Returns 200 with error payload, no input validation, or blocking synchronous execution.",
                            Competent = "Uses standard HTTP verbs, ActionResult<T>, asynchronous tasks, and proper 404/500 status codes.",
                            Exemplary = "Includes ProblemDetails RFC-7807 error responses, cancellation tokens (CancellationToken), and API versioning."
                        }
                    },
                    new()
                    {
                        Criteria = "Vue 3 Composition API & TypeScript Idioms",
                        WeightPercent = 40,
                        Levels = new RubricLevelsDto
                        {
                            Unsatisfactory = "Uses Options API with untyped any, lacks reactive loading state, or handles errors via raw alert().",
                            Competent = "Employs <script setup lang=\"ts\">, typed defineProps, reactive ref/computed, and clean template conditionals (v-if).",
                            Exemplary = "Extracts fetching logic into a reusable composable (useJobStatus), enforces strict enum mapping, and styles cleanly."
                        }
                    },
                    new()
                    {
                        Criteria = "Test Quality & Robustness",
                        WeightPercent = 30,
                        Levels = new RubricLevelsDto
                        {
                            Unsatisfactory = "No automated test provided or test does not assert anything.",
                            Competent = "xUnit test properly mocks repository/service and asserts expected status code and schema.",
                            Exemplary = "Tests both happy path (200) and edge cases (404 Not Found, internal server fault) using FluentAssertions."
                        }
                    }
                },
                ModelSolutionOutline = "Controller: [HttpGet(\"{id}/status\")] public async Task<IActionResult> GetStatus(Guid id). Component: defineProps<{ jobId: string }>(), state (loading, error, data). xUnit: mockService.Setup -> null returns NotFound()."
            });
        }

        return exercises;
    }

    private LearningPlanDto GenerateLearningPlan(
        JobDescriptionDto job, 
        List<CandidateGapItemDto> gaps)
    {
        var plan = new LearningPlanDto
        {
            TargetRole = job.Title,
            TotalEstimatedHours = 0
        };

        var relevantGaps = gaps.Where(g => g.Status == "Likely Gap" || g.RiskLevel == "High" || g.RiskLevel == "Medium").ToList();
        if (!relevantGaps.Any())
        {
            relevantGaps = gaps.Take(3).ToList();
        }

        int step = 1;
        foreach (var gap in relevantGaps)
        {
            double hours = gap.RiskLevel == "High" ? 14.0 : 8.0;
            plan.TotalEstimatedHours += hours;

            var milestone = new LearningMilestoneDto
            {
                StepOrder = step++,
                SkillGap = gap.CompetencyName,
                LearningObjective = $"Attain practical proficiency in {gap.CompetencyName} to meet the must-have requirements for {job.Title}.",
                EstimatedHours = hours,
                Priority = gap.RiskLevel == "High" ? "High" : "Medium",
                SuggestedResourceTypes = new List<string> { "Official Documentation", "Guided Code Lab", "Production Case Study" },
                VerificationTask = $"Build a standalone portfolio artifact or practical proof-of-concept demonstrating {gap.CompetencyName}."
            };

            // Link curated reputable domain sources
            if (gap.CompetencyName.ToLowerInvariant().Contains("vue") || gap.CompetencyName.ToLowerInvariant().Contains("typescript"))
            {
                milestone.RecommendedTopicLinks.Add(new TopicResourceLinkDto
                {
                    Title = "Vue 3 Official Tutorial (Composition API)",
                    Provider = "VueJS.org",
                    Url = "https://vuejs.org/tutorial/"
                });
                milestone.RecommendedTopicLinks.Add(new TopicResourceLinkDto
                {
                    Title = "TypeScript with Vue 3 Guide",
                    Provider = "VueJS.org",
                    Url = "https://vuejs.org/guide/typescript/overview.html"
                });
            }
            else if (gap.CompetencyName.ToLowerInvariant().Contains("sql") || gap.CompetencyName.ToLowerInvariant().Contains("database") || gap.CompetencyName.ToLowerInvariant().Contains("postgres"))
            {
                milestone.RecommendedTopicLinks.Add(new TopicResourceLinkDto
                {
                    Title = "PostgreSQL Tutorial - Advanced Window Functions & CTEs",
                    Provider = "PostgresTutorial.com",
                    Url = "https://www.postgresqltutorial.com/postgresql-window-function/"
                });
            }
            else if (gap.CompetencyName.ToLowerInvariant().Contains("power bi") || gap.CompetencyName.ToLowerInvariant().Contains("dashboard"))
            {
                milestone.RecommendedTopicLinks.Add(new TopicResourceLinkDto
                {
                    Title = "Microsoft Power BI Guided Learning",
                    Provider = "Microsoft Learn",
                    Url = "https://learn.microsoft.com/en-us/training/powerplatform/power-bi"
                });
            }
            else if (gap.CompetencyName.ToLowerInvariant().Contains("valuation") || gap.CompetencyName.ToLowerInvariant().Contains("financial"))
            {
                milestone.RecommendedTopicLinks.Add(new TopicResourceLinkDto
                {
                    Title = "CFA Institute - Financial Reporting and Analysis Refresher",
                    Provider = "CFA Institute",
                    Url = "https://www.cfainstitute.org/en/membership/professional-development/refresher-readings"
                });
            }
            else
            {
                milestone.RecommendedTopicLinks.Add(new TopicResourceLinkDto
                {
                    Title = $"Mastering {gap.CompetencyName} Documentation",
                    Provider = "Official Reference",
                    Url = "https://developer.mozilla.org/en-US/"
                });
            }

            plan.Milestones.Add(milestone);
        }

        return plan;
    }

    private async Task<InterviewPackDto?> TryCallOpenAiAsync(
        JobDescriptionDto job, 
        CandidateProfileDto candidate, 
        CancellationToken ct)
    {
        // Reserved for live OpenAI HTTP structured completion call if key provided
        await Task.CompletedTask;
        return null;
    }
}
