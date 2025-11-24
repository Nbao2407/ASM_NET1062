<template>
  <div class="checkout-view">
    <div class="container mx-auto px-4 py-8">
      <h1 class="text-3xl font-bold mb-6">Checkout</h1>

      <!-- Order Confirmation Success -->
      <div v-if="orderConfirmed" class="max-w-2xl mx-auto">
        <div class="bg-white rounded-lg shadow-md p-8 text-center">
          <div class="text-green-500 mb-4">
            <svg
              class="w-20 h-20 mx-auto"
              fill="none"
              stroke="currentColor"
              viewBox="0 0 24 24"
            >
              <path
                stroke-linecap="round"
                stroke-linejoin="round"
                stroke-width="2"
                d="M9 12l2 2 4-4m6 2a9 9 0 11-18 0 9 9 0 0118 0z"
              />
            </svg>
          </div>
          <h2 class="text-2xl font-bold text-gray-800 mb-2">Order Confirmed!</h2>
          <p class="text-gray-600 mb-4">Thank you for your order</p>
          <div class="bg-gray-50 rounded-lg p-4 mb-6">
            <p class="text-sm text-gray-600 mb-1">Order Number</p>
            <p class="text-2xl font-bold text-blue-600">{{ confirmedOrderNumber }}</p>
          </div>
          <p class="text-gray-600 mb-6">
            We've received your order and will start preparing it shortly. You can track your
            order status in your order history.
          </p>
          <div class="flex gap-4 justify-center">
            <router-link
              to="/menu"
              class="bg-gray-200 text-gray-700 px-6 py-3 rounded-lg hover:bg-gray-300 transition font-semibold"
            >
              Continue Shopping
            </router-link>
            <router-link
              to="/profile"
              class="bg-blue-600 text-white px-6 py-3 rounded-lg hover:bg-blue-700 transition font-semibold"
            >
              View Orders
            </router-link>
          </div>
        </div>
      </div>

      <!-- Checkout Form -->
      <div v-else class="grid grid-cols-1 lg:grid-cols-3 gap-8">
        <!-- Checkout Form -->
        <div class="lg:col-span-2">
          <form @submit.prevent="submitOrder" class="space-y-6">
            <!-- Delivery Address -->
            <div class="bg-white rounded-lg shadow-md p-6">
              <h2 class="text-xl font-bold mb-4">Delivery Address</h2>
              <div class="space-y-4">
                <div>
                  <label for="address" class="block text-sm font-medium text-gray-700 mb-1">
                    Street Address *
                  </label>
                  <textarea
                    id="address"
                    v-model="deliveryAddress"
                    rows="3"
                    required
                    class="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-transparent"
                    placeholder="Enter your delivery address"
                  ></textarea>
                </div>
              </div>
            </div>

            <!-- Payment Method -->
            <div class="bg-white rounded-lg shadow-md p-6">
              <h2 class="text-xl font-bold mb-4">Payment Method</h2>
              <div class="space-y-3">
                <label
                  v-for="method in paymentMethods"
                  :key="method.value"
                  class="flex items-center p-4 border rounded-lg cursor-pointer hover:bg-gray-50 transition"
                  :class="{ 'border-blue-500 bg-blue-50': paymentMethod === method.value }"
                >
                  <input
                    type="radio"
                    v-model="paymentMethod"
                    :value="method.value"
                    class="w-4 h-4 text-blue-600"
                    required
                  />
                  <span class="ml-3 font-medium">{{ method.label }}</span>
                </label>
              </div>
            </div>

            <!-- Error Message -->
            <div v-if="errorMessage" class="bg-red-50 border border-red-200 rounded-lg p-4">
              <p class="text-red-700">{{ errorMessage }}</p>
            </div>

            <!-- Submit Button -->
            <div class="flex gap-4">
              <button
                type="button"
                @click="goBack"
                class="flex-1 bg-gray-200 text-gray-700 py-3 rounded-lg hover:bg-gray-300 transition font-semibold"
              >
                Back to Cart
              </button>
              <button
                type="submit"
                :disabled="isSubmitting"
                class="flex-1 bg-blue-600 text-white py-3 rounded-lg hover:bg-blue-700 transition font-semibold disabled:bg-gray-400 disabled:cursor-not-allowed"
              >
                {{ isSubmitting ? 'Processing...' : 'Place Order' }}
              </button>
            </div>
          </form>
        </div>

        <!-- Order Summary -->
        <div class="lg:col-span-1">
          <div class="bg-white rounded-lg shadow-md p-6 sticky top-4">
            <h2 class="text-xl font-bold mb-4">Order Summary</h2>

            <!-- Items List -->
            <div class="space-y-3 mb-4 max-h-64 overflow-y-auto">
              <div
                v-for="item in cartStore.items"
                :key="`${item.type}-${item.id}`"
                class="flex justify-between text-sm"
              >
                <div class="flex-1">
                  <p class="font-medium">{{ item.name }}</p>
                  <p class="text-gray-500">Qty: {{ item.quantity }}</p>
                </div>
                <p class="font-semibold">${{ (item.price * item.quantity).toFixed(2) }}</p>
              </div>
            </div>

            <div class="border-t pt-4 space-y-3">
              <div class="flex justify-between text-gray-600">
                <span>Subtotal ({{ cartStore.totalItems }} items)</span>
                <span>${{ cartStore.totalAmount.toFixed(2) }}</span>
              </div>
              <div class="flex justify-between text-gray-600">
                <span>Tax (10%)</span>
                <span>${{ (cartStore.totalAmount * 0.1).toFixed(2) }}</span>
              </div>
              <div class="flex justify-between text-gray-600">
                <span>Delivery Fee</span>
                <span>$5.00</span>
              </div>
              <div class="border-t pt-3 flex justify-between font-bold text-lg">
                <span>Total</span>
                <span>${{ calculateTotal() }}</span>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue';
