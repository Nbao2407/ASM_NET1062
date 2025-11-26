<template>
  <div class="search-bar">
    <div class="bg-white rounded-xl shadow-sm border border-gray-200 p-4">
      <!-- Main Search Row -->
      <div class="flex gap-3 items-center">
        <!-- Search Input -->
        <div class="flex-1 relative">
          <svg class="absolute left-3 top-1/2 -translate-y-1/2 w-5 h-5 text-gray-400" fill="none" stroke="currentColor" viewBox="0 0 24 24">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M21 21l-6-6m2-5a7 7 0 11-14 0 7 7 0 0114 0z" />
          </svg>
          <input
            v-model="searchQuery"
            type="text"
            placeholder="Tìm kiếm món ăn..."
            class="w-full pl-10 pr-4 py-2.5 border border-gray-300 rounded-lg focus:outline-none focus:ring-2 focus:ring-red-500 focus:border-transparent"
            @input="handleSearch"
          />
        </div>

        <!-- Sort Dropdown -->
        <select
          v-model="sortBy"
          class="px-4 py-2.5 border border-gray-300 rounded-lg focus:outline-none focus:ring-2 focus:ring-red-500 bg-white min-w-[150px]"
          @change="handleSearch"
        >
          <option value="">Mặc định</option>
          <option value="name-asc">Tên A-Z</option>
          <option value="name-desc">Tên Z-A</option>
          <option value="price-asc">Giá thấp - cao</option>
          <option value="price-desc">Giá cao - thấp</option>
        </select>

        <!-- Filter Button -->
        <button
          @click="toggleAdvanced"
          :class="[
            'px-5 py-2.5 rounded-lg font-medium transition-all duration-200 flex items-center gap-2 whitespace-nowrap',
            showAdvanced 
              ? 'bg-red-600 text-white' 
              : 'bg-red-500 text-white hover:bg-red-600'
          ]"
        >
          <svg class="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M3 4a1 1 0 011-1h16a1 1 0 011 1v2.586a1 1 0 01-.293.707l-6.414 6.414a1 1 0 00-.293.707V17l-4 4v-6.586a1 1 0 00-.293-.707L3.293 7.293A1 1 0 013 6.586V4z" />
          </svg>
          Bộ lọc
        </button>
      </div>

      <!-- Advanced Filters -->
      <div v-if="showAdvanced" class="mt-4 pt-4 border-t border-gray-200 animate-slide-down">
        <div class="grid grid-cols-1 md:grid-cols-3 gap-4">
          <!-- Category -->
          <div>
            <label class="block text-sm font-medium text-gray-700 mb-2">Danh mục</label>
            <select
              v-model="filters.category"
              class="w-full px-3 py-2 border border-gray-300 rounded-lg focus:outline-none focus:ring-2 focus:ring-red-500"
              @change="handleSearch"
            >
              <option value="">Tất cả danh mục</option>
              <option v-for="cat in categories" :key="cat" :value="cat">{{ cat }}</option>
            </select>
          </div>

          <!-- Min Price -->
          <div>
            <label class="block text-sm font-medium text-gray-700 mb-2">Giá tối thiểu</label>
            <input
              v-model.number="filters.minPrice"
              type="number"
              placeholder="0đ"
              class="w-full px-3 py-2 border border-gray-300 rounded-lg focus:outline-none focus:ring-2 focus:ring-red-500"
              @input="handleSearch"
            />
          </div>

          <!-- Max Price -->
          <div>
            <label class="block text-sm font-medium text-gray-700 mb-2">Giá tối đa</label>
            <input
              v-model.number="filters.maxPrice"
              type="number"
              placeholder="1,000,000đ"
              class="w-full px-3 py-2 border border-gray-300 rounded-lg focus:outline-none focus:ring-2 focus:ring-red-500"
              @input="handleSearch"
            />
          </div>
        </div>

        <!-- Clear Button -->
        <div class="flex justify-end mt-4">
          <button
            @click="clearFilters"
            class="px-4 py-2 text-gray-600 hover:text-gray-800 font-medium transition-colors"
          >
            Xóa bộ lọc
          </button>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, reactive } from 'vue'

defineProps({
  categories: {
    type: Array,
    default: () => []
  }
})

const emit = defineEmits(['search', 'clear'])

const searchQuery = ref('')
const sortBy = ref('')
const showAdvanced = ref(false)
const filters = reactive({
  category: '',
  minPrice: null,
  maxPrice: null
})

function toggleAdvanced() {
  showAdvanced.value = !showAdvanced.value
}

function handleSearch() {
  const searchParams = {
    name: searchQuery.value,
    sortBy: sortBy.value,
    ...filters
  }
  
  // Remove empty values
  Object.keys(searchParams).forEach(key => {
    if (!searchParams[key] && searchParams[key] !== 0) {
      delete searchParams[key]
    }
  })
  
  emit('search', searchParams)
}

function clearFilters() {
  searchQuery.value = ''
  sortBy.value = ''
  filters.category = ''
  filters.minPrice = null
  filters.maxPrice = null
  emit('clear')
}
</script>

<style scoped>
/* No custom CSS needed */
</style>
