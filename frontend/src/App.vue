<template>
  <div id="app" class="min-h-screen bg-light">
    <!-- Navigation Header -->
    <header class="bg-white shadow-sm sticky top-0 z-50 border-b border-gray-200">
      <nav class="container mx-auto px-6 py-4">
        <div class="flex items-center justify-between">
          <!-- 1. Logo (Left) -->
          <router-link to="/" class="flex items-center gap-3 group">
            <div class="w-10 h-10 bg-gradient-to-br from-primary to-primary-dark rounded-xl flex items-center justify-center text-white font-bold text-xl shadow-md group-hover:shadow-lg transition-all duration-300">
              🍔
            </div>
            <span class="font-heading font-bold text-2xl text-gray-900 group-hover:text-primary transition-colors">YumYum</span>
          </router-link>

          <!-- 2. Main Navigation (Center) -->
          <div class="hidden md:flex items-center gap-8 absolute left-1/2 transform -translate-x-1/2">
            <router-link 
              to="/" 
              class="text-base font-medium text-gray-600 hover:text-primary transition-colors relative py-1 after:content-[''] after:absolute after:bottom-0 after:left-0 after:w-0 after:h-0.5 after:bg-primary after:transition-all after:duration-300 hover:after:w-full"
              active-class="text-primary after:w-full"
            >
              Trang chủ
            </router-link>
            <router-link 
              to="/menu" 
              class="text-base font-medium text-gray-600 hover:text-primary transition-colors relative py-1 after:content-[''] after:absolute after:bottom-0 after:left-0 after:w-0 after:h-0.5 after:bg-primary after:transition-all after:duration-300 hover:after:w-full"
              active-class="text-primary after:w-full"
            >
              Thực đơn
            </router-link>
          </div>

          <!-- 3. User Area & Cart (Right) -->
          <div class="hidden md:flex items-center gap-6">
            <!-- Cart Button (Highlighted) -->
            <router-link to="/cart" class="relative group">
              <button class="flex items-center gap-2 bg-white border-2 border-primary text-primary px-4 py-2 rounded-full text-sm font-bold hover:bg-primary hover:text-white transition-all duration-300 shadow-sm hover:shadow-md">
                <svg class="w-5 h-5" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
                  <path stroke-linecap="round" stroke-linejoin="round" d="M3 3h2l.4 2M7 13h10l4-8H5.4M7 13L5.4 5M7 13l-2.293 2.293c-.63.63-.184 1.707.707 1.707H17m0 0a2 2 0 100 4 2 2 0 000-4zm-8 2a2 2 0 11-4 0 2 2 0 014 0z" />
                </svg>
                <span>Giỏ hàng</span>
                <span v-if="cartCount > 0" class="bg-red-500 text-white text-xs font-bold rounded-full w-5 h-5 flex items-center justify-center group-hover:bg-white group-hover:text-primary transition-colors">
                  {{ cartCount }}
                </span>
              </button>
            </router-link>

            <!-- User Dropdown or Login -->
            <template v-if="!authStore.isAuthenticated">
              <router-link 
                to="/login" 
                class="bg-primary text-white px-6 py-2 rounded-full text-sm font-bold hover:bg-primary-dark transition-all duration-300 shadow-md hover:shadow-lg transform hover:-translate-y-0.5"
              >
                Đăng nhập
              </router-link>
            </template>
            <template v-else>
              <div class="relative group">
                <button class="flex items-center gap-3 focus:outline-none">
                  <div class="text-right hidden lg:block">
                    <p class="text-sm font-bold text-gray-900">{{ authStore.user?.fullName || 'Tài khoản' }}</p>
                    <p class="text-xs text-gray-500">{{ authStore.user?.role === 'Admin' ? 'Quản trị viên' : 'Thành viên' }}</p>
                  </div>
                  <div class="w-10 h-10 rounded-full bg-gray-100 border-2 border-white shadow-sm flex items-center justify-center text-xl overflow-hidden">
                    <img v-if="authStore.user?.avatar" :src="authStore.user.avatar" alt="Avatar" class="w-full h-full object-cover" />
                    <span v-else>👤</span>
                  </div>
                  <svg class="w-4 h-4 text-gray-400 group-hover:text-gray-600 transition-colors" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                    <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M19 9l-7 7-7-7" />
                  </svg>
                </button>

                <!-- Dropdown Menu -->
                <div class="absolute right-0 mt-2 w-56 bg-white rounded-xl shadow-xl border border-gray-100 opacity-0 invisible group-hover:opacity-100 group-hover:visible transition-all duration-200 transform origin-top-right z-50">
                  <div class="py-2">
                    <div class="px-4 py-3 border-b border-gray-50 lg:hidden">
                      <p class="text-sm font-bold text-gray-900">{{ authStore.user?.fullName }}</p>
                      <p class="text-xs text-gray-500">{{ authStore.user?.email }}</p>
                    </div>
                    
                    <router-link to="/profile" class="flex items-center gap-3 px-4 py-2.5 text-sm text-gray-700 hover:bg-gray-50 hover:text-primary transition-colors">
                      <svg class="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M16 7a4 4 0 11-8 0 4 4 0 018 0zM12 14a7 7 0 00-7 7h14a7 7 0 00-7-7z" /></svg>
                      Hồ sơ cá nhân
                    </router-link>
                    
                    <router-link to="/orders" class="flex items-center gap-3 px-4 py-2.5 text-sm text-gray-700 hover:bg-gray-50 hover:text-primary transition-colors">
                      <svg class="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 5H7a2 2 0 00-2 2v12a2 2 0 002 2h10a2 2 0 002-2V7a2 2 0 00-2-2h-2M9 5a2 2 0 002 2h2a2 2 0 002-2M9 5a2 2 0 012-2h2a2 2 0 012 2m-3 7h3m-3 4h3m-6-4h.01M9 16h.01" /></svg>
                      Đơn hàng của tôi
                    </router-link>

                    <template v-if="authStore.user?.role === 'Admin'">
                      <div class="border-t border-gray-100 my-1"></div>
                      <router-link to="/admin/dashboard" class="flex items-center gap-3 px-4 py-2.5 text-sm text-purple-600 font-medium hover:bg-purple-50 transition-colors">
                        <svg class="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M10.325 4.317c.426-1.756 2.924-1.756 3.35 0a1.724 1.724 0 002.573 1.066c1.543-.94 3.31.826 2.37 2.37a1.724 1.724 0 001.065 2.572c1.756.426 1.756 2.924 0 3.35a1.724 1.724 0 00-1.066 2.573c.94 1.543-.826 3.31-2.37 2.37a1.724 1.724 0 00-2.572 1.065c-.426 1.756-2.924 1.756-3.35 0a1.724 1.724 0 00-2.573-1.066c-1.543.94-3.31-.826-2.37-2.37a1.724 1.724 0 00-1.065-2.572c-1.756-.426-1.756-2.924 0-3.35a1.724 1.724 0 001.066-2.573c-.94-1.543.826-3.31 2.37-2.37.996.608 2.296.07 2.572-1.065z" /><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M15 12a3 3 0 11-6 0 3 3 0 016 0z" /></svg>
                        Trang Quản Trị
                      </router-link>
                    </template>

                    <div class="border-t border-gray-100 my-1"></div>
                    <button @click="handleLogout" class="flex w-full items-center gap-3 px-4 py-2.5 text-sm text-red-600 hover:bg-red-50 transition-colors">
                      <svg class="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M17 16l4-4m0 0l-4-4m4 4H7m6 4v1a3 3 0 01-3 3H6a3 3 0 01-3-3V7a3 3 0 013-3h4a3 3 0 013 3v1" /></svg>
                      Đăng xuất
                    </button>
                  </div>
                </div>
              </div>
            </template>
          </div>

          <!-- Mobile Menu Button -->
          <button class="md:hidden p-2 text-gray-600 hover:text-primary transition-colors" @click="mobileMenuOpen = !mobileMenuOpen">
            <svg class="w-6 h-6" fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M4 6h16M4 12h16M4 18h16" />
            </svg>
          </button>
        </div>

        <!-- Mobile Menu -->
        <div v-if="mobileMenuOpen" class="md:hidden mt-4 pt-4 border-t border-gray-100">
          <div class="flex flex-col gap-2">
            <router-link to="/" class="font-medium text-gray-700 hover:text-primary py-2 px-4 rounded-lg hover:bg-gray-50" @click="mobileMenuOpen = false">
              Trang chủ
            </router-link>
            <router-link to="/menu" class="font-medium text-gray-700 hover:text-primary py-2 px-4 rounded-lg hover:bg-gray-50" @click="mobileMenuOpen = false">
              Thực đơn
            </router-link>
            <router-link to="/cart" class="font-medium text-gray-700 hover:text-primary py-2 px-4 rounded-lg hover:bg-gray-50 flex justify-between items-center" @click="mobileMenuOpen = false">
              Giỏ hàng
              <span v-if="cartCount > 0" class="bg-primary text-white text-xs font-bold rounded-full w-5 h-5 flex items-center justify-center">{{ cartCount }}</span>
            </router-link>
            
            <!-- Auth Links Mobile -->
            <template v-if="!authStore.isAuthenticated">
              <router-link to="/login" class="font-medium text-primary py-2 px-4 rounded-lg hover:bg-primary-50 mt-2" @click="mobileMenuOpen = false">
                Đăng nhập
              </router-link>
            </template>
            <template v-else>
              <div class="border-t border-gray-100 mt-2 pt-2">
                <div class="px-4 py-2 mb-2">
                  <p class="text-sm font-bold text-gray-900">{{ authStore.user?.fullName }}</p>
                  <p class="text-xs text-gray-500">{{ authStore.user?.email }}</p>
                </div>
                <router-link to="/profile" class="block font-medium text-gray-700 hover:text-primary py-2 px-4 rounded-lg hover:bg-gray-50" @click="mobileMenuOpen = false">
                  Hồ sơ cá nhân
                </router-link>
                <router-link to="/orders" class="block font-medium text-gray-700 hover:text-primary py-2 px-4 rounded-lg hover:bg-gray-50" @click="mobileMenuOpen = false">
                  Đơn hàng của tôi
                </router-link>
                <template v-if="authStore.user?.role === 'Admin'">
                  <router-link to="/admin/dashboard" class="block font-medium text-purple-600 hover:text-purple-800 py-2 px-4 rounded-lg hover:bg-purple-50" @click="mobileMenuOpen = false">
                    Trang Quản Trị
                  </router-link>
                </template>
                <button @click="handleLogout" class="w-full text-left font-medium text-red-600 hover:text-red-700 py-2 px-4 rounded-lg hover:bg-red-50">
                  Đăng xuất
                </button>
              </div>
            </template>
          </div>
        </div>
      </nav>
    </header>

    <!-- Main Content -->
    <main>
      <router-view />
    </main>

    <!-- Footer -->
    <footer class="bg-dark text-white mt-5xl">
      <div class="container mx-auto px-base py-5xl">
        <div class="grid grid-cols-1 md:grid-cols-3 gap-xl mb-xl">
          <div>
            <h3 class="font-heading font-bold text-xl mb-lg">YumYum</h3>
            <p class="text-gray-300 leading-relaxed">
              Thức ăn ngon được giao tận nơi nhanh chóng. Hãy đặt hàng ngay và thưởng thức!
            </p>
          </div>
          <div>
            <h4 class="font-heading font-bold text-lg mb-lg">Liên kết nhanh</h4>
            <ul class="space-y-md text-gray-300">
              <li><router-link to="/" class="hover:text-primary transition-colors">Trang chủ</router-link></li>
              <li><router-link to="/menu" class="hover:text-primary transition-colors">Thực đơn</router-link></li>
              <li><router-link to="/cart" class="hover:text-primary transition-colors">Giỏ hàng</router-link></li>
            </ul>
          </div>
          <div>
            <h4 class="font-heading font-bold text-lg mb-lg">Liên hệ</h4>
            <ul class="space-y-md text-gray-300">
              <li>📞 Điện thoại: (123) 456-7890</li>
              <li>✉️ Email: info@yumyum.com</li>
              <li>📍 Địa chỉ: 123 Đường Ẩm thực</li>
            </ul>
          </div>
        </div>
        <div class="text-center pt-xl border-t border-gray-700 text-gray-400">
          <p>&copy; 2025 YumYum. Tất cả quyền lợi được bảo vệ.</p>
        </div>
      </div>
    </footer>

    <!-- Toast Notifications -->
    <ToastNotification ref="toast" />
  </div>
</template>

<script setup>
import { ref, computed, provide } from 'vue'
import { useCartStore } from './stores/cart'
import { useAuthStore } from './stores/auth'
import ToastNotification from './components/ui/ToastNotification.vue'

const cartStore = useCartStore()
const authStore = useAuthStore()
const mobileMenuOpen = ref(false)
const toast = ref(null)

const cartCount = computed(() => cartStore.totalItems)

// Provide toast to all child components
// Provide toast to all child components
provide('toast', toast)

// Check auth status on mount
authStore.checkAuth()

const handleLogout = () => {
  authStore.logout()
  mobileMenuOpen.value = false
  // Optional: Redirect to home or show toast
}
</script>

<style>
/* No custom CSS needed */
</style>
