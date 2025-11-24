<script setup lang="ts">
import { ref, onMounted } from 'vue';
import { useRouter } from 'vue-router';
import { foodItemService, type FoodItem } from '@/services/foodItemService';
import { comboService, type Combo } from '@/services/comboService';
import { useCartStore } from '@/stores/cart';
import FoodItemCard from '@/components/FoodItemCard.vue';
import ComboCard from '@/components/ComboCard.vue';

const router = useRouter();
const cartStore = useCartStore();
const featuredItems = ref<FoodItem[]>([]);
const featuredCombos = ref<Combo[]>([]);
const loading = ref(true);

onMounted(async () => {
  try {
    // Fetch featured items (first 4)
    const items = await foodItemService.getAll();
    featuredItems.value = items.slice(0, 4);

    // Fetch featured combos (first 2)
    const combos = await comboService.getAll();
    featuredCombos.value = combos.slice(0, 2);
  } catch (error) {
    console.error('Error loading featured items:', error);
  } finally {
    loading.value = false;
  }
});

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

function navigateToMenu() {
  router.push('/menu');
}

function navigateToSearch() {
  router.push('/search');
}
</script>

<template>
  <div class="home-view">
    <!-- Hero Section -->
    <section class="hero">
      <div class="container">
        <div class="hero-content">
          <h1 class="hero-title">Delicious Food, Delivered Fast</h1>
          <p class="hero-subtitle">
            Order your favorite meals from our extensive menu and get them delivered right to your
            door
          </p>
          <div class="hero-actions">
            <button @click="navigateToMenu" class="btn-primary-large">
              Browse Menu
              <svg class="icon" viewBox="0 0 24 24" fill="none" stroke="currentColor">
                <path
                  stroke-linecap="round"
                  stroke-linejoin="round"
                  stroke-width="2"
                  d="M13 7l5 5m0 0l-5 5m5-5H6"
                />
              </svg>
            </button>
            <button @click="navigateToSearch" class="btn-secondary-large">Search Items</button>
          </div>
        </div>
        <div class="hero-image">
          <div class="hero-image-placeholder">
            <svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
              <path
                d="M3 3h18v18H3z M9 9h6v6H9z M12 3v6 M12 15v6 M3 12h6 M15 12h6"
                stroke-linecap="round"
              />
            </svg>
          </div>
        </div>
      </div>
    </section>

    <!-- Features Section -->
    <section class="features">
      <div class="container">
        <div class="features-grid">
          <div class="feature-card">
            <div class="feature-icon">
              <svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
                <path
                  stroke-linecap="round"
                  stroke-linejoin="round"
                  d="M13 10V3L4 14h7v7l9-11h-7z"
                />
              </svg>
            </div>
            <h3>Fast Delivery</h3>
            <p>Get your food delivered in 30 minutes or less</p>
          </div>
          <div class="feature-card">
            <div class="feature-icon">
              <svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
                <path
                  stroke-linecap="round"
                  stroke-linejoin="round"
                  d="M9 12l2 2 4-4m6 2a9 9 0 11-18 0 9 9 0 0118 0z"
                />
              </svg>
            </div>
            <h3>Quality Food</h3>
            <p>Fresh ingredients and delicious recipes</p>
          </div>
          <div class="feature-card">
            <div class="feature-icon">
              <svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
                <path
                  stroke-linecap="round"
                  stroke-linejoin="round"
                  d="M12 8c-1.657 0-3 .895-3 2s1.343 2 3 2 3 .895 3 2-1.343 2-3 2m0-8c1.11 0 2.08.402 2.599 1M12 8V7m0 1v8m0 0v1m0-1c-1.11 0-2.08-.402-2.599-1M21 12a9 9 0 11-18 0 9 9 0 0118 0z"
                />
              </svg>
            </div>
            <h3>Great Prices</h3>
            <p>Affordable meals and combo deals</p>
          </div>
        </div>
      </div>
    </section>

    <!-- Featured Items Section -->
    <section class="featured-section">
      <div class="container">
        <div class="section-header">
          <h2>Featured Items</h2>
          <router-link to="/menu" class="view-all-link">
            View All
            <svg class="icon-small" viewBox="0 0 24 24" fill="none" stroke="currentColor">
              <path
                stroke-linecap="round"
                stroke-linejoin="round"
                stroke-width="2"
                d="M9 5l7 7-7 7"
              />
            </svg>
          </router-link>
        </div>

        <div v-if="loading" class="loading-state">
          <div class="spinner"></div>
          <p>Loading featured items...</p>
        </div>

        <div v-else class="items-grid">
          <FoodItemCard
            v-for="item in featuredItems"
            :key="item.foodItemId"
            :item="item"
            @add-to-cart="addFoodItemToCart"
          />
        </div>
      </div>
    </section>

    <!-- Featured Combos Section -->
    <section class="featured-section combos-section">
      <div class="container">
        <div class="section-header">
          <h2>Special Combos</h2>
          <router-link to="/menu" class="view-all-link">
            View All
            <svg class="icon-small" viewBox="0 0 24 24" fill="none" stroke="currentColor">
              <path
                stroke-linecap="round"
                stroke-linejoin="round"
                stroke-width="2"
                d="M9 5l7 7-7 7"
              />
            </svg>
          </router-link>
        </div>

        <div v-if="loading" class="loading-state">
          <div class="spinner"></div>
          <p>Loading combos...</p>
        </div>

        <div v-else class="combos-grid">
          <ComboCard
            v-for="combo in featuredCombos"
            :key="combo.comboId"
            :combo="combo"
            @add-to-cart="addComboToCart"
          />
        </div>
      </div>
    </section>

    <!-- CTA Section -->
    <section class="cta-section">
      <div class="container">
        <div class="cta-content">
          <h2>Ready to Order?</h2>
          <p>Browse our full menu and find your favorite dishes</p>
          <button @click="navigateToMenu" class="btn-primary-large">Order Now</button>
        </div>
      </div>
    </section>
  </div>
