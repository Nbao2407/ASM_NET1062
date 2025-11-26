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

      <!-- Combo Detail -->
      <div v-else-if="combo" class="bg-white rounded-2xl shadow-lg overflow-hidden">
        <div class="grid grid-cols-1 lg:grid-cols-2 gap-8">
          <!-- Image Section -->
          <div class="relative">
            <img
              :src="displayImageUrl"
              :alt="combo.name"
              class="w-full h-full object-cover"
              @error="handleImageError"
            />
            <div v-if="!combo.isAvailable" class="absolute inset-0 bg-black bg-opacity-50 flex items-center justify-center">
              <span class="bg-red-500 text-white px-6 py-3 rounded-full font-bold text-lg">
                Tạm hết hàng
              </span>
            </div>
            <!-- Combo Badge -->
            <div class="absolute top-4 left-4">
              <span class="bg-purple-600 text-white px-4 py-2 rounded-full text-sm font-bold shadow-lg flex items-center gap-2">
                <svg class="w-4 h-4" fill="currentColor" viewBox="0 0 20 20">
                  <path d="M3 4a1 1 0 011-1h12a1 1 0 011 1v2a1 1 0 01-1 1H4a1 1 0 01-1-1V4zM3 10a1 1 0 011-1h6a1 1 0 011 1v6a1 1 0 01-1 1H4a1 1 0 01-1-1v-6zM14 9a1 1 0 00-1 1v6a1 1 0 001 1h2a1 1 0 001-1v-6a1 1 0 00-1-1h-2z" />
                </svg>
                COMBO
              </span>
            </div>
            <!-- Discount Badge (if applicable) -->
            <div v-if="combo.discountPercent" class="absolute top-4 right-4">
              <span class="bg-red-500 text-white px-4 py-2 rounded-full text-sm font-bold shadow-lg">
                -{{ combo.discountPercent }}%
              </span>
            </div>
          </div>

          <!-- Info Section -->
          <div class="p-8 pb-32 md:pb-8">
            <!-- Title & Price -->
            <div class="mb-8 border-b border-gray-100 pb-6">
              <h1 class="text-3xl md:text-4xl font-heading font-bold text-gray-900 mb-4 leading-tight">{{ combo.name }}</h1>
              <div class="flex items-end gap-4">
                <span class="text-4xl font-bold text-primary">
                  {{ formatPrice(combo.price) }}
                </span>
                <span v-if="combo.originalPrice" class="text-xl text-gray-400 line-through mb-1">
                  {{ formatPrice(combo.originalPrice) }}
                </span>
                <span v-if="combo.savings" class="px-3 py-1 bg-green-100 text-green-700 rounded-full text-sm font-bold mb-1">
                  Tiết kiệm {{ formatPrice(combo.savings) }}
                </span>
              </div>
            </div>

            <!-- Description -->
            <div class="mb-8">
              <h2 class="text-lg font-bold text-gray-900 mb-3 flex items-center gap-2">
                <svg class="w-5 h-5 text-gray-400" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M4 6h16M4 12h16M4 18h7" /></svg>
                Mô tả
              </h2>
              <p class="text-gray-600 leading-relaxed text-base">
                {{ combo.description || 'Combo đặc biệt với nhiều món ăn ngon.' }}
              </p>
            </div>

            <!-- Combo Items List (Improved) -->
            <div class="mb-8">
              <h2 class="text-lg font-bold text-gray-900 mb-4 flex items-center gap-2">
                <svg class="w-5 h-5 text-gray-400" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M19 11H5m14 0a2 2 0 012 2v6a2 2 0 01-2 2H5a2 2 0 01-2-2v-6a2 2 0 012-2m14 0V9a2 2 0 00-2-2M5 11V9a2 2 0 012-2m0 0V5a2 2 0 012-2h6a2 2 0 012 2v2M7 7h10" /></svg>
                Combo bao gồm
              </h2>
              <ul class="space-y-3">
                <li
                  v-for="(item, index) in combo.items"
                  :key="index"
                  class="flex items-start gap-3 group"
                >
                  <div class="w-6 h-6 rounded-full bg-green-100 flex items-center justify-center flex-shrink-0 mt-0.5 group-hover:bg-green-200 transition-colors">
                    <svg class="w-4 h-4 text-green-600" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                      <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M5 13l4 4L19 7" />
                    </svg>
                  </div>
                  <div class="flex-1 border-b border-gray-100 pb-2 group-last:border-0">
                    <div class="flex justify-between items-start">
                      <span class="font-medium text-gray-900 text-lg">{{ item.name }}</span>
                      <span class="text-sm font-bold text-gray-500 bg-gray-100 px-2 py-0.5 rounded">x{{ item.quantity }}</span>
                    </div>
                    <p v-if="item.description" class="text-sm text-gray-500 mt-0.5">{{ item.description }}</p>
                  </div>
                </li>
              </ul>
            </div>

            <!-- Additional Info (Compact Cards) -->
            <div v-if="combo.servingSize || combo.estimatedCalories" class="mb-8">
              <h2 class="text-lg font-bold text-gray-900 mb-4">Thông tin thêm</h2>
              <div class="flex gap-4">
                <div v-if="combo.servingSize" class="flex-1 bg-blue-50 rounded-xl p-4 flex items-center gap-3 border border-blue-100">
                  <div class="w-10 h-10 rounded-full bg-white flex items-center justify-center text-xl shadow-sm">👥</div>
                  <div>
                    <p class="text-xs text-blue-600 font-semibold uppercase tracking-wider">Phục vụ</p>
                    <p class="text-lg font-bold text-gray-900">{{ combo.servingSize }} người</p>
                  </div>
                </div>
                <div v-if="combo.estimatedCalories" class="flex-1 bg-orange-50 rounded-xl p-4 flex items-center gap-3 border border-orange-100">
                  <div class="w-10 h-10 rounded-full bg-white flex items-center justify-center text-xl shadow-sm">🔥</div>
                  <div>
                    <p class="text-xs text-orange-600 font-semibold uppercase tracking-wider">Calo</p>
                    <p class="text-lg font-bold text-gray-900">{{ combo.estimatedCalories }} kcal</p>
                  </div>
                </div>
              </div>
            </div>

            <!-- Allergen Warning -->
            <div v-if="combo.allergens && combo.allergens.length > 0" class="mb-8">
              <div class="bg-yellow-50 border border-yellow-200 rounded-xl p-4 flex gap-3">
                <svg class="w-6 h-6 text-yellow-600 flex-shrink-0" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                  <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 9v2m0 4h.01m-6.938 4h13.856c1.54 0 2.502-1.667 1.732-3L13.732 4c-.77-1.333-2.694-1.333-3.464 0L3.34 16c-.77 1.333.192 3 1.732 3z" />
                </svg>
                <div>
                  <p class="font-bold text-yellow-800 mb-2">Cảnh báo dị ứng</p>
                  <div class="flex flex-wrap gap-2">
                    <span v-for="allergen in combo.allergens" :key="allergen" class="bg-white text-yellow-800 px-3 py-1 rounded-full text-sm font-medium border border-yellow-100 shadow-sm">
                      {{ allergen }}
                    </span>
                  </div>
                </div>
              </div>
            </div>

            <!-- Sticky Action Bar (Mobile) / Static (Desktop) -->
            <div class="fixed bottom-0 left-0 right-0 p-4 bg-white border-t border-gray-200 shadow-[0_-4px_6px_-1px_rgba(0,0,0,0.1)] z-40 md:static md:bg-transparent md:border-0 md:shadow-none md:p-0">
              <div class="max-w-7xl mx-auto md:max-w-none flex flex-col md:flex-row items-stretch md:items-center gap-4">
                <!-- Quantity -->
                <div class="flex items-center justify-between md:justify-start gap-4 bg-gray-50 rounded-xl p-2 md:bg-transparent md:p-0">
                  <span class="text-gray-700 font-medium md:hidden">Số lượng:</span>
                  <div class="flex items-center gap-3">
                    <button
                      @click="decrementQuantity"
                      :disabled="quantity <= 1"
                      class="w-10 h-10 rounded-lg bg-white border border-gray-200 flex items-center justify-center hover:bg-gray-50 disabled:opacity-50 disabled:cursor-not-allowed transition-colors shadow-sm"
                    >
                      <svg class="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M20 12H4" /></svg>
                    </button>
                    <span class="text-xl font-bold w-8 text-center">{{ quantity }}</span>
                    <button
                      @click="incrementQuantity"
                      class="w-10 h-10 rounded-lg bg-white border border-gray-200 flex items-center justify-center hover:bg-gray-50 transition-colors shadow-sm"
                    >
                      <svg class="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 4v16m8-8H4" /></svg>
                    </button>
                  </div>
                </div>

                <!-- Buttons -->
                <div class="flex gap-3 flex-1">
                  <button
                    @click="addToCart"
                    :disabled="!combo.isAvailable"
                    class="flex-1 bg-primary text-white py-3 md:py-4 rounded-xl font-bold text-lg hover:bg-primary-dark disabled:bg-gray-300 disabled:cursor-not-allowed transition-all duration-200 shadow-lg shadow-primary/30 flex items-center justify-center gap-2"
                  >
                    <svg class="w-6 h-6" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                      <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M3 3h2l.4 2M7 13h10l4-8H5.4M7 13L5.4 5M7 13l-2.293 2.293c-.63.63-.184 1.707.707 1.707H17m0 0a2 2 0 100 4 2 2 0 000-4zm-8 2a2 2 0 11-4 0 2 2 0 014 0z" />
                    </svg>
                    <span class="hidden sm:inline">{{ !combo.isAvailable ? 'Tạm hết hàng' : 'Thêm vào giỏ' }}</span>
                    <span class="sm:hidden">{{ !combo.isAvailable ? 'Hết hàng' : 'Thêm vào giỏ' }}</span>
                  </button>
                </div>
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
import { comboService } from '@/services/comboService'

