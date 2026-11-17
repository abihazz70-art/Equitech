<script setup lang="ts">
import { ref } from 'vue';
import type { JobDescription, CandidateProfile } from '../types';
import { sampleJobs, sampleCandidates } from '../data/sampleData';

const emit = defineEmits<{
  (e: 'generate', payload: { job: JobDescription; candidate: CandidateProfile }): void;
}>();

const selectedRoleIndex = ref(2); // Default to Software Developer (.NET & Vue 3)
const customMode = ref(false);

const customJobTitle = ref('');
const customEmployer = ref('');
const customJobDesc = ref('');
const customCandidateName = ref('');
const customCandidateHeadline = ref('');
const customCandidateSummary = ref('');

const selectPreset = (index: number) => {
  selectedRoleIndex.value = index;
  customMode.value = false;
};

const handleGeneratePreset = () => {
  const job = sampleJobs[selectedRoleIndex.value];
  const candidate = sampleCandidates[selectedRoleIndex.value];
  emit('generate', { job, candidate });
};

const handleGenerateCustom = () => {
  const job: JobDescription = {
    title: customJobTitle.value || 'Custom Role',
    employer: customEmployer.value || 'Custom Employer',
    location: 'Remote',
    description: customJobDesc.value
  };

  const candidate: CandidateProfile = {
    full_name: customCandidateName.value || 'Custom Candidate',
    headline: customCandidateHeadline.value || 'Professional',
    location: 'India',
    summary: customCandidateSummary.value,
    employment_history: [],
    education: [],
    skills: [],
    certifications: [],
    achievements: []
  };

  emit('generate', { job, candidate });
};
</script>

<template>
  <div class="selector-container">
    <div class="hero-card">
      <h2 class="hero-title">Job Search & Career Development Agent</h2>
      <p class="hero-subtitle">
        Interview Preparation & Learning Module • AI-powered competency extraction, gap diagnostic, and tailored preparation pack.
      </p>

      <div class="ground-rules-banner">
        <span class="ground-icon">⚖️</span>
        <div class="ground-text">
          <strong>Ground Rules Enforced:</strong> Strictly synthetic test data • Evidence-grounded (no fabricated candidate skills) • Practice questions are AI predictions, never confidential employer materials.
        </div>
      </div>
    </div>

    <!-- Presets Selector -->
    <div class="setup-card">
      <div class="mode-toggle-row">
        <h3 class="setup-heading">Select Sample Candidate & Job Pair</h3>
        <button 
          @click="customMode = !customMode" 
          class="btn-toggle-mode"
        >
          {{ customMode ? 'Back to Sample Presets' : 'Custom Input Mode' }}
        </button>
      </div>

      <!-- Preset Cards Grid -->
      <div v-if="!customMode" class="presets-grid">
        <div
          v-for="(job, idx) in sampleJobs"
          :key="job.source_job_id"
          :class="['preset-card', { selected: selectedRoleIndex === idx }]"
          @click="selectPreset(idx)"
        >
          <div class="preset-badge">Role {{ idx + 1 }}</div>
          <h4 class="preset-title">{{ job.title }}</h4>
          <span class="preset-employer">{{ job.employer }} • {{ job.location }}</span>

          <div class="candidate-preview">
            <span class="cand-label">Candidate:</span>
            <span class="cand-name">{{ sampleCandidates[idx].full_name }}</span>
            <span class="cand-headline">{{ sampleCandidates[idx].headline }}</span>
          </div>

          <div class="preset-check" v-if="selectedRoleIndex === idx">✓ Selected</div>
        </div>
      </div>

      <!-- Custom Form Mode -->
      <div v-else class="custom-form-grid">
        <div class="form-col">
          <h4 class="form-subheading">Job Description Details</h4>
          <div class="input-group">
            <label>Job Title</label>
            <input v-model="customJobTitle" placeholder="e.g. Senior Data Engineer" />
          </div>
          <div class="input-group">
            <label>Employer</label>
            <input v-model="customEmployer" placeholder="e.g. Acme Corp" />
          </div>
          <div class="input-group">
            <label>Job Description & Requirements</label>
            <textarea v-model="customJobDesc" rows="6" placeholder="Paste full JD text here..."></textarea>
          </div>
        </div>

        <div class="form-col">
          <h4 class="form-subheading">Candidate Profile Details</h4>
          <div class="input-group">
            <label>Candidate Full Name</label>
            <input v-model="customCandidateName" placeholder="e.g. Priya Nair" />
          </div>
          <div class="input-group">
            <label>Headline</label>
            <input v-model="customCandidateHeadline" placeholder="e.g. Python Developer" />
          </div>
          <div class="input-group">
            <label>Experience & Skills Summary</label>
            <textarea v-model="customCandidateSummary" rows="6" placeholder="Summarize candidate's verified background and skills..."></textarea>
          </div>
        </div>
      </div>

      <!-- Action Button -->
      <div class="generate-action-bar">
        <button 
          v-if="!customMode" 
          @click="handleGeneratePreset" 
          class="btn-generate"
        >
          Generate Interview Pack for {{ sampleCandidates[selectedRoleIndex].full_name }}
        </button>
        <button 
          v-else 
          @click="handleGenerateCustom" 
          class="btn-generate" 
          :disabled="!customJobDesc.trim() || !customCandidateSummary.trim()"
        >
          Generate Pack from Custom Data
        </button>
      </div>
    </div>
  </div>
