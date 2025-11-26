<template>
  <div>
    <div class="flex justify-between items-center mb-6">
      <h1 class="text-2xl font-bold text-gray-900">Quản lý món ăn</h1>
      <button @click="openCreateModal" class="bg-primary hover:bg-primary-dark text-white px-4 py-2 rounded-lg flex items-center gap-2">
        <svg class="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24">
          <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 4v16m8-8H4" />
        </svg>
        Thêm món ăn
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
        <input v-model="searchQuery" type="text" class="pl-10 block w-full rounded-md border-gray-300 shadow-sm focus:border-primary focus:ring focus:ring-primary focus:ring-opacity-50" placeholder="Tìm kiếm món...">
      </div>
      <div class="w-full md:w-48">
        <select v-model="categoryFilter" class="block w-full rounded-md border-gray-300 shadow-sm focus:border-primary focus:ring focus:ring-primary focus:ring-opacity-50">
          <option value="">Tất cả danh mục</option>
          <option v-for="cat in categories" :key="cat" :value="cat">{{ cat }}</option>
        </select>
      </div>
    </div>

    <!-- Food Items Table -->
    <div class="bg-white rounded-lg shadow-sm border border-gray-200 overflow-hidden">
      <div class="overflow-x-auto">
        <table class="min-w-full divide-y divide-gray-200">
          <thead class="bg-gray-50">
            <tr>
              <th scope="col" class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                Ảnh
              </th>
              <th scope="col" class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider cursor-pointer" @click="sortBy('name')">
                Tên <span v-if="sortKey === 'name'">{{ sortOrder === 'asc' ? '↑' : '↓' }}</span>
              </th>
              <th scope="col" class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider cursor-pointer" @click="sortBy('category')">
                Danh mục <span v-if="sortKey === 'category'">{{ sortOrder === 'asc' ? '↑' : '↓' }}</span>
              </th>
              <th scope="col" class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider cursor-pointer" @click="sortBy('price')">
                Giá <span v-if="sortKey === 'price'">{{ sortOrder === 'asc' ? '↑' : '↓' }}</span>
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
            <tr v-for="item in paginatedItems" :key="item.foodItemId">
              <td class="px-6 py-4 whitespace-nowrap">
                <img :src="item.imageUrl" alt="" class="h-12 w-12 rounded-md object-cover">
              </td>
              <td class="px-6 py-4 whitespace-nowrap">
                <div class="text-sm font-medium text-gray-900">{{ item.name }}</div>
                <div class="text-sm text-gray-500 truncate max-w-xs">{{ item.description }}</div>
              </td>
              <td class="px-6 py-4 whitespace-nowrap">
                <span class="px-2 inline-flex text-xs leading-5 font-semibold rounded-full bg-blue-100 text-blue-800">
                  {{ item.category }}
                </span>
              </td>
              <td class="px-6 py-4 whitespace-nowrap text-sm text-gray-900">
                {{ formatCurrency(item.price) }}
              </td>
              <td class="px-6 py-4 whitespace-nowrap">
                <span class="px-2 inline-flex text-xs leading-5 font-semibold rounded-full" :class="item.isActive ? 'bg-green-100 text-green-800' : 'bg-red-100 text-red-800'">
                  {{ item.isActive ? 'Đang bán' : 'Ngừng bán' }}
                </span>
              </td>
              <td class="px-6 py-4 whitespace-nowrap text-right text-sm font-medium">
                <button @click="openEditModal(item)" class="text-indigo-600 hover:text-indigo-900 mr-3">Sửa</button>
                <button @click="confirmDelete(item)" class="text-red-600 hover:text-red-900">Xóa</button>
              </td>
            </tr>
            <tr v-if="paginatedItems.length === 0">
              <td colspan="6" class="px-6 py-4 text-center text-gray-500">
                Không tìm thấy món ăn nào.
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
              Hiển thị <span class="font-medium">{{ startIndex + 1 }}</span> đến <span class="font-medium">{{ Math.min(endIndex, filteredItems.length) }}</span> trong <span class="font-medium">{{ filteredItems.length }}</span> kết quả
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
    <Modal :isOpen="isModalOpen" :title="editingItem ? 'Sửa món ăn' : 'Thêm món ăn'" @close="closeModal" @confirm="saveItem">
      <form @submit.prevent="saveItem" class="space-y-4">
        <div>
          <label class="block text-sm font-medium text-gray-700">Tên món</label>
          <input v-model="form.name" type="text" required class="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-primary focus:ring focus:ring-primary focus:ring-opacity-50">
        </div>
        <div>
          <label class="block text-sm font-medium text-gray-700">Mô tả</label>
          <textarea v-model="form.description" rows="3" class="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-primary focus:ring focus:ring-primary focus:ring-opacity-50"></textarea>
        </div>
        <div class="grid grid-cols-2 gap-4">
          <div>
            <label class="block text-sm font-medium text-gray-700">Giá</label>
            <input v-model.number="form.price" type="number" min="0" step="0.01" required class="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-primary focus:ring focus:ring-primary focus:ring-opacity-50">
          </div>
          <div>
            <label class="block text-sm font-medium text-gray-700">Danh mục</label>
            <input v-model="form.category" type="text" required list="category-list" class="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-primary focus:ring focus:ring-primary focus:ring-opacity-50">
            <datalist id="category-list">
              <option v-for="cat in categories" :key="cat" :value="cat"></option>
            </datalist>
          </div>
        </div>
        <div>
          <label class="block text-sm font-medium text-gray-700">Link ảnh</label>
          <input v-model="form.imageUrl" type="url" class="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-primary focus:ring focus:ring-primary focus:ring-opacity-50">
          <div v-if="form.imageUrl" class="mt-2">
            <img :src="form.imageUrl" alt="Preview" class="h-20 w-20 object-cover rounded-md">
          </div>
        </div>
        <div>
          <label class="block text-sm font-medium text-gray-700">Chủ đề (Tùy chọn)</label>
          <input v-model="form.theme" type="text" class="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-primary focus:ring focus:ring-primary focus:ring-opacity-50">
        </div>
        <div class="flex items-center">
          <input v-model="form.isActive" type="checkbox" class="h-4 w-4 text-primary focus:ring-primary border-gray-300 rounded">
          <label class="ml-2 block text-sm text-gray-900">Đang bán</label>
        </div>
      </form>
      <template #footer>
        <button type="button" class="w-full inline-flex justify-center rounded-md border border-transparent shadow-sm px-4 py-2 bg-primary text-base font-medium text-white hover:bg-primary-dark focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-primary sm:ml-3 sm:w-auto sm:text-sm" @click="saveItem">
          {{ editingItem ? 'Cập nhật' : 'Tạo mới' }}
        </button>
        <button type="button" class="mt-3 w-full inline-flex justify-center rounded-md border border-gray-300 shadow-sm px-4 py-2 bg-white text-base font-medium text-gray-700 hover:bg-gray-50 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-indigo-500 sm:mt-0 sm:ml-3 sm:w-auto sm:text-sm" @click="closeModal">
          Hủy
        </button>
      </template>
    </Modal>

    <!-- Delete Confirmation Modal -->
    <Modal :isOpen="isDeleteModalOpen" title="Xóa món ăn" @close="closeDeleteModal" @confirm="deleteItem">
      <p class="text-sm text-gray-500">
        Bạn có chắc chắn muốn xóa <strong>{{ itemToDelete?.name }}</strong>? Hành động này không thể hoàn tác.
      </p>
      <template #footer>
        <button type="button" class="w-full inline-flex justify-center rounded-md border border-transparent shadow-sm px-4 py-2 bg-red-600 text-base font-medium text-white hover:bg-red-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-red-500 sm:ml-3 sm:w-auto sm:text-sm" @click="deleteItem">
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
const items = ref([])
const categories = ref([])
const searchQuery = ref('')
const categoryFilter = ref('')
const sortKey = ref('name')
const sortOrder = ref('asc')
const currentPage = ref(1)
const itemsPerPage = 10