const route = useRoute()
const router = useRouter()
const cartStore = useCartStore()

const combo = ref(null)
const loading = ref(true)
const error = ref(null)
const quantity = ref(1)
const imageError = ref(false)

const fallbackImageUrl = `https://ui-avatars.com/api/?name=${encodeURIComponent('Combo')}&size=800&background=9333ea&color=fff&bold=true`

const displayImageUrl = computed(() => {
  if (imageError.value) return fallbackImageUrl
  if (!combo.value) return fallbackImageUrl
  
  let url = combo.value.imageUrl
  if (url && url.startsWith('photo-')) {
    return `https://images.unsplash.com/${url}`
  }
  return url || fallbackImageUrl
})

async function loadCombo() {
  try {
    loading.value = true
    error.value = null
    
    const id = route.params.id
    const item = await comboService.getById(id)
    item.isAvailable = item.isActive
    
    // Map API response to view model
    if (item.comboItems) {
      item.items = item.comboItems.map(ci => ({
        name: ci.foodItemName,
        description: '', // API doesn't return description for items in combo
        quantity: ci.quantity
      }))
    }

    // Add mock additional info for demonstration
    if (!item.servingSize) {
      item.servingSize = Math.floor(Math.random() * 3) + 2 // 2-4 people
    }
    
    if (!item.estimatedCalories) {
      item.estimatedCalories = Math.floor(Math.random() * 1000) + 800 // 800-1800 kcal
    }
    
    if (!item.allergens) {
      const possibleAllergens = ['Gluten', 'Sữa', 'Trứng', 'Đậu phộng', 'Hải sản', 'Đậu nành']
      const numAllergens = Math.floor(Math.random() * 4)
      item.allergens = possibleAllergens.slice(0, numAllergens)
    }
    
    if (!item.specialNotes) {
      const notes = [
        'Có thể tùy chỉnh món ăn theo yêu cầu',
        'Miễn phí giao hàng cho đơn từ 200.000đ',
        'Giá đã bao gồm VAT',
        'Thời gian chuẩn bị khoảng 15-20 phút'
      ]
      item.specialNotes = notes[Math.floor(Math.random() * notes.length)]
    }
    
    if (!item.discountPercent && Math.random() > 0.5) {
      item.discountPercent = Math.floor(Math.random() * 20) + 10 // 10-30%
    }
    
    combo.value = item
  } catch (err) {
    error.value = 'Không thể tải thông tin combo. Vui lòng thử lại sau.'
    console.error('Error loading combo:', err)
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
  if (!combo.value.isAvailable) return
  
  for (let i = 0; i < quantity.value; i++) {
    cartStore.addCombo(combo.value)
  }
  
  // Show success notification
  alert(`Đã thêm ${quantity.value} combo ${combo.value.name} vào giỏ hàng!`)
  quantity.value = 1
}

onMounted(() => {
  loadCombo()
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
