<script setup lang="ts">
import { ref, computed, onUnmounted } from 'vue';

const props = defineProps<{
  initialMinutes: number;
}>();

const totalSeconds = props.initialMinutes * 60;
const remainingSeconds = ref(totalSeconds);
const isRunning = ref(false);
let timerInterval: any = null;

const formattedTime = computed(() => {
  const mins = Math.floor(remainingSeconds.value / 60);
  const secs = remainingSeconds.value % 60;
  return `${mins.toString().padStart(2, '0')}:${secs.toString().padStart(2, '0')}`;
});

const progressPercent = computed(() => {
  return ((totalSeconds - remainingSeconds.value) / totalSeconds) * 100;
});

const isExpired = computed(() => remainingSeconds.value <= 0);

const startTimer = () => {
  if (isRunning.value || remainingSeconds.value <= 0) return;
  isRunning.value = true;
  timerInterval = setInterval(() => {
    if (remainingSeconds.value > 0) {
      remainingSeconds.value--;
    } else {
      pauseTimer();
    }
  }, 1000);
};

const pauseTimer = () => {
  isRunning.value = false;
  if (timerInterval) {
    clearInterval(timerInterval);
    timerInterval = null;
  }
};

const resetTimer = () => {
  pauseTimer();
  remainingSeconds.value = totalSeconds;
};

onUnmounted(() => {
  if (timerInterval) clearInterval(timerInterval);
});
</script>

<template>
  <div class="exercise-timer">
    <div class="timer-display" :class="{ 'timer-urgent': remainingSeconds < 300, 'timer-done': isExpired }">
      <span class="clock-icon">⏱️</span>
      <span class="digits">{{ formattedTime }}</span>
    </div>

    <!-- Progress bar -->
    <div class="progress-track">
      <div class="progress-fill" :style="{ width: `${progressPercent}%` }"></div>
    </div>

    <div class="timer-controls">
      <button v-if="!isRunning" @click="startTimer" class="btn-timer btn-start" :disabled="isExpired">
        {{ remainingSeconds < totalSeconds ? 'Resume' : 'Start Timer' }}
      </button>
      <button v-else @click="pauseTimer" class="btn-timer btn-pause">
        Pause
      </button>
      <button @click="resetTimer" class="btn-timer btn-reset">
        Reset
      </button>
    </div>
  </div>
</template>

<style scoped>
.exercise-timer {
  background: #f8fafc;
  border: 1px solid #e2e8f0;
  border-radius: 8px;
  padding: 0.85rem 1.15rem;
  display: flex;
  flex-direction: column;
  gap: 0.5rem;
}

.timer-display {
  display: flex;
  align-items: center;
  gap: 0.5rem;
  font-family: monospace;
  font-size: 1.5rem;
  font-weight: 700;
  color: #1e293b;
}

.timer-urgent {
  color: #ea580c;
}

.timer-done {
  color: #dc2626;
}

.digits {
  letter-spacing: 0.05em;
}

.progress-track {
  width: 100%;
  height: 6px;
  background: #e2e8f0;
  border-radius: 3px;
  overflow: hidden;
}

.progress-fill {
  height: 100%;
  background: #3b82f6;
  transition: width 1s linear;
}

.timer-controls {
  display: flex;
  gap: 0.4rem;
}

.btn-timer {
  padding: 0.25rem 0.65rem;
  border-radius: 4px;
  font-size: 0.75rem;
  font-weight: 600;
  cursor: pointer;
  border: none;
  transition: opacity 0.15s;
}

.btn-start {
  background: #10b981;
  color: white;
}

.btn-pause {
  background: #f59e0b;
  color: white;
}

.btn-reset {
  background: #64748b;
  color: white;
}

.btn-timer:hover {
  opacity: 0.9;
}

.btn-timer:disabled {
  opacity: 0.4;
  cursor: not-allowed;
}
</style>
