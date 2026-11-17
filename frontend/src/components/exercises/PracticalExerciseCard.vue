<script setup lang="ts">
import { ref } from 'vue';
import type { PracticalExercise, ExerciseEvaluationResult } from '../../types';
import { evaluateExercise } from '../../api/interviewApi';
import ExerciseTimer from './ExerciseTimer.vue';
import RubricTable from './RubricTable.vue';

const props = defineProps<{
  exercise: PracticalExercise;
}>();

const candidateSubmission = ref('');
const isEvaluating = ref(false);
const evaluationResult = ref<ExerciseEvaluationResult | null>(null);
const evalError = ref<string | null>(null);

const handleEvaluate = async () => {
  if (!candidateSubmission.value.trim()) return;

  isEvaluating.value = true;
  evalError.value = null;
  try {
    evaluationResult.value = await evaluateExercise(props.exercise, candidateSubmission.value);
  } catch (err: any) {
    evalError.value = err.message || 'Failed to evaluate answer.';
  } finally {
    isEvaluating.value = false;
  }
};
</script>

<template>
  <div class="practical-exercise-card">
    <div class="exercise-top">
      <div class="exercise-info">
        <span class="ex-code">{{ exercise.exercise_id }}</span>
        <h4 class="ex-title">{{ exercise.title }}</h4>
        <p class="ex-scenario">{{ exercise.scenario }}</p>
      </div>

      <div class="timer-wrapper">
        <ExerciseTimer :initial-minutes="exercise.timebox_minutes" />
      </div>
    </div>

    <!-- Instructions -->
    <div class="instructions-box">
      <h5 class="box-title">Task Instructions</h5>
      <ol class="instructions-list">
        <li v-for="(instr, idx) in exercise.instructions" :key="idx">
          {{ instr }}
        </li>
      </ol>
    </div>

    <!-- Deliverable Requirement -->
    <div class="deliverable-box">
      <span class="deliv-label">Expected Deliverable:</span>
      <span class="deliv-text">{{ exercise.expected_deliverable }}</span>
    </div>

    <!-- Interactive Candidate Workspace -->
    <div class="workspace-box">
      <div class="workspace-header">
        <label for="submission-input" class="workspace-label">Candidate Practice Workspace</label>
        <span class="word-count">{{ candidateSubmission.length }} characters</span>
      </div>
      <textarea
        id="submission-input"
        v-model="candidateSubmission"
        class="submission-textarea"
        rows="7"
        placeholder="Type or paste your code snippet, SQL query, or analytical memo here to test against the rubric..."
      ></textarea>

      <div class="workspace-actions">
        <button 
          @click="handleEvaluate" 
          class="btn-evaluate" 
          :disabled="isEvaluating || !candidateSubmission.trim()"
        >
          <span v-if="isEvaluating">Analyzing against rubric...</span>
          <span v-else>Evaluate Answer with AI</span>
        </button>
        <span v-if="evalError" class="error-msg">{{ evalError }}</span>
      </div>
    </div>

    <!-- AI Evaluation Feedback Panel -->
    <div v-if="evaluationResult" class="eval-result-panel">
      <div class="result-header">
        <div class="score-badge">
          <span class="score-val">{{ evaluationResult.overallScore }}</span>
          <span class="score-denom">/100</span>
        </div>
        <div class="result-summary">
          <h5 class="eval-title">AI Rubric Evaluation</h5>
          <p class="eval-desc">{{ evaluationResult.summaryFeedback }}</p>
        </div>
      </div>

      <div class="criteria-scores">
        <div 
          v-for="(crit, idx) in evaluationResult.criterionScores" 
          :key="idx" 
          class="criterion-row"
        >
          <div class="crit-left">
            <span class="crit-name">{{ crit.criteria }}</span>
            <span :class="['crit-level', `level-${crit.levelAchieved.toLowerCase()}`]">
              {{ crit.levelAchieved }}
            </span>
          </div>
          <p class="crit-feedback">{{ crit.feedback }}</p>
        </div>
      </div>
    </div>

    <!-- Embedded Scoring Rubric -->
    <RubricTable :rubric="exercise.scoring_rubric" />
  </div>
</template>

<style scoped>
.practical-exercise-card {
  background: #ffffff;
  border: 1px solid #e5e7eb;
  border-radius: 12px;
  padding: 1.5rem;
  margin-bottom: 1.5rem;
  box-shadow: 0 1px 3px rgba(0, 0, 0, 0.05);
}

.exercise-top {
  display: flex;
  justify-content: space-between;
  align-items: flex-start;
  gap: 1.5rem;
  margin-bottom: 1.25rem;
  flex-wrap: wrap;
}

.exercise-info {
  flex: 1;
  min-width: 280px;
}

