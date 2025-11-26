<script setup>
import { useRoute, useRouter } from 'vue-router'

const route = useRoute()
const router = useRouter()
const orderId = route.params.id

const goToHome = () => {
  router.push('/')
}

const goToOrders = () => {
  // Assuming there will be a My Orders page, otherwise just go home
  router.push('/') 
}
</script>

<template>
  <div class="container mx-auto px-4 py-16 text-center">
    <div class="bg-white rounded-lg shadow-md p-8 max-w-md mx-auto">
      <div class="w-16 h-16 bg-green-100 rounded-full flex items-center justify-center mx-auto mb-6">
        <span class="text-3xl">✅</span>
      </div>
      
      <h1 class="text-2xl font-bold text-gray-800 mb-2">Đặt hàng thành công!</h1>
      <p class="text-gray-600 mb-6">
        Cảm ơn bạn đã đặt hàng. Chúng tôi đã nhận được yêu cầu của bạn và sẽ bắt đầu xử lý ngay.
      </p>

      <div class="bg-gray-50 rounded-md p-4 mb-8">
        <p class="text-sm text-gray-500 mb-1">Mã đơn hàng</p>
        <p class="text-xl font-mono font-bold text-primary">#{{ orderId }}</p>
      </div>

      <div class="space-y-3">
        <button
          @click="goToHome"
          class="w-full bg-primary text-white py-2 px-4 rounded-md font-semibold hover:bg-primary-dark transition-colors"
        >
          Tiếp tục mua sắm
        </button>
        <!-- 
        <button
          @click="goToOrders"
          class="w-full bg-white text-gray-700 border border-gray-300 py-2 px-4 rounded-md font-semibold hover:bg-gray-50 transition-colors"
        >
          View My Orders
        </button>
        -->
      </div>
    </div>
  </div>
</template>
