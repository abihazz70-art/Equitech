<script setup lang="ts">
import { ref } from 'vue';
import type { InterviewQuestion } from '../../types';
import StatusBadge from '../common/StatusBadge.vue';

defineProps<{
  question: InterviewQuestion;
}>();

const isOpen = ref(false);
const toggleOpen = () => {
  isOpen.value = !isOpen.value;
};
</script>

<template>
  <div class="question-card">
    <div class="card-summary" @click="toggleOpen">
      <div class="summary-left">
        <div class="meta-row">
          <span class="q-id">{{ question.question_id }}</span>
          <StatusBadge type="category" :value="question.category" />
          <span class="difficulty-tag" :class="`diff-${question.difficulty.toLowerCase()}`">
            {{ question.difficulty }}
          </span>
        </div>
        <h4 class="q-text">{{ question.question_text }}</h4>
      </div>

      <button class="toggle-btn" :aria-expanded="isOpen">
        <span>{{ isOpen ? 'Hide Guide' : 'View Guide' }}</span>
        <svg 
          :class="['chevron', { rotated: isOpen }]" 
          viewBox="0 0 20 20" 
          fill="currentColor" 
          width="18" 
          height="18"
        >
          <path fill-rule="evenodd" d="M5.293 7.293a1 1 0 011.414 0L10 10.586l3.293-3.293a1 1 0 111.414 1.414l-4 4a1 1 0 01-1.414 0l-4-4a1 1 0 010-1.414z" clip-rule="evenodd" />
        </svg>
      </button>
    </div>

    <!-- Collapsible Coaching & Answer Framework Guide -->
    <div v-if="isOpen" class="guide-panel">
      <!-- Rationale -->
      <div class="guide-section rationale-section">
        <h5 class="guide-heading">Interviewer Rationale:</h5>
        <p class="guide-body">{{ question.rationale }}</p>
      </div>

      <!-- Framework & Approach -->
      <div class="guide-section framework-section">
        <div class="framework-header">
          <span class="framework-model">Model: {{ question.answer_framework.model_structure }}</span>
          <h5 class="guide-heading">Recommended Answering Approach</h5>
        </div>
        <p class="guide-body">{{ question.answer_framework.recommended_approach }}</p>
      </div>

      <div class="grid-two-col">
        <!-- Candidate-specific Talking Points -->
        <div class="guide-section points-section">
          <h5 class="guide-heading text-green">Your Profile Talking Points</h5>
          <ul class="bullet-list">
            <li v-for="(point, idx) in question.answer_framework.candidate_talking_points" :key="idx">
              {{ point }}
            </li>
          </ul>
        </div>

        <!-- Pitfalls to Avoid -->
        <div class="guide-section pitfalls-section">
          <h5 class="guide-heading text-red">Pitfalls to Avoid</h5>
          <ul class="bullet-list">
            <li v-for="(pitfall, idx) in question.answer_framework.pitfalls_to_avoid" :key="idx">
              {{ pitfall }}
            </li>
          </ul>
        </div>
      </div>
    </div>
  </div>
</template>

<style scoped>
.question-card {
  background: #ffffff;
  border: 1px solid #e5e7eb;
  border-radius: 10px;
  margin-bottom: 1rem;
  box-shadow: 0 1px 3px rgba(0, 0, 0, 0.05);
  overflow: hidden;
  transition: border-color 0.15s;
}

.question-card:hover {
  border-color: #cbd5e1;
}

.card-summary {
  padding: 1.25rem;
  display: flex;
  justify-content: space-between;
  align-items: flex-start;
  gap: 1rem;
  cursor: pointer;
}

.summary-left {
  flex: 1;
}

.meta-row {
  display: flex;
  align-items: center;
  gap: 0.5rem;
  margin-bottom: 0.5rem;
  flex-wrap: wrap;
}

.q-id {
  font-size: 0.75rem;
  font-weight: 700;
  color: #6b7280;
  background: #f3f4f6;
  padding: 0.2rem 0.45rem;
  border-radius: 4px;
}

.difficulty-tag {
  font-size: 0.75rem;
  font-weight: 600;
  padding: 0.2rem 0.5rem;
  border-radius: 6px;
}

.diff-foundational {
  background: #f0fdf4;
  color: #166534;
}

.diff-intermediate {
  background: #eff6ff;
  color: #1e40af;
}

.diff-advanced {
  background: #fdf2f8;
  color: #9d174d;
}

.q-text {
  font-size: 1.05rem;
  font-weight: 600;
  color: #111827;
  margin: 0;
  line-height: 1.4;
}

.toggle-btn {
  background: #f8fafc;
  border: 1px solid #e2e8f0;
  padding: 0.4rem 0.75rem;
  border-radius: 6px;
  font-size: 0.8rem;
  font-weight: 600;
  color: #3b82f6;
  display: flex;
  align-items: center;
  gap: 0.35rem;
  cursor: pointer;
  white-space: nowrap;
}

.toggle-btn:hover {
  background: #f1f5f9;
}

.chevron {
  transition: transform 0.2s ease;
}

.chevron.rotated {
  transform: rotate(180deg);
}

.guide-panel {
  background: #f9fafb;
  border-top: 1px solid #e5e7eb;
  padding: 1.25rem;
}

.guide-section {
  margin-bottom: 1rem;
}

.guide-heading {
  font-size: 0.8rem;
  font-weight: 700;
  text-transform: uppercase;
  letter-spacing: 0.04em;
  margin: 0 0 0.35rem 0;
  color: #4b5563;
}

.guide-body {
  font-size: 0.875rem;
  color: #374151;
  margin: 0;
  line-height: 1.5;
}

.rationale-section {
  background: #f1f5f9;
  padding: 0.75rem 1rem;
  border-radius: 6px;
  border-left: 3px solid #64748b;
}

.framework-section {
  background: #eff6ff;
  padding: 0.75rem 1rem;
  border-radius: 6px;
  border-left: 3px solid #3b82f6;
}

.framework-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 0.35rem;
}

.framework-model {
  font-size: 0.75rem;
  font-weight: 700;
  background: #dbeafe;
  color: #1e40af;
  padding: 0.15rem 0.5rem;
  border-radius: 4px;
}

.grid-two-col {
  display: grid;
  grid-template-columns: 1fr 1fr;
  gap: 1rem;
}

@media (max-width: 640px) {
  .grid-two-col {
    grid-template-columns: 1fr;
  }
}

.points-section {
  background: #f0fdf4;
  padding: 0.75rem 1rem;
  border-radius: 6px;
  border: 1px solid #dcfce7;
}

.pitfalls-section {
  background: #fef2f2;
  padding: 0.75rem 1rem;
  border-radius: 6px;
  border: 1px solid #fee2e2;
}

.text-green {
  color: #15803d;
}

.text-red {
  color: #b91c1c;
}

.bullet-list {
  margin: 0;
  padding-left: 1.2rem;
  font-size: 0.825rem;
  color: #374151;
  line-height: 1.45;
}

.bullet-list li {
  margin-bottom: 0.35rem;
}
</style>
