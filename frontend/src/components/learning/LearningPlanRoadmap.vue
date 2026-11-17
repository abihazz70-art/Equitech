<script setup lang="ts">
import { ref, computed } from 'vue';
import type { LearningPlan } from '../../types';
import MilestoneCard from './MilestoneCard.vue';

const props = defineProps<{
  plan: LearningPlan;
}>();

const completedSteps = ref<Set<number>>(new Set());

const handleToggle = (stepOrder: number, isDone: boolean) => {
  if (isDone) {
    completedSteps.value.add(stepOrder);
  } else {
    completedSteps.value.delete(stepOrder);
  }
};

const progressPercentage = computed(() => {
  if (!props.plan.milestones.length) return 0;
  return Math.round((completedSteps.value.size / props.plan.milestones.length) * 100);
});

const completedHours = computed(() => {
  return props.plan.milestones
    .filter(m => completedSteps.value.has(m.step_order))
    .reduce((sum, m) => sum + m.estimated_hours, 0);
});
</script>

<template>
  <div class="learning-roadmap">
    <!-- Summary Header -->
    <div class="plan-header-card">
      <div class="header-left">
        <h3 class="plan-role">Learning Plan: {{ plan.target_role }}</h3>
        <p class="plan-desc">Structured skill gap remediation ordered by priority and prerequisite impact.</p>
      </div>

      <div class="header-stats">
        <div class="stat-bubble">
          <span class="bubble-num">{{ plan.total_estimated_hours }} hrs</span>
          <span class="bubble-label">Total Effort</span>
        </div>
        <div class="stat-bubble bubble-progress">
          <span class="bubble-num">{{ progressPercentage }}%</span>
          <span class="bubble-label">{{ completedHours }}h Completed</span>
        </div>
      </div>
    </div>

    <!-- Progress Track -->
    <div class="overall-progress-bar">
      <div class="bar-fill" :style="{ width: `${progressPercentage}%` }"></div>
    </div>

    <!-- Milestones List -->
    <div class="milestones-container">
      <MilestoneCard 
        v-for="item in plan.milestones" 
        :key="item.step_order" 
        :milestone="item" 
        @toggle-complete="handleToggle"
      />
    </div>
  </div>
</template>

<style scoped>
.learning-roadmap {
  margin-bottom: 2rem;
}

.plan-header-card {
  background: #ffffff;
  border: 1px solid #e5e7eb;
  border-radius: 10px;
  padding: 1.25rem 1.5rem;
  display: flex;
  justify-content: space-between;
  align-items: center;
  gap: 1.5rem;
  margin-bottom: 1rem;
  box-shadow: 0 1px 3px rgba(0, 0, 0, 0.05);
  flex-wrap: wrap;
}

.header-left {
  flex: 1;
  min-width: 250px;
}

.plan-role {
  font-size: 1.15rem;
  font-weight: 700;
  color: #111827;
  margin: 0 0 0.25rem 0;
}

.plan-desc {
  font-size: 0.85rem;
  color: #6b7280;
  margin: 0;
}

.header-stats {
  display: flex;
  gap: 1rem;
}

.stat-bubble {
  background: #f1f5f9;
  border-radius: 8px;
  padding: 0.5rem 1rem;
  text-align: center;
  border: 1px solid #e2e8f0;
}

.bubble-progress {
  background: #ecfdf5;
  border-color: #a7f3d0;
}

.bubble-num {
  display: block;
  font-size: 1.25rem;
  font-weight: 800;
  color: #0f172a;
}

.bubble-progress .bubble-num {
  color: #065f46;
}

.bubble-label {
  font-size: 0.7rem;
  font-weight: 600;
  text-transform: uppercase;
  color: #64748b;
  letter-spacing: 0.04em;
}

.bubble-progress .bubble-label {
  color: #047857;
}

.overall-progress-bar {
  width: 100%;
  height: 8px;
  background: #e2e8f0;
  border-radius: 4px;
  overflow: hidden;
  margin-bottom: 1.5rem;
}

.bar-fill {
  height: 100%;
  background: linear-gradient(90deg, #3b82f6, #10b981);
  transition: width 0.3s ease;
}

.milestones-container {
  display: flex;
  flex-direction: column;
}
</style>
