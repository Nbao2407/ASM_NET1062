<template>
  <div class="menu-view">
    <div class="menu-header">
      <h1>Our Menu</h1>
      <p>Browse our delicious food items and combo deals</p>
    </div>

    <!-- Search Bar -->
    <SearchBar :categories="categories" @search="handleSearch" @clear="handleClearSearch" />

    <!-- Category Filter -->
    <div class="category-filter">
      <button
        :class="['category-btn', { active: selectedCategory === null }]"
        @click="selectCategory(null)"
      >
        All Items
      </button>
      <button
        v-for="category in categories"
        :key="category"
        :class="['category-btn', { active: selectedCategory === category }]"
        @click="selectCategory(category)"
      >
        {{ category }}
      </button>
    </div>

    <!-- Loading State -->
    <div v-if="loading" class="loading">
      <p>Loading menu...</p>
    </div>

    <!-- Error State -->
    <div v-else-if="error" class="error">
      <p>{{ error }}</p>
      <button @click="loadMenu" class="btn-retry">Retry</button>
    </div>

    <!-- Menu Content -->
    <div v-else class="menu-content">
      <!-- Combos Section -->
      <section v-if="filteredCombos.length > 0" class="menu-section">
        <h2 class="section-title">Combo Deals</h2>
        <div class="items-grid">
          <ComboCard
            v-for="combo in filteredCombos"
            :key="combo.comboId"
            :combo="combo"
            @add-to-cart="addComboToCart"
            @click="viewComboDetail(combo.comboId)"
          />
        </div>
      </section>

      <!-- Food Items Section -->
      <section v-if="filteredFoodItems.length > 0" class="menu-section">
        <h2 class="section-title">
          {{ selectedCategory ? selectedCategory : 'Food Items' }}
        </h2>
        <div class="items-grid">
          <FoodItemCard
            v-for="item in filteredFoodItems"
            :key="item.foodItemId"
            :item="item"
            @add-to-cart="addFoodItemToCart"
            @click="viewFoodItemDetail(item.foodItemId)"
          />
        </div>
      </section>

      <!-- Empty State -->
      <div v-if="filteredFoodItems.length === 0 && filteredCombos.length === 0" class="empty-state">
        <p>No items found in this category.</p>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, computed, onMounted } from 'vue';
import { useRouter } from 'vue-router';
import { foodItemService, type FoodItem } from '@/services/foodItemService';
import { comboService, type Combo } from '@/services/comboService';
import { useCartStore } from '@/stores/cart';
import FoodItemCard from '@/components/FoodItemCard.vue';
import ComboCard from '@/components/ComboCard.vue';
import SearchBar from '@/components/SearchBar.vue';
import type { SearchParams } from '@/services/foodItemService';

const router = useRouter();
const cartStore = useCartStore();

const foodItems = ref<FoodItem[]>([]);
const combos = ref<Combo[]>([]);
const categories = ref<string[]>([]);
const selectedCategory = ref<string | null>(null);
const loading = ref(false);
const error = ref<string | null>(null);
const searchActive = ref(false);
const searchResults = ref<FoodItem[]>([]);

const filteredFoodItems = computed(() => {
  // If search is active, show search results
  if (searchActive.value) {
    return searchResults.value.filter(item => item.isActive);
  }
  
  // Otherwise, apply category filter
  if (!selectedCategory.value) {
    return foodItems.value.filter(item => item.isActive);
  }
  return foodItems.value.filter(
    item => item.isActive && item.category === selectedCategory.value
  );
});

const filteredCombos = computed(() => {
  // Don't show combos when search is active
  if (searchActive.value) {
    return [];
  }
  
  // Show combos only when "All Items" is selected or no category is selected
  if (!selectedCategory.value) {
    return combos.value.filter(combo => combo.isActive);
  }
  return [];
});

const selectCategory = (category: string | null) => {
  selectedCategory.value = category;
  // Clear search when selecting a category
  searchActive.value = false;
  searchResults.value = [];
};

