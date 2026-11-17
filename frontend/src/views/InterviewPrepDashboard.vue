<script setup lang="ts">
import { ref, computed } from 'vue';
import type { InterviewPack } from '../types';
import CompetencyMatrix from '../components/competencies/CompetencyMatrix.vue';
import GapSummaryList from '../components/gaps/GapSummaryList.vue';
import QuestionFilterTabs from '../components/questions/QuestionFilterTabs.vue';
import QuestionCard from '../components/questions/QuestionCard.vue';
import PracticalExerciseCard from '../components/exercises/PracticalExerciseCard.vue';
import LearningPlanRoadmap from '../components/learning/LearningPlanRoadmap.vue';

const props = defineProps<{
  pack: InterviewPack;
}>();

const emit = defineEmits<{
  (e: 'reset'): void;
}>();

type ActiveTab = 'overview' | 'gaps' | 'questions' | 'exercises' | 'learning';
const currentTab = ref<ActiveTab>('overview');

const activeQuestionCategory = ref('All');
const questionCategories = computed(() => {
  const cats = new Set(props.pack.questions.map(q => q.category));
  return ['All', ...Array.from(cats)];
});

const filteredQuestions = computed(() => {
  if (activeQuestionCategory.value === 'All') {
    return props.pack.questions;
  }
  return props.pack.questions.filter(q => q.category === activeQuestionCategory.value);
});

const downloadPackJson = () => {
  const dataStr = "data:text/json;charset=utf-8," + encodeURIComponent(JSON.stringify(props.pack, null, 2));
  const downloadAnchor = document.createElement('a');
  downloadAnchor.setAttribute("href", dataStr);
  downloadAnchor.setAttribute("download", `InterviewPack_${props.pack.job_id}_${props.pack.profile_id}.json`);
  document.body.appendChild(downloadAnchor);
  downloadAnchor.click();
  downloadAnchor.remove();
};
</script>

<template>
  <div class="dashboard-container">
    <!-- Header Summary Card -->
    <div class="dashboard-header-card">
      <div class="pack-meta">
        <span class="pack-badge">Interview Pack (Appendix A5)</span>
        <h2 class="pack-title">{{ pack.source_basis.job_title }}</h2>
        <span class="pack-employer">{{ pack.source_basis.employer }}</span>
      </div>

      <div class="pack-actions">
        <button @click="downloadPackJson" class="btn-secondary">
          Export JSON
        </button>
        <button @click="$emit('reset')" class="btn-primary">
          Switch Role / Profile
        </button>
      </div>
    </div>

    <!-- Mandatory Disclaimer Banner -->
    <div class="disclaimer-alert">
      <span class="alert-icon">ℹ️</span>
      <p class="alert-text">{{ pack.disclaimer }}</p>
    </div>

    <!-- Segmented Navigation Tabs -->
    <nav class="nav-segments">
      <button 
        :class="['segment-btn', { active: currentTab === 'overview' }]"
        @click="currentTab = 'overview'"
      >
        <span class="tab-label">1. Overview & Competencies</span>
        <span class="tab-count">{{ pack.role_competencies.length }}</span>
      </button>

      <button 
        :class="['segment-btn', { active: currentTab === 'gaps' }]"
        @click="currentTab = 'gaps'"
      >
        <span class="tab-label">2. Fit & Gap Analysis</span>
        <span class="tab-count">{{ pack.candidate_gap_analysis.length }}</span>
      </button>

      <button 
        :class="['segment-btn', { active: currentTab === 'questions' }]"
        @click="currentTab = 'questions'"
      >
        <span class="tab-label">3. Question Bank</span>
        <span class="tab-count">{{ pack.questions.length }}</span>
      </button>

      <button 
        :class="['segment-btn', { active: currentTab === 'exercises' }]"
        @click="currentTab = 'exercises'"
      >
        <span class="tab-label">4. Practical Exercises</span>
        <span class="tab-count">{{ pack.practical_exercises.length }}</span>
      </button>

      <button 
        :class="['segment-btn', { active: currentTab === 'learning' }]"
        @click="currentTab = 'learning'"
      >
        <span class="tab-label">5. Learning Roadmap</span>
        <span class="tab-count">{{ pack.learning_plan.milestones.length }}</span>
      </button>
    </nav>

    <!-- Tab Contents -->
    <main class="tab-content">
      <!-- 1. Overview & Competencies -->
      <section v-if="currentTab === 'overview'">
        <CompetencyMatrix :competencies="pack.role_competencies" />
      </section>

      <!-- 2. Gap Analysis -->
      <section v-if="currentTab === 'gaps'">
        <GapSummaryList :gaps="pack.candidate_gap_analysis" />
      </section>

      <!-- 3. Question Bank -->
      <section v-if="currentTab === 'questions'">
        <QuestionFilterTabs 
          :categories="questionCategories" 
          :active-category="activeQuestionCategory" 
          @select="cat => activeQuestionCategory = cat" 
        />
        <div class="questions-list">
          <QuestionCard 
            v-for="q in filteredQuestions" 
            :key="q.question_id" 
            :question="q" 
          />
        </div>
      </section>

      <!-- 4. Practical Exercises & Rubrics -->
      <section v-if="currentTab === 'exercises'">
        <div class="exercises-intro">
          <h3 class="section-title">Practical Simulation Tasks</h3>
          <p class="section-desc">Timed real-world challenges designed to assess the candidate under authentic workplace constraints.</p>
        </div>
        <PracticalExerciseCard 
          v-for="ex in pack.practical_exercises" 
          :key="ex.exercise_id" 
          :exercise="ex" 
        />
      </section>

      <!-- 5. Learning Roadmap -->
      <section v-if="currentTab === 'learning'">
        <LearningPlanRoadmap :plan="pack.learning_plan" />
      </section>
    </main>
  </div>
