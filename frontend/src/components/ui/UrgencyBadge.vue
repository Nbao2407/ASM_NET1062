<template>
  <div 
    :class="[
      'relative inline-block px-lg py-sm rounded-full font-bold text-sm cursor-default select-none',
      'animate-bounce-subtle',
      colorClass
    ]"
  >
    <div class="flex items-center gap-sm">
      <!-- Icon -->
      <svg class="w-4 h-4" viewBox="0 0 24 24" fill="currentColor">
        <path d="M13 10V3L4 14h7v7l9-11h-7z" />
      </svg>
      
      <!-- Text -->
      <span>{{ text }}</span>
      
      <!-- Optional Timer -->
      <span v-if="showTimer && timeRemaining" class="ml-sm font-mono">
        {{ timeRemaining }}
      </span>
    </div>
    
    <!-- Glow Effect -->
    <div class="absolute inset-0 rounded-full blur-md opacity-50" :class="glowClass"></div>
  </div>
</template>

<script setup lang="ts">
import { computed } from 'vue';

interface Props {
  variant?: 'hot' | 'new' | 'limited' | 'sale';
  text?: string;
  showTimer?: boolean;
  timeRemaining?: string;
}

const props = withDefaults(defineProps<Props>(), {
  variant: 'hot',
  showTimer: false
});

const colorClass = computed(() => {
  const variants = {
    hot: 'bg-gradient-to-r from-accent-warning to-red-500 text-white shadow-lg',
    new: 'bg-gradient-to-r from-accent-info to-blue-600 text-white shadow-lg',
    limited: 'bg-gradient-to-r from-primary to-primary-dark text-white shadow-lg',
    sale: 'bg-gradient-to-r from-secondary to-secondary-dark text-white shadow-lg'
  };
  return variants[props.variant];
});

const glowClass = computed(() => {
  const variants = {
    hot: 'bg-accent-warning',
    new: 'bg-accent-info',
    limited: 'bg-primary',
    sale: 'bg-secondary'
  };
  return variants[props.variant];
});
</script>

<style scoped>
/* No custom CSS needed */
</style>
