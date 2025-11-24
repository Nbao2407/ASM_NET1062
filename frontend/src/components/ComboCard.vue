<template>
  <div class="combo-card">
    <div class="card-badge">COMBO</div>
    <div class="card-image">
      <img
        :src="combo.imageUrl || '/placeholder-combo.jpg'"
        :alt="combo.name"
        @error="handleImageError"
      />
    </div>
    <div class="card-content">
      <h3 class="combo-name">{{ combo.name }}</h3>
      <p class="combo-description">{{ truncatedDescription }}</p>
      <div class="combo-items">
        <span class="items-label">Includes:</span>
        <span class="items-count">{{ combo.items.length }} items</span>
      </div>
      <div class="card-footer">
        <span class="combo-price">${{ combo.price.toFixed(2) }}</span>
        <button @click="handleAddToCart" class="btn-add-cart">
          Add to Cart
        </button>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { computed } from 'vue';
import type { Combo } from '@/services/comboService';

interface Props {
  combo: Combo;
}

const props = defineProps<Props>();
const emit = defineEmits<{
  addToCart: [combo: Combo];
  click: [combo: Combo];
}>();

const truncatedDescription = computed(() => {
  if (!props.combo.description) return '';
  return props.combo.description.length > 100
    ? props.combo.description.substring(0, 100) + '...'
    : props.combo.description;
});

const handleImageError = (event: Event) => {
  const target = event.target as HTMLImageElement;
  target.src = '/placeholder-combo.jpg';
};

const handleAddToCart = () => {
  emit('addToCart', props.combo);
};
</script>

<style scoped>
.combo-card {
  border: 2px solid #ff9800;
  border-radius: 8px;
  overflow: hidden;
  transition: transform 0.2s, box-shadow 0.2s;
  background: white;
  cursor: pointer;
  height: 100%;
  display: flex;
  flex-direction: column;
  position: relative;
}

.combo-card:hover {
  transform: translateY(-4px);
  box-shadow: 0 4px 12px rgba(255, 152, 0, 0.3);
}

.card-badge {
  position: absolute;
  top: 10px;
  right: 10px;
  background: #ff9800;
  color: white;
  padding: 0.25rem 0.75rem;
  border-radius: 4px;
  font-size: 0.75rem;
  font-weight: 700;
  z-index: 1;
}

.card-image {
  width: 100%;
  height: 200px;
  overflow: hidden;
  background: #f5f5f5;
}

.card-image img {
  width: 100%;
  height: 100%;
  object-fit: cover;
}

.card-content {
  padding: 1rem;
  flex: 1;
  display: flex;
  flex-direction: column;
}

.combo-name {
  font-size: 1.25rem;
  font-weight: 600;
  margin: 0 0 0.5rem 0;
  color: #333;
}

.combo-description {
  font-size: 0.875rem;
  color: #666;
  margin: 0 0 1rem 0;
  flex: 1;
}

.combo-items {
  display: flex;
  align-items: center;
  gap: 0.5rem;
  margin-bottom: 1rem;
  font-size: 0.875rem;
}

.items-label {
  color: #666;
}

.items-count {
  color: #ff9800;
  font-weight: 600;
}

.card-footer {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-top: auto;
}

.combo-price {
  font-size: 1.5rem;
  font-weight: 700;
  color: #ff9800;
}

.btn-add-cart {
  background: #ff9800;
  color: white;
  border: none;
  padding: 0.5rem 1rem;
  border-radius: 4px;
  cursor: pointer;
  font-weight: 600;
  transition: background 0.2s;
}

.btn-add-cart:hover {
  background: #e68900;
}
</style>
