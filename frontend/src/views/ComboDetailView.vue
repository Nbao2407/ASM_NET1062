<template>
  <div class="combo-detail">
    <!-- Loading State -->
    <div v-if="loading" class="loading">
      <p>Loading combo details...</p>
    </div>

    <!-- Error State -->
    <div v-else-if="error" class="error">
      <p>{{ error }}</p>
      <button @click="loadCombo" class="btn-retry">Retry</button>
    </div>

    <!-- Combo Detail -->
    <div v-else-if="combo" class="detail-content">
      <button @click="goBack" class="btn-back">← Back to Menu</button>

      <div class="detail-grid">
        <!-- Image Section -->
        <div class="image-section">
          <div class="combo-badge">COMBO DEAL</div>
          <img
            :src="combo.imageUrl || '/placeholder-combo.jpg'"
            :alt="combo.name"
            @error="handleImageError"
          />
        </div>

        <!-- Info Section -->
        <div class="info-section">
          <h1 class="combo-name">{{ combo.name }}</h1>
          <p class="combo-price">${{ combo.price.toFixed(2) }}</p>
          <p class="combo-description">{{ combo.description }}</p>

          <!-- Included Items -->
          <div class="detail-block">
            <h3>Included Items ({{ combo.items.length }})</h3>
            <ul class="items-list">
              <li v-for="item in combo.items" :key="item.foodItemId" class="combo-item">
                <span class="item-name">{{ item.name }}</span>
                <span v-if="item.quantity > 1" class="item-quantity">x{{ item.quantity }}</span>
              </li>
            </ul>
          </div>

          <!-- Savings Info -->
          <div v-if="totalSavings > 0" class="savings-info">
            <p>💰 Save ${{ totalSavings.toFixed(2) }} with this combo!</p>
          </div>

          <!-- Add to Cart -->
          <div class="cart-actions">
            <div class="quantity-selector">
              <button @click="decrementQuantity" :disabled="quantity <= 1">-</button>
              <input v-model.number="quantity" type="number" min="1" />
              <button @click="incrementQuantity">+</button>
            </div>
            <button @click="addToCart" class="btn-add-cart">
              Add to Cart - ${{ (combo.price * quantity).toFixed(2) }}
            </button>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, computed, onMounted } from 'vue';
import { useRoute, useRouter } from 'vue-router';
import { comboService, type Combo } from '@/services/comboService';
import { useCartStore } from '@/stores/cart';

const route = useRoute();
const router = useRouter();
const cartStore = useCartStore();

const combo = ref<Combo | null>(null);
const quantity = ref(1);
const loading = ref(false);
const error = ref<string | null>(null);

// Calculate potential savings (this is a simplified calculation)
const totalSavings = computed(() => {
  if (!combo.value) return 0;
  // This would ideally fetch individual item prices and calculate the difference
  // For now, we'll show a placeholder calculation
  return 0;
});

const handleImageError = (event: Event) => {
  const target = event.target as HTMLImageElement;
  target.src = '/placeholder-combo.jpg';
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
  if (combo.value) {
    cartStore.addItem({
      id: combo.value.comboId,
      name: combo.value.name,
      price: combo.value.price,
      quantity: quantity.value,
      type: 'combo',
      imageUrl: combo.value.imageUrl,
    });
    // Show success feedback (could add a toast notification here)
    alert(`Added ${quantity.value} ${combo.value.name} to cart!`);
    quantity.value = 1;
  }
};

const goBack = () => {
  router.push({ name: 'menu' });
};

const loadCombo = async () => {
  loading.value = true;
  error.value = null;

  try {
    const id = Number(route.params.id);
    if (isNaN(id)) {
      throw new Error('Invalid combo ID');
    }
    combo.value = await comboService.getById(id);
  } catch (err) {
    error.value = 'Failed to load combo details. Please try again.';
    console.error('Error loading combo:', err);
  } finally {
    loading.value = false;
  }
};

onMounted(() => {
  loadCombo();
});
</script>

<style scoped>
.combo-detail {
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
  background: #ff9800;
  color: white;
  border: none;
  border-radius: 4px;
  cursor: pointer;
  font-weight: 600;
  margin-bottom: 2rem;
}

.btn-back:hover,
.btn-retry:hover {
  background: #e68900;
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
  position: relative;
}

.combo-badge {
  position: absolute;
  top: 20px;
  right: 20px;
  background: #ff9800;
  color: white;
  padding: 0.5rem 1rem;
  border-radius: 4px;
  font-size: 0.875rem;
  font-weight: 700;
  z-index: 1;
}

.image-section img {
  width: 100%;
  height: auto;
  border-radius: 8px;
  box-shadow: 0 4px 12px rgba(255, 152, 0, 0.3);
  border: 2px solid #ff9800;
}

.info-section {
  display: flex;
  flex-direction: column;
  gap: 1rem;
}

.combo-name {
  font-size: 2.5rem;
  color: #2c3e50;
  margin: 0;
}

.combo-price {
  font-size: 2rem;
  font-weight: 700;
  color: #ff9800;
  margin: 0;
}

.combo-description {
  font-size: 1.125rem;
  color: #666;
  line-height: 1.6;
}

.detail-block {
  padding: 1rem;
  background: #fff8e1;
  border-radius: 8px;
  border: 2px solid #ff9800;
}

.detail-block h3 {
  margin: 0 0 1rem 0;
  color: #2c3e50;
  font-size: 1.125rem;
}

.items-list {
  list-style: none;
  padding: 0;
  margin: 0;
  display: flex;
  flex-direction: column;
  gap: 0.75rem;
}

.combo-item {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 0.75rem;
  background: white;
  border-radius: 4px;
  border: 1px solid #ffe0b2;
}

.combo-item .item-name {
  font-weight: 600;
  color: #2c3e50;
}

.combo-item .item-quantity {
  background: #ff9800;
  color: white;
  padding: 0.25rem 0.5rem;
  border-radius: 4px;
  font-size: 0.875rem;
  font-weight: 600;
}

.savings-info {
  padding: 1rem;
  background: #e8f5e9;
  border: 2px solid #4caf50;
  border-radius: 8px;
  text-align: center;
}

.savings-info p {
  margin: 0;
  font-size: 1.125rem;
  font-weight: 600;
  color: #2e7d32;
}

.cart-actions {
  display: flex;
  gap: 1rem;
  margin-top: 2rem;
}

.quantity-selector {
  display: flex;
  align-items: center;
  border: 2px solid #ff9800;
  border-radius: 4px;
  overflow: hidden;
}

.quantity-selector button {
  width: 40px;
  height: 40px;
  border: none;
  background: #fff8e1;
  cursor: pointer;
  font-size: 1.25rem;
  font-weight: 600;
  transition: background 0.2s;
  color: #ff9800;
}

.quantity-selector button:hover:not(:disabled) {
  background: #ffe0b2;
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
  background: #ff9800;
  color: white;
  border: none;
  border-radius: 4px;
  cursor: pointer;
  font-weight: 600;
  font-size: 1.125rem;
  transition: background 0.2s;
}

.btn-add-cart:hover {
  background: #e68900;
}

@media (max-width: 768px) {
  .detail-grid {
    grid-template-columns: 1fr;
    gap: 2rem;
  }

  .combo-name {
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
