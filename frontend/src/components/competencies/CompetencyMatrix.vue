<script setup lang="ts">
import type { RoleCompetency } from '../../types';
import StatusBadge from '../common/StatusBadge.vue';

defineProps<{
  competencies: RoleCompetency[];
}>();
</script>

<template>
  <div class="competency-matrix">
    <div class="section-intro">
      <h3 class="section-title">Extracted Job Competencies</h3>
      <p class="section-desc">Key requirements and skills extracted from the vacancy by the AI engine.</p>
    </div>

    <div class="competency-grid">
      <div 
        v-for="comp in competencies" 
        :key="comp.competency_id" 
        class="competency-card"
      >
        <div class="card-header">
          <span class="comp-id">{{ comp.competency_id }}</span>
          <div class="badges-row">
            <StatusBadge type="category" :value="comp.category" />
            <StatusBadge type="importance" :value="comp.importance" />
          </div>
        </div>

        <h4 class="comp-name">{{ comp.name }}</h4>
        <p class="comp-desc">{{ comp.description }}</p>
      </div>
    </div>
  </div>
</template>

<style scoped>
.competency-matrix {
  margin-bottom: 2rem;
}

.section-intro {
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

.competency-grid {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(320px, 1fr));
  gap: 1rem;
}

.competency-card {
  background: #ffffff;
  border: 1px solid #e5e7eb;
  border-radius: 10px;
  padding: 1.15rem;
  box-shadow: 0 1px 3px rgba(0, 0, 0, 0.05);
  transition: transform 0.15s, box-shadow 0.15s;
}

.competency-card:hover {
  transform: translateY(-2px);
  box-shadow: 0 4px 6px -1px rgba(0, 0, 0, 0.1);
}

.card-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 0.75rem;
}

.comp-id {
  font-size: 0.75rem;
  font-weight: 700;
  color: #9ca3af;
  letter-spacing: 0.05em;
}

.badges-row {
  display: flex;
  gap: 0.4rem;
}

.comp-name {
  font-size: 1rem;
  font-weight: 600;
  color: #1f2937;
  margin: 0 0 0.5rem 0;
  line-height: 1.35;
}

.comp-desc {
  font-size: 0.85rem;
  color: #4b5563;
  margin: 0;
  line-height: 1.45;
}
</style>
