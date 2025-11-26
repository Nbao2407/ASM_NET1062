<template>
  <div>
    <div class="flex justify-between items-center mb-6">
      <h1 class="text-2xl font-bold text-gray-900">Quản lý người dùng</h1>
      <button @click="openCreateModal" class="bg-primary hover:bg-primary-dark text-white px-4 py-2 rounded-lg flex items-center gap-2">
        <svg class="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24">
          <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 4v16m8-8H4" />
        </svg>
        Thêm người dùng
      </button>
    </div>

    <!-- Search and Filter -->
    <div class="bg-white p-4 rounded-lg shadow-sm border border-gray-200 mb-6 flex flex-col md:flex-row gap-4">
      <div class="flex-1 relative">
        <span class="absolute inset-y-0 left-0 pl-3 flex items-center pointer-events-none">
          <svg class="w-5 h-5 text-gray-400" fill="none" stroke="currentColor" viewBox="0 0 24 24">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M21 21l-6-6m2-5a7 7 0 11-14 0 7 7 0 0114 0z" />
          </svg>
        </span>
        <input v-model="searchQuery" type="text" class="pl-10 block w-full rounded-md border-gray-300 shadow-sm focus:border-primary focus:ring focus:ring-primary focus:ring-opacity-50" placeholder="Tìm kiếm người dùng...">
      </div>
      <div class="w-full md:w-48">
        <select v-model="roleFilter" class="block w-full rounded-md border-gray-300 shadow-sm focus:border-primary focus:ring focus:ring-primary focus:ring-opacity-50">
          <option value="">Tất cả vai trò</option>
          <option value="Admin">Admin</option>
          <option value="Customer">Khách hàng</option>
        </select>
      </div>
    </div>

    <!-- Users Table -->
    <div class="bg-white rounded-lg shadow-sm border border-gray-200 overflow-hidden">
      <div class="overflow-x-auto">
        <table class="min-w-full divide-y divide-gray-200">
          <thead class="bg-gray-50">
            <tr>
              <th scope="col" class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider cursor-pointer" @click="sortBy('userId')">
                ID <span v-if="sortKey === 'userId'">{{ sortOrder === 'asc' ? '↑' : '↓' }}</span>
              </th>
              <th scope="col" class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider cursor-pointer" @click="sortBy('fullName')">
                Tên <span v-if="sortKey === 'fullName'">{{ sortOrder === 'asc' ? '↑' : '↓' }}</span>
              </th>
              <th scope="col" class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                Liên hệ
              </th>
              <th scope="col" class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider cursor-pointer" @click="sortBy('role')">
                Vai trò <span v-if="sortKey === 'role'">{{ sortOrder === 'asc' ? '↑' : '↓' }}</span>
              </th>
              <th scope="col" class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                Trạng thái
              </th>
              <th scope="col" class="px-6 py-3 text-right text-xs font-medium text-gray-500 uppercase tracking-wider">
                Thao tác
              </th>
            </tr>
          </thead>
          <tbody class="bg-white divide-y divide-gray-200">
            <tr v-for="user in paginatedUsers" :key="user.userId">
              <td class="px-6 py-4 whitespace-nowrap text-sm text-gray-500">
                #{{ user.userId }}
              </td>
              <td class="px-6 py-4 whitespace-nowrap">
                <div class="flex items-center">
                  <div class="h-10 w-10 flex-shrink-0">
                    <div class="h-10 w-10 rounded-full bg-primary text-white flex items-center justify-center font-bold">
                      {{ user.fullName.charAt(0).toUpperCase() }}
                    </div>
                  </div>
                  <div class="ml-4">
                    <div class="text-sm font-medium text-gray-900">{{ user.fullName }}</div>
                    <div class="text-sm text-gray-500">Tham gia {{ formatDate(user.createdAt) }}</div>
                  </div>
                </div>
              </td>
              <td class="px-6 py-4 whitespace-nowrap">
                <div class="text-sm text-gray-900">{{ user.email }}</div>
                <div class="text-sm text-gray-500">{{ user.phoneNumber }}</div>
              </td>
              <td class="px-6 py-4 whitespace-nowrap">
                <span class="px-2 inline-flex text-xs leading-5 font-semibold rounded-full" :class="user.role === 'Admin' ? 'bg-purple-100 text-purple-800' : 'bg-green-100 text-green-800'">
                  {{ user.role }}
                </span>
              </td>
              <td class="px-6 py-4 whitespace-nowrap">
                <span class="px-2 inline-flex text-xs leading-5 font-semibold rounded-full" :class="user.isActive ? 'bg-green-100 text-green-800' : 'bg-red-100 text-red-800'">
                  {{ user.isActive ? 'Hoạt động' : 'Đã khóa' }}
                </span>
              </td>
              <td class="px-6 py-4 whitespace-nowrap text-right text-sm font-medium">
                <button @click="openEditModal(user)" class="text-indigo-600 hover:text-indigo-900 mr-3">Sửa</button>
                <button @click="confirmDelete(user)" class="text-red-600 hover:text-red-900">Xóa</button>
              </td>
            </tr>
            <tr v-if="paginatedUsers.length === 0">
              <td colspan="6" class="px-6 py-4 text-center text-gray-500">
                Không tìm thấy người dùng nào.
              </td>
            </tr>
          </tbody>
        </table>
      </div>
      
      <!-- Pagination -->
      <div class="bg-white px-4 py-3 border-t border-gray-200 flex items-center justify-between sm:px-6">
        <div class="hidden sm:flex-1 sm:flex sm:items-center sm:justify-between">
          <div>
            <p class="text-sm text-gray-700">
              Hiển thị <span class="font-medium">{{ startIndex + 1 }}</span> đến <span class="font-medium">{{ Math.min(endIndex, filteredUsers.length) }}</span> trong <span class="font-medium">{{ filteredUsers.length }}</span> kết quả
            </p>
          </div>
          <div>
            <nav class="relative z-0 inline-flex rounded-md shadow-sm -space-x-px" aria-label="Pagination">
              <button @click="currentPage--" :disabled="currentPage === 1" class="relative inline-flex items-center px-2 py-2 rounded-l-md border border-gray-300 bg-white text-sm font-medium text-gray-500 hover:bg-gray-50" :class="{ 'opacity-50 cursor-not-allowed': currentPage === 1 }">
                <span class="sr-only">Previous</span>
                <svg class="h-5 w-5" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 20 20" fill="currentColor" aria-hidden="true">
                  <path fill-rule="evenodd" d="M12.707 5.293a1 1 0 010 1.414L9.414 10l3.293 3.293a1 1 0 01-1.414 1.414l-4-4a1 1 0 010-1.414l4-4a1 1 0 011.414 0z" clip-rule="evenodd" />
                </svg>
              </button>
              <button v-for="page in totalPages" :key="page" @click="currentPage = page" class="relative inline-flex items-center px-4 py-2 border border-gray-300 bg-white text-sm font-medium" :class="currentPage === page ? 'z-10 bg-indigo-50 border-indigo-500 text-indigo-600' : 'text-gray-500 hover:bg-gray-50'">
                {{ page }}
              </button>
              <button @click="currentPage++" :disabled="currentPage === totalPages" class="relative inline-flex items-center px-2 py-2 rounded-r-md border border-gray-300 bg-white text-sm font-medium text-gray-500 hover:bg-gray-50" :class="{ 'opacity-50 cursor-not-allowed': currentPage === totalPages }">
                <span class="sr-only">Next</span>
                <svg class="h-5 w-5" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 20 20" fill="currentColor" aria-hidden="true">
                  <path fill-rule="evenodd" d="M7.293 14.707a1 1 0 010-1.414L10.586 10 7.293 6.707a1 1 0 011.414-1.414l4 4a1 1 0 010 1.414l-4 4a1 1 0 01-1.414 0z" clip-rule="evenodd" />
                </svg>
              </button>
            </nav>
          </div>
        </div>
      </div>
    </div>

    <!-- Create/Edit Modal -->
    <Modal :isOpen="isModalOpen" :title="editingUser ? 'Sửa người dùng' : 'Thêm người dùng'" @close="closeModal" @confirm="saveUser">
      <form @submit.prevent="saveUser" class="space-y-4">
        <div>
          <label class="block text-sm font-medium text-gray-700">Họ tên</label>
          <input v-model="form.fullName" type="text" required class="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-primary focus:ring focus:ring-primary focus:ring-opacity-50">
        </div>
        <div>
          <label class="block text-sm font-medium text-gray-700">Email</label>
          <input v-model="form.email" type="email" required class="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-primary focus:ring focus:ring-primary focus:ring-opacity-50">
        </div>
        <div v-if="!editingUser">
          <label class="block text-sm font-medium text-gray-700">Mật khẩu</label>
          <input v-model="form.password" type="password" required class="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-primary focus:ring focus:ring-primary focus:ring-opacity-50">
        </div>
        <div>
          <label class="block text-sm font-medium text-gray-700">Số điện thoại</label>
          <input v-model="form.phoneNumber" type="tel" class="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-primary focus:ring focus:ring-primary focus:ring-opacity-50">
        </div>
        <div>
          <label class="block text-sm font-medium text-gray-700">Địa chỉ</label>
          <textarea v-model="form.address" rows="3" class="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-primary focus:ring focus:ring-primary focus:ring-opacity-50"></textarea>
        </div>
        <div>
          <label class="block text-sm font-medium text-gray-700">Vai trò</label>
          <select v-model="form.role" class="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-primary focus:ring focus:ring-primary focus:ring-opacity-50">
            <option value="Customer">Khách hàng</option>
            <option value="Admin">Admin</option>
          </select>
        </div>
        <div class="flex items-center">
          <input v-model="form.isActive" type="checkbox" class="h-4 w-4 text-primary focus:ring-primary border-gray-300 rounded">
          <label class="ml-2 block text-sm text-gray-900">Kích hoạt tài khoản</label>
        </div>
      </form>
      <template #footer>
        <button type="button" class="w-full inline-flex justify-center rounded-md border border-transparent shadow-sm px-4 py-2 bg-primary text-base font-medium text-white hover:bg-primary-dark focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-primary sm:ml-3 sm:w-auto sm:text-sm" @click="saveUser">
          {{ editingUser ? 'Cập nhật' : 'Tạo mới' }}
        </button>
        <button type="button" class="mt-3 w-full inline-flex justify-center rounded-md border border-gray-300 shadow-sm px-4 py-2 bg-white text-base font-medium text-gray-700 hover:bg-gray-50 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-indigo-500 sm:mt-0 sm:ml-3 sm:w-auto sm:text-sm" @click="closeModal">
          Hủy
        </button>
      </template>
    </Modal>

    <!-- Delete Confirmation Modal -->
    <Modal :isOpen="isDeleteModalOpen" title="Xóa người dùng" @close="closeDeleteModal" @confirm="deleteUser">
      <p class="text-sm text-gray-500">
        Bạn có chắc chắn muốn xóa người dùng <strong>{{ userToDelete?.fullName }}</strong>? Hành động này không thể hoàn tác.
      </p>
      <template #footer>
        <button type="button" class="w-full inline-flex justify-center rounded-md border border-transparent shadow-sm px-4 py-2 bg-red-600 text-base font-medium text-white hover:bg-red-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-red-500 sm:ml-3 sm:w-auto sm:text-sm" @click="deleteUser">
          Xóa
        </button>
        <button type="button" class="mt-3 w-full inline-flex justify-center rounded-md border border-gray-300 shadow-sm px-4 py-2 bg-white text-base font-medium text-gray-700 hover:bg-gray-50 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-indigo-500 sm:mt-0 sm:ml-3 sm:w-auto sm:text-sm" @click="closeDeleteModal">
          Hủy
        </button>
      </template>
    </Modal>
  </div>
