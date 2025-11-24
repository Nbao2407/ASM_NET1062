<template>
  <div class="search-view">
    <div class="search-header">
      <h1>Search Menu</h1>
      <p>Find your favorite food items and combos</p>
    </div>

    <!-- Search Bar -->
    <SearchBar :categories="categories" @search="handleSearch" @clear="handleClear" />

    <!-- Loading State -->
    <div v-if="loading" class="loading">
      <p>Searching...</p>
    </div>

    <!-- Error State -->
    <div v-else-if="error" class="error">
      <p>{{ error }}</p>
    </div>

    <!-- Search Results -->
    <div v-else-if="hasSearched" class="search-results">
      <!-- Results Count -->
      <div class="results-header">
        <h2>
          {{ searchResults.length }}
          {{ searchResults.length === 1 ? 'result' : 'results' }} found
        </h2>
      </div>

      <!-- Results Grid -->
      <div v-if="searchResults.length > 0" class="results-grid">
        <FoodItemCard
          v-for="item in searchResults"
          :key="item.foodItemId"
          :item="item"
          @add-to-cart="addFoodItemToCart"
          @click="viewFoodItemDetail(item.foodItemId)"
        />
      </div>

      <!-- Empty Results -->
      <div v-else class="empty-results">
        <div class="empty-icon">🔍</div>
        <h3>No results found</h3>
        <p>Try adjusting your search criteria or browse our full menu</p>
        <button @click="goToMenu" class="btn-browse-menu">
          Browse Full Menu
        </button>
      </div>
    </div>

    <!-- Initial State -->
    <div v-else class="initial-state">
      <div class="initial-icon">🍔</div>
      <h3>Start searching</h3>
      <p>Use the search bar above to find your favorite food items</p>
      <div class="quick-links">
        <h4>Quick Links:</h4>
        <div class="links-grid">
          <button
            v-for="category in categories"
            :key="category"
            @click="quickSearch(category)"
            class="quick-link-btn"
          >
            {{ category }}
          </button>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue';
import { useRouter } from 'vue-router';
import { foodItemService, type FoodItem, type SearchParams } from '@/services/foodItemService';
import { useCartStore } from '@/stores/cart';
import SearchBar from '@/components/SearchBar.vue';
import FoodItemCard from '@/components/FoodItemCard.vue';

const router = useRouter();
const cartStore = useCartStore();

const searchResults = ref<FoodItem[]>([]);
const categories = ref<string[]>([]);
const loading = ref(false);
const error = ref<string | null>(null);
const hasSearched = ref(false);

const handleSearch = async (params: SearchParams) => {
  loading.value = true;
  error.value = null;
  hasSearched.value = true;

  try {
    searchResults.value = await foodItemService.search(params);
  } catch (err) {
    error.value = 'Failed to search. Please try again.';
    console.error('Error searching:', err);
  } finally {
    loading.value = false;
  }
};

const handleClear = () => {
  searchResults.value = [];
  hasSearched.value = false;
  error.value = null;
};

const quickSearch = (category: string) => {
  handleSearch({ category });
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

const viewFoodItemDetail = (id: number) => {
  router.push({ name: 'food-item-detail', params: { id } });
};

const goToMenu = () => {
  router.push({ name: 'menu' });
};

const loadCategories = async () => {
  try {
    categories.value = await foodItemService.getCategories();
  } catch (err) {
    console.error('Error loading categories:', err);
  }
};

onMounted(() => {
  loadCategories();
});
</script>

<style scoped>
.search-view {
  max-width: 1200px;
  margin: 0 auto;
  padding: 2rem 1rem;
}

.search-header {
  text-align: center;
  margin-bottom: 2rem;
}

.search-header h1 {
  font-size: 2.5rem;
  color: #2c3e50;
  margin: 0 0 0.5rem 0;
}

.search-header p {
  font-size: 1.125rem;
  color: #666;
  margin: 0;
}

.loading,
.error {
  text-align: center;
  padding: 3rem 1rem;
}

.error {
  color: #e74c3c;
}

.search-results {
  margin-top: 2rem;
}

.results-header {
  margin-bottom: 1.5rem;
}

.results-header h2 {
  font-size: 1.5rem;
  color: #2c3e50;
  margin: 0;
}

.results-grid {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(280px, 1fr));
  gap: 1.5rem;
}

.empty-results,
.initial-state {
  text-align: center;
  padding: 4rem 1rem;
}

.empty-icon,
.initial-icon {
  font-size: 4rem;
  margin-bottom: 1rem;
}

.empty-results h3,
.initial-state h3 {
  font-size: 1.75rem;
  color: #2c3e50;
  margin: 0 0 0.5rem 0;
}

.empty-results p,
.initial-state p {
  font-size: 1.125rem;
  color: #666;
  margin: 0 0 2rem 0;
}

.btn-browse-menu {
  padding: 0.75rem 2rem;
  background: #42b983;
  color: white;
  border: none;
  border-radius: 4px;
  cursor: pointer;
  font-weight: 600;
  font-size: 1rem;
  transition: background 0.2s;
}

.btn-browse-menu:hover {
  background: #359268;
}

.quick-links {
  margin-top: 3rem;
}

.quick-links h4 {
  font-size: 1.125rem;
  color: #2c3e50;
  margin: 0 0 1rem 0;
}

.links-grid {
  display: flex;
  flex-wrap: wrap;
  gap: 0.5rem;
  justify-content: center;
}

.quick-link-btn {
  padding: 0.5rem 1rem;
  background: #f5f5f5;
  border: 2px solid #e0e0e0;
  border-radius: 20px;
  cursor: pointer;
  font-weight: 600;
  transition: all 0.2s;
}

.quick-link-btn:hover {
  background: #42b983;
  color: white;
  border-color: #42b983;
}

@media (max-width: 768px) {
  .search-header h1 {
    font-size: 2rem;
  }

  .results-grid {
    grid-template-columns: repeat(auto-fill, minmax(250px, 1fr));
    gap: 1rem;
  }
}

@media (max-width: 480px) {
  .results-grid {
    grid-template-columns: 1fr;
  }
}
</style>
