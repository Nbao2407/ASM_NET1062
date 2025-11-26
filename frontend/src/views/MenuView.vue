<script setup lang="ts">
import { ref, computed, onMounted } from 'vue';
import { useRouter } from 'vue-router';
import { foodItemService, type FoodItem, type SearchParams } from '@/services/foodItemService';
import { comboService, type Combo } from '@/services/comboService';
import { useCartStore } from '@/stores/cart';
import FoodItemCard from '@/components/FoodItemCard.vue';
import ComboCard from '@/components/ComboCard.vue';
import SearchBar from '@/components/SearchBar.vue';

const router = useRouter();
const cartStore = useCartStore();

const foodItems = ref<FoodItem[]>([]);
const combos = ref<Combo[]>([]);
const loading = ref(true);
const error = ref<string | null>(null);
const searchParams = ref<SearchParams>({});
const selectedCategory = ref<string | null>(null);

const categories = computed(() => {
  const cats = new Set(foodItems.value.map(item => item.category).filter(Boolean));
  return Array.from(cats).sort();
});

const filteredCombos = computed(() => {
  if (selectedCategory.value) return [];

  let result = combos.value;
  
  // Filter by name from search params
  if (searchParams.value.name) {
    const query = searchParams.value.name.toLowerCase();
    result = result.filter(c => c.name.toLowerCase().includes(query) || c.description?.toLowerCase().includes(query));
  }

  if (searchParams.value.category) {
    return [];
  }

  return result;
});

const filteredFoodItems = computed(() => {
  let result = foodItems.value;

  if (selectedCategory.value) {
    result = result.filter(item => item.category === selectedCategory.value);
  }

  if (searchParams.value.name) {
    const query = searchParams.value.name.toLowerCase();
    result = result.filter(item => 
      item.name.toLowerCase().includes(query) || 
      item.description?.toLowerCase().includes(query)
    );
  }

  if (searchParams.value.category) {
    result = result.filter(item => item.category === searchParams.value.category);
  }

  if (searchParams.value.minPrice !== undefined) {
    result = result.filter(item => item.price >= searchParams.value.minPrice!);
  }

  if (searchParams.value.maxPrice !== undefined) {
    result = result.filter(item => item.price <= searchParams.value.maxPrice!);
  }

  if (searchParams.value.theme) {
     const theme = searchParams.value.theme.toLowerCase();
     result = result.filter(item => item.description?.toLowerCase().includes(theme));
  }

  return result;
});

async function loadMenu() {
  loading.value = true;
  error.value = null;
  try {
    const [items, comboList] = await Promise.all([
      foodItemService.getAll(),
      comboService.getAll()
    ]);
    foodItems.value = items;
    combos.value = comboList;
  } catch (err) {
    console.error('Error loading menu:', err);
    error.value = 'Failed to load menu items. Please try again.';
  } finally {
    loading.value = false;
  }
}

function handleSearch(params: SearchParams) {
  searchParams.value = params;
  if (params.category) {
    selectedCategory.value = null;
  }
}

function handleClearSearch() {
  searchParams.value = {};
}

function selectCategory(category: string | null) {
  selectedCategory.value = category;
  if (searchParams.value.category) {
    searchParams.value = { ...searchParams.value, category: undefined };
  }
}

function addFoodItemToCart(item: FoodItem) {
  cartStore.addItem({
    id: item.foodItemId,
    name: item.name,
    price: item.price,
    quantity: 1,
    type: 'food',
    imageUrl: item.imageUrl,
  });
}

function addComboToCart(combo: Combo) {
  cartStore.addItem({
    id: combo.comboId,
    name: combo.name,
    price: combo.price,
    quantity: 1,
    type: 'combo',
    imageUrl: combo.imageUrl,
  });
}



onMounted(() => {
  loadMenu();
});
</script>