</template>

<script setup>
import { ref, computed, onMounted, inject } from 'vue'
import AdminService from '../../services/adminService'
import Modal from '../../components/ui/Modal.vue'

const toast = inject('toast')
const users = ref([])
const searchQuery = ref('')
const roleFilter = ref('')
const sortKey = ref('userId')
const sortOrder = ref('desc')
const currentPage = ref(1)
const itemsPerPage = 10

const isModalOpen = ref(false)
const isDeleteModalOpen = ref(false)
const editingUser = ref(null)
const userToDelete = ref(null)

const form = ref({
  fullName: '',
  email: '',
  password: '',
  phoneNumber: '',
  address: '',
  role: 'Customer',
  isActive: true
})

const fetchUsers = async () => {
  try {
    const response = await AdminService.getUsers()
    users.value = response.data
  } catch (error) {
    toast.value?.addToast({
      title: 'Error',
      message: 'Failed to fetch users',
      type: 'error'
    })
  }
}

const filteredUsers = computed(() => {
  return users.value
    .filter(user => {
      const matchesSearch = user.fullName.toLowerCase().includes(searchQuery.value.toLowerCase()) ||
                            user.email.toLowerCase().includes(searchQuery.value.toLowerCase()) ||
                            (user.phoneNumber && user.phoneNumber.includes(searchQuery.value))
      const matchesRole = roleFilter.value ? user.role === roleFilter.value : true
      return matchesSearch && matchesRole
    })
    .sort((a, b) => {
      let modifier = sortOrder.value === 'asc' ? 1 : -1
      if (a[sortKey.value] < b[sortKey.value]) return -1 * modifier
      if (a[sortKey.value] > b[sortKey.value]) return 1 * modifier
      return 0
    })
})