const handleSearch = async (params: SearchParams) => {
  loading.value = true;
  error.value = null;

  try {
    searchResults.value = await foodItemService.search(params);
    searchActive.value = true;
    // Clear category filter when searching
    selectedCategory.value = null;
  } catch (err) {
    error.value = 'Failed to search. Please try again.';
    console.error('Error searching:', err);
  } finally {
    loading.value = false;
  }
};

const handleClearSearch = () => {
  searchActive.value = false;
  searchResults.value = [];
  error.value = null;
};

const addFoodItemToCart = (item: FoodItem) => {
  cartStore.addItem({
    id: item.foodItemId,
    name: item.name,
    price: item.price,
    quantity: 1,
    type: 'food',
    imageUrl: item.imageUrl,
  });
};

const addComboToCart = (combo: Combo) => {
  cartStore.addItem({
    id: combo.comboId,
    name: combo.name,
    price: combo.price,
    quantity: 1,
    type: 'combo',
    imageUrl: combo.imageUrl,
  });
};

const viewFoodItemDetail = (id: number) => {
  router.push({ name: 'food-item-detail', params: { id } });
};

const viewComboDetail = (id: number) => {
  router.push({ name: 'combo-detail', params: { id } });
};

const loadMenu = async () => {
  loading.value = true;
  error.value = null;

  try {
    // Load all data in parallel
    const [foodItemsData, combosData, categoriesData] = await Promise.all([
      foodItemService.getAll(),
      comboService.getAll(),
      foodItemService.getCategories(),
    ]);

    foodItems.value = foodItemsData;
    combos.value = combosData;
    categories.value = categoriesData;
  } catch (err) {
    error.value = 'Failed to load menu. Please try again.';
    console.error('Error loading menu:', err);
  } finally {
    loading.value = false;
  }
};

onMounted(() => {
  loadMenu();
});
</script>

<style scoped>
.menu-view {
  max-width: 1200px;
  margin: 0 auto;
  padding: 2rem 1rem;
}

.menu-header {
  text-align: center;
  margin-bottom: 2rem;
}

.menu-header h1 {
  font-size: 2.5rem;
  color: #2c3e50;
  margin: 0 0 0.5rem 0;
}

.menu-header p {
  font-size: 1.125rem;
  color: #666;
  margin: 0;
}

.category-filter {
  display: flex;
  flex-wrap: wrap;
  gap: 0.5rem;
  justify-content: center;
  margin-bottom: 2rem;
  padding: 1rem;
  background: #f5f5f5;
  border-radius: 8px;
}

.category-btn {
  padding: 0.5rem 1rem;
  border: 2px solid #e0e0e0;
  background: white;
  border-radius: 20px;
  cursor: pointer;
  font-weight: 600;
  transition: all 0.2s;
}

.category-btn:hover {
  border-color: #42b983;
  color: #42b983;
}

.category-btn.active {
  background: #42b983;
  color: white;
  border-color: #42b983;
}

.loading,
.error,
.empty-state {
  text-align: center;
  padding: 3rem 1rem;
}

.error {
  color: #e74c3c;
}

.btn-retry {
  margin-top: 1rem;
  padding: 0.5rem 1.5rem;
  background: #42b983;
  color: white;
  border: none;
  border-radius: 4px;
  cursor: pointer;
  font-weight: 600;
}

.menu-content {
  display: flex;
  flex-direction: column;
  gap: 3rem;
}

.menu-section {
  width: 100%;
}

.section-title {
  font-size: 2rem;
  color: #2c3e50;
  margin: 0 0 1.5rem 0;
  padding-bottom: 0.5rem;
  border-bottom: 3px solid #42b983;
}

.items-grid {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(280px, 1fr));
  gap: 1.5rem;
}

@media (max-width: 768px) {
  .menu-header h1 {
    font-size: 2rem;
  }

  .items-grid {
    grid-template-columns: repeat(auto-fill, minmax(250px, 1fr));
    gap: 1rem;
  }

  .category-filter {
    padding: 0.5rem;
  }
}

@media (max-width: 480px) {
  .items-grid {
    grid-template-columns: 1fr;
  }
}
</style>
