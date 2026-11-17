<script setup lang="ts">
import { computed } from 'vue';
import type { CandidateGapItem } from '../../types';
import GapAnalysisCard from './GapAnalysisCard.vue';

const props = defineProps<{
  gaps: CandidateGapItem[];
}>();

const fitsCount = computed(() => props.gaps.filter(g => g.status === 'Confirmed Fit').length);
const gapsCount = computed(() => props.gaps.filter(g => g.status === 'Likely Gap').length);
const unknownsCount = computed(() => props.gaps.filter(g => g.status === 'Unknown / Insufficient Data').length);
</script>

<template>
  <div class="gap-summary-section">
    <!-- Stat Summary Bar -->
    <div class="stats-overview">
      <div class="stat-card stat-fit">
        <span class="stat-num">{{ fitsCount }}</span>
        <span class="stat-label">Confirmed Fits</span>
      </div>
      <div class="stat-card stat-gap">
        <span class="stat-num">{{ gapsCount }}</span>
        <span class="stat-label">Likely Gaps</span>
      </div>
      <div class="stat-card stat-unknown">
        <span class="stat-num">{{ unknownsCount }}</span>
        <span class="stat-label">Unknowns</span>
      </div>
    </div>

    <!-- Cards List -->
    <div class="gaps-list">
      <GapAnalysisCard 
        v-for="item in gaps" 
        :key="item.competency_id" 
        :gap="item" 
      />
    </div>
  </div>
</template>

<style scoped>
.gap-summary-section {
  margin-bottom: 2rem;
}

.stats-overview {
  display: grid;
  grid-template-columns: repeat(3, 1fr);
  gap: 1rem;
  margin-bottom: 1.5rem;
}

.stat-card {
  padding: 1rem;
  border-radius: 8px;
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  border: 1px solid transparent;
}

.stat-fit {
  background: #f0fdf4;
  border-color: #bbf7d0;
  color: #166534;
}

.stat-gap {
  background: #fef2f2;
  border-color: #fecaca;
  color: #991b1b;
}

.stat-unknown {
  background: #fffbeb;
  border-color: #fde68a;
  color: #92400e;
}

.stat-num {
  font-size: 1.75rem;
  font-weight: 800;
  line-height: 1;
  margin-bottom: 0.25rem;
}

.stat-label {
  font-size: 0.8rem;
  font-weight: 600;
  text-transform: uppercase;
  letter-spacing: 0.05em;
}
</style>