const totalPages = computed(() => Math.ceil(filteredUsers.value.length / itemsPerPage))
const startIndex = computed(() => (currentPage.value - 1) * itemsPerPage)
const endIndex = computed(() => startIndex.value + itemsPerPage)
const paginatedUsers = computed(() => filteredUsers.value.slice(startIndex.value, endIndex.value))

const sortBy = (key) => {
  if (sortKey.value === key) {
    sortOrder.value = sortOrder.value === 'asc' ? 'desc' : 'asc'
  } else {
    sortKey.value = key
    sortOrder.value = 'asc'
  }
}

const formatDate = (dateString) => {
  return new Date(dateString).toLocaleDateString()
}

const openCreateModal = () => {
  editingUser.value = null
  form.value = {
    fullName: '',
    email: '',
    password: '',
    phoneNumber: '',
    address: '',
    role: 'Customer',
    isActive: true
  }
  isModalOpen.value = true
}

const openEditModal = (user) => {
  editingUser.value = user
  form.value = { ...user }
  isModalOpen.value = true
}

const closeModal = () => {
  isModalOpen.value = false
  editingUser.value = null
}

const saveUser = async () => {
  try {
    if (editingUser.value) {
      await AdminService.updateUser(editingUser.value.userId, form.value)
      toast.value?.addToast({ title: 'Success', message: 'User updated successfully', type: 'success' })
    } else {
      await AdminService.createUser(form.value)
      toast.value?.addToast({ title: 'Success', message: 'User created successfully', type: 'success' })
    }
    closeModal()
    fetchUsers()
  } catch (error) {
    toast.value?.addToast({
      title: 'Error',
      message: error.response?.data?.message || 'Failed to save user',
      type: 'error'
    })
  }
}

const confirmDelete = (user) => {
  userToDelete.value = user
  isDeleteModalOpen.value = true
}

const closeDeleteModal = () => {
  isDeleteModalOpen.value = false
  userToDelete.value = null
}

const deleteUser = async () => {
  if (!userToDelete.value) return
  
  try {
    await AdminService.deleteUser(userToDelete.value.userId)
    toast.value?.addToast({ title: 'Success', message: 'User deleted successfully', type: 'success' })
    closeDeleteModal()
    fetchUsers()
  } catch (error) {
    toast.value?.addToast({
      title: 'Error',
      message: error.response?.data?.message || 'Failed to delete user',
      type: 'error'
    })
  }
}

onMounted(() => {
  fetchUsers()
})
</script>
