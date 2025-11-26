<template>
  <div class="cart-view min-h-screen bg-gray-50">
    <div class="container mx-auto px-4 py-12">
      <!-- Header -->
      <div class="mb-8">
        <h1 class="font-heading text-4xl md:text-5xl font-bold text-gray-900 mb-2">Giỏ hàng</h1>
        <p class="text-lg text-gray-600">Xem xét các mặt hàng của bạn và tiến hành thanh toán</p>
      </div>

      <!-- Empty Cart State -->
      <div v-if="cartStore.items.length === 0" class="text-center py-16 bg-white rounded-2xl shadow-sm border border-gray-100">
        <div class="text-gray-300 mb-6">
          <svg class="w-24 h-24 mx-auto" fill="none" stroke="currentColor" viewBox="0 0 24 24" stroke-width="1">
            <path stroke-linecap="round" stroke-linejoin="round" d="M3 3h2l.4 2M7 13h10l4-8H5.4M7 13L5.4 5M7 13l-2.293 2.293c-.63.63-.184 1.707.707 1.707H17m0 0a2 2 0 100 4 2 2 0 000-4zm-8 2a2 2 0 11-4 0 2 2 0 014 0z" />
          </svg>
        </div>
        <h2 class="text-2xl font-bold text-gray-800 mb-4">Giỏ hàng của bạn trống</h2>
        <p class="text-gray-600 mb-8">Thêm một số mặt hàng ngon để bắt đầu!</p>
        <router-link to="/menu" class="inline-flex items-center justify-center bg-primary text-white px-8 py-3 rounded-xl hover:bg-primary-hover transition-all transform hover:-translate-y-1 shadow-lg shadow-primary/30 font-bold text-lg">
          Xem thực đơn
        </router-link>
      </div>

      <!-- Cart Items -->
      <div v-else class="grid grid-cols-1 lg:grid-cols-3 gap-8">
        <!-- Cart Items List -->
        <div class="lg:col-span-2 space-y-4">
          <div class="bg-white rounded-2xl shadow-sm border border-gray-100 overflow-hidden">
            <!-- Cart Item -->
            <div 
              v-for="(item, index) in cartStore.items" 
              :key="`${item.type}-${item.id}`" 
              class="group flex flex-col sm:flex-row gap-6 p-6 transition-colors hover:bg-gray-50"
              :class="{ 'border-b border-gray-100': index !== cartStore.items.length - 1 }"
            >
              <!-- Item Image -->
              <div class="relative overflow-hidden rounded-xl w-full sm:w-32 h-32 flex-shrink-0">
                  <img
                  :src="getImageUrl(item)"
                  :alt="item.name"
                  @error="(e) => handleImageError(e, item)"
                  class="w-full h-full object-cover transition-transform duration-500 group-hover:scale-110"
                />
              </div>

              <!-- Item Details -->
              <div class="flex-1 flex flex-col justify-between">
                <div>
                  <div class="flex justify-between items-start">
                    <h3 class="font-heading font-bold text-xl text-gray-900 mb-1">{{ item.name }}</h3>
                    <button
                      @click="removeItem(item)"
                      class="text-gray-400 hover:text-red-500 transition-colors p-1 rounded-full hover:bg-red-50"
                      title="Remove item"
                    >
                      <svg class="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24" stroke-width="2">
                        <path stroke-linecap="round" stroke-linejoin="round" d="M19 7l-.867 12.142A2 2 0 0116.138 21H7.862a2 2 0 01-1.995-1.858L5 7m5 4v6m4-6v6m1-10V4a1 1 0 00-1-1h-4a1 1 0 00-1 1v3M4 7h16" />
                      </svg>
                    </button>
                  </div>
                  <p class="text-sm font-medium text-gray-500 bg-gray-100 inline-block px-2 py-1 rounded-md mb-2">
                    {{ item.type === 'combo' ? '🎁 Combo Deal' : '🍔 Mặt hàng thực phẩm' }}
                  </p>
                  <p class="text-primary font-bold text-lg">
                    {{ formatPrice(item.price) }}
                  </p>
                </div>

                <div class="flex justify-between items-end mt-4 sm:mt-0">
                  <!-- Quantity Controls -->
                  <div class="flex items-center bg-gray-100 rounded-lg p-1">
                    <button
                      @click="decreaseQuantity(item)"
                      class="w-8 h-8 flex items-center justify-center bg-white rounded-md shadow-sm text-gray-600 hover:text-primary hover:bg-gray-50 transition-all disabled:opacity-50 disabled:cursor-not-allowed"
                      :disabled="item.quantity <= 1"
                    >
                      <svg class="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24" stroke-width="2">
                        <path stroke-linecap="round" stroke-linejoin="round" d="M20 12H4" />
                      </svg>
                    </button>
                    <span class="w-10 text-center font-bold text-gray-900">{{ item.quantity }}</span>
                    <button
                      @click="increaseQuantity(item)"
                      class="w-8 h-8 flex items-center justify-center bg-white rounded-md shadow-sm text-gray-600 hover:text-primary hover:bg-gray-50 transition-all"
                    >
                      <svg class="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24" stroke-width="2">
                        <path stroke-linecap="round" stroke-linejoin="round" d="M12 4v16m8-8H4" />
                      </svg>
                    </button>
                  </div>

                  <!-- Item Subtotal -->
                  <div class="text-right">
                    <p class="text-xs text-gray-500 mb-1">Tạm tính</p>
                    <p class="font-bold text-xl text-gray-900">
                      {{ formatPrice(item.price * item.quantity) }}
                    </p>
                  </div>
                </div>
              </div>
            </div>
          </div>

          <div class="flex justify-start">
            <router-link to="/menu" class="text-gray-600 hover:text-primary font-medium flex items-center gap-2 transition-colors group">
              <span class="transform transition-transform group-hover:-translate-x-1">←</span> Tiếp tục mua sắm
            </router-link>
          </div>
        </div>

        <!-- Order Summary (Sticky) -->
        <div class="lg:col-span-1">
          <div class="bg-white rounded-2xl shadow-lg border border-gray-100 p-6 sticky top-24">
            <h2 class="font-heading text-2xl font-bold mb-6 text-gray-900">Tóm tắt đơn hàng</h2>

            <!-- Pricing Breakdown -->
            <div class="space-y-4 mb-6 pb-6 border-b border-gray-100">
              <div class="flex justify-between text-gray-600">
                <span>Tổng cộng</span>
                <span class="font-semibold">{{ formatPrice(cartStore.totalAmount) }}</span>
              </div>
              <div class="flex justify-between text-gray-600">
                <span>Thuế (10%)</span>
                <span class="font-semibold">{{ formatPrice(cartStore.totalAmount * 0.1) }}</span>
              </div>
              <div class="flex justify-between text-gray-600">
                <span>Phí giao hàng</span>
                <span class="font-semibold">{{ formatPrice(15) }}</span>
              </div>
            </div>

            <div class="flex justify-between items-center mb-8">
              <span class="font-heading font-bold text-xl text-gray-900">Tổng cộng</span>
              <span class="font-heading text-3xl font-bold text-primary">{{ calculateTotal() }}</span>
            </div>

            <!-- CTAs -->
            <button
              @click="proceedToCheckout"
              class="w-full bg-primary text-white py-4 rounded-xl hover:bg-primary-hover transition-all transform hover:-translate-y-1 shadow-lg shadow-primary/30 font-bold text-lg mb-4 flex items-center justify-center gap-2"
            >
              Tiến hành thanh toán
              <svg class="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24" stroke-width="2">
                <path stroke-linecap="round" stroke-linejoin="round" d="M14 5l7 7m0 0l-7 7m7-7H3" />
              </svg>
            </button>

            <p class="text-center text-sm text-gray-500">
              Thanh toán an toàn • Hoàn trả miễn phí
            </p>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { useCartStore } from '@/stores/cart';
import { useRouter } from 'vue-router';

const cartStore = useCartStore();
const router = useRouter();

function increaseQuantity(item) {
  cartStore.increaseQuantity(item);
}

function decreaseQuantity(item) {
  cartStore.decreaseQuantity(item);
}

function removeItem(item) {
  if (confirm(`Bạn có chắc chắn muốn xóa ${item.name} khỏi giỏ hàng?`)) {
    cartStore.removeItem(item);
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

function proceedToCheckout() {
  router.push('/checkout');
}

function getImageUrl(item) {
  if (!item.imageUrl) {
    return `https://ui-avatars.com/api/?name=${encodeURIComponent(item.name)}&size=400&background=f3f4f6&color=6b7280&bold=true`;
  }
  if (item.imageUrl.startsWith('photo-')) {
    return `https://images.unsplash.com/${item.imageUrl}`;
  }
  return item.imageUrl;
}

function handleImageError(event, item) {
  event.target.src = `https://ui-avatars.com/api/?name=${encodeURIComponent(item.name)}&size=400&background=f3f4f6&color=6b7280&bold=true`;
}
</script>

<style scoped>
/* Custom scrollbar for cart items if needed */
</style>
