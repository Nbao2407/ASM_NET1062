<script setup lang="ts">
import { ref, onMounted } from 'vue';
import { useRouter } from 'vue-router';
import { foodItemService, type FoodItem } from '@/services/foodItemService';
import { comboService, type Combo } from '@/services/comboService';
import { useCartStore } from '@/stores/cart';
import FoodItemCard from '@/components/FoodItemCard.vue';
import ComboCard from '@/components/ComboCard.vue';

const router = useRouter();
const cartStore = useCartStore();
const featuredItems = ref<FoodItem[]>([]);
const featuredCombos = ref<Combo[]>([]);
const loading = ref(true);

onMounted(async () => {
  try {
    // Fetch featured items (first 4)
    const items = await foodItemService.getAll();
    featuredItems.value = items.slice(0, 4);

    // Fetch featured combos (first 2)
    const combos = await comboService.getAll();
    featuredCombos.value = combos.slice(0, 2);
  } catch (error) {
    console.error('Error loading featured items:', error);
  } finally {
    loading.value = false;
  }
});

function addFoodItemToCart(item: FoodItem) {
  cartStore.addItem({
    id: item.foodItemId,
    name: item.name,
    price: item.price,
    quantity: 1,
    type: 'food',
    imageUrl: item.imageUrl,
  });
}

function addComboToCart(combo: Combo) {
  cartStore.addItem({
    id: combo.comboId,
    name: combo.name,
    price: combo.price,
    quantity: 1,
    type: 'combo',
    imageUrl: combo.imageUrl,
  });
}

function navigateToMenu() {
  router.push('/menu');
}

function navigateToSearch() {
  router.push('/search');
}
</script>

