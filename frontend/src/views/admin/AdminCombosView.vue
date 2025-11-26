<template>
  <div>
    <div class="flex justify-between items-center mb-6">
      <h1 class="text-2xl font-bold text-gray-900">Quản lý Combo</h1>
      <button @click="openCreateModal" class="bg-primary hover:bg-primary-dark text-white px-4 py-2 rounded-lg flex items-center gap-2">
        <svg class="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24">
          <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 4v16m8-8H4" />
        </svg>
        Thêm Combo
      </button>
    </div>

    <!-- Combos Table -->
    <div class="bg-white rounded-lg shadow-sm border border-gray-200 overflow-hidden">
      <div class="overflow-x-auto">
        <table class="min-w-full divide-y divide-gray-200">
          <thead class="bg-gray-50">
            <tr>
              <th scope="col" class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                Ảnh
              </th>
              <th scope="col" class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                Tên
              </th>
              <th scope="col" class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                Món trong combo
              </th>
              <th scope="col" class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                Giá
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
            <tr v-for="combo in combos" :key="combo.comboId">
              <td class="px-6 py-4 whitespace-nowrap">
                <img :src="combo.imageUrl" alt="" class="h-12 w-12 rounded-md object-cover">
              </td>
              <td class="px-6 py-4 whitespace-nowrap">
                <div class="text-sm font-medium text-gray-900">{{ combo.name }}</div>
                <div class="text-sm text-gray-500 truncate max-w-xs">{{ combo.description }}</div>
              </td>
              <td class="px-6 py-4">
                <div class="flex flex-wrap gap-1">
                  <span v-for="item in combo.comboItems" :key="item.foodItemId" class="px-2 py-0.5 rounded text-xs bg-gray-100 text-gray-800 border border-gray-200">
                    {{ item.quantity }}x {{ item.foodItemName }}
                  </span>
                </div>
              </td>
              <td class="px-6 py-4 whitespace-nowrap text-sm text-gray-900">
                {{ formatCurrency(combo.price) }}
              </td>
              <td class="px-6 py-4 whitespace-nowrap">
                <span class="px-2 inline-flex text-xs leading-5 font-semibold rounded-full" :class="combo.isActive ? 'bg-green-100 text-green-800' : 'bg-red-100 text-red-800'">
                  {{ combo.isActive ? 'Đang bán' : 'Ngừng bán' }}
                </span>
              </td>
              <td class="px-6 py-4 whitespace-nowrap text-right text-sm font-medium">
                <button @click="openEditModal(combo)" class="text-indigo-600 hover:text-indigo-900 mr-3">Sửa</button>
                <button @click="confirmDelete(combo)" class="text-red-600 hover:text-red-900">Xóa</button>
              </td>
            </tr>
            <tr v-if="combos.length === 0">
              <td colspan="6" class="px-6 py-4 text-center text-gray-500">
                Không tìm thấy combo nào.
              </td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>

    <!-- Create/Edit Modal -->
    <Modal :isOpen="isModalOpen" :title="editingCombo ? 'Sửa Combo' : 'Thêm Combo'" @close="closeModal" @confirm="saveCombo">
      <form @submit.prevent="saveCombo" class="space-y-4">
        <div>
          <label class="block text-sm font-medium text-gray-700">Tên Combo</label>
          <input v-model="form.name" type="text" required class="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-primary focus:ring focus:ring-primary focus:ring-opacity-50">
        </div>
        <div>
          <label class="block text-sm font-medium text-gray-700">Mô tả</label>
          <textarea v-model="form.description" rows="2" class="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-primary focus:ring focus:ring-primary focus:ring-opacity-50"></textarea>
        </div>
        <div>
          <label class="block text-sm font-medium text-gray-700">Link ảnh</label>
          <input v-model="form.imageUrl" type="url" class="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-primary focus:ring focus:ring-primary focus:ring-opacity-50">
        </div>
        
        <!-- Combo Items Selection -->
        <div class="border-t border-b border-gray-200 py-4 my-4">
          <h4 class="font-medium text-gray-900 mb-2">Món trong Combo</h4>
          
          <div class="flex gap-2 mb-2">
            <select v-model="selectedFoodItem" class="flex-1 rounded-md border-gray-300 shadow-sm focus:border-primary focus:ring focus:ring-primary focus:ring-opacity-50">
              <option :value="null">Chọn món ăn</option>
              <option v-for="item in availableFoodItems" :key="item.foodItemId" :value="item">
                {{ item.name }} - {{ formatCurrency(item.price) }}
              </option>
            </select>
            <input v-model.number="selectedQuantity" type="number" min="1" class="w-20 rounded-md border-gray-300 shadow-sm focus:border-primary focus:ring focus:ring-primary focus:ring-opacity-50" placeholder="SL">
            <button type="button" @click="addComboItem" :disabled="!selectedFoodItem" class="bg-gray-200 hover:bg-gray-300 text-gray-800 px-3 py-2 rounded-md">
              Thêm
            </button>
          </div>

          <!-- Selected Items List -->
          <div class="bg-gray-50 rounded-md p-2 space-y-2 max-h-40 overflow-y-auto">
            <div v-for="(item, index) in form.comboItems" :key="index" class="flex justify-between items-center bg-white p-2 rounded border border-gray-200 shadow-sm">
              <span class="text-sm">{{ item.quantity }}x {{ getFoodItemName(item.foodItemId) }}</span>
              <div class="flex items-center gap-2">
                <span class="text-xs text-gray-500">{{ formatCurrency(getFoodItemPrice(item.foodItemId) * item.quantity) }}</span>
                <button type="button" @click="removeComboItem(index)" class="text-red-500 hover:text-red-700">
                  <svg class="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                    <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M6 18L18 6M6 6l12 12" />
                  </svg>
                </button>
              </div>
            </div>
            <div v-if="form.comboItems.length === 0" class="text-center text-gray-500 text-sm py-2">
              Chưa có món nào được thêm vào combo.
            </div>
          </div>
          
          <div class="mt-2 text-right text-sm text-gray-600">
            Tổng giá trị: <span class="font-bold">{{ formatCurrency(itemsTotalValue) }}</span>
          </div>
        </div>

        <div>
          <label class="block text-sm font-medium text-gray-700">Giá bán Combo</label>
          <input v-model.number="form.price" type="number" min="0" step="0.01" required class="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-primary focus:ring focus:ring-primary focus:ring-opacity-50">
          <p v-if="form.price >= itemsTotalValue && itemsTotalValue > 0" class="text-red-500 text-xs mt-1">
            Giá combo nên thấp hơn tổng giá trị các món lẻ ({{ formatCurrency(itemsTotalValue) }}).
          </p>
          <p v-else-if="itemsTotalValue > 0" class="text-green-600 text-xs mt-1">
            Tiết kiệm: {{ formatCurrency(itemsTotalValue - form.price) }}
          </p>
        </div>

        <div class="flex items-center">
          <input v-model="form.isActive" type="checkbox" class="h-4 w-4 text-primary focus:ring-primary border-gray-300 rounded">
          <label class="ml-2 block text-sm text-gray-900">Đang bán</label>
        </div>
      </form>
      <template #footer>
        <button type="button" class="w-full inline-flex justify-center rounded-md border border-transparent shadow-sm px-4 py-2 bg-primary text-base font-medium text-white hover:bg-primary-dark focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-primary sm:ml-3 sm:w-auto sm:text-sm" @click="saveCombo">
          {{ editingCombo ? 'Cập nhật' : 'Tạo mới' }}
        </button>
        <button type="button" class="mt-3 w-full inline-flex justify-center rounded-md border border-gray-300 shadow-sm px-4 py-2 bg-white text-base font-medium text-gray-700 hover:bg-gray-50 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-indigo-500 sm:mt-0 sm:ml-3 sm:w-auto sm:text-sm" @click="closeModal">
          Hủy
        </button>
      </template>
    </Modal>

    <!-- Delete Confirmation Modal -->
    <Modal :isOpen="isDeleteModalOpen" title="Xóa Combo" @close="closeDeleteModal" @confirm="deleteCombo">
      <p class="text-sm text-gray-500">
        Bạn có chắc chắn muốn xóa <strong>{{ comboToDelete?.name }}</strong>? Hành động này không thể hoàn tác.
      </p>
      <template #footer>
        <button type="button" class="w-full inline-flex justify-center rounded-md border border-transparent shadow-sm px-4 py-2 bg-red-600 text-base font-medium text-white hover:bg-red-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-red-500 sm:ml-3 sm:w-auto sm:text-sm" @click="deleteCombo">
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
const combos = ref([])
const foodItems = ref([])

