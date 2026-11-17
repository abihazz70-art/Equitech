<script setup lang="ts">
import { ref } from 'vue';
import type { JobDescription, CandidateProfile, InterviewPack } from './types';
import { generateInterviewPack } from './api/interviewApi';
import AppHeader from './components/common/AppHeader.vue';
import JobProfileSelector from './views/JobProfileSelector.vue';
import InterviewPrepDashboard from './views/InterviewPrepDashboard.vue';

const activePack = ref<InterviewPack | null>(null);
const isLoading = ref(false);
const errorMessage = ref<string | null>(null);

const handleGenerate = async (payload: { job: JobDescription; candidate: CandidateProfile }) => {
  isLoading.value = true;
  errorMessage.value = null;

  try {
    activePack.value = await generateInterviewPack(payload.job, payload.candidate);
  } catch (err: any) {
    console.error('Generation error:', err);
    errorMessage.value = err.message || 'Failed to generate interview pack. Please ensure the backend is running.';
  } finally {
    isLoading.value = false;
  }
};

const handleReset = () => {
  activePack.value = null;
  errorMessage.value = null;
};
</script>

<template>
  <div class="app-shell">
    <AppHeader
      :job-title="activePack?.source_basis.job_title"
      :candidate-name="activePack?.source_basis.candidate_name"
      :has-pack="!!activePack"
      @reset="handleReset"
    />

    <!-- Global Loading Overlay -->
    <div v-if="isLoading" class="loading-overlay">
      <div class="spinner-card">
        <div class="spinner"></div>
        <h3 class="spinner-title">AI Agent Analyzing Vacancy & Profile...</h3>
        <p class="spinner-desc">
          Extracting competencies • Mapping candidate evidence • Synthesizing STAR question frameworks • Calculating learning gap milestones
        </p>
      </div>
    </div>

    <!-- Error Banner -->
    <div v-if="errorMessage" class="error-banner">
      <span class="err-icon">⚠️</span>
      <div class="err-content">
        <strong>Error:</strong> {{ errorMessage }}
      </div>
      <button @click="errorMessage = null" class="btn-close-err">✕</button>
    </div>

    <!-- Main View Switch -->
    <main class="main-body">
      <JobProfileSelector
        v-if="!activePack"
        @generate="handleGenerate"
      />
      <InterviewPrepDashboard
        v-else
        :pack="activePack"
        @reset="handleReset"
      />
    </main>

    <footer class="app-footer">
      <div class="footer-inner">
        <span>Equitech Job Search & Career Development Agent • Phase 5: Interview Preparation & Learning</span>
        <span class="footer-sep">|</span>
        <span>Vue 3 + ASP.NET Core 8 Web API + Supabase PostgreSQL</span>
      </div>
    </footer>
  </div>
</template>

<style>
/* Global resets & typography */
body {
  margin: 0;
  font-family: -apple-system, BlinkMacSystemFont, "Segoe UI", Roboto, Helvetica, Arial, sans-serif;
  background-color: #f8fafc;
  color: #0f172a;
  -webkit-font-smoothing: antialiased;
}

* {
  box-sizing: border-box;
}
</style>

<style scoped>
.app-shell {
  min-height: 100vh;
  display: flex;
  flex-direction: column;
}

.main-body {
  flex: 1;
}

.loading-overlay {
  position: fixed;
  inset: 0;
  background: rgba(15, 23, 42, 0.7);
  backdrop-filter: blur(4px);
  display: flex;
  align-items: center;
  justify-content: center;
  z-index: 100;
}

.spinner-card {
  background: #ffffff;
  border-radius: 12px;
  padding: 2rem 2.5rem;
  max-width: 480px;
  text-align: center;
  box-shadow: 0 20px 25px -5px rgba(0, 0, 0, 0.2);
}

.spinner {
  width: 44px;
  height: 44px;
  border: 4px solid #e2e8f0;
  border-top-color: #2563eb;
  border-radius: 50%;
  animation: spin 0.8s linear infinite;
  margin: 0 auto 1.25rem auto;
}

@keyframes spin {
  to { transform: rotate(360deg); }
}

.spinner-title {
  font-size: 1.15rem;
  font-weight: 700;
  color: #0f172a;
  margin: 0 0 0.5rem 0;
}

.spinner-desc {
  font-size: 0.85rem;
  color: #64748b;
  margin: 0;
  line-height: 1.5;
}

.error-banner {
  max-width: 1080px;
  margin: 1rem auto 0 auto;
  padding: 0.75rem 1.25rem;
  background: #fef2f2;
  border: 1px solid #fecaca;
  border-radius: 8px;
  display: flex;
  align-items: center;
  gap: 0.75rem;
  color: #991b1b;
  font-size: 0.875rem;
}

.err-icon {
  font-size: 1.2rem;
}

.err-content {
  flex: 1;
}

.btn-close-err {
  background: transparent;
  border: none;
  font-size: 1rem;
  color: #991b1b;
  cursor: pointer;
}

.app-footer {
  background: #ffffff;
  border-top: 1px solid #e2e8f0;
  padding: 1.25rem 1rem;
  margin-top: 3rem;
  text-align: center;
  font-size: 0.8rem;
  color: #64748b;
}

.footer-inner {
  display: flex;
  justify-content: center;
  align-items: center;
  gap: 0.75rem;
  flex-wrap: wrap;
}

.footer-sep {
  color: #cbd5e1;
}
</style>