import { useCartStore } from '@/stores/cart';
import { useAuthStore } from '@/stores/auth';
import { useRouter } from 'vue-router';
import { orderService, type CreateOrderRequest } from '@/services/orderService';

const cartStore = useCartStore();
const authStore = useAuthStore();
const router = useRouter();

const deliveryAddress = ref('');
const paymentMethod = ref('CreditCard');
const isSubmitting = ref(false);
const errorMessage = ref('');
const orderConfirmed = ref(false);
const confirmedOrderNumber = ref('');

const paymentMethods = [
  { value: 'CreditCard', label: 'Credit Card' },
  { value: 'DebitCard', label: 'Debit Card' },
  { value: 'Cash', label: 'Cash on Delivery' },
  { value: 'GooglePay', label: 'Google Pay' },
  { value: 'ApplePay', label: 'Apple Pay' },
];

onMounted(() => {
  // Redirect to cart if cart is empty
  if (cartStore.items.length === 0) {
    router.push('/cart');
  }

  // Redirect to login if not authenticated
  if (!authStore.isAuthenticated()) {
    router.push({ name: 'login', query: { redirect: '/checkout' } });
  }
});

function calculateTotal(): string {
  const subtotal = cartStore.totalAmount;
  const tax = subtotal * 0.1;
  const deliveryFee = 5.0;
  const total = subtotal + tax + deliveryFee;
  return total.toFixed(2);
}

async function submitOrder() {
  if (!deliveryAddress.value.trim()) {
    errorMessage.value = 'Please enter a delivery address';
    return;
  }

  if (!paymentMethod.value) {
    errorMessage.value = 'Please select a payment method';
    return;
  }

  isSubmitting.value = true;
  errorMessage.value = '';

  try {
    // Prepare order items
    const items = cartStore.items.map((item) => ({
      foodItemId: item.type === 'food' ? item.id : undefined,
      comboId: item.type === 'combo' ? item.id : undefined,
      quantity: item.quantity,
    }));

    const orderData: CreateOrderRequest = {
      items,
      deliveryAddress: deliveryAddress.value,
      paymentMethod: paymentMethod.value,
    };

    // Submit order
    const order = await orderService.create(orderData);

    // Show confirmation
    confirmedOrderNumber.value = order.orderNumber;
    orderConfirmed.value = true;

    // Clear cart
    cartStore.clearCart();
  } catch (error: any) {
    console.error('Order submission error:', error);
    errorMessage.value =
      error.response?.data?.message || 'Failed to place order. Please try again.';
  } finally {
    isSubmitting.value = false;
  }
}

function goBack() {
  router.push('/cart');
}
</script>

<style scoped>
.checkout-view {
  min-height: calc(100vh - 200px);
  background-color: #f9fafb;
}
</style>
