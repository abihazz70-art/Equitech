<script setup lang="ts">
defineProps<{
  jobTitle?: string;
  candidateName?: string;
  hasPack?: boolean;
}>();

defineEmits<{
  (e: 'reset'): void;
}>();
</script>

<template>
  <header class="app-header">
    <div class="header-container">
      <div class="brand-section">
        <div class="logo-badge">EQ</div>
        <div>
          <h1 class="brand-title">Equitech AI</h1>
          <p class="brand-subtitle">Interview Preparation & Learning Module</p>
        </div>
      </div>

      <div class="header-meta">
        <div class="db-pill">
          <span class="status-dot"></span>
          <span>Supabase Connected</span>
        </div>

        <div v-if="hasPack" class="session-info">
          <span class="candidate-tag">{{ candidateName }}</span>
          <span class="arrow-sep">-></span>
          <span class="role-tag">{{ jobTitle }}</span>
          <button @click="$emit('reset')" class="btn-change">Change Pair</button>
        </div>
      </div>
    </div>
  </header>
</template>

<style scoped>
.app-header {
  background: #111827;
  color: #f9fafb;
  border-bottom: 1px solid #1f2937;
  padding: 1rem 1.5rem;
  position: sticky;
  top: 0;
  z-index: 50;
  box-shadow: 0 4px 6px -1px rgba(0, 0, 0, 0.1);
}

.header-container {
  max-width: 1280px;
  margin: 0 auto;
  display: flex;
  justify-content: space-between;
  align-items: center;
  flex-wrap: wrap;
  gap: 1rem;
}

.brand-section {
  display: flex;
  align-items: center;
  gap: 0.75rem;
}

.logo-badge {
  background: linear-gradient(135deg, #3b82f6, #8b5cf6);
  color: white;
  font-weight: 800;
  font-size: 1.1rem;
  width: 40px;
  height: 40px;
  border-radius: 8px;
  display: flex;
  align-items: center;
  justify-content: center;
  letter-spacing: -0.5px;
}

.brand-title {
  font-size: 1.25rem;
  font-weight: 700;
  margin: 0;
  line-height: 1.2;
}

.brand-subtitle {
  font-size: 0.8rem;
  color: #9ca3af;
  margin: 0;
}

.header-meta {
  display: flex;
  align-items: center;
  gap: 1rem;
}

.db-pill {
  display: inline-flex;
  align-items: center;
  gap: 0.4rem;
  background: #1f2937;
  color: #34d399;
  border: 1px solid #374151;
  padding: 0.3rem 0.75rem;
  border-radius: 9999px;
  font-size: 0.75rem;
  font-weight: 600;
}

.status-dot {
  width: 7px;
  height: 7px;
  border-radius: 50%;
  background-color: #34d399;
  box-shadow: 0 0 6px #34d399;
}

.session-info {
  display: flex;
  align-items: center;
  gap: 0.5rem;
  background: #1e293b;
  padding: 0.35rem 0.75rem;
  border-radius: 8px;
  border: 1px solid #334155;
  font-size: 0.85rem;
}

.candidate-tag {
  color: #93c5fd;
  font-weight: 600;
}

.arrow-sep {
  color: #64748b;
}

.role-tag {
  color: #f1f5f9;
  font-weight: 500;
  max-width: 220px;
  overflow: hidden;
  text-overflow: ellipsis;
  white-space: nowrap;
}

.btn-change {
  background: #374151;
  color: #e5e7eb;
  border: none;
  padding: 0.25rem 0.6rem;
  border-radius: 4px;
  font-size: 0.75rem;
  cursor: pointer;
  margin-left: 0.5rem;
  transition: background 0.15s;
}

.btn-change:hover {
  background: #4b5563;
  color: #ffffff;
}
</style>
