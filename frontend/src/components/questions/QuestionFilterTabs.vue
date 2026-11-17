<script setup lang="ts">
defineProps<{
  categories: string[];
  activeCategory: string;
}>();

defineEmits<{
  (e: 'select', category: string): void;
}>();
</script>

<template>
  <div class="filter-tabs">
    <button
      v-for="cat in categories"
      :key="cat"
      :class="['filter-btn', { active: activeCategory === cat }]"
      @click="$emit('select', cat)"
    >
      {{ cat }}
    </button>
  </div>
</template>

<style scoped>
.filter-tabs {
  display: flex;
  gap: 0.5rem;
  margin-bottom: 1.25rem;
  overflow-x: auto;
  padding-bottom: 0.25rem;
}

.filter-btn {
  background: #f3f4f6;
  color: #4b5563;
  border: 1px solid #e5e7eb;
  padding: 0.45rem 1rem;
  border-radius: 9999px;
  font-size: 0.825rem;
  font-weight: 600;
  cursor: pointer;
  transition: all 0.15s;
  white-space: nowrap;
}

.filter-btn:hover {
  background: #e5e7eb;
  color: #111827;
}

.filter-btn.active {
  background: #1e40af;
  color: #ffffff;
  border-color: #1e40af;
}
</style>
