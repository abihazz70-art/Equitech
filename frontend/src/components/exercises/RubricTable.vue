<script setup lang="ts">
import type { ScoringRubricItem } from '../../types';

defineProps<{
  rubric: ScoringRubricItem[];
}>();
</script>

<template>
  <div class="rubric-container">
    <div class="rubric-header">
      <h5 class="rubric-title">Scoring Rubric (Appendix A5)</h5>
      <span class="rubric-subtitle">Evaluation standards applied to your practical submission</span>
    </div>

    <div class="table-wrapper">
      <table class="rubric-table">
        <thead>
          <tr>
            <th class="col-criteria">Criteria & Weight</th>
            <th class="col-level level-unsat">Unsatisfactory</th>
            <th class="col-level level-comp">Competent (Passing)</th>
            <th class="col-level level-exemp">Exemplary</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="(item, idx) in rubric" :key="idx">
            <td class="cell-criteria">
              <span class="criteria-name">{{ item.criteria }}</span>
              <span class="weight-badge">{{ item.weight_percent }}%</span>
            </td>
            <td class="cell-desc desc-unsat">
              {{ item.levels.unsatisfactory }}
            </td>
            <td class="cell-desc desc-comp">
              {{ item.levels.competent }}
            </td>
            <td class="cell-desc desc-exemp">
              {{ item.levels.exemplary }}
            </td>
          </tr>
        </tbody>
      </table>
    </div>
  </div>
</template>

<style scoped>
.rubric-container {
  margin-top: 1.25rem;
  background: #ffffff;
  border: 1px solid #e5e7eb;
  border-radius: 8px;
  overflow: hidden;
}

.rubric-header {
  padding: 0.85rem 1.15rem;
  background: #f8fafc;
  border-bottom: 1px solid #e2e8f0;
}

.rubric-title {
  font-size: 0.875rem;
  font-weight: 700;
  color: #1e293b;
  margin: 0 0 0.15rem 0;
}

.rubric-subtitle {
  font-size: 0.75rem;
  color: #64748b;
}

.table-wrapper {
  overflow-x: auto;
}

.rubric-table {
  width: 100%;
  border-collapse: collapse;
  font-size: 0.825rem;
  text-align: left;
}

.rubric-table th {
  padding: 0.75rem 1rem;
  background: #f1f5f9;
  color: #475569;
  font-weight: 600;
  border-bottom: 1px solid #cbd5e1;
}

.level-unsat {
  color: #991b1b;
}

.level-comp {
  color: #1e40af;
}

.level-exemp {
  color: #065f46;
}

.rubric-table td {
  padding: 0.85rem 1rem;
  border-bottom: 1px solid #f1f5f9;
  vertical-align: top;
  line-height: 1.45;
}

.cell-criteria {
  font-weight: 600;
  color: #1e293b;
  width: 24%;
}

.criteria-name {
  display: block;
  margin-bottom: 0.25rem;
}

.weight-badge {
  display: inline-block;
  font-size: 0.7rem;
  font-weight: 700;
  background: #e2e8f0;
  color: #334155;
  padding: 0.1rem 0.4rem;
  border-radius: 4px;
}

.cell-desc {
  width: 25%;
  color: #4b5563;
}

.desc-unsat {
  background: #fef2f208;
}

.desc-comp {
  background: #eff6ff08;
}

.desc-exemp {
  background: #ecfdf508;
}
</style>
