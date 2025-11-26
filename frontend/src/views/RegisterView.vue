<script setup>
import { ref, reactive } from 'vue'
import { useRouter } from 'vue-router'
import { useAuthStore } from '../stores/auth'
import Button from '../components/ui/Button.vue'

const router = useRouter()
const authStore = useAuthStore()

const form = reactive({
  name: '',
  email: '',
  password: '',
  confirmPassword: '',
  phone: '',
  address: '',
  dateOfBirth: ''
})

const errors = reactive({
  name: '',
  email: '',
  password: '',
  confirmPassword: '',
  phone: '',
  address: '',
  dateOfBirth: '',
  general: ''
})

const loading = ref(false)

const validate = () => {
  let isValid = true
  // Reset errors
  Object.keys(errors).forEach(key => errors[key] = '')

  if (!form.name.trim()) {
    errors.name = 'Full name is required'
    isValid = false
  }

  if (!form.email.trim()) {
    errors.email = 'Email is required'
    isValid = false
  } else if (!/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(form.email)) {
    errors.email = 'Invalid email format'
    isValid = false
  }

  if (!form.password) {
    errors.password = 'Password is required'
    isValid = false
  } else if (form.password.length < 8) {
    errors.password = 'Password must be at least 8 characters'
    isValid = false
  }

  if (form.password !== form.confirmPassword) {
    errors.confirmPassword = 'Passwords do not match'
    isValid = false
  }

  if (!form.phone.trim()) {
    errors.phone = 'Phone number is required'
    isValid = false
  } else if (!/^\d{10,11}$/.test(form.phone.replace(/\D/g, ''))) {
    errors.phone = 'Invalid phone number'
    isValid = false
  }

  if (!form.address.trim()) {
    errors.address = 'Address is required'
    isValid = false
  }

  if (!form.dateOfBirth) {
    errors.dateOfBirth = 'Date of birth is required'
    isValid = false
  }

  return isValid
}

const handleRegister = async () => {
  if (!validate()) return

  loading.value = true
  errors.general = ''

  try {
    await authStore.register({
      fullName: form.name,
      email: form.email,
      password: form.password,
      phoneNumber: form.phone,
      address: form.address,
      dateOfBirth: form.dateOfBirth
    })
    // Redirect to login or auto-login
    router.push('/login')
  } catch (err) {
    errors.general = err.response?.data?.message || 'Registration failed. Please try again.'
    if (err.response?.data?.errors) {
        // Handle validation errors from backend
        const backendErrors = err.response.data.errors
        if (backendErrors.Email) errors.email = backendErrors.Email[0]
        if (backendErrors.Password) errors.password = backendErrors.Password[0]
        if (backendErrors.FullName) errors.name = backendErrors.FullName[0]
        if (backendErrors.PhoneNumber) errors.phone = backendErrors.PhoneNumber[0]
        if (backendErrors.Address) errors.address = backendErrors.Address[0]
        if (backendErrors.DateOfBirth) errors.dateOfBirth = backendErrors.DateOfBirth[0]
    }
  } finally {
    loading.value = false
  }
}
</script>

