<script setup>
import { ref, onMounted, reactive } from 'vue'
import { useAuthStore } from '../stores/auth'
import { useRouter } from 'vue-router'
import api from '../services/api'

const authStore = useAuthStore()
const router = useRouter()

const activeTab = ref('profile')
const isLoading = ref(false)
const message = ref({ type: '', text: '' })

const profileForm = reactive({
  fullName: '',
  email: '',
  phoneNumber: '',
  address: '',
  dateOfBirth: ''
})

const passwordForm = reactive({
  currentPassword: '',
  newPassword: '',
  confirmPassword: ''
})

onMounted(async () => {
  if (!authStore.isAuthenticated) {
    router.push('/login')
    return
  }
  
  // Load user data
  await fetchProfile()
})

const fetchProfile = async () => {
  try {
    const response = await api.get('/customers/profile')
    const data = response.data
    profileForm.fullName = data.fullName
    profileForm.email = data.email
    profileForm.phoneNumber = data.phoneNumber
    profileForm.address = data.address
    // Format date for input field (YYYY-MM-DD)
    if (data.dateOfBirth) {
      profileForm.dateOfBirth = new Date(data.dateOfBirth).toISOString().split('T')[0]
    }
  } catch (error) {
    console.error('Error fetching profile:', error)
  }
}

const updateProfile = async () => {
  isLoading.value = true
  message.value = { type: '', text: '' }
  
  try {
    const response = await api.put('/customers/profile', profileForm)
    const data = response.data
    message.value = { type: 'success', text: 'Profile updated successfully!' }
    // Update auth store user data if needed
    authStore.user = { ...authStore.user, ...data }
  } catch (error) {
    message.value = { type: 'error', text: error.response?.data?.message || 'Failed to update profile' }
  } finally {
    isLoading.value = false
  }
}

const changePassword = async () => {
  if (passwordForm.newPassword !== passwordForm.confirmPassword) {
    message.value = { type: 'error', text: 'New passwords do not match' }
    return
  }
  
  isLoading.value = true
  message.value = { type: '', text: '' }
  
  try {
    await api.put('/customers/password', {
      currentPassword: passwordForm.currentPassword,
      newPassword: passwordForm.newPassword
    })
    message.value = { type: 'success', text: 'Password changed successfully!' }
    passwordForm.currentPassword = ''
    passwordForm.newPassword = ''
    passwordForm.confirmPassword = ''
  } catch (error) {
    message.value = { type: 'error', text: error.response?.data?.message || 'Failed to change password' }
  } finally {
    isLoading.value = false
  }
}
</script>

