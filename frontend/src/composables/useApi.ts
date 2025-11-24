import { ref } from 'vue';
import type { AxiosError } from 'axios';
import type { ApiError } from '@/types';

/**
 * Composable for handling API calls with loading and error states
 */
export function useApi<T>() {
  const data = ref<T | null>(null);
  const loading = ref(false);
  const error = ref<string | null>(null);
  const errors = ref<Record<string, string[]> | null>(null);

  const execute = async (apiCall: () => Promise<T>): Promise<T | null> => {
    loading.value = true;
    error.value = null;
    errors.value = null;

    try {
      const result = await apiCall();
      data.value = result;
      return result;
    } catch (err) {
      const axiosError = err as AxiosError<ApiError>;
      
      if (axiosError.response?.data) {
        error.value = axiosError.response.data.message || 'An error occurred';
        errors.value = axiosError.response.data.errors || null;
      } else {
        error.value = 'Network error - please check your connection';
      }
      
      return null;
    } finally {
      loading.value = false;
    }
  };

  const reset = () => {
    data.value = null;
    loading.value = false;
    error.value = null;
    errors.value = null;
  };

  return {
    data,
    loading,
    error,
    errors,
    execute,
    reset,
  };
}