</template>

<style scoped>
.selector-container {
  max-width: 1080px;
  margin: 2rem auto;
  padding: 0 1rem;
}

.hero-card {
  text-align: center;
  margin-bottom: 2rem;
}

.hero-title {
  font-size: 2rem;
  font-weight: 800;
  color: #0f172a;
  margin: 0 0 0.5rem 0;
  letter-spacing: -0.02em;
}

.hero-subtitle {
  font-size: 1rem;
  color: #64748b;
  max-width: 700px;
  margin: 0 auto 1.5rem auto;
  line-height: 1.5;
}

.ground-rules-banner {
  display: inline-flex;
  align-items: center;
  gap: 0.75rem;
  background: #f8fafc;
  border: 1px solid #e2e8f0;
  border-radius: 8px;
  padding: 0.65rem 1.25rem;
  font-size: 0.8rem;
  color: #334155;
  text-align: left;
}

.ground-icon {
  font-size: 1.25rem;
}

.setup-card {
  background: #ffffff;
  border: 1px solid #e2e8f0;
  border-radius: 12px;
  padding: 1.75rem;
  box-shadow: 0 4px 6px -1px rgba(0, 0, 0, 0.05);
}

.mode-toggle-row {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 1.25rem;
}

.setup-heading {
  font-size: 1.15rem;
  font-weight: 700;
  color: #0f172a;
  margin: 0;
}

.btn-toggle-mode {
  background: #f1f5f9;
  border: 1px solid #cbd5e1;
  padding: 0.35rem 0.85rem;
  border-radius: 6px;
  font-size: 0.8rem;
  font-weight: 600;
  color: #3b82f6;
  cursor: pointer;
}

.presets-grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(280px, 1fr));
  gap: 1.25rem;
  margin-bottom: 2rem;
}

.preset-card {
  background: #f8fafc;
  border: 2px solid #e2e8f0;
  border-radius: 10px;
  padding: 1.25rem;
  cursor: pointer;
  position: relative;
  transition: all 0.2s ease;
}

.preset-card:hover {
  border-color: #93c5fd;
  transform: translateY(-2px);
}

.preset-card.selected {
  border-color: #2563eb;
  background: #eff6ff;
  box-shadow: 0 4px 12px rgba(37, 99, 235, 0.1);
}

.preset-badge {
  display: inline-block;
  font-size: 0.7rem;
  font-weight: 700;
  background: #e2e8f0;
  color: #475569;
  padding: 0.15rem 0.45rem;
  border-radius: 4px;
  margin-bottom: 0.5rem;
}

.preset-card.selected .preset-badge {
  background: #dbeafe;
  color: #1e40af;
}

.preset-title {
  font-size: 1.05rem;
  font-weight: 700;
  color: #0f172a;
  margin: 0 0 0.25rem 0;
  line-height: 1.35;
}

.preset-employer {
  font-size: 0.8rem;
  color: #64748b;
  display: block;
  margin-bottom: 1rem;
}

.candidate-preview {
  border-top: 1px dashed #cbd5e1;
  padding-top: 0.75rem;
  display: flex;
  flex-direction: column;
  gap: 0.15rem;
}

.cand-label {
  font-size: 0.7rem;
  text-transform: uppercase;
  color: #64748b;
  font-weight: 700;
}

.cand-name {
  font-size: 0.95rem;
  font-weight: 600;
  color: #1e293b;
}

.cand-headline {
  font-size: 0.8rem;
  color: #475569;
}

.preset-check {
  margin-top: 0.75rem;
  font-size: 0.8rem;
  font-weight: 700;
  color: #2563eb;
}

.custom-form-grid {
  display: grid;
  grid-template-columns: 1fr 1fr;
  gap: 1.5rem;
  margin-bottom: 1.5rem;
}

@media (max-width: 640px) {
  .custom-form-grid {
    grid-template-columns: 1fr;
  }
}

.form-subheading {
  font-size: 0.95rem;
  font-weight: 700;
  color: #1e293b;
  margin: 0 0 0.75rem 0;
}

.input-group {
  margin-bottom: 0.85rem;
}

.input-group label {
  display: block;
  font-size: 0.8rem;
  font-weight: 600;
  color: #475569;
  margin-bottom: 0.25rem;
}

.input-group input,
.input-group textarea {
  width: 100%;
  box-sizing: border-box;
  padding: 0.6rem 0.75rem;
  border-radius: 6px;
  border: 1px solid #cbd5e1;
  font-size: 0.875rem;
}

.generate-action-bar {
  text-align: center;
  border-top: 1px solid #f1f5f9;
  padding-top: 1.5rem;
}

.btn-generate {
  background: linear-gradient(135deg, #2563eb, #4f46e5);
  color: white;
  font-size: 1rem;
  font-weight: 700;
  padding: 0.85rem 2.25rem;
  border-radius: 8px;
  border: none;
  cursor: pointer;
  box-shadow: 0 4px 6px -1px rgba(37, 99, 235, 0.25);
  transition: transform 0.15s, box-shadow 0.15s;
}

.btn-generate:hover:not(:disabled) {
  transform: translateY(-2px);
  box-shadow: 0 6px 12px -2px rgba(37, 99, 235, 0.35);
}

.btn-generate:disabled {
  opacity: 0.5;
  cursor: not-allowed;
}
</style>
