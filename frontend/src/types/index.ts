export type CompetencyCategory = 'Technical' | 'DomainKnowledge' | 'Behavioral' | 'Methodology';
export type CompetencyImportance = 'Must-Have' | 'Preferred' | 'Bonus';
export type GapStatus = 'Confirmed Fit' | 'Likely Gap' | 'Unknown / Insufficient Data';
export type RiskLevel = 'Low' | 'Medium' | 'High';
export type QuestionCategory = 'Technical' | 'Behavioral' | 'Role-Specific' | 'Case-Study';
export type QuestionDifficulty = 'Foundational' | 'Intermediate' | 'Advanced';
export type AnswerFrameworkType = 'STAR' | 'CAR' | 'Technical Root-Cause' | 'Executive Summary';

export interface JobDescription {
  source_id?: string;
  source_job_id?: string;
  title: string;
  employer: string;
  canonical_url?: string;
  location: string;
  country_code?: string;
  employment_type?: string;
  salary_min?: number | null;
  salary_max?: number | null;
  salary_currency?: string;
  posted_at?: string;
  status?: string;
  description: string;
}

export interface CandidateEmployment {
  company: string;
  role: string;
  start_date: string;
  end_date?: string | null;
  is_current: boolean;
  responsibilities: string[];
}

export interface CandidateEducation {
  institution: string;
  degree: string;
  graduation_year: number;
}

export interface CandidateSkill {
  name: string;
  proficiency: string;
  evidence: string;
}

export interface CandidateProfile {
  profile_id?: string;
  full_name: string;
  headline: string;
  location: string;
  summary: string;
  employment_history: CandidateEmployment[];
  education: CandidateEducation[];
  skills: CandidateSkill[];
  certifications: string[];
  achievements: string[];
}

export interface SourceBasis {
  job_title: string;
  employer: string;
  candidate_name: string;
  profile_version: number;
}

export interface RoleCompetency {
  competency_id: string;
  name: string;
  category: string;
  importance: string;
  description: string;
}

export interface CandidateGapItem {
  competency_id: string;
  competency_name: string;
  status: GapStatus;
  candidate_evidence: string | null;
  gap_summary: string;
  risk_level: RiskLevel;
}

export interface AnswerFramework {
  model_structure: string;
  recommended_approach: string;
  candidate_talking_points: string[];
  pitfalls_to_avoid: string[];
}

export interface InterviewQuestion {
  question_id: string;
  category: string;
  difficulty: string;
  question_text: string;
  rationale: string;
  target_competency_ids: string[];
  answer_framework: AnswerFramework;
}

export interface RubricLevels {
  unsatisfactory: string;
  competent: string;
  exemplary: string;
}

export interface ScoringRubricItem {
  criteria: string;
  weight_percent: number;
  levels: RubricLevels;
}

export interface PracticalExercise {
  exercise_id: string;
  title: string;
  scenario: string;
  timebox_minutes: number;
  instructions: string[];
  expected_deliverable: string;
  scoring_rubric: ScoringRubricItem[];
  model_solution_outline: string;
}

export interface TopicResourceLink {
  title: string;
  provider: string;
  url: string;
}

export interface LearningMilestone {
  step_order: number;
  skill_gap: string;
  learning_objective: string;
  estimated_hours: number;
  priority: string;
  suggested_resource_types: string[];
  recommended_topic_links: TopicResourceLink[];
  verification_task: string;
  isCompleted?: boolean;
}

export interface LearningPlan {
  target_role: string;
  total_estimated_hours: number;
  milestones: LearningMilestone[];
}

export interface InterviewPack {
  pack_id: string;
  job_id: string;
  profile_id: string;
  generated_at: string;
  source_basis: SourceBasis;
  disclaimer: string;
  role_competencies: RoleCompetency[];
  candidate_gap_analysis: CandidateGapItem[];
  questions: InterviewQuestion[];
  practical_exercises: PracticalExercise[];
  learning_plan: LearningPlan;
}

export interface RubricCriterionScore {
  criteria: string;
  levelAchieved: string;
  scoreAwarded: number;
  feedback: string;
}

export interface ExerciseEvaluationResult {
  overallScore: number;
  summaryFeedback: string;
  criterionScores: RubricCriterionScore[];
}
