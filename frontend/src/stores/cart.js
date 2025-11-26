import { defineStore } from 'pinia'
import { ref, computed } from 'vue'

export const useCartStore = defineStore('cart', () => {
  const items = ref([])

  const totalItems = computed(() => {
    return items.value.reduce((total, item) => total + item.quantity, 0)
  })

  const totalAmount = computed(() => {
    return items.value.reduce((total, item) => total + (item.price * item.quantity), 0)
  })

  function addItem(item) {
    const itemId = item.foodItemId || item.id
    const existingItem = items.value.find(i => {
      const existingId = i.foodItemId || i.id
      return existingId === itemId && i.type === 'food'
    })
    
    if (existingItem) {
      existingItem.quantity++
    } else {
      items.value.push({ 
        ...item, 
        id: itemId,
        quantity: 1, 
        type: 'food' 
      })
    }
  }

  function addCombo(combo) {
    const comboId = combo.comboId || combo.id
    const existingItem = items.value.find(i => {
      const existingId = i.comboId || i.id
      return existingId === comboId && i.type === 'combo'
    })
    
    if (existingItem) {
      existingItem.quantity++
    } else {
      items.value.push({ 
        ...combo, 
        id: comboId,
        quantity: 1, 
        type: 'combo' 
      })
    }
  }

  function removeItem(item) {
    const index = items.value.findIndex(i => i.id === item.id && i.type === item.type)
    if (index > -1) {
      items.value.splice(index, 1)
    }
  }

  function updateQuantity(item, quantity) {
    const existingItem = items.value.find(i => i.id === item.id && i.type === item.type)
    if (existingItem) {
      existingItem.quantity = quantity
      if (existingItem.quantity <= 0) {
        removeItem(item)
      }
    }
  }

  function increaseQuantity(item) {
    const existingItem = items.value.find(i => i.id === item.id && i.type === item.type)
    if (existingItem) {
      existingItem.quantity++
    }
  }

  function decreaseQuantity(item) {
    const existingItem = items.value.find(i => i.id === item.id && i.type === item.type)
    if (existingItem && existingItem.quantity > 1) {
      existingItem.quantity--
    }
  }

  function clearCart() {
    items.value = []
  }

  return {
    items,
    totalItems,
    totalAmount,
    addItem,
    addCombo,
    removeItem,
    updateQuantity,
    increaseQuantity,
    decreaseQuantity,
    clearCart
  }
})
