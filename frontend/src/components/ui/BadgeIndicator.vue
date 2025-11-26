<template>
  <div class="inline-block">
    <span 
      :class="[
        'relative inline-flex items-center justify-center',
        sizeClass
      ]"
    >
      <!-- Main Content -->
      <slot></slot>

      <!-- Badge Indicator -->
      <span 
        v-if="showBadge"
        :class="[
          'absolute flex items-center justify-center font-bold text-white rounded-full ring-2 ring-white',
          badgePositionClass,
          badgeColorClass,
          badgeSizeClass
        ]"
      >
        {{ badgeContent }}
      </span>

      <!-- Pulse Animation (for notifications) -->
      <span 
        v-if="pulse"
        class="absolute top-0 right-0 flex h-3 w-3"
      >
        <span class="animate-ping absolute inline-flex h-full w-full rounded-full bg-primary opacity-75"></span>
        <span class="relative inline-flex rounded-full h-3 w-3 bg-primary"></span>
      </span>
    </span>
  </div>
</template>

<script setup lang="ts">
import { computed } from 'vue';

interface Props {
  count?: number;
  max?: number;
  showBadge?: boolean;
  pulse?: boolean;
  size?: 'sm' | 'md' | 'lg';
  position?: 'top-right' | 'top-left' | 'bottom-right' | 'bottom-left';
  color?: 'primary' | 'secondary' | 'success' | 'warning' | 'danger';
}

const props = withDefaults(defineProps<Props>(), {
  count: 0,
  max: 99,
  showBadge: true,
  pulse: false,
  size: 'md',
  position: 'top-right',
  color: 'primary'
});

const badgeContent = computed(() => {
  if (props.count === 0) return '';
  return props.count > props.max ? `${props.max}+` : props.count.toString();
});

const sizeClass = computed(() => {
  const sizes = {
    sm: 'w-8 h-8',
    md: 'w-10 h-10',
    lg: 'w-12 h-12'
  };
  return sizes[props.size];
});

const badgeSizeClass = computed(() => {
  const sizes = {
    sm: 'min-w-[16px] h-4 text-xs px-1',
    md: 'min-w-[20px] h-5 text-xs px-1',
    lg: 'min-w-[24px] h-6 text-sm px-1.5'
  };
  return sizes[props.size];
});

const badgePositionClass = computed(() => {
  const positions = {
    'top-right': '-top-1 -right-1',
    'top-left': '-top-1 -left-1',
    'bottom-right': '-bottom-1 -right-1',
    'bottom-left': '-bottom-1 -left-1'
  };
  return positions[props.position];
});

const badgeColorClass = computed(() => {
  const colors = {
    primary: 'bg-primary',
    secondary: 'bg-secondary',
    success: 'bg-accent-success',
    warning: 'bg-accent-warning',
    danger: 'bg-accent-danger'
  };
  return colors[props.color];
});
</script>

<style scoped>
/* No custom CSS needed */
</style>
