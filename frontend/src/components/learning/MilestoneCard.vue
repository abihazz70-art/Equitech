<script setup lang="ts">
import { ref } from 'vue';
import type { LearningMilestone } from '../../types';
import StatusBadge from '../common/StatusBadge.vue';

const props = defineProps<{
  milestone: LearningMilestone;
}>();

const emit = defineEmits<{
  (e: 'toggleComplete', stepOrder: number, isDone: boolean): void;
}>();

const isDone = ref(props.milestone.isCompleted || false);

const handleToggle = () => {
  isDone.value = !isDone.value;
  emit('toggleComplete', props.milestone.step_order, isDone.value);
};
</script>

<template>
  <div :class="['milestone-card', { 'milestone-done': isDone }]">
    <div class="milestone-left">
      <button 
        @click="handleToggle" 
        :class="['check-box', { checked: isDone }]" 
        :aria-label="isDone ? 'Mark as incomplete' : 'Mark as completed'"
      >
        <span v-if="isDone">✓</span>
      </button>

      <div class="step-indicator">
        <span class="step-num">Step {{ milestone.step_order }}</span>
      </div>
    </div>

    <div class="milestone-content">
      <div class="milestone-top">
        <h4 class="skill-gap">{{ milestone.skill_gap }}</h4>
        <div class="top-tags">
          <StatusBadge type="risk" :value="`${milestone.priority} Priority`" />
          <span class="hours-tag">{{ milestone.estimated_hours }} Hours</span>
        </div>
      </div>

      <p class="objective-text">{{ milestone.learning_objective }}</p>

      <!-- Verification Task -->
      <div class="verification-box">
        <span class="verif-label">Practical Verification Task:</span>
        <p class="verif-text">{{ milestone.verification_task }}</p>
      </div>

      <!-- Resource Links -->
      <div v-if="milestone.recommended_topic_links.length > 0" class="resources-row">
        <span class="res-label">Curated References:</span>
        <div class="links-list">
          <a 
            v-for="(link, idx) in milestone.recommended_topic_links" 
            :key="idx" 
            :href="link.url" 
            target="_blank" 
            rel="noopener noreferrer"
            class="resource-link"
          >
            {{ link.title }} <span class="provider-pill">({{ link.provider }}) ↗</span>
          </a>
        </div>
      </div>
    </div>
  </div>
</template>

<style scoped>
.milestone-card {
  background: #ffffff;
  border: 1px solid #e5e7eb;
  border-radius: 10px;
  padding: 1.25rem;
  margin-bottom: 1rem;
  display: flex;
  gap: 1.25rem;
  box-shadow: 0 1px 3px rgba(0, 0, 0, 0.05);
  transition: all 0.2s;
}

.milestone-done {
  background: #f8fafc;
  border-color: #cbd5e1;
  opacity: 0.85;
}

.milestone-left {
  display: flex;
  flex-direction: column;
  align-items: center;
  gap: 0.5rem;
}

.check-box {
  width: 28px;
  height: 28px;
  border-radius: 6px;
  border: 2px solid #cbd5e1;
  background: #ffffff;
  display: flex;
  align-items: center;
  justify-content: center;
  font-weight: 800;
  color: white;
  font-size: 0.9rem;
  cursor: pointer;
  transition: all 0.15s;
}

.check-box.checked {
  background: #10b981;
  border-color: #10b981;
}

.step-num {
  font-size: 0.7rem;
  font-weight: 700;
  color: #64748b;
  white-space: nowrap;
}

.milestone-content {
  flex: 1;
}

.milestone-top {
  display: flex;
  justify-content: space-between;
  align-items: flex-start;
  gap: 1rem;
  margin-bottom: 0.4rem;
  flex-wrap: wrap;
}

.skill-gap {
  font-size: 1.05rem;
  font-weight: 700;
  color: #111827;
  margin: 0;
}

.top-tags {
  display: flex;
  align-items: center;
  gap: 0.5rem;
}

.hours-tag {
  background: #f1f5f9;
  color: #475569;
  font-size: 0.75rem;
  font-weight: 700;
  padding: 0.2rem 0.5rem;
  border-radius: 6px;
  border: 1px solid #e2e8f0;
}

.objective-text {
  font-size: 0.875rem;
  color: #374151;
  margin: 0 0 0.75rem 0;
  line-height: 1.45;
}

.verification-box {
  background: #f8fafc;
  border-left: 3px solid #6366f1;
  padding: 0.5rem 0.75rem;
  border-radius: 4px;
  margin-bottom: 0.75rem;
  font-size: 0.8rem;
}

.verif-label {
  font-weight: 700;
  color: #4338ca;
  margin-right: 0.35rem;
}

.verif-text {
  display: inline;
  color: #1e293b;
  margin: 0;
}

.resources-row {
  font-size: 0.8rem;
  display: flex;
  flex-direction: column;
  gap: 0.35rem;
}

.res-label {
  font-weight: 600;
  color: #64748b;
}

.links-list {
  display: flex;
  flex-wrap: wrap;
  gap: 0.5rem;
}

.resource-link {
  background: #eff6ff;
  color: #1d4ed8;
  padding: 0.25rem 0.6rem;
  border-radius: 6px;
  text-decoration: none;
  font-weight: 500;
  font-size: 0.775rem;
  border: 1px solid #bfdbfe;
  transition: all 0.15s;
}

.resource-link:hover {
  background: #dbeafe;
  text-decoration: underline;
}

.provider-pill {
  color: #6b7280;
  font-size: 0.7rem;
}
</style>
