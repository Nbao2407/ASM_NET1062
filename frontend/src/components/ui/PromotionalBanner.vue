<template>
  <div 
    :class="[
      'relative overflow-hidden rounded-lg p-lg flex items-center gap-lg animate-slide-down',
      typeClasses
    ]"
    role="alert"
  >
    <!-- Icon -->
    <div class="flex-shrink-0">
      <svg v-if="type === 'limited'" class="w-6 h-6" viewBox="0 0 24 24" fill="currentColor">
        <path d="M13 10V3L4 14h7v7l9-11h-7z" />
      </svg>
      <svg v-else-if="type === 'flash'" class="w-6 h-6" viewBox="0 0 24 24" fill="currentColor">
        <path d="M12 2L2 7l10 5 10-5-10-5zM2 17l10 5 10-5M2 12l10 5 10-5" />
      </svg>
      <svg v-else-if="type === 'combo'" class="w-6 h-6" viewBox="0 0 24 24" fill="currentColor">
        <path d="M12 2a10 10 0 100 20 10 10 0 000-20zm0 18a8 8 0 110-16 8 8 0 010 16zm-1-13h2v6h-2zm0 8h2v2h-2z" />
      </svg>
      <svg v-else class="w-6 h-6" viewBox="0 0 24 24" fill="currentColor">
        <path d="M13 2.05v2.02c3.95.49 7 3.85 7 7.93 0 4.08-3.05 7.44-7 7.93v2.02c5.05-.5 9-4.76 9-9.95 0-5.19-3.95-9.45-9-9.95zM12 19c-3.87 0-7-3.13-7-7s3.13-7 7-7v14z" />
      </svg>
    </div>

    <!-- Content -->
    <div class="flex-1 min-w-0">
      <h3 class="font-heading font-bold text-lg mb-xs">
        <slot name="title">{{ title }}</slot>
      </h3>
      <p class="text-sm opacity-90">
        <slot name="description">{{ description }}</slot>
      </p>
    </div>

    <!-- Countdown Timer (Optional) -->
    <div v-if="showCountdown && countdown" class="text-center flex-shrink-0 bg-white/20 backdrop-blur-sm px-lg py-md rounded-lg">
      <div class="font-heading font-bold text-2xl">{{ countdown }}</div>
      <div class="text-xs opacity-80">Remaining</div>
    </div>

    <!-- Animated Background Pattern -->
    <div class="absolute inset-0 opacity-10 pointer-events-none">
      <div class="absolute inset-0 bg-[url('/pattern.png')]"></div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { computed } from 'vue';

interface Props {
  type?: 'limited' | 'flash' | 'combo' | 'new';
  title?: string;
  description?: string;
  countdown?: string;
  showCountdown?: boolean;
}

const props = withDefaults(defineProps<Props>(), {
  type: 'limited',
  showCountdown: false
});

const typeClasses = computed(() => {
  const classes = {
    limited: 'bg-gradient-to-r from-accent-warning to-accent-warning/80 text-white shadow-lg',
    flash: 'bg-gradient-to-r from-primary to-primary-dark text-white shadow-lg shadow-primary/30',
    combo: 'bg-gradient-to-r from-secondary to-secondary-dark text-white shadow-lg',
    new: 'bg-gradient-to-r from-accent-info to-blue-600 text-white shadow-lg'
  };
  return classes[props.type];
});
</script>

<style scoped>
/* Subtle pulse animation for promotional banners */
@keyframes pulse-glow {
  0%, 100% {
    box-shadow: 0 0 20px rgba(225, 29, 72, 0.3);
  }
  50% {
    box-shadow: 0 0 30px rgba(225, 29, 72, 0.5);
  }
}

div[role="alert"] {
  animation: pulse-glow 3s ease-in-out infinite;
}
</style>
