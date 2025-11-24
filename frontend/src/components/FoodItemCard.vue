<template>
  <div class="food-item-card">
    <div class="card-image">
      <img
        :src="item.imageUrl || '/placeholder-food.jpg'"
        :alt="item.name"
        @error="handleImageError"
      />
    </div>
    <div class="card-content">
      <h3 class="item-name">{{ item.name }}</h3>
      <p class="item-category">{{ item.category }}</p>
      <p class="item-description">{{ truncatedDescription }}</p>
      <div class="card-footer">
        <span class="item-price">${{ item.price.toFixed(2) }}</span>
        <button @click="handleAddToCart" class="btn-add-cart">
          Add to Cart
        </button>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { computed } from 'vue';
import type { FoodItem } from '@/services/foodItemService';

interface Props {
  item: FoodItem;
}

const props = defineProps<Props>();
const emit = defineEmits<{
  addToCart: [item: FoodItem];
  click: [item: FoodItem];
}>();

const truncatedDescription = computed(() => {
  if (!props.item.description) return '';
  return props.item.description.length > 100
    ? props.item.description.substring(0, 100) + '...'
    : props.item.description;
});

const handleImageError = (event: Event) => {
  const target = event.target as HTMLImageElement;
  target.src = '/placeholder-food.jpg';
};

const handleAddToCart = () => {
  emit('addToCart', props.item);
};
</script>

<style scoped>
.food-item-card {
  border: 1px solid #e0e0e0;
  border-radius: 8px;
  overflow: hidden;
  transition: transform 0.2s, box-shadow 0.2s;
  background: white;
  cursor: pointer;
  height: 100%;
  display: flex;
  flex-direction: column;
}

.food-item-card:hover {
  transform: translateY(-4px);
  box-shadow: 0 4px 12px rgba(0, 0, 0, 0.1);
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

.item-name {
  font-size: 1.25rem;
  font-weight: 600;
  margin: 0 0 0.5rem 0;
  color: #333;
}

.item-category {
  font-size: 0.875rem;
  color: #666;
  text-transform: uppercase;
  margin: 0 0 0.5rem 0;
}

.item-description {
  font-size: 0.875rem;
  color: #666;
  margin: 0 0 1rem 0;
  flex: 1;
}

.card-footer {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-top: auto;
}

.item-price {
  font-size: 1.5rem;
  font-weight: 700;
  color: #2c3e50;
}

.btn-add-cart {
  background: #42b983;
  color: white;
  border: none;
  padding: 0.5rem 1rem;
  border-radius: 4px;
  cursor: pointer;
  font-weight: 600;
  transition: background 0.2s;
}

.btn-add-cart:hover {
  background: #359268;
}
</style>