<template>
  <div class="min-h-screen bg-gray-50 py-8">
    <div class="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
      <!-- Back Button -->
      <button
        @click="router.back()"
        class="mb-6 flex items-center gap-2 text-gray-600 hover:text-gray-900 transition-colors"
      >
        <svg class="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24">
          <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M15 19l-7-7 7-7" />
        </svg>
        Quay lại
      </button>

      <!-- Loading State -->
      <div v-if="loading" class="flex justify-center items-center h-96">
        <div class="animate-spin rounded-full h-16 w-16 border-b-2 border-red-500"></div>
      </div>

      <!-- Error State -->
      <div v-else-if="error" class="bg-red-50 border border-red-200 rounded-xl p-6 text-center">
        <p class="text-red-600 font-medium">{{ error }}</p>
      </div>

      <!-- Food Item Detail -->
      <div v-else-if="foodItem" class="bg-white rounded-2xl shadow-lg overflow-hidden">
        <div class="grid grid-cols-1 lg:grid-cols-2 gap-8">
          <!-- Image Section -->
          <div class="relative">
            <img
              :src="displayImageUrl"
              :alt="foodItem.name"
              class="w-full h-full object-cover"
              @error="handleImageError"
            />
            <div v-if="!foodItem.isAvailable" class="absolute inset-0 bg-black bg-opacity-50 flex items-center justify-center">
              <span class="bg-red-500 text-white px-6 py-3 rounded-full font-bold text-lg">
                Tạm hết hàng
              </span>
            </div>
            <div v-if="foodItem.category" class="absolute top-4 left-4">
              <span class="bg-white text-red-500 px-4 py-2 rounded-full text-sm font-bold shadow-lg">
                {{ foodItem.category }}
              </span>
            </div>
          </div>

          <!-- Info Section -->
          <div class="p-8">
            <!-- Title & Price -->
            <div class="mb-6">
              <h1 class="text-4xl font-bold text-gray-900 mb-3">{{ foodItem.name }}</h1>
              <div class="flex items-baseline gap-3">
                <span class="text-3xl font-bold text-red-500">
                  {{ formatPrice(foodItem.price) }}
                </span>
                <span v-if="foodItem.originalPrice" class="text-xl text-gray-400 line-through">
                  {{ formatPrice(foodItem.originalPrice) }}
                </span>
              </div>
            </div>

            <!-- Rating (if available) -->
            <div v-if="foodItem.rating" class="flex items-center gap-2 mb-6">
              <div class="flex gap-1">
                <svg v-for="i in 5" :key="i" class="w-5 h-5" :class="i <= foodItem.rating ? 'text-yellow-400' : 'text-gray-300'" fill="currentColor" viewBox="0 0 20 20">
                  <path d="M9.049 2.927c.3-.921 1.603-.921 1.902 0l1.07 3.292a1 1 0 00.95.69h3.462c.969 0 1.371 1.24.588 1.81l-2.8 2.034a1 1 0 00-.364 1.118l1.07 3.292c.3.921-.755 1.688-1.54 1.118l-2.8-2.034a1 1 0 00-1.175 0l-2.8 2.034c-.784.57-1.838-.197-1.539-1.118l1.07-3.292a1 1 0 00-.364-1.118L2.98 8.72c-.783-.57-.38-1.81.588-1.81h3.461a1 1 0 00.951-.69l1.07-3.292z" />
                </svg>
              </div>
              <span class="text-gray-600 text-sm">({{ foodItem.rating }}/5)</span>
            </div>

            <!-- Description -->
            <div class="mb-8">
              <h2 class="text-lg font-bold text-gray-900 mb-3">Mô tả</h2>
              <p class="text-gray-600 leading-relaxed">
                {{ foodItem.description || 'Món ăn ngon tuyệt vời với hương vị đặc biệt.' }}
              </p>
            </div>

            <!-- Nutritional Info -->
            <div v-if="foodItem.nutritionalInfo" class="mb-8">
              <h2 class="text-lg font-bold text-gray-900 mb-4">Thông tin dinh dưỡng</h2>
              <div class="grid grid-cols-2 gap-4">
                <div v-if="foodItem.nutritionalInfo.calories" class="bg-gray-50 rounded-lg p-4">
                  <p class="text-sm text-gray-500 mb-1">Calo</p>
                  <p class="text-xl font-bold text-gray-900">{{ foodItem.nutritionalInfo.calories }} kcal</p>
                </div>
                <div v-if="foodItem.nutritionalInfo.protein" class="bg-gray-50 rounded-lg p-4">
                  <p class="text-sm text-gray-500 mb-1">Protein</p>
                  <p class="text-xl font-bold text-gray-900">{{ foodItem.nutritionalInfo.protein }}g</p>
                </div>
                <div v-if="foodItem.nutritionalInfo.carbs" class="bg-gray-50 rounded-lg p-4">
                  <p class="text-sm text-gray-500 mb-1">Carbs</p>
                  <p class="text-xl font-bold text-gray-900">{{ foodItem.nutritionalInfo.carbs }}g</p>
                </div>
                <div v-if="foodItem.nutritionalInfo.fat" class="bg-gray-50 rounded-lg p-4">
                  <p class="text-sm text-gray-500 mb-1">Chất béo</p>
                  <p class="text-xl font-bold text-gray-900">{{ foodItem.nutritionalInfo.fat }}g</p>
                </div>
              </div>
            </div>

            <!-- Allergen Warnings -->
            <div v-if="foodItem.allergens && foodItem.allergens.length > 0" class="mb-8">
              <h2 class="text-lg font-bold text-gray-900 mb-3">Cảnh báo dị ứng</h2>
              <div class="bg-yellow-50 border border-yellow-200 rounded-lg p-4">
                <div class="flex items-start gap-3">
                  <svg class="w-6 h-6 text-yellow-600 flex-shrink-0 mt-0.5" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                    <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 9v2m0 4h.01m-6.938 4h13.856c1.54 0 2.502-1.667 1.732-3L13.732 4c-.77-1.333-2.694-1.333-3.464 0L3.34 16c-.77 1.333.192 3 1.732 3z" />
                  </svg>
                  <div>
                    <p class="font-medium text-yellow-800 mb-2">Món này có chứa:</p>
                    <div class="flex flex-wrap gap-2">
                      <span v-for="allergen in foodItem.allergens" :key="allergen" class="bg-yellow-100 text-yellow-800 px-3 py-1 rounded-full text-sm font-medium">
                        {{ allergen }}
                      </span>
                    </div>
                  </div>
                </div>
              </div>
            </div>

            <!-- Quantity Selector & Add to Cart -->
            <div class="border-t pt-6">
              <div class="flex items-center gap-4 mb-4">
                <label class="text-gray-700 font-medium">Số lượng:</label>
                <div class="flex items-center gap-3">
                  <button
                    @click="decrementQuantity"
                    :disabled="quantity <= 1"
                    class="w-10 h-10 rounded-full border-2 border-gray-300 flex items-center justify-center hover:bg-gray-100 disabled:opacity-50 disabled:cursor-not-allowed transition-colors"
                  >
                    <svg class="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                      <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M20 12H4" />
                    </svg>
                  </button>
                  <span class="text-2xl font-bold w-12 text-center">{{ quantity }}</span>
                  <button
                    @click="incrementQuantity"
                    class="w-10 h-10 rounded-full border-2 border-gray-300 flex items-center justify-center hover:bg-gray-100 transition-colors"
                  >
                    <svg class="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                      <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 4v16m8-8H4" />
                    </svg>
                  </button>
                </div>
              </div>

              <div class="flex gap-3">
                <button
                  @click="addToCart"
                  :disabled="!foodItem.isAvailable"
                  class="flex-1 bg-red-500 text-white py-4 rounded-xl font-bold text-lg hover:bg-red-600 disabled:bg-gray-300 disabled:cursor-not-allowed transition-all duration-200 transform hover:scale-105 active:scale-95 flex items-center justify-center gap-2"
                >
                  <svg class="w-6 h-6" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                    <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M3 3h2l.4 2M7 13h10l4-8H5.4M7 13L5.4 5M7 13l-2.293 2.293c-.63.63-.184 1.707.707 1.707H17m0 0a2 2 0 100 4 2 2 0 000-4zm-8 2a2 2 0 11-4 0 2 2 0 014 0z" />
                  </svg>
                  {{ !foodItem.isAvailable ? 'Tạm hết hàng' : 'Thêm vào giỏ hàng' }}
                </button>
                <button
                  @click="router.push('/cart')"
                  class="px-6 py-4 border-2 border-red-500 text-red-500 rounded-xl font-bold hover:bg-red-50 transition-colors"
                >
                  Xem giỏ hàng
                </button>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted, computed } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import { useCartStore } from '@/stores/cart'
import { foodItemService } from '@/services/foodItemService'

const route = useRoute()
const router = useRouter()
const cartStore = useCartStore()

const foodItem = ref(null)
const loading = ref(true)
const error = ref(null)
const quantity = ref(1)
const imageError = ref(false)

const fallbackImageUrl = `https://ui-avatars.com/api/?name=${encodeURIComponent('Food Item')}&size=800&background=ef4444&color=fff&bold=true`

const displayImageUrl = computed(() => {
  if (imageError.value) return fallbackImageUrl
  if (!foodItem.value) return fallbackImageUrl
  
  let url = foodItem.value.imageUrl
  if (url && url.startsWith('photo-')) {
    return `https://images.unsplash.com/${url}`
  }
  return url || fallbackImageUrl
})

async function loadFoodItem() {
  try {
    loading.value = true
    error.value = null
    
    const id = route.params.id
    const item = await foodItemService.getById(id)
    item.isAvailable = item.isActive
    
    // Parse JSON fields if they are strings
    if (item.nutritionalInfo && typeof item.nutritionalInfo === 'string') {
      try {
        item.nutritionalInfo = JSON.parse(item.nutritionalInfo)
      } catch (e) {
        console.error('Error parsing nutritionalInfo', e)
        item.nutritionalInfo = null
      }
    }

    if (item.allergenWarnings) {
        if (typeof item.allergenWarnings === 'string') {
            try {
                // Try parsing as JSON array
                const parsed = JSON.parse(item.allergenWarnings)
                if (Array.isArray(parsed)) {
                    item.allergens = parsed
                } else {
                    // Fallback to comma separated
                    item.allergens = item.allergenWarnings.split(',').map(s => s.trim())
                }
            } catch (e) {
                // Fallback to comma separated
                item.allergens = item.allergenWarnings.split(',').map(s => s.trim())
            }
        } else if (Array.isArray(item.allergenWarnings)) {
            item.allergens = item.allergenWarnings
        }
    }

    // Add mock nutritional info and allergens for demonstration if missing
    if (!item.nutritionalInfo) {
      item.nutritionalInfo = {
        calories: Math.floor(Math.random() * 500) + 200,
        protein: Math.floor(Math.random() * 30) + 10,
        carbs: Math.floor(Math.random() * 50) + 20,
        fat: Math.floor(Math.random() * 25) + 5
      }
    }
    
    if (!item.allergens) {
      const possibleAllergens = ['Gluten', 'Sữa', 'Trứng', 'Đậu phộng', 'Hải sản', 'Đậu nành']
      const numAllergens = Math.floor(Math.random() * 3)
      item.allergens = possibleAllergens.slice(0, numAllergens)
    }
    
    if (!item.rating) {
      item.rating = Math.floor(Math.random() * 2) + 3 // 3-5 stars
    }
    
    foodItem.value = item
  } catch (err) {
    error.value = 'Không thể tải thông tin món ăn. Vui lòng thử lại sau.'
    console.error('Error loading food item:', err)
  } finally {
    loading.value = false
  }
}

function formatPrice(price) {
  return new Intl.NumberFormat('vi-VN', {
    style: 'currency',
    currency: 'VND'
  }).format(price * 1000)
}

function handleImageError(e) {
  imageError.value = true
}

function incrementQuantity() {
  quantity.value++
}

function decrementQuantity() {
  if (quantity.value > 1) {
    quantity.value--
  }
}

function addToCart() {
  if (!foodItem.value.isAvailable) return
  
  for (let i = 0; i < quantity.value; i++) {
    cartStore.addItem(foodItem.value)
  }
  
  // Show success notification (you can implement a toast notification system)
  alert(`Đã thêm ${quantity.value} ${foodItem.value.name} vào giỏ hàng!`)
  quantity.value = 1
}

onMounted(() => {
  loadFoodItem()
})
</script>

<style scoped>
@keyframes slide-down {
  from {
    opacity: 0;
    transform: translateY(-10px);
  }
  to {
    opacity: 1;
    transform: translateY(0);
  }
}

.animate-slide-down {
  animation: slide-down 0.3s ease-out;
}
</style>
