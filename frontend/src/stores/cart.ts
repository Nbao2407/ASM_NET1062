import { defineStore } from 'pinia';
import { ref, computed } from 'vue';

interface CartItem {
  id: number;
  name: string;
  price: number;
  quantity: number;
  type: 'food' | 'combo';
  imageUrl?: string;
}

export const useCartStore = defineStore('cart', () => {
  const items = ref<CartItem[]>([]);

  const totalItems = computed(() => {
    return items.value.reduce((sum, item) => sum + item.quantity, 0);
  });

  const totalAmount = computed(() => {
    return items.value.reduce((sum, item) => sum + item.price * item.quantity, 0);
  });

  function addItem(item: CartItem) {
    const existingItem = items.value.find(
      (i) => i.id === item.id && i.type === item.type
    );
    if (existingItem) {
      existingItem.quantity += item.quantity;
    } else {
      items.value.push({ ...item });
    }
  }

  function updateQuantity(id: number, type: 'food' | 'combo', quantity: number) {
    const item = items.value.find((i) => i.id === id && i.type === type);
    if (item) {
      item.quantity = quantity;
      if (item.quantity <= 0) {
        removeItem(id, type);
      }
    }
  }

  function removeItem(id: number, type: 'food' | 'combo') {
    items.value = items.value.filter((i) => !(i.id === id && i.type === type));
  }

  function clearCart() {
    items.value = [];
  }

  return {
    items,
    totalItems,
    totalAmount,
    addItem,
    updateQuantity,
    removeItem,
    clearCart,
  };
});