const isModalOpen = ref(false)
const isDeleteModalOpen = ref(false)
const editingCombo = ref(null)
const comboToDelete = ref(null)

const selectedFoodItem = ref(null)
const selectedQuantity = ref(1)

const form = ref({
  name: '',
  description: '',
  price: 0,
  imageUrl: '',
  isActive: true,
  comboItems: []
})

const fetchCombos = async () => {
  try {
    const response = await AdminService.getCombos()
    combos.value = response.data
  } catch (error) {
    toast.value?.addToast({
      title: 'Error',
      message: 'Failed to fetch combos',
      type: 'error'
    })
  }
}

const fetchFoodItems = async () => {
  try {
    const response = await AdminService.getFoodItems()
    foodItems.value = response.data
  } catch (error) {
    console.error('Failed to fetch food items', error)
  }
}

const availableFoodItems = computed(() => {
  return foodItems.value.filter(item => item.isActive)
})

const itemsTotalValue = computed(() => {
  return form.value.comboItems.reduce((sum, item) => {
    const foodItem = foodItems.value.find(f => f.foodItemId === item.foodItemId)
    return sum + (foodItem ? foodItem.price * item.quantity : 0)
  }, 0)
})

const getFoodItemName = (id) => {
  const item = foodItems.value.find(f => f.foodItemId === id)
  return item ? item.name : 'Unknown Item'
}

