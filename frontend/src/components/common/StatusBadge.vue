<script setup lang="ts">
import { computed } from 'vue';

const props = defineProps<{
  type: 'status' | 'risk' | 'importance' | 'category';
  value: string;
}>();

const badgeClass = computed(() => {
  const val = props.value.toLowerCase();
  
  if (props.type === 'status') {
    if (val.includes('confirmed') || val.includes('fit')) return 'badge-green';
    if (val.includes('gap')) return 'badge-red';
    return 'badge-amber';
  }
  
  if (props.type === 'risk') {
    if (val === 'high') return 'badge-red';
    if (val === 'medium') return 'badge-amber';
    return 'badge-green';
  }

  if (props.type === 'importance') {
    if (val === 'must-have') return 'badge-purple';
    if (val === 'preferred') return 'badge-blue';
    return 'badge-gray';
  }

  return 'badge-slate';
});
</script>

<template>
  <span :class="['status-badge', badgeClass]">
    {{ value }}
  </span>
</template>

<style scoped>
.status-badge {
  display: inline-flex;
  align-items: center;
  padding: 0.2rem 0.55rem;
  border-radius: 6px;
  font-size: 0.75rem;
  font-weight: 600;
  text-transform: capitalize;
  letter-spacing: 0.02em;
  white-space: nowrap;
}

.badge-green {
  background-color: #ecfdf5;
  color: #065f46;
  border: 1px solid #a7f3d0;
}

.badge-red {
  background-color: #fef2f2;
  color: #991b1b;
  border: 1px solid #fecaca;
}

.badge-amber {
  background-color: #fffbeb;
  color: #92400e;
  border: 1px solid #fde68a;
}

.badge-purple {
  background-color: #f5f3ff;
  color: #5b21b6;
  border: 1px solid #ddd6fe;
}

.badge-blue {
  background-color: #eff6ff;
  color: #1e40af;
  border: 1px solid #bfdbfe;
}

.badge-gray {
  background-color: #f3f4f6;
  color: #374151;
  border: 1px solid #e5e7eb;
}

.badge-slate {
  background-color: #f1f5f9;
  color: #475569;
  border: 1px solid #cbd5e1;
}
</style>