<template>
  <div class="min-h-screen bg-gray-50 py-12">
    <div class="container mx-auto px-4">
      <div class="max-w-5xl mx-auto">
        <h1 class="text-3xl font-heading font-bold text-gray-900 mb-8">Cài đặt tài khoản</h1>

        <div class="grid grid-cols-1 md:grid-cols-4 gap-8">
          <!-- Sidebar -->
          <div class="md:col-span-1">
            <div class="bg-white rounded-2xl shadow-sm border border-gray-100 overflow-hidden sticky top-24">
              <div class="p-6 text-center border-b border-gray-50 bg-gradient-to-b from-primary/5 to-transparent">
                <div class="w-24 h-24 bg-white border-4 border-white shadow-md rounded-full flex items-center justify-center text-4xl font-bold mx-auto mb-4 text-primary">
                  {{ profileForm.fullName ? profileForm.fullName.charAt(0).toUpperCase() : 'U' }}
                </div>
                <h2 class="font-bold text-gray-900 text-lg">{{ profileForm.fullName }}</h2>
                <p class="text-sm text-gray-500">{{ profileForm.email }}</p>
              </div>
              <nav class="p-3 space-y-1">
                <button
                  @click="activeTab = 'profile'"
                  class="w-full text-left px-4 py-3 rounded-xl transition-all flex items-center font-medium"
                  :class="activeTab === 'profile' ? 'bg-primary/10 text-primary' : 'text-gray-600 hover:bg-gray-50'"
                >
                  <span class="mr-3 text-xl">👤</span> Thông tin cá nhân
                </button>
                <button
                  @click="activeTab = 'security'"
                  class="w-full text-left px-4 py-3 rounded-xl transition-all flex items-center font-medium"
                  :class="activeTab === 'security' ? 'bg-primary/10 text-primary' : 'text-gray-600 hover:bg-gray-50'"
                >
                  <span class="mr-3 text-xl">🔒</span> Bảo mật
                </button>
              </nav>
            </div>
          </div>

          <!-- Main Content -->
          <div class="md:col-span-3">
            <div class="bg-white rounded-2xl shadow-sm border border-gray-100 p-6 md:p-8 min-h-[500px]">
              <!-- Alert Message -->
              <transition name="fade">
                <div v-if="message.text" :class="`mb-6 p-4 rounded-xl flex items-center ${message.type === 'success' ? 'bg-green-50 text-green-700 border border-green-100' : 'bg-red-50 text-red-700 border border-red-100'}`">
                  <span class="mr-2 text-lg">{{ message.type === 'success' ? '✅' : '⚠️' }}</span>
                  {{ message.text }}
                </div>
              </transition>

              <!-- Profile Info Tab -->
              <div v-if="activeTab === 'profile'" class="animate-fade-in">
                <div class="flex justify-between items-center mb-6">
                  <h2 class="text-2xl font-bold text-gray-900">Thông tin cá nhân</h2>
                  <span class="text-sm text-gray-500">Quản lý thông tin chi tiết của bạn</span>
                </div>
                
                <form @submit.prevent="updateProfile" class="space-y-6">
                  <div class="grid grid-cols-1 md:grid-cols-2 gap-6">
                    <div class="space-y-2">
                      <label class="block text-sm font-semibold text-gray-700">Họ và tên</label>
                      <input 
                        v-model="profileForm.fullName" 
                        type="text" 
                        class="w-full px-4 py-3 border border-gray-200 rounded-xl focus:ring-2 focus:ring-primary/20 focus:border-primary transition-all" 
                        required
                      >
                    </div>
                    <div class="space-y-2">
                      <label class="block text-sm font-semibold text-gray-700">Email</label>
                      <input 
                        v-model="profileForm.email" 
                        type="email" 
                        class="w-full px-4 py-3 border border-gray-200 rounded-xl bg-gray-50 text-gray-500 cursor-not-allowed" 
                        disabled
                      >
                    </div>
                    <div class="space-y-2">
                      <label class="block text-sm font-semibold text-gray-700">Số điện thoại</label>
                      <input 
                        v-model="profileForm.phoneNumber" 
                        type="tel" 
                        class="w-full px-4 py-3 border border-gray-200 rounded-xl focus:ring-2 focus:ring-primary/20 focus:border-primary transition-all"
                      >
                    </div>
                    <div class="space-y-2">
                      <label class="block text-sm font-semibold text-gray-700">Ngày sinh</label>
                      <input 
                        v-model="profileForm.dateOfBirth" 
                        type="date" 
                        class="w-full px-4 py-3 border border-gray-200 rounded-xl focus:ring-2 focus:ring-primary/20 focus:border-primary transition-all"
                      >
                    </div>
                    <div class="md:col-span-2 space-y-2">
                      <label class="block text-sm font-semibold text-gray-700">Địa chỉ</label>
                      <textarea 
                        v-model="profileForm.address" 
                        rows="3" 
                        class="w-full px-4 py-3 border border-gray-200 rounded-xl focus:ring-2 focus:ring-primary/20 focus:border-primary transition-all resize-none"
                      ></textarea>
                    </div>
                  </div>
                  <div class="pt-4 border-t border-gray-100 flex justify-end">
                    <button 
                      type="submit" 
                      :disabled="isLoading" 
                      class="bg-primary text-white px-8 py-3 rounded-xl font-bold hover:bg-primary-hover transition-all transform hover:-translate-y-0.5 shadow-lg shadow-primary/30 disabled:opacity-50 disabled:transform-none"
                    >
                      {{ isLoading ? 'Đang lưu...' : 'Lưu thay đổi' }}
                    </button>
                  </div>
                </form>
              </div>

              <!-- Security Tab -->
              <div v-if="activeTab === 'security'" class="animate-fade-in">
                <div class="flex justify-between items-center mb-6">
                  <h2 class="text-2xl font-bold text-gray-900">Cài đặt bảo mật</h2>
                  <span class="text-sm text-gray-500">Cập nhật mật khẩu của bạn</span>
                </div>

                <form @submit.prevent="changePassword" class="space-y-6 max-w-lg">
                  <div class="space-y-2">
                    <label class="block text-sm font-semibold text-gray-700">Mật khẩu hiện tại</label>
                    <input 
                      v-model="passwordForm.currentPassword" 
                      type="password" 
                      class="w-full px-4 py-3 border border-gray-200 rounded-xl focus:ring-2 focus:ring-primary/20 focus:border-primary transition-all" 
                      required
                    >
                  </div>
                  <div class="space-y-2">
                    <label class="block text-sm font-semibold text-gray-700">Mật khẩu mới</label>
                    <input 
                      v-model="passwordForm.newPassword" 
                      type="password" 
                      class="w-full px-4 py-3 border border-gray-200 rounded-xl focus:ring-2 focus:ring-primary/20 focus:border-primary transition-all" 
                      required
                    >
                  </div>
                  <div class="space-y-2">
                    <label class="block text-sm font-semibold text-gray-700">Xác nhận mật khẩu mới</label>
                    <input 
                      v-model="passwordForm.confirmPassword" 
                      type="password" 
                      class="w-full px-4 py-3 border border-gray-200 rounded-xl focus:ring-2 focus:ring-primary/20 focus:border-primary transition-all" 
                      required
                    >
                  </div>
                  <div class="pt-4 border-t border-gray-100 flex justify-end">
                    <button 
                      type="submit" 
                      :disabled="isLoading" 
                      class="bg-primary text-white px-8 py-3 rounded-xl font-bold hover:bg-primary-hover transition-all transform hover:-translate-y-0.5 shadow-lg shadow-primary/30 disabled:opacity-50 disabled:transform-none"
                    >
                      {{ isLoading ? 'Đang cập nhật...' : 'Cập nhật mật khẩu' }}
                    </button>
                  </div>
                </form>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<style scoped>
.animate-fade-in {
  animation: fadeIn 0.3s ease-out;
}

@keyframes fadeIn {
  from { opacity: 0; transform: translateY(10px); }
  to { opacity: 1; transform: translateY(0); }
}
</style>
