<script setup>
import { ref, computed, onMounted } from 'vue'
import { useCartStore } from '../stores/cart'
import { useRouter } from 'vue-router'
import { useAuthStore } from '../stores/auth'
import api from '../services/api'

const cartStore = useCartStore()
const authStore = useAuthStore()
const router = useRouter()

const deliveryAddress = ref('')
const paymentMethod = ref('Cash on Delivery')
const isSubmitting = ref(false)
const error = ref('')

const paymentMethods = [
  { id: 'Cash on Delivery', name: 'Cash on Delivery', icon: '💵', description: 'Pay when you receive your order' },
  { id: 'Credit Card', name: 'Credit Card', icon: '💳', description: 'Secure online payment' }
]

onMounted(() => {
  if (cartStore.items.length === 0) {
    router.push('/menu')
  }
  
  // Pre-fill address if available in user profile
  if (authStore.user && authStore.user.address) {
    deliveryAddress.value = authStore.user.address
  }
})

const submitOrder = async () => {
  if (!deliveryAddress.value.trim()) {
    error.value = 'Please enter a delivery address'
    return
  }

  isSubmitting.value = true
  error.value = ''

  try {
    const orderItems = cartStore.items.map(item => ({
      foodItemId: item.type === 'food' ? item.id : null,
      comboId: item.type === 'combo' ? item.id : null,
      quantity: item.quantity
    }))

    const response = await api.post('/orders', {
      items: orderItems,
      deliveryAddress: deliveryAddress.value,
      paymentMethod: paymentMethod.value
    })

    const order = response.data
    cartStore.clearCart()
    router.push(`/order-confirmation/${order.orderId}`)
  } catch (err) {
    error.value = err.response?.data?.message || err.message || 'Failed to place order'
  } finally {
    isSubmitting.value = false
  }
}

function formatPrice(price) {
  return new Intl.NumberFormat('vi-VN', { style: 'currency', currency: 'VND' }).format(price * 1000)
}

function calculateTotal() {
  const subtotal = cartStore.totalAmount;
  const tax = subtotal * 0.1;
  const deliveryFee = 15; // 15k VND
  const total = subtotal + tax + deliveryFee;
  return formatPrice(total);
}
</script>