</template>

<style scoped>
.dashboard-container {
  max-width: 1180px;
  margin: 1.5rem auto;
  padding: 0 1rem;
}

.dashboard-header-card {
  background: #ffffff;
  border: 1px solid #e2e8f0;
  border-radius: 12px;
  padding: 1.5rem;
  display: flex;
  justify-content: space-between;
  align-items: center;
  gap: 1.5rem;
  margin-bottom: 1rem;
  box-shadow: 0 1px 3px rgba(0, 0, 0, 0.05);
  flex-wrap: wrap;
}

.pack-badge {
  display: inline-block;
  font-size: 0.75rem;
  font-weight: 700;
  background: #eff6ff;
  color: #2563eb;
  padding: 0.15rem 0.5rem;
  border-radius: 4px;
  margin-bottom: 0.35rem;
}

.pack-title {
  font-size: 1.5rem;
  font-weight: 800;
  color: #0f172a;
  margin: 0 0 0.25rem 0;
}

.pack-employer {
  font-size: 0.95rem;
  color: #64748b;
  font-weight: 500;
}

.pack-actions {
  display: flex;
  gap: 0.75rem;
}

.btn-secondary {
  background: #f1f5f9;
  color: #334155;
  border: 1px solid #cbd5e1;
  padding: 0.5rem 1rem;
  border-radius: 6px;
  font-weight: 600;
  font-size: 0.85rem;
  cursor: pointer;
}

.btn-primary {
  background: #2563eb;
  color: white;
  border: none;
  padding: 0.5rem 1rem;
  border-radius: 6px;
  font-weight: 600;
  font-size: 0.85rem;
  cursor: pointer;
}

.disclaimer-alert {
  background: #fffbeb;
  border: 1px solid #fde68a;
  border-radius: 8px;
  padding: 0.75rem 1rem;
  display: flex;
  align-items: flex-start;
  gap: 0.75rem;
  margin-bottom: 1.5rem;
}

.alert-icon {
  font-size: 1.1rem;
}

.alert-text {
  font-size: 0.825rem;
  color: #92400e;
  margin: 0;
  line-height: 1.45;
}

.nav-segments {
  display: flex;
  gap: 0.5rem;
  border-bottom: 2px solid #e2e8f0;
  margin-bottom: 1.75rem;
  overflow-x: auto;
}

.segment-btn {
  background: transparent;
  border: none;
  border-bottom: 3px solid transparent;
  padding: 0.75rem 1rem;
  font-size: 0.9rem;
  font-weight: 600;
  color: #64748b;
  cursor: pointer;
  display: flex;
  align-items: center;
  gap: 0.5rem;
  margin-bottom: -2px;
  transition: all 0.15s;
  white-space: nowrap;
}

.segment-btn:hover {
  color: #1e293b;
}

.segment-btn.active {
  color: #2563eb;
  border-bottom-color: #2563eb;
}

.tab-count {
  background: #f1f5f9;
  font-size: 0.75rem;
  font-weight: 700;
  color: #475569;
  padding: 0.1rem 0.45rem;
  border-radius: 9999px;
}

.segment-btn.active .tab-count {
  background: #dbeafe;
  color: #1d4ed8;
}

.tab-content {
  min-height: 400px;
}

.exercises-intro {
  margin-bottom: 1.25rem;
}

.section-title {
  font-size: 1.15rem;
  font-weight: 700;
  color: #111827;
  margin: 0 0 0.25rem 0;
}

.section-desc {
  font-size: 0.875rem;
  color: #6b7280;
  margin: 0;
}
</style>
