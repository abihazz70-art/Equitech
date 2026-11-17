<script setup lang="ts">
import type { CandidateGapItem } from '../../types';
import StatusBadge from '../common/StatusBadge.vue';

defineProps<{
  gap: CandidateGapItem;
}>();
</script>

<template>
  <div :class="['gap-card', `border-${gap.status === 'Confirmed Fit' ? 'fit' : gap.status === 'Likely Gap' ? 'gap' : 'unknown'}`]">
    <div class="gap-top">
      <div class="title-section">
        <span class="gap-code">{{ gap.competency_id }}</span>
        <h4 class="gap-title">{{ gap.competency_name }}</h4>
      </div>

      <div class="badges">
        <StatusBadge type="status" :value="gap.status" />
        <StatusBadge type="risk" :value="`${gap.risk_level} Risk`" />
      </div>
    </div>

    <!-- Candidate Evidence Quote -->
    <div v-if="gap.candidate_evidence" class="evidence-box">
      <span class="evidence-label">Candidate Evidence:</span>
      <p class="evidence-text">"{{ gap.candidate_evidence }}"</p>
    </div>
    <div v-else class="missing-evidence-box">
      <span class="missing-label">No Profile Evidence:</span>
      <span class="missing-text">This skill was not found in the candidate profile or employment history.</span>
    </div>

    <div class="summary-box">
      <span class="summary-label">Assessment:</span>
      <p class="summary-text">{{ gap.gap_summary }}</p>
    </div>
  </div>
</template>

<style scoped>
.gap-card {
  background: #ffffff;
  border-radius: 10px;
  padding: 1.25rem;
  margin-bottom: 1rem;
  border: 1px solid #e5e7eb;
  border-left-width: 5px;
  box-shadow: 0 1px 3px rgba(0, 0, 0, 0.05);
}

.border-fit {
  border-left-color: #10b981;
}

.border-gap {
  border-left-color: #ef4444;
}

.border-unknown {
  border-left-color: #f59e0b;
}

.gap-top {
  display: flex;
  justify-content: space-between;
  align-items: flex-start;
  gap: 1rem;
  margin-bottom: 0.85rem;
  flex-wrap: wrap;
}

.title-section {
  display: flex;
  align-items: center;
  gap: 0.5rem;
}

.gap-code {
  font-size: 0.75rem;
  font-weight: 700;
  color: #6b7280;
  background: #f3f4f6;
  padding: 0.2rem 0.45rem;
  border-radius: 4px;
}

.gap-title {
  font-size: 1.05rem;
  font-weight: 600;
  color: #111827;
  margin: 0;
}

.badges {
  display: flex;
  gap: 0.4rem;
}

.evidence-box {
  background: #f0fdf4;
  border: 1px solid #bbf7d0;
  border-radius: 6px;
  padding: 0.65rem 0.85rem;
  margin-bottom: 0.65rem;
}

.evidence-label {
  display: block;
  font-size: 0.7rem;
  font-weight: 700;
  text-transform: uppercase;
  color: #166534;
  letter-spacing: 0.04em;
  margin-bottom: 0.2rem;
}

.evidence-text {
  font-size: 0.85rem;
  color: #14532d;
  font-style: italic;
  margin: 0;
  line-height: 1.4;
}

.missing-evidence-box {
  background: #fef2f2;
  border: 1px solid #fecaca;
  border-radius: 6px;
  padding: 0.65rem 0.85rem;
  margin-bottom: 0.65rem;
  font-size: 0.85rem;
}

.missing-label {
  font-weight: 700;
  color: #991b1b;
  margin-right: 0.35rem;
}

.missing-text {
  color: #7f1d1d;
}

.summary-box {
  display: flex;
  gap: 0.4rem;
  font-size: 0.85rem;
  color: #374151;
  line-height: 1.45;
}

.summary-label {
  font-weight: 600;
  color: #4b5563;
  flex-shrink: 0;
}

.summary-text {
  margin: 0;
}
</style>