<template>
  <div class="min-h-screen flex items-center justify-center bg-gray-50 py-12 px-4 sm:px-6 lg:px-8">
    <div class="max-w-md w-full space-y-8">
      <div>
        <h2 class="mt-6 text-center text-3xl font-extrabold text-gray-900">
          Tạo tài khoản mới
        </h2>
        <p class="mt-2 text-center text-sm text-gray-600">
          Đã có tài khoản?
          <router-link to="/login" class="font-medium text-orange-600 hover:text-orange-500">
            Đăng nhập
          </router-link>
        </p>
      </div>
      <form class="mt-8 space-y-6" @submit.prevent="handleRegister">
        <div class="rounded-md shadow-sm -space-y-px">
          <!-- Full Name -->
          <div class="mb-4">
            <label for="name" class="block text-sm font-medium text-gray-700">Họ và tên</label>
            <input id="name" name="name" type="text" required v-model="form.name" class="appearance-none rounded-md relative block w-full px-3 py-2 border border-gray-300 placeholder-gray-500 text-gray-900 focus:outline-none focus:ring-orange-500 focus:border-orange-500 focus:z-10 sm:text-sm" :class="{'border-red-500': errors.name}" placeholder="Nguyễn Văn A" />
            <p v-if="errors.name" class="mt-1 text-xs text-red-500">{{ errors.name }}</p>
          </div>

          <!-- Email -->
          <div class="mb-4">
            <label for="email-address" class="block text-sm font-medium text-gray-700">Địa chỉ Email</label>
            <input id="email-address" name="email" type="email" autocomplete="email" required v-model="form.email" class="appearance-none rounded-md relative block w-full px-3 py-2 border border-gray-300 placeholder-gray-500 text-gray-900 focus:outline-none focus:ring-orange-500 focus:border-orange-500 focus:z-10 sm:text-sm" :class="{'border-red-500': errors.email}" placeholder="email@example.com" />
            <p v-if="errors.email" class="mt-1 text-xs text-red-500">{{ errors.email }}</p>
          </div>

          <!-- Phone -->
          <div class="mb-4">
            <label for="phone" class="block text-sm font-medium text-gray-700">Số điện thoại</label>
            <input id="phone" name="phone" type="tel" autocomplete="tel" required v-model="form.phone" class="appearance-none rounded-md relative block w-full px-3 py-2 border border-gray-300 placeholder-gray-500 text-gray-900 focus:outline-none focus:ring-orange-500 focus:border-orange-500 focus:z-10 sm:text-sm" :class="{'border-red-500': errors.phone}" placeholder="0912345678" />
            <p v-if="errors.phone" class="mt-1 text-xs text-red-500">{{ errors.phone }}</p>
          </div>

          <!-- Date of Birth -->
          <div class="mb-4">
            <label for="dob" class="block text-sm font-medium text-gray-700">Ngày sinh</label>
            <input id="dob" name="dob" type="date" required v-model="form.dateOfBirth" class="appearance-none rounded-md relative block w-full px-3 py-2 border border-gray-300 placeholder-gray-500 text-gray-900 focus:outline-none focus:ring-orange-500 focus:border-orange-500 focus:z-10 sm:text-sm" :class="{'border-red-500': errors.dateOfBirth}" />
            <p v-if="errors.dateOfBirth" class="mt-1 text-xs text-red-500">{{ errors.dateOfBirth }}</p>
          </div>

          <!-- Address -->
          <div class="mb-4">
            <label for="address" class="block text-sm font-medium text-gray-700">Địa chỉ</label>
            <textarea id="address" name="address" rows="2" required v-model="form.address" class="appearance-none rounded-md relative block w-full px-3 py-2 border border-gray-300 placeholder-gray-500 text-gray-900 focus:outline-none focus:ring-orange-500 focus:border-orange-500 focus:z-10 sm:text-sm" :class="{'border-red-500': errors.address}" placeholder="123 Đường ABC, Quận XYZ"></textarea>
            <p v-if="errors.address" class="mt-1 text-xs text-red-500">{{ errors.address }}</p>
          </div>

          <!-- Password -->
          <div class="mb-4">
            <label for="password" class="block text-sm font-medium text-gray-700">Mật khẩu</label>
            <input id="password" name="password" type="password" autocomplete="new-password" required v-model="form.password" class="appearance-none rounded-md relative block w-full px-3 py-2 border border-gray-300 placeholder-gray-500 text-gray-900 focus:outline-none focus:ring-orange-500 focus:border-orange-500 focus:z-10 sm:text-sm" :class="{'border-red-500': errors.password}" placeholder="Mật khẩu" />
            <p v-if="errors.password" class="mt-1 text-xs text-red-500">{{ errors.password }}</p>
          </div>

          <!-- Confirm Password -->
          <div class="mb-4">
            <label for="confirm-password" class="block text-sm font-medium text-gray-700">Xác nhận mật khẩu</label>
            <input id="confirm-password" name="confirm-password" type="password" autocomplete="new-password" required v-model="form.confirmPassword" class="appearance-none rounded-md relative block w-full px-3 py-2 border border-gray-300 placeholder-gray-500 text-gray-900 focus:outline-none focus:ring-orange-500 focus:border-orange-500 focus:z-10 sm:text-sm" :class="{'border-red-500': errors.confirmPassword}" placeholder="Nhập lại mật khẩu" />
            <p v-if="errors.confirmPassword" class="mt-1 text-xs text-red-500">{{ errors.confirmPassword }}</p>
          </div>
        </div>

        <div v-if="errors.general" class="text-red-500 text-sm text-center">
          {{ errors.general }}
        </div>

        <div>
          <Button type="submit" class="w-full flex justify-center py-2 px-4 border border-transparent text-sm font-medium rounded-md text-white bg-orange-600 hover:bg-orange-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-orange-500" :disabled="loading">
            <span v-if="loading">Đang tạo tài khoản...</span>
            <span v-else>Đăng ký</span>
          </Button>
        </div>
      </form>
    </div>
  </div>
</template>