<template>
  <div class="min-h-screen bg-gray-50 py-12">
    <div class="container mx-auto px-4">
      <!-- Breadcrumb / Progress -->
      <div class="max-w-4xl mx-auto mb-10">
        <div class="flex items-center justify-center space-x-4">
          <div class="flex items-center text-green-600">
            <div class="w-8 h-8 rounded-full bg-green-100 flex items-center justify-center font-bold mr-2">✓</div>
            <span class="font-medium">Giỏ hàng</span>
          </div>
          <div class="w-16 h-1 bg-gray-200 rounded-full overflow-hidden">
            <div class="h-full bg-primary w-full"></div>
          </div>
          <div class="flex items-center text-primary">
            <div class="w-8 h-8 rounded-full bg-primary text-white flex items-center justify-center font-bold mr-2">2</div>
            <span class="font-bold">Thanh toán</span>
          </div>
          <div class="w-16 h-1 bg-gray-200 rounded-full"></div>
          <div class="flex items-center text-gray-400">
            <div class="w-8 h-8 rounded-full bg-gray-100 flex items-center justify-center font-bold mr-2">3</div>
            <span class="font-medium">Hoàn tất</span>
          </div>
        </div>
      </div>

      <div class="grid grid-cols-1 lg:grid-cols-3 gap-8 max-w-6xl mx-auto">
        <!-- Left Column: Forms -->
        <div class="lg:col-span-2 space-y-6">
          <h1 class="text-3xl font-heading font-bold text-gray-900 mb-6">Chi tiết thanh toán</h1>

          <!-- Delivery Address -->
          <div class="bg-white rounded-2xl shadow-sm border border-gray-100 p-6 md:p-8">
            <h2 class="text-xl font-bold text-gray-900 mb-6 flex items-center">
              <span class="w-8 h-8 rounded-lg bg-blue-100 text-blue-600 flex items-center justify-center mr-3 text-lg">📍</span>
              Địa chỉ giao hàng
            </h2>
            <div class="space-y-4">
              <label class="block text-sm font-medium text-gray-700">Chúng tôi nên giao đơn hàng của bạn đến đâu?</label>
              <textarea
                v-model="deliveryAddress"
                rows="3"
                class="w-full px-4 py-3 border border-gray-200 rounded-xl focus:ring-2 focus:ring-primary/20 focus:border-primary transition-all resize-none"
                placeholder="Nhập địa chỉ giao hàng đầy đủ của bạn (Số nhà, Đường, Phường/Xã, Quận/Huyện)..."
                required
              ></textarea>
              <p class="text-sm text-gray-500">Chúng tôi sẽ giao hàng đến địa chỉ này. Vui lòng đảm bảo nó chính xác.</p>
            </div>
          </div>

          <!-- Payment Method -->
          <div class="bg-white rounded-2xl shadow-sm border border-gray-100 p-6 md:p-8">
            <h2 class="text-xl font-bold text-gray-900 mb-6 flex items-center">
              <span class="w-8 h-8 rounded-lg bg-yellow-100 text-yellow-600 flex items-center justify-center mr-3 text-lg">💳</span>
              Phương thức thanh toán
            </h2>
            <div class="grid grid-cols-1 md:grid-cols-2 gap-4">
              <label
                v-for="method in paymentMethods"
                :key="method.id"
                class="relative flex flex-col p-4 border-2 rounded-xl cursor-pointer transition-all hover:bg-gray-50"
                :class="paymentMethod === method.id ? 'border-primary bg-primary/5' : 'border-gray-100'"
              >
                <input
                  type="radio"
                  v-model="paymentMethod"
                  :value="method.id"
                  class="absolute top-4 right-4 w-5 h-5 text-primary border-gray-300 focus:ring-primary"
                >
                <span class="text-3xl mb-3">{{ method.icon }}</span>
                <span class="font-bold text-gray-900 mb-1">{{ method.name === 'Cash on Delivery' ? 'Thanh toán khi nhận hàng' : 'Thẻ tín dụng' }}</span>
                <span class="text-sm text-gray-500">{{ method.description === 'Pay when you receive your order' ? 'Thanh toán khi bạn nhận được đơn hàng' : 'Thanh toán trực tuyến an toàn' }}</span>
              </label>
            </div>
          </div>
        </div>

        <!-- Right Column: Order Summary -->
        <div class="lg:col-span-1">
          <div class="bg-white rounded-2xl shadow-lg border border-gray-100 p-6 sticky top-8">
            <h2 class="text-xl font-bold text-gray-900 mb-6">Tóm tắt đơn hàng</h2>
            
            <div class="space-y-4 mb-6 max-h-80 overflow-y-auto pr-2 scrollbar-thin">
              <div v-for="item in cartStore.items" :key="`${item.type}-${item.id}`" class="flex justify-between items-start py-3 border-b border-gray-50 last:border-0">
                <div class="flex-1">
                  <div class="flex items-center gap-2">
                    <span class="font-bold text-gray-900 text-sm bg-gray-100 px-2 py-0.5 rounded text-xs">{{ item.quantity }}x</span>
                    <h3 class="font-medium text-gray-800 text-sm">{{ item.name }}</h3>
                  </div>
                </div>
                <p class="font-medium text-gray-800 text-sm">
                  {{ formatPrice(item.price * item.quantity) }}
                </p>
              </div>
            </div>

            <div class="border-t border-gray-100 pt-4 space-y-3">
              <div class="flex justify-between text-gray-600 text-sm">
                <span>Tạm tính</span>
                <span>{{ formatPrice(cartStore.totalAmount) }}</span>
              </div>
              <div class="flex justify-between text-gray-600 text-sm">
                <span>Thuế (10%)</span>
                <span>{{ formatPrice(cartStore.totalAmount * 0.1) }}</span>
              </div>
              <div class="flex justify-between text-gray-600 text-sm">
                <span>Phí giao hàng</span>
                <span>{{ formatPrice(15) }}</span>
              </div>
              <div class="flex justify-between text-xl font-bold text-gray-900 pt-4 border-t border-gray-100 mt-2">
                <span>Tổng cộng</span>
                <span class="text-primary">{{ calculateTotal() }}</span>
              </div>
            </div>

            <div v-if="error" class="mt-4 p-4 bg-red-50 text-red-700 rounded-xl text-sm border border-red-100 flex items-start">
              <span class="mr-2">⚠️</span>
              {{ error }}
            </div>

            <button
              @click="submitOrder"
              :disabled="isSubmitting"
              class="w-full mt-6 bg-primary text-white py-4 px-6 rounded-xl font-bold text-lg hover:bg-primary-hover transition-all transform hover:-translate-y-1 shadow-lg shadow-primary/30 disabled:opacity-50 disabled:cursor-not-allowed disabled:transform-none flex justify-center items-center"
            >
              <span v-if="isSubmitting" class="animate-spin mr-2">⏳</span>
              {{ isSubmitting ? 'Đang xử lý đơn hàng...' : 'Đặt hàng' }}
            </button>
            
            <p class="text-center text-xs text-gray-400 mt-4">
              Bằng việc đặt hàng, bạn đồng ý với Điều khoản dịch vụ của chúng tôi.
            </p>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>
