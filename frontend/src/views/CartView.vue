<template>
  <div class="cart-view">
    <div class="container mx-auto px-4 py-8">
      <h1 class="text-3xl font-bold mb-6">Shopping Cart</h1>

      <!-- Empty Cart State -->
      <div v-if="cartStore.items.length === 0" class="text-center py-12">
        <div class="text-gray-400 mb-4">
          <svg
            class="w-24 h-24 mx-auto"
            fill="none"
            stroke="currentColor"
            viewBox="0 0 24 24"
          >
            <path
              stroke-linecap="round"
              stroke-linejoin="round"
              stroke-width="2"
              d="M3 3h2l.4 2M7 13h10l4-8H5.4M7 13L5.4 5M7 13l-2.293 2.293c-.63.63-.184 1.707.707 1.707H17m0 0a2 2 0 100 4 2 2 0 000-4zm-8 2a2 2 0 11-4 0 2 2 0 014 0z"
            />
          </svg>
        </div>
        <h2 class="text-2xl font-semibold text-gray-700 mb-2">Your cart is empty</h2>
        <p class="text-gray-500 mb-6">Add some delicious items to get started!</p>
        <router-link
          to="/menu"
          class="inline-block bg-blue-600 text-white px-6 py-3 rounded-lg hover:bg-blue-700 transition"
        >
          Browse Menu
        </router-link>
      </div>

      <!-- Cart Items -->
      <div v-else class="grid grid-cols-1 lg:grid-cols-3 gap-8">
        <!-- Cart Items List -->
        <div class="lg:col-span-2">
          <div class="bg-white rounded-lg shadow-md">
            <div
              v-for="item in cartStore.items"
              :key="`${item.type}-${item.id}`"
              class="flex items-center gap-4 p-4 border-b last:border-b-0"
            >
              <!-- Item Image -->
              <img
                :src="item.imageUrl || '/placeholder-food.jpg'"
                :alt="item.name"
                class="w-24 h-24 object-cover rounded-lg"
              />

              <!-- Item Details -->
              <div class="flex-1">
                <h3 class="font-semibold text-lg">{{ item.name }}</h3>
                <p class="text-gray-600 text-sm">
                  {{ item.type === 'combo' ? 'Combo' : 'Food Item' }}
                </p>
                <p class="text-blue-600 font-semibold mt-1">
                  ${{ item.price.toFixed(2) }}
                </p>
              </div>

              <!-- Quantity Controls -->
              <div class="flex items-center gap-2">
                <button
                  @click="decreaseQuantity(item)"
                  class="w-8 h-8 flex items-center justify-center bg-gray-200 rounded hover:bg-gray-300 transition"
                  :disabled="item.quantity <= 1"
                >
                  <svg class="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                    <path
                      stroke-linecap="round"
                      stroke-linejoin="round"
                      stroke-width="2"
                      d="M20 12H4"
                    />
                  </svg>
                </button>
                <span class="w-12 text-center font-semibold">{{ item.quantity }}</span>
                <button
                  @click="increaseQuantity(item)"
                  class="w-8 h-8 flex items-center justify-center bg-gray-200 rounded hover:bg-gray-300 transition"
                >
                  <svg class="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                    <path
                      stroke-linecap="round"
                      stroke-linejoin="round"
                      stroke-width="2"
                      d="M12 4v16m8-8H4"
                    />
                  </svg>
                </button>
              </div>

              <!-- Item Subtotal -->
              <div class="text-right min-w-[100px]">
                <p class="font-semibold text-lg">
                  ${{ (item.price * item.quantity).toFixed(2) }}
                </p>
              </div>

              <!-- Remove Button -->
              <button
                @click="removeItem(item)"
                class="text-red-500 hover:text-red-700 transition p-2"
                title="Remove item"
              >
                <svg class="w-6 h-6" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                  <path
                    stroke-linecap="round"
                    stroke-linejoin="round"
                    stroke-width="2"
                    d="M19 7l-.867 12.142A2 2 0 0116.138 21H7.862a2 2 0 01-1.995-1.858L5 7m5 4v6m4-6v6m1-10V4a1 1 0 00-1-1h-4a1 1 0 00-1 1v3M4 7h16"
                  />
                </svg>
              </button>
            </div>
          </div>
        </div>

        <!-- Cart Summary -->
        <div class="lg:col-span-1">
          <div class="bg-white rounded-lg shadow-md p-6 sticky top-4">
            <h2 class="text-xl font-bold mb-4">Order Summary</h2>

            <div class="space-y-3 mb-4">
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

            <button
              @click="proceedToCheckout"
              class="w-full bg-blue-600 text-white py-3 rounded-lg hover:bg-blue-700 transition font-semibold"
            >
              Proceed to Checkout
            </button>

            <button
              @click="continueShopping"
              class="w-full mt-3 bg-gray-200 text-gray-700 py-3 rounded-lg hover:bg-gray-300 transition font-semibold"
            >
              Continue Shopping
            </button>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { useCartStore } from '@/stores/cart';
import { useRouter } from 'vue-router';

const cartStore = useCartStore();
const router = useRouter();

interface CartItem {
  id: number;
  name: string;
  price: number;
  quantity: number;
  type: 'food' | 'combo';
  imageUrl?: string;
}

function increaseQuantity(item: CartItem) {
  cartStore.updateQuantity(item.id, item.type, item.quantity + 1);
}

function decreaseQuantity(item: CartItem) {
  if (item.quantity > 1) {
    cartStore.updateQuantity(item.id, item.type, item.quantity - 1);
  }
}

function removeItem(item: CartItem) {
  if (confirm(`Remove ${item.name} from cart?`)) {
    cartStore.removeItem(item.id, item.type);
  }
}

function calculateTotal(): string {
  const subtotal = cartStore.totalAmount;
  const tax = subtotal * 0.1;
  const deliveryFee = 5.0;
  const total = subtotal + tax + deliveryFee;
  return total.toFixed(2);
}

function proceedToCheckout() {
  router.push('/checkout');
}

function continueShopping() {
  router.push('/menu');
}
</script>

<style scoped>
.cart-view {
  min-height: calc(100vh - 200px);
  background-color: #f9fafb;
}
</style>