</template>

<style scoped>
.home-view {
  background: #f9fafb;
}

.container {
  max-width: 1200px;
  margin: 0 auto;
  padding: 0 1rem;
}

/* Hero Section */
.hero {
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  color: white;
  padding: 4rem 0;
}

.hero-content {
  max-width: 600px;
}

.hero-title {
  font-size: 3rem;
  font-weight: 800;
  margin-bottom: 1rem;
  line-height: 1.2;
}

.hero-subtitle {
  font-size: 1.25rem;
  margin-bottom: 2rem;
  opacity: 0.95;
}

.hero-actions {
  display: flex;
  gap: 1rem;
  flex-wrap: wrap;
}

.btn-primary-large,
.btn-secondary-large {
  padding: 1rem 2rem;
  border-radius: 0.5rem;
  font-weight: 600;
  font-size: 1.125rem;
  cursor: pointer;
  transition: all 0.2s;
  border: none;
  display: flex;
  align-items: center;
  gap: 0.5rem;
}

.btn-primary-large {
  background: white;
  color: #667eea;
}

.btn-primary-large:hover {
  transform: translateY(-2px);
  box-shadow: 0 10px 20px rgba(0, 0, 0, 0.2);
}

.btn-secondary-large {
  background: rgba(255, 255, 255, 0.2);
  color: white;
  border: 2px solid white;
}

.btn-secondary-large:hover {
  background: rgba(255, 255, 255, 0.3);
}

.icon {
  width: 20px;
  height: 20px;
}

.hero-image {
  display: none;
}

.hero-image-placeholder {
  width: 100%;
  height: 400px;
  background: rgba(255, 255, 255, 0.1);
  border-radius: 1rem;
  display: flex;
  align-items: center;
  justify-content: center;
}

.hero-image-placeholder svg {
  width: 200px;
  height: 200px;
  opacity: 0.3;
}

@media (min-width: 768px) {
  .hero .container {
    display: grid;
    grid-template-columns: 1fr 1fr;
    gap: 3rem;
    align-items: center;
  }

  .hero-image {
    display: block;
  }
}

/* Features Section */
.features {
  padding: 4rem 0;
  background: white;
}

.features-grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(250px, 1fr));
  gap: 2rem;
}

.feature-card {
  text-align: center;
  padding: 2rem;
}

.feature-icon {
  width: 64px;
  height: 64px;
  margin: 0 auto 1rem;
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  border-radius: 1rem;
  display: flex;
  align-items: center;
  justify-content: center;
  color: white;
}

.feature-icon svg {
  width: 32px;
  height: 32px;
}

.feature-card h3 {
  font-size: 1.5rem;
  margin-bottom: 0.5rem;
  color: #1f2937;
}

.feature-card p {
  color: #6b7280;
}

/* Featured Sections */
.featured-section {
  padding: 4rem 0;
}

.combos-section {
  background: white;
}

.section-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 2rem;
}

.section-header h2 {
  font-size: 2rem;
  font-weight: 700;
  color: #1f2937;
}

.view-all-link {
  display: flex;
  align-items: center;
  gap: 0.5rem;
  color: #667eea;
  text-decoration: none;
  font-weight: 600;
  transition: gap 0.2s;
}

.view-all-link:hover {
  gap: 0.75rem;
}

.icon-small {
  width: 16px;
  height: 16px;
}

.items-grid {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(280px, 1fr));
  gap: 2rem;
}

.combos-grid {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(350px, 1fr));
  gap: 2rem;
}

.loading-state {
  text-align: center;
  padding: 4rem 0;
}

.spinner {
  width: 48px;
  height: 48px;
  border: 4px solid #e5e7eb;
  border-top-color: #667eea;
  border-radius: 50%;
  animation: spin 1s linear infinite;
  margin: 0 auto 1rem;
}

@keyframes spin {
  to {
    transform: rotate(360deg);
  }
}

/* CTA Section */
.cta-section {
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  color: white;
  padding: 4rem 0;
}

.cta-content {
  text-align: center;
  max-width: 600px;
  margin: 0 auto;
}

.cta-content h2 {
  font-size: 2.5rem;
  font-weight: 800;
  margin-bottom: 1rem;
}

.cta-content p {
  font-size: 1.25rem;
  margin-bottom: 2rem;
  opacity: 0.95;
}
</style>
