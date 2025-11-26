<template>
  <button
    :class="[
      'relative overflow-hidden font-bold rounded-base transition-all duration-300 flex items-center justify-center gap-md',
      'disabled:opacity-50 disabled:cursor-not-allowed',
      'active:scale-95',
      sizeClasses,
      variantClasses
    ]"
    :disabled="disabled || loading"
    @click="handleClick"
  >
    <!-- Loading Spinner -->
    <div v-if="loading" class="absolute inset-0 flex items-center justify-center bg-inherit">
      <svg class="animate-spin h-5 w-5" viewBox="0 0 24 24" fill="none">
        <circle class="opacity-25" cx="12" cy="12" r="10" stroke="currentColor" stroke-width="4"></circle>
        <path class="opacity-75" fill="currentColor" d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0 014 12H0c0 3.042 1.135 5.824 3 7.938l3-2.647z"></path>
      </svg>
    </div>

    <!-- Content (hidden when loading) -->
    <span :class="{ 'invisible': loading }">
      <slot name="icon-left"></slot>
      <slot></slot>
      <slot name="icon-right">
        <svg v-if="showArrow" class="w-5 h-5 transform group-hover:translate-x-1 transition-transform" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
          <path stroke-linecap="round" stroke-linejoin="round" d="M17 8l4 4m0 0l-4 4m4-4H3" />
        </svg>
      </slot>
    </span>

    <!-- Ripple Effect -->
    <span v-if="ripple" class="absolute inset-0 overflow-hidden rounded-base">
      <span class="absolute inset-0 ripple-effect"></span>
    </span>
  </button>
</template>

<script setup lang="ts">
import { computed } from 'vue';

interface Props {
  variant?: 'primary' | 'secondary' | 'outline' | 'ghost' | 'danger';
  size?: 'sm' | 'md' | 'lg';
  disabled?: boolean;
  loading?: boolean;
  showArrow?: boolean;
  ripple?: boolean;
  fullWidth?: boolean;
}

const props = withDefaults(defineProps<Props>(), {
  variant: 'primary',
  size: 'md',
  disabled: false,
  loading: false,
  showArrow: false,
  ripple: true,
  fullWidth: false
});

const emit = defineEmits<{
  click: [event: MouseEvent];
}>();

const sizeClasses = computed(() => {
  const sizes = {
    sm: 'px-base py-sm text-sm h-9',
    md: 'px-lg py-md text-base h-11',
    lg: 'px-xl py-base text-lg h-14'
  };
  const width = props.fullWidth ? 'w-full' : '';
  return `${sizes[props.size]} ${width}`;
});

const variantClasses = computed(() => {
  const variants = {
    primary: 'bg-primary text-white hover:bg-primary-hover shadow-sm hover:shadow-md',
    secondary: 'bg-secondary text-white hover:bg-secondary-hover shadow-sm hover:shadow-md',
    outline: 'bg-transparent border-2 border-primary text-primary hover:bg-primary hover:text-white',
    ghost: 'bg-transparent text-gray-700 hover:bg-gray-100',
    danger: 'bg-accent-danger text-white hover:bg-red-600 shadow-sm hover:shadow-md'
  };
  return variants[props.variant];
});

function handleClick(event: MouseEvent) {
  if (!props.disabled && !props.loading) {
    emit('click', event);
  }
}
</script>

<style scoped>
@keyframes ripple {
  0% {
    transform: scale(0);
    opacity: 0.5;
  }
  100% {
    transform: scale(4);
    opacity: 0;
  }
}

.ripple-effect {
  animation: ripple 0.6s ease-out;
}
</style>