const getFoodItemPrice = (id) => {
  const item = foodItems.value.find(f => f.foodItemId === id)
  return item ? item.price : 0
}

const formatCurrency = (value) => {
  return new Intl.NumberFormat('vi-VN', { style: 'currency', currency: 'VND' }).format(value * 1000)
}

const addComboItem = () => {
  if (!selectedFoodItem.value) return

  const existingItemIndex = form.value.comboItems.findIndex(i => i.foodItemId === selectedFoodItem.value.foodItemId)
  
  if (existingItemIndex >= 0) {
    form.value.comboItems[existingItemIndex].quantity += selectedQuantity.value
  } else {
    form.value.comboItems.push({
      foodItemId: selectedFoodItem.value.foodItemId,
      quantity: selectedQuantity.value
    })
  }
  
  selectedFoodItem.value = null
  selectedQuantity.value = 1
}

const removeComboItem = (index) => {
  form.value.comboItems.splice(index, 1)
}

const openCreateModal = () => {
  editingCombo.value = null
  form.value = {
    name: '',
    description: '',
    price: 0,
    imageUrl: '',
    isActive: true,
    comboItems: []
  }
  isModalOpen.value = true
}

const openEditModal = (combo) => {
  editingCombo.value = combo
  // Deep copy to avoid modifying original while editing
  form.value = JSON.parse(JSON.stringify(combo))
  // Ensure comboItems structure matches what we expect (API might return nested objects)
  // The API returns ComboItems with FoodItemId, FoodItemName, etc.
  // We just need FoodItemId and Quantity for the update request
  form.value.comboItems = combo.comboItems.map(item => ({
    foodItemId: item.foodItemId,
    quantity: item.quantity
  }))
  
  isModalOpen.value = true
}

const closeModal = () => {
  isModalOpen.value = false
  editingCombo.value = null
}

const saveCombo = async () => {
  if (form.value.comboItems.length === 0) {
    toast.value?.addToast({ title: 'Lỗi', message: 'Vui lòng thêm ít nhất một món vào combo', type: 'error' })
    return
  }

  try {
    if (editingCombo.value) {
      await AdminService.updateCombo(editingCombo.value.comboId, form.value)
      toast.value?.addToast({ title: 'Success', message: 'Combo updated successfully', type: 'success' })
    } else {
      await AdminService.createCombo(form.value)
      toast.value?.addToast({ title: 'Success', message: 'Combo created successfully', type: 'success' })
    }
    closeModal()
    fetchCombos()
  } catch (error) {
    toast.value?.addToast({
      title: 'Error',
      message: error.response?.data?.message || 'Failed to save combo',
      type: 'error'
    })
  }
}

const confirmDelete = (combo) => {
  comboToDelete.value = combo
  isDeleteModalOpen.value = true
}

const closeDeleteModal = () => {
  isDeleteModalOpen.value = false
  comboToDelete.value = null
}

const deleteCombo = async () => {
  if (!comboToDelete.value) return
  
  try {
    await AdminService.deleteCombo(comboToDelete.value.comboId)
    toast.value?.addToast({ title: 'Success', message: 'Combo deleted successfully', type: 'success' })
    closeDeleteModal()
    fetchCombos()
  } catch (error) {
    toast.value?.addToast({
      title: 'Error',
      message: error.response?.data?.message || 'Failed to delete combo',
      type: 'error'
    })
  }
}

onMounted(() => {
  fetchCombos()
  fetchFoodItems()
})
</script>