<template>
  <div class="min-h-screen bg-gray-50">
    <!-- Header Section -->
    <div class="bg-white border-b border-gray-100 shadow-sm">
      <div class="container mx-auto px-base py-lg">
        <div class="text-center mb-lg">
          <h1 class="font-heading text-4xl md:text-5xl font-bold text-gray-900 mb-md">Thực đơn của chúng tôi</h1>
        </div>

        <!-- Search Bar -->
        <div class="mb-0 max-w-7xl mx-auto">
          <SearchBar :categories="categories" @search="handleSearch" @clear="handleClearSearch" />
        </div>
      </div>
    </div>

    <!-- Category Navigation -->
    <div class="bg-white border-b border-gray-100   overflow-x-auto">
      <div class="container mx-auto px-base">
        <div class="flex gap-md py-md overflow-x-auto scrollbar-hide">
          <button
            :class="['px-lg py-md rounded-base font-bold transition-all duration-200 flex-shrink-0 whitespace-nowrap', 
              selectedCategory === null 
                ? 'bg-primary text-white shadow-sm' 
                : 'bg-gray-50 text-gray-600 hover:bg-gray-100']"
            @click="selectCategory(null)"
          >
            Tất cả mặt hàng
          </button>
          <button
            v-for="category in categories"
            :key="category"
            :class="['px-lg py-md rounded-base font-bold transition-all duration-200 flex-shrink-0 whitespace-nowrap', 
              selectedCategory === category 
                ? 'bg-primary text-white shadow-sm' 
                : 'bg-gray-50 text-gray-600 hover:bg-gray-100']"
            @click="selectCategory(category)"
          >
            {{ category }}
          </button>
        </div>
      </div>
    </div>

    <!-- Main Content -->
    <div class="py-5xl">
      <div class="container mx-auto px-base">
        <!-- Loading State -->
        <div v-if="loading" class="flex flex-col items-center justify-center py-5xl">
          <div class="w-12 h-12 border-4 border-gray-200 border-t-primary rounded-full animate-spin mb-md"></div>
          <p class="text-gray-500 font-medium">Đang tải thực đơn...</p>
        </div>

        <!-- Error State -->
        <div v-else-if="error" class="text-center py-5xl bg-white rounded-lg border border-red-200">
          <p class="text-red-600 text-lg mb-lg font-semibold">{{ error }}</p>
          <button @click="loadMenu" class="px-xl py-base bg-primary text-white rounded-base font-bold hover:bg-primary-hover transition-colors">
            Retry
          </button>
        </div>

        <!-- Menu Content -->
        <div v-else class="space-y-5xl">
          <!-- Combos Section -->
          <section v-if="filteredCombos.length > 0">
            <div class="flex items-center gap-lg mb-xxl">
              <h2 class="font-heading text-3xl md:text-4xl font-bold text-gray-900">Combo Đặc Biệt</h2>
              <div class="h-1 flex-1 bg-gradient-to-r from-secondary to-secondary/20 rounded-full"></div>
            </div>
            <div class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-xl">
              <ComboCard
                v-for="combo in filteredCombos"
                :key="combo.comboId"
                :combo="combo"
                @add-to-cart="addComboToCart"
              />
            </div>
          </section>

          <!-- Food Items Section -->
          <section v-if="filteredFoodItems.length > 0">
            <div class="flex items-center gap-lg mb-xxl">
              <h2 class="font-heading text-3xl md:text-4xl font-bold text-gray-900">
                {{ selectedCategory ? selectedCategory : 'Món Ăn' }}
              </h2>
              <div class="h-1 flex-1 bg-gradient-to-r from-primary to-primary/20 rounded-full"></div>
            </div>
            <div class="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-4 gap-xl">
              <FoodItemCard
                v-for="item in filteredFoodItems"
                :key="item.foodItemId"
                :item="item"
                @add-to-cart="addFoodItemToCart"
              />
            </div>
          </section>

          <!-- Empty State -->
          <div v-if="filteredFoodItems.length === 0 && filteredCombos.length === 0" class="text-center py-5xl bg-white rounded-lg shadow-sm border border-gray-100">
            <div class="text-gray-300 mb-lg">
              <svg class="w-24 h-24 mx-auto" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="1">
                <path stroke-linecap="round" stroke-linejoin="round" d="M9.172 16.172a4 4 0 015.656 0M9 10h.01M15 10h.01M21 12a9 9 0 11-18 0 9 9 0 0118 0z" />
              </svg>
            </div>
            <h3 class="text-xl font-bold text-gray-800 mb-md">Không tìm thấy món nào</h3>
            <p class="text-gray-600">Hãy thử điều chỉnh bộ lọc tìm kiếm hoặc danh mục.</p>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<style scoped>
/* No custom CSS needed */
</style>