<template>
  <div class="min-h-screen bg-gray-50">
    <!-- Hero Section -->
    <section class="relative bg-gradient-to-br from-primary to-primary-hover text-white overflow-hidden">
      <div class="absolute inset-0 bg-[url('/pattern.png')] opacity-10"></div>
      <div class="container mx-auto px-base py-5xl md:py-4xl relative z-10">
        <div class="grid md:grid-cols-2 gap-4xl items-center">
          <div class="space-y-lg text-center md:text-left">
            <h1 class="font-heading font-extrabold text-5xl md:text-6xl leading-tight">
              Đồ ăn ngon, <br/>
              <span class="text-secondary">Giao nhanh</span>
            </h1>
            <p class="text-lg md:text-xl text-white/90 max-w-lg mx-auto md:mx-0 font-light leading-relaxed">
              Đặt các món ăn yêu thích của bạn từ thực đơn rộng của chúng tôi và nhận chúng được giao tận cửa trong vài phút.
            </p>
            <div class="flex flex-col sm:flex-row gap-md justify-center md:justify-start pt-lg">
              <button @click="navigateToMenu" class="px-xl py-base bg-white text-primary font-bold text-lg rounded-full shadow-md hover:shadow-lg hover:bg-gray-100 transition-all transform hover:-translate-y-1 flex items-center justify-center gap-md h-11">
                Xem thực đơn
                <svg class="w-5 h-5" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
                  <path stroke-linecap="round" stroke-linejoin="round" d="M13 7l5 5m0 0l-5 5m5-5H6" />
                </svg>
              </button>
              <button @click="navigateToSearch" class="px-xl py-base bg-transparent border-2 border-white text-white font-bold text-lg rounded-full hover:bg-white/10 transition-all flex items-center justify-center h-11">
                Tìm kiếm
              </button>
            </div>
          </div>
          <div class="hidden md:block relative">
            <div class="absolute inset-0 bg-secondary/20 blur-3xl rounded-full transform scale-110"></div>
            <div class="relative bg-white/10 backdrop-blur-sm p-lg rounded-xl border border-white/20 shadow-lg transform rotate-3 hover:rotate-0 transition-transform duration-500">
              <div class="aspect-square bg-gray-200 rounded-lg flex items-center justify-center overflow-hidden">
                <img src="/hero_food_image.png" alt="Delicious Food" class="w-full h-full object-cover transform hover:scale-105 transition-transform duration-700" />
              </div>
            </div>
          </div>
        </div>
      </div>
    </section>

    <!-- Features Section -->
    <section class="py-5xl bg-white">
      <div class="container mx-auto px-base">
        <div class="grid md:grid-cols-3 gap-xl">
          <div class="p-lg rounded-lg bg-gray-50 hover:bg-white hover:shadow-md transition-all duration-300 border border-transparent hover:border-gray-100 group">
            <div class="w-16 h-16 bg-primary/10 text-primary rounded-lg flex items-center justify-center mb-lg group-hover:scale-110 transition-transform">
              <svg class="w-8 h-8" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
                <path stroke-linecap="round" stroke-linejoin="round" d="M13 10V3L4 14h7v7l9-11h-7z" />
              </svg>
            </div>
            <h3 class="font-heading text-xl font-bold text-gray-900 mb-md">Giao nhanh</h3>
            <p class="text-gray-600 leading-relaxed text-sm">Nhận thức ăn của bạn trong vòng 30 phút hoặc ít hơn, nóng và tươi tại cửa nhà.</p>
          </div>
          <div class="p-lg rounded-lg bg-gray-50 hover:bg-white hover:shadow-md transition-all duration-300 border border-transparent hover:border-gray-100 group">
            <div class="w-16 h-16 bg-secondary/10 text-secondary rounded-lg flex items-center justify-center mb-lg group-hover:scale-110 transition-transform">
              <svg class="w-8 h-8" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
                <path stroke-linecap="round" stroke-linejoin="round" d="M9 12l2 2 4-4m6 2a9 9 0 11-18 0 9 9 0 0118 0z" />
              </svg>
            </div>
            <h3 class="font-heading text-xl font-bold text-gray-900 mb-md">Thức ăn chất lượng</h3>
            <p class="text-gray-600 leading-relaxed text-sm">Chúng tôi chỉ sử dụng các nguyên liệu tươi nhất và công thức xác thực để có hương vị tốt nhất.</p>
          </div>
          <div class="p-lg rounded-lg bg-gray-50 hover:bg-white hover:shadow-md transition-all duration-300 border border-transparent hover:border-gray-100 group">
            <div class="w-16 h-16 bg-green-100 text-green-600 rounded-lg flex items-center justify-center mb-lg group-hover:scale-110 transition-transform">
              <svg class="w-8 h-8" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
                <path stroke-linecap="round" stroke-linejoin="round" d="M12 8c-1.657 0-3 .895-3 2s1.343 2 3 2 3 .895 3 2-1.343 2-3 2m0-8c1.11 0 2.08.402 2.599 1M12 8V7m0 1v8m0 0v1m0-1c-1.11 0-2.08-.402-2.599-1M21 12a9 9 0 11-18 0 9 9 0 0118 0z" />
              </svg>
            </div>
            <h3 class="font-heading text-xl font-bold text-gray-900 mb-md">Giá tốt</h3>
            <p class="text-gray-600 leading-relaxed text-sm">Thưởng thức những bữa ăn ngon với giá cả hợp lý nhờ các combo deal tuyệt vời của chúng tôi.</p>
          </div>
        </div>
      </div>
    </section>

    <!-- Featured Items Section -->
    <section class="py-5xl bg-gray-50">
      <div class="container mx-auto px-base">
        <div class="flex justify-between items-end mb-xxl">
          <div>
            <h2 class="font-heading text-4xl md:text-5xl font-bold text-gray-900 mb-md">Món ăn nổi bật</h2>
            <p class="text-gray-600 text-lg">Các món ăn phổ biến nhất được chọn cho bạn</p>
          </div>
          <router-link to="/menu" class="text-primary font-bold hover:text-primary-hover flex items-center gap-md group hidden sm:flex">
            Xem tất cả
            <svg class="w-5 h-5 transform group-hover:translate-x-1 transition-transform" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
              <path stroke-linecap="round" stroke-linejoin="round" d="M17 8l4 4m0 0l-4 4m4-4H3" />
            </svg>
          </router-link>
        </div>

        <div v-if="loading" class="flex flex-col items-center justify-center py-5xl">
          <div class="w-12 h-12 border-4 border-gray-200 border-t-primary rounded-full animate-spin mb-md"></div>
          <p class="text-gray-500 font-medium">Đang tải món ăn...</p>
        </div>

        <div v-else class="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-4 gap-xl">
          <FoodItemCard
            v-for="item in featuredItems"
            :key="item.foodItemId"
            :item="item"
            @add-to-cart="addFoodItemToCart"
          />
        </div>
      </div>
    </section>

    <!-- Featured Combos Section -->
    <section class="py-5xl bg-white">
      <div class="container mx-auto px-base">
        <div class="flex justify-between items-end mb-xxl">
          <div>
            <h2 class="font-heading text-4xl md:text-5xl font-bold text-gray-900 mb-md">Combo Đặc Biệt</h2>
            <p class="text-gray-600 text-lg">Những bữa ăn giá trị nhất dành cho bạn</p>
          </div>
          <router-link to="/menu" class="text-primary font-bold hover:text-primary-hover flex items-center gap-md group hidden sm:flex">
            Xem tất cả
            <svg class="w-5 h-5 transform group-hover:translate-x-1 transition-transform" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
              <path stroke-linecap="round" stroke-linejoin="round" d="M17 8l4 4m0 0l-4 4m4-4H3" />
            </svg>
          </router-link>
        </div>

        <div v-if="loading" class="flex flex-col items-center justify-center py-5xl">
          <div class="w-12 h-12 border-4 border-gray-200 border-t-primary rounded-full animate-spin mb-md"></div>
          <p class="text-gray-500 font-medium">Đang tải combo...</p>
        </div>

        <div v-else class="grid grid-cols-1 md:grid-cols-2 gap-xl">
          <ComboCard
            v-for="combo in featuredCombos"
            :key="combo.comboId"
            :combo="combo"
            @add-to-cart="addComboToCart"
          />
        </div>
      </div>
    </section>

    <!-- CTA Section -->
    <section class="py-5xl bg-dark relative overflow-hidden">
      <div class="absolute inset-0 bg-primary/10"></div>
      <div class="container mx-auto px-base relative z-10 text-center">
        <h2 class="font-heading text-4xl md:text-5xl font-bold text-white mb-lg">Sẵn sàng đặt món?</h2>
        <p class="text-lg text-gray-300 mb-xxl max-w-2xl mx-auto leading-relaxed">Duyệt qua thực đơn đầy đủ của chúng tôi và tìm các món ăn yêu thích của bạn. Giao hàng nhanh và đảm bảo thức ăn nóng hổi.</p>
        <button @click="navigateToMenu" class="px-xl py-base bg-primary text-white font-bold text-lg rounded-full shadow-md hover:bg-primary-hover hover:shadow-lg transition-all transform hover:-translate-y-1 h-11">
          Đặt món ngay
        </button>
      </div>
    </section>
  </div>
</template>

<style scoped>
/* No custom CSS needed, all handled by Tailwind */
</style>