const isModalOpen = ref(false)
const isDeleteModalOpen = ref(false)
const editingItem = ref(null)
const itemToDelete = ref(null)

const form = ref({
  name: '',
  description: '',
  price: 0,
  category: '',
  theme: '',
  imageUrl: '',
  isActive: true
})

const fetchItems = async () => {
  try {
    const response = await AdminService.getFoodItems()
    items.value = response.data
    
    // Extract unique categories
    const cats = new Set(items.value.map(i => i.category))
    categories.value = Array.from(cats).sort()
  } catch (error) {
    toast.value?.addToast({
      title: 'Error',
      message: 'Failed to fetch food items',
      type: 'error'
    })
  }
}

const filteredItems = computed(() => {
  return items.value
    .filter(item => {
      const matchesSearch = item.name.toLowerCase().includes(searchQuery.value.toLowerCase()) ||
                            (item.description && item.description.toLowerCase().includes(searchQuery.value.toLowerCase()))
      const matchesCategory = categoryFilter.value ? item.category === categoryFilter.value : true
      return matchesSearch && matchesCategory
    })
    .sort((a, b) => {
      let modifier = sortOrder.value === 'asc' ? 1 : -1
      if (a[sortKey.value] < b[sortKey.value]) return -1 * modifier
      if (a[sortKey.value] > b[sortKey.value]) return 1 * modifier
      return 0
    })
})

