<template>
  <div class="fixed bottom-lg right-lg z-90 max-w-sm">
    <transition-group name="toast" tag="div" class="space-y-md">
      <div
        v-for="toast in toasts"
        :key="toast.id"
        :class="[
          'flex items-start gap-lg p-lg rounded-lg shadow-lg border backdrop-blur-sm animate-slide-up',
          toastClasses(toast.type)
        ]"
      >
        <!-- Icon -->
        <div class="flex-shrink-0 mt-xs">
          <svg v-if="toast.type === 'success'" class="w-5 h-5" viewBox="0 0 24 24" fill="currentColor">
            <path d="M9 12l2 2 4-4m6 2a9 9 0 11-18 0 9 9 0 0118 0z" />
          </svg>
          <svg v-else-if="toast.type === 'error'" class="w-5 h-5" viewBox="0 0 24 24" fill="currentColor">
            <path d="M10 14l2-2m0 0l2-2m-2 2l-2-2m2 2l2 2m7-2a9 9 0 11-18 0 9 9 0 0118 0z" />
          </svg>
          <svg v-else-if="toast.type === 'warning'" class="w-5 h-5" viewBox="0 0 24 24" fill="currentColor">
            <path d="M12 9v2m0 4h.01m-6.938 4h13.856c1.54 0 2.502-1.667 1.732-3L13.732 4c-.77-1.333-2.694-1.333-3.464 0L3.34 16c-.77 1.333.192 3 1.732 3z" />
          </svg>
          <svg v-else class="w-5 h-5" viewBox="0 0 24 24" fill="currentColor">
            <path d="M13 16h-1v-4h-1m1-4h.01M21 12a9 9 0 11-18 0 9 9 0 0118 0z" />
          </svg>
        </div>

        <!-- Content -->
        <div class="flex-1 min-w-0">
          <h4 class="font-bold text-sm mb-xs">{{ toast.title }}</h4>
          <p class="text-sm opacity-90">{{ toast.message }}</p>
        </div>

        <!-- Close Button -->
        <button
          @click="removeToast(toast.id)"
          class="flex-shrink-0 hover:opacity-70 transition-opacity"
          aria-label="Close notification"
        >
          <svg class="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24" stroke-width="2">
            <path stroke-linecap="round" stroke-linejoin="round" d="M6 18L18 6M6 6l12 12" />
          </svg>
        </button>
      </div>
    </transition-group>
  </div>
</template>

<script setup lang="ts">
import { ref } from 'vue';

interface Toast {
  id: number;
  type: 'success' | 'error' | 'warning' | 'info';
  title: string;
  message: string;
}

const toasts = ref<Toast[]>([]);
let toastId = 0;

function toastClasses(type: Toast['type']) {
  const classes = {
    success: 'bg-accent-success/90 text-white border-accent-success',
    error: 'bg-accent-danger/90 text-white border-accent-danger',
    warning: 'bg-accent-warning/90 text-white border-accent-warning',
    info: 'bg-accent-info/90 text-white border-accent-info'
  };
  return classes[type];
}

function addToast(toast: Omit<Toast, 'id'>) {
  const id = toastId++;
  toasts.value.push({ ...toast, id });
  
  setTimeout(() => {
    removeToast(id);
  }, 5000);
}

function removeToast(id: number) {
  const index = toasts.value.findIndex(t => t.id === id);
  if (index > -1) {
    toasts.value.splice(index, 1);
  }
}

defineExpose({
  addToast
});
</script>

<style scoped>
.toast-enter-active,
.toast-leave-active {
  transition: all 0.3s ease;
}

.toast-enter-from {
  transform: translateX(100%);
  opacity: 0;
}

.toast-leave-to {
  transform: translateX(100%);
  opacity: 0;
}
</style>