.ex-code {
  font-size: 0.75rem;
  font-weight: 700;
  color: #6b7280;
  background: #f3f4f6;
  padding: 0.2rem 0.45rem;
  border-radius: 4px;
}

.ex-title {
  font-size: 1.2rem;
  font-weight: 700;
  color: #111827;
  margin: 0.35rem 0 0.5rem 0;
}

.ex-scenario {
  font-size: 0.9rem;
  color: #4b5563;
  margin: 0;
  line-height: 1.5;
}

.instructions-box {
  background: #f8fafc;
  border: 1px solid #e2e8f0;
  border-radius: 8px;
  padding: 1rem 1.25rem;
  margin-bottom: 1rem;
}

.box-title {
  font-size: 0.85rem;
  font-weight: 700;
  text-transform: uppercase;
  color: #334155;
  margin: 0 0 0.5rem 0;
}

.instructions-list {
  margin: 0;
  padding-left: 1.2rem;
  font-size: 0.875rem;
  color: #334155;
  line-height: 1.55;
}

.instructions-list li {
  margin-bottom: 0.35rem;
}

.deliverable-box {
  background: #fdf4ff;
  border: 1px solid #f0abfc;
  border-radius: 8px;
  padding: 0.75rem 1rem;
  margin-bottom: 1.25rem;
  font-size: 0.875rem;
}

.deliv-label {
  font-weight: 700;
  color: #86198f;
  margin-right: 0.5rem;
}

.deliv-text {
  color: #701a75;
}

.workspace-box {
  background: #f1f5f9;
  border: 1px solid #cbd5e1;
  border-radius: 8px;
  padding: 1rem;
  margin-bottom: 1.25rem;
}

.workspace-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 0.4rem;
}

.workspace-label {
  font-size: 0.825rem;
  font-weight: 700;
  color: #1e293b;
}

.word-count {
  font-size: 0.75rem;
  color: #64748b;
}

.submission-textarea {
  width: 100%;
  box-sizing: border-box;
  padding: 0.75rem;
  border-radius: 6px;
  border: 1px solid #cbd5e1;
  font-family: monospace;
  font-size: 0.875rem;
  line-height: 1.45;
  color: #0f172a;
  background: #ffffff;
  resize: vertical;
}

.workspace-actions {
  display: flex;
  align-items: center;
  gap: 1rem;
  margin-top: 0.65rem;
}

.btn-evaluate {
  background: #2563eb;
  color: #ffffff;
  font-weight: 600;
  font-size: 0.85rem;
  padding: 0.5rem 1.15rem;
  border-radius: 6px;
  border: none;
  cursor: pointer;
  transition: background 0.15s;
}

.btn-evaluate:hover:not(:disabled) {
  background: #1d4ed8;
}

.btn-evaluate:disabled {
  opacity: 0.5;
  cursor: not-allowed;
}

.error-msg {
  color: #dc2626;
  font-size: 0.8rem;
}

.eval-result-panel {
  background: #f0fdf4;
  border: 1px solid #86efac;
  border-radius: 8px;
  padding: 1rem 1.25rem;
  margin-bottom: 1.25rem;
}

.result-header {
  display: flex;
  align-items: center;
  gap: 1rem;
  margin-bottom: 0.75rem;
}

.score-badge {
  background: #15803d;
  color: white;
  padding: 0.35rem 0.75rem;
  border-radius: 8px;
  font-weight: 800;
}

.score-val {
  font-size: 1.4rem;
}

.score-denom {
  font-size: 0.85rem;
  opacity: 0.8;
}

.eval-title {
  font-size: 0.95rem;
  font-weight: 700;
  color: #14532d;
  margin: 0;
}

.eval-desc {
  font-size: 0.825rem;
  color: #166534;
  margin: 0.2rem 0 0 0;
}

.criteria-scores {
  border-top: 1px solid #bbf7d0;
  padding-top: 0.75rem;
  display: flex;
  flex-direction: column;
  gap: 0.5rem;
}

.criterion-row {
  font-size: 0.825rem;
}

.crit-left {
  display: flex;
  align-items: center;
  gap: 0.5rem;
  margin-bottom: 0.15rem;
}

.crit-name {
  font-weight: 600;
  color: #14532d;
}

.crit-level {
  font-size: 0.7rem;
  font-weight: 700;
  padding: 0.1rem 0.35rem;
  border-radius: 4px;
}

.level-unsatisfactory {
  background: #fee2e2;
  color: #991b1b;
}

.level-competent {
  background: #dbeafe;
  color: #1e40af;
}

.level-exemplary {
  background: #dcfce7;
  color: #15803d;
}

.crit-feedback {
  margin: 0;
  color: #166534;
  font-size: 0.8rem;
}
</style>
