<template>
  <div class="search-bar">
    <div class="search-container">
      <!-- Basic Search -->
      <div class="basic-search">
        <input
          v-model="searchQuery"
          type="text"
          placeholder="Search for food items..."
          @input="handleBasicSearch"
          class="search-input"
        />
        <button @click="handleSearch" class="btn-search">
          🔍 Search
        </button>
        <button @click="toggleAdvanced" class="btn-toggle">
          {{ showAdvanced ? 'Simple' : 'Advanced' }}
        </button>
      </div>

      <!-- Advanced Search -->
      <transition name="slide">
        <div v-if="showAdvanced" class="advanced-search">
          <div class="search-grid">
            <div class="search-field">
              <label>Category</label>
              <select v-model="advancedFilters.category">
                <option value="">All Categories</option>
                <option v-for="cat in categories" :key="cat" :value="cat">
                  {{ cat }}
                </option>
              </select>
            </div>

            <div class="search-field">
              <label>Min Price</label>
              <input
                v-model.number="advancedFilters.minPrice"
                type="number"
                min="0"
                step="0.01"
                placeholder="$0.00"
              />
            </div>

            <div class="search-field">
              <label>Max Price</label>
              <input
                v-model.number="advancedFilters.maxPrice"
                type="number"
                min="0"
                step="0.01"
                placeholder="$999.99"
              />
            </div>

            <div class="search-field">
              <label>Theme</label>
              <input
                v-model="advancedFilters.theme"
                type="text"
                placeholder="e.g., Spicy, Vegetarian"
              />
            </div>
          </div>

          <div class="search-actions">
            <button @click="handleAdvancedSearch" class="btn-search-advanced">
              Apply Filters
            </button>
            <button @click="clearFilters" class="btn-clear">
              Clear All
            </button>
          </div>
        </div>
      </transition>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, watch } from 'vue';
import type { SearchParams } from '@/services/foodItemService';

interface Props {
  categories?: string[];
}

withDefaults(defineProps<Props>(), {
  categories: () => [],
});

const emit = defineEmits<{
  search: [params: SearchParams];
  clear: [];
}>();

const searchQuery = ref('');
const showAdvanced = ref(false);
const advancedFilters = ref<SearchParams>({
  name: '',
  category: '',
  theme: '',
  minPrice: undefined,
  maxPrice: undefined,
});

// Debounce timer for basic search
let debounceTimer: ReturnType<typeof setTimeout> | null = null;

const toggleAdvanced = () => {
  showAdvanced.value = !showAdvanced.value;
};

const handleBasicSearch = () => {
  // Clear any existing timer
  if (debounceTimer) {
    clearTimeout(debounceTimer);
  }

  // Set a new timer for debounced search
  debounceTimer = setTimeout(() => {
    if (searchQuery.value.trim()) {
      emit('search', { name: searchQuery.value.trim() });
    } else {
      emit('clear');
    }
  }, 300);
};

const handleSearch = () => {
  if (searchQuery.value.trim()) {
    emit('search', { name: searchQuery.value.trim() });
  } else {
    emit('clear');
  }
};

const handleAdvancedSearch = () => {
  const params: SearchParams = {
    name: searchQuery.value.trim() || undefined,
    category: advancedFilters.value.category || undefined,
    theme: advancedFilters.value.theme?.trim() || undefined,
    minPrice: advancedFilters.value.minPrice,
    maxPrice: advancedFilters.value.maxPrice,
  };

  // Remove undefined values
  const cleanParams: SearchParams = {};
  for (const [key, value] of Object.entries(params)) {
    if (value !== undefined && value !== '') {
      const paramKey = key as keyof SearchParams;
      if (paramKey === 'minPrice' || paramKey === 'maxPrice') {
        cleanParams[paramKey] = value as number;
      } else {
        cleanParams[paramKey] = value as string;
      }
    }
  }

  emit('search', cleanParams);
};

const clearFilters = () => {
  searchQuery.value = '';
  advancedFilters.value = {
    name: '',
    category: '',
    theme: '',
    minPrice: undefined,
    maxPrice: undefined,
  };
  emit('clear');
};

// Watch for changes in advanced filters
watch(
  () => advancedFilters.value,
  () => {
    // Sync the name field with basic search
    if (advancedFilters.value.name !== searchQuery.value) {
      advancedFilters.value.name = searchQuery.value;
    }
  },
  { deep: true }
);
</script>

<style scoped>
.search-bar {
  width: 100%;
  margin-bottom: 2rem;
}

.search-container {
  background: white;
  border-radius: 8px;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);
  padding: 1.5rem;
}

.basic-search {
  display: flex;
  gap: 0.5rem;
}

.search-input {
  flex: 1;
  padding: 0.75rem 1rem;
  border: 2px solid #e0e0e0;
  border-radius: 4px;
  font-size: 1rem;
  transition: border-color 0.2s;
}

.search-input:focus {
  outline: none;
  border-color: #42b983;
}

.btn-search,
.btn-toggle {
  padding: 0.75rem 1.5rem;
  border: none;
  border-radius: 4px;
  cursor: pointer;
  font-weight: 600;
  transition: all 0.2s;
}

.btn-search {
  background: #42b983;
  color: white;
}

.btn-search:hover {
  background: #359268;
}

.btn-toggle {
  background: #f5f5f5;
  color: #2c3e50;
}

.btn-toggle:hover {
  background: #e0e0e0;
}

.advanced-search {
  margin-top: 1.5rem;
  padding-top: 1.5rem;
  border-top: 2px solid #f5f5f5;
}

.search-grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(200px, 1fr));
  gap: 1rem;
  margin-bottom: 1rem;
}

.search-field {
  display: flex;
  flex-direction: column;
  gap: 0.5rem;
}

.search-field label {
  font-size: 0.875rem;
  font-weight: 600;
  color: #666;
}

.search-field input,
.search-field select {
  padding: 0.5rem;
  border: 2px solid #e0e0e0;
  border-radius: 4px;
  font-size: 0.875rem;
  transition: border-color 0.2s;
}

.search-field input:focus,
.search-field select:focus {
  outline: none;
  border-color: #42b983;
}

.search-actions {
  display: flex;
  gap: 0.5rem;
  justify-content: flex-end;
}

.btn-search-advanced,
.btn-clear {
  padding: 0.5rem 1.5rem;
  border: none;
  border-radius: 4px;
  cursor: pointer;
  font-weight: 600;
  transition: all 0.2s;
}

.btn-search-advanced {
  background: #42b983;
  color: white;
}

.btn-search-advanced:hover {
  background: #359268;
}

.btn-clear {
  background: #e0e0e0;
  color: #666;
}

.btn-clear:hover {
  background: #d0d0d0;
}

/* Transition animations */
.slide-enter-active,
.slide-leave-active {
  transition: all 0.3s ease;
  max-height: 500px;
  overflow: hidden;
}

.slide-enter-from,
.slide-leave-to {
  max-height: 0;
  opacity: 0;
}

@media (max-width: 768px) {
  .basic-search {
    flex-direction: column;
  }

  .search-grid {
    grid-template-columns: 1fr;
  }

  .search-actions {
    flex-direction: column;
  }

  .btn-search-advanced,
  .btn-clear {
    width: 100%;
  }
}
</style>
