import type { 
  JobDescription, 
  CandidateProfile, 
  InterviewPack, 
  PracticalExercise, 
  ExerciseEvaluationResult 
} from '../types';

const API_BASE_URL = import.meta.env.VITE_API_BASE_URL || 'http://localhost:5000/api/v1';

export async function fetchSampleJobs(): Promise<JobDescription[]> {
  try {
    const res = await fetch(`${API_BASE_URL}/sample-data/jobs`);
    if (res.ok) {
      return await res.json();
    }
  } catch (err) {
    console.warn('Backend offline, using fallback sample data:', err);
  }
  return [];
}

export async function fetchSampleCandidates(): Promise<CandidateProfile[]> {
  try {
    const res = await fetch(`${API_BASE_URL}/sample-data/candidates`);
    if (res.ok) {
      return await res.json();
    }
  } catch (err) {
    console.warn('Backend offline, using fallback sample data:', err);
  }
  return [];
}

export async function fetchSamplePair(roleKey: string): Promise<{ job: JobDescription; candidate: CandidateProfile } | null> {
  try {
    const res = await fetch(`${API_BASE_URL}/sample-data/pair/${roleKey}`);
    if (res.ok) {
      return await res.json();
    }
  } catch (err) {
    console.warn('Backend offline:', err);
  }
  return null;
}

export async function generateInterviewPack(
  jobDescription: JobDescription,
  candidateProfile: CandidateProfile
): Promise<InterviewPack> {
  const res = await fetch(`${API_BASE_URL}/interview-packs/generate`, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify({
      jobDescription,
      candidateProfile
    })
  });

  if (!res.ok) {
    const errData = await res.json().catch(() => ({ message: 'Failed to generate pack' }));
    throw new Error(errData.message || `Server responded with status ${res.status}`);
  }

  return await res.json();
}

export async function evaluateExercise(
  exercise: PracticalExercise,
  candidateSubmission: string
): Promise<ExerciseEvaluationResult> {
  const res = await fetch(`${API_BASE_URL}/practice-sessions/evaluate`, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify({
      exercise,
      candidateSubmission
    })
  });

  if (!res.ok) {
    throw new Error(`Failed to evaluate exercise (${res.status})`);
  }

  return await res.json();
}