const totalPages = computed(() => Math.ceil(filteredItems.value.length / itemsPerPage))
const startIndex = computed(() => (currentPage.value - 1) * itemsPerPage)
const endIndex = computed(() => startIndex.value + itemsPerPage)
const paginatedItems = computed(() => filteredItems.value.slice(startIndex.value, endIndex.value))

const sortBy = (key) => {
  if (sortKey.value === key) {
    sortOrder.value = sortOrder.value === 'asc' ? 'desc' : 'asc'
  } else {
    sortKey.value = key
    sortOrder.value = 'asc'
  }
}

const formatCurrency = (value) => {
  return new Intl.NumberFormat('vi-VN', { style: 'currency', currency: 'VND' }).format(value * 1000)
}

const openCreateModal = () => {
  editingItem.value = null
  form.value = {
    name: '',
    description: '',
    price: 0,
    category: '',
    theme: '',
    imageUrl: '',
    isActive: true
  }
  isModalOpen.value = true
}

const openEditModal = (item) => {
  editingItem.value = item
  form.value = { ...item }
  isModalOpen.value = true
}

const closeModal = () => {
  isModalOpen.value = false
  editingItem.value = null
}

const saveItem = async () => {
  try {
    if (editingItem.value) {
      await AdminService.updateFoodItem(editingItem.value.foodItemId, form.value)
      toast.value?.addToast({ title: 'Success', message: 'Item updated successfully', type: 'success' })
    } else {
      await AdminService.createFoodItem(form.value)
      toast.value?.addToast({ title: 'Success', message: 'Item created successfully', type: 'success' })
    }
    closeModal()
    fetchItems()
  } catch (error) {
    toast.value?.addToast({
      title: 'Error',
      message: error.response?.data?.message || 'Failed to save item',
      type: 'error'
    })
  }
}

const confirmDelete = (item) => {
  itemToDelete.value = item
  isDeleteModalOpen.value = true
}

const closeDeleteModal = () => {
  isDeleteModalOpen.value = false
  itemToDelete.value = null
}

const deleteItem = async () => {
  if (!itemToDelete.value) return
  
  try {
    await AdminService.deleteFoodItem(itemToDelete.value.foodItemId)
    toast.value?.addToast({ title: 'Success', message: 'Item deleted successfully', type: 'success' })
    closeDeleteModal()
    fetchItems()
  } catch (error) {
    toast.value?.addToast({
      title: 'Error',
      message: error.response?.data?.message || 'Failed to delete item',
      type: 'error'
    })
  }
}

onMounted(() => {
  fetchItems()
})
</script>
