<template>
  <div class="food-item-detail">
    <!-- Loading State -->
    <div v-if="loading" class="loading">
      <p>Loading item details...</p>
    </div>

    <!-- Error State -->
    <div v-else-if="error" class="error">
      <p>{{ error }}</p>
      <button @click="loadItem" class="btn-retry">Retry</button>
    </div>

    <!-- Item Detail -->
    <div v-else-if="item" class="detail-content">
      <button @click="goBack" class="btn-back">← Back to Menu</button>

      <div class="detail-grid">
        <!-- Image Section -->
        <div class="image-section">
          <img
            :src="item.imageUrl || '/placeholder-food.jpg'"
            :alt="item.name"
            @error="handleImageError"
          />
        </div>

        <!-- Info Section -->
        <div class="info-section">
          <div class="category-badge">{{ item.category }}</div>
          <h1 class="item-name">{{ item.name }}</h1>
          <p class="item-price">${{ item.price.toFixed(2) }}</p>
          <p class="item-description">{{ item.description }}</p>

          <!-- Ingredients -->
          <div v-if="item.ingredients" class="detail-block">
            <h3>Ingredients</h3>
            <p>{{ item.ingredients }}</p>
          </div>

          <!-- Nutritional Info -->
          <div v-if="item.nutritionalInfo" class="detail-block">
            <h3>Nutritional Information</h3>
            <p>{{ item.nutritionalInfo }}</p>
          </div>

          <!-- Allergen Warnings -->
          <div v-if="item.allergenWarnings" class="detail-block allergen-warning">
            <h3>⚠️ Allergen Warnings</h3>
            <p>{{ item.allergenWarnings }}</p>
          </div>

          <!-- Theme -->
          <div v-if="item.theme" class="detail-block">
            <h3>Theme</h3>
            <p>{{ item.theme }}</p>
          </div>

          <!-- Add to Cart -->
          <div class="cart-actions">
            <div class="quantity-selector">
              <button @click="decrementQuantity" :disabled="quantity <= 1">-</button>
              <input v-model.number="quantity" type="number" min="1" />
              <button @click="incrementQuantity">+</button>
            </div>
            <button @click="addToCart" class="btn-add-cart">
              Add to Cart - ${{ (item.price * quantity).toFixed(2) }}
            </button>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue';
import { useRoute, useRouter } from 'vue-router';
import { foodItemService, type FoodItem } from '@/services/foodItemService';
import { useCartStore } from '@/stores/cart';

const route = useRoute();
const router = useRouter();
const cartStore = useCartStore();

const item = ref<FoodItem | null>(null);
const quantity = ref(1);
const loading = ref(false);
const error = ref<string | null>(null);

const handleImageError = (event: Event) => {
  const target = event.target as HTMLImageElement;
  target.src = '/placeholder-food.jpg';
};

const incrementQuantity = () => {
  quantity.value++;
};

const decrementQuantity = () => {
  if (quantity.value > 1) {
    quantity.value--;
  }
};

const addToCart = () => {
  if (item.value) {
    cartStore.addItem({
      id: item.value.foodItemId,
      name: item.value.name,
      price: item.value.price,
      quantity: quantity.value,
      type: 'food',
      imageUrl: item.value.imageUrl,
    });
    // Show success feedback (could add a toast notification here)
    alert(`Added ${quantity.value} ${item.value.name} to cart!`);
    quantity.value = 1;
  }
};

const goBack = () => {
  router.push({ name: 'menu' });
};

const loadItem = async () => {
  loading.value = true;
  error.value = null;

  try {
    const id = Number(route.params.id);
    if (isNaN(id)) {
      throw new Error('Invalid item ID');
    }
    item.value = await foodItemService.getById(id);
  } catch (err) {
    error.value = 'Failed to load item details. Please try again.';
    console.error('Error loading item:', err);
  } finally {
    loading.value = false;
  }
};

onMounted(() => {
  loadItem();
});
</script>

<style scoped>
.food-item-detail {
  max-width: 1200px;
  margin: 0 auto;
  padding: 2rem 1rem;
}

.loading,
.error {
  text-align: center;
  padding: 3rem 1rem;
}

.error {
  color: #e74c3c;
}

.btn-retry,
.btn-back {
  padding: 0.5rem 1.5rem;
  background: #42b983;
  color: white;
  border: none;
  border-radius: 4px;
  cursor: pointer;
  font-weight: 600;
  margin-bottom: 2rem;
}

.btn-back:hover,
.btn-retry:hover {
  background: #359268;
}

.detail-content {
  width: 100%;
}

.detail-grid {
  display: grid;
  grid-template-columns: 1fr 1fr;
  gap: 3rem;
  margin-top: 2rem;
}

.image-section {
  width: 100%;
}

.image-section img {
  width: 100%;
  height: auto;
  border-radius: 8px;
  box-shadow: 0 4px 12px rgba(0, 0, 0, 0.1);
}

.info-section {
  display: flex;
  flex-direction: column;
  gap: 1rem;
}

.category-badge {
  display: inline-block;
  background: #42b983;
  color: white;
  padding: 0.25rem 0.75rem;
  border-radius: 4px;
  font-size: 0.875rem;
  font-weight: 600;
  text-transform: uppercase;
  width: fit-content;
}

.item-name {
  font-size: 2.5rem;
  color: #2c3e50;
  margin: 0;
}

.item-price {
  font-size: 2rem;
  font-weight: 700;
  color: #42b983;
  margin: 0;
}

.item-description {
  font-size: 1.125rem;
  color: #666;
  line-height: 1.6;
}

.detail-block {
  padding: 1rem;
  background: #f5f5f5;
  border-radius: 8px;
}

.detail-block h3 {
  margin: 0 0 0.5rem 0;
  color: #2c3e50;
  font-size: 1.125rem;
}

.detail-block p {
  margin: 0;
  color: #666;
  line-height: 1.6;
}

.allergen-warning {
  background: #fff3cd;
  border: 2px solid #ffc107;
}

.allergen-warning h3 {
  color: #856404;
}

.allergen-warning p {
  color: #856404;
}

.cart-actions {
  display: flex;
  gap: 1rem;
  margin-top: 2rem;
}

.quantity-selector {
  display: flex;
  align-items: center;
  border: 2px solid #e0e0e0;
  border-radius: 4px;
  overflow: hidden;
}

.quantity-selector button {
  width: 40px;
  height: 40px;
  border: none;
  background: #f5f5f5;
  cursor: pointer;
  font-size: 1.25rem;
  font-weight: 600;
  transition: background 0.2s;
}

.quantity-selector button:hover:not(:disabled) {
  background: #e0e0e0;
}

.quantity-selector button:disabled {
  opacity: 0.5;
  cursor: not-allowed;
}

.quantity-selector input {
  width: 60px;
  height: 40px;
  border: none;
  text-align: center;
  font-size: 1rem;
  font-weight: 600;
}

.btn-add-cart {
  flex: 1;
  padding: 0.75rem 1.5rem;
  background: #42b983;
  color: white;
  border: none;
  border-radius: 4px;
  cursor: pointer;
  font-weight: 600;
  font-size: 1.125rem;
  transition: background 0.2s;
}

.btn-add-cart:hover {
  background: #359268;
}

@media (max-width: 768px) {
  .detail-grid {
    grid-template-columns: 1fr;
    gap: 2rem;
  }

  .item-name {
    font-size: 2rem;
  }

  .cart-actions {
    flex-direction: column;
  }

  .quantity-selector {
    justify-content: center;
  }
}
</style>
