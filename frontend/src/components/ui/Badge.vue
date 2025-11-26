<template>
  <span 
    :class="[
      'inline-flex items-center gap-xs font-bold text-xs px-md py-xs rounded-full transition-all duration-200',
      variantClasses,
      sizeClasses
    ]"
  >
    <slot name="icon"></slot>
    <slot></slot>
  </span>
</template>

<script setup lang="ts">
import { computed } from 'vue';

interface Props {
  variant?: 'primary' | 'secondary' | 'success' | 'warning' | 'danger' | 'info' | 'neutral';
  size?: 'sm' | 'md' | 'lg';
}

const props = withDefaults(defineProps<Props>(), {
  variant: 'neutral',
  size: 'md'
});

const variantClasses = computed(() => {
  const variants = {
    primary: 'bg-primary text-white shadow-sm',
    secondary: 'bg-secondary text-white shadow-sm',
    success: 'bg-accent-success text-white shadow-sm',
    warning: 'bg-accent-warning text-white shadow-sm',
    danger: 'bg-accent-danger text-white shadow-sm',
    info: 'bg-accent-info text-white shadow-sm',
    neutral: 'bg-gray-100 text-gray-700 border border-gray-200'
  };
  return variants[props.variant];
});

const sizeClasses = computed(() => {
  const sizes = {
    sm: 'text-xs px-sm py-xs',
    md: 'text-xs px-md py-xs',
    lg: 'text-sm px-base py-sm'
  };
  return sizes[props.size];
});
</script>

<style scoped>
/* No custom CSS needed */
</style>
