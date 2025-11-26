<template>
  <div>
    <h1 class="text-2xl font-bold text-gray-900 mb-6">Quản lý đơn hàng</h1>

    <!-- Filters -->
    <div class="bg-white p-4 rounded-lg shadow-sm border border-gray-200 mb-6 flex flex-col md:flex-row gap-4">
      <div class="w-full md:w-64">
        <label class="block text-sm font-medium text-gray-700 mb-1">Lọc theo trạng thái</label>
        <select v-model="statusFilter" @change="fetchOrders" class="block w-full rounded-md border-gray-300 shadow-sm focus:border-primary focus:ring focus:ring-primary focus:ring-opacity-50">
          <option value="">Tất cả trạng thái</option>
          <option value="NotDelivered">Chưa giao</option>
          <option value="BeingDelivered">Đang giao</option>
          <option value="Delivered">Đã giao</option>
          <option value="Cancelled">Đã hủy</option>
        </select>
      </div>
      <div class="flex-1 flex items-end">
        <button @click="fetchOrders" class="bg-gray-100 hover:bg-gray-200 text-gray-800 px-4 py-2 rounded-lg flex items-center gap-2 transition-colors">
          <svg class="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M4 4v5h.582m15.356 2A8.001 8.001 0 004.582 9m0 0H9m11 11v-5h-.581m0 0a8.003 8.003 0 01-15.357-2m15.357 2H15" />
          </svg>
          Làm mới
        </button>
      </div>
    </div>

    <!-- Orders Table -->
    <div class="bg-white rounded-lg shadow-sm border border-gray-200 overflow-hidden">
      <div class="overflow-x-auto">
        <table class="min-w-full divide-y divide-gray-200">
          <thead class="bg-gray-50">
            <tr>
              <th scope="col" class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                Mã đơn
              </th>
              <th scope="col" class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                Ngày
              </th>
              <th scope="col" class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                Khách hàng
              </th>
              <th scope="col" class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                Tổng tiền
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
            <tr v-for="order in orders" :key="order.orderId">
              <td class="px-6 py-4 whitespace-nowrap text-sm font-medium text-gray-900">
                #{{ order.orderNumber }}
              </td>
              <td class="px-6 py-4 whitespace-nowrap text-sm text-gray-500">
                {{ formatDate(order.orderDate) }}
              </td>
              <td class="px-6 py-4 whitespace-nowrap text-sm text-gray-500">
                User #{{ order.userId }} <!-- Ideally fetch user name -->
              </td>
              <td class="px-6 py-4 whitespace-nowrap text-sm text-gray-900">
                {{ formatCurrency(order.totalAmount) }}
              </td>
              <td class="px-6 py-4 whitespace-nowrap">
                <span :class="getStatusClass(order.status)" class="px-2 inline-flex text-xs leading-5 font-semibold rounded-full">
                  {{ getStatusText(order.status) }}
                </span>
              </td>
              <td class="px-6 py-4 whitespace-nowrap text-right text-sm font-medium">
                <button @click="openDetailsModal(order)" class="text-primary hover:text-primary-dark">Xem chi tiết</button>
              </td>
            </tr>
            <tr v-if="orders.length === 0">
              <td colspan="6" class="px-6 py-4 text-center text-gray-500">
                Không tìm thấy đơn hàng nào.
              </td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>

    <!-- Order Details Modal -->
    <Modal :isOpen="isModalOpen" title="Chi tiết đơn hàng" @close="closeModal" @confirm="closeModal">
      <div v-if="selectedOrder" class="space-y-6">
        <!-- Order Info -->
        <div class="grid grid-cols-2 gap-4 text-sm">
          <div>
            <p class="text-gray-500">Mã đơn hàng</p>
            <p class="font-medium text-gray-900">#{{ selectedOrder.orderNumber }}</p>
          </div>
          <div>
            <p class="text-gray-500">Ngày đặt</p>
            <p class="font-medium text-gray-900">{{ formatDate(selectedOrder.orderDate) }}</p>
          </div>
          <div>
            <p class="text-gray-500">Phương thức thanh toán</p>
            <p class="font-medium text-gray-900">{{ selectedOrder.paymentMethod }}</p>
          </div>
          <div>
            <p class="text-gray-500">Trạng thái thanh toán</p>
            <p class="font-medium text-gray-900">{{ selectedOrder.paymentStatus }}</p>
          </div>
        </div>

        <!-- Delivery Address -->
        <div>
          <h4 class="font-medium text-gray-900 mb-2">Địa chỉ giao hàng</h4>
          <p class="text-sm text-gray-600 bg-gray-50 p-3 rounded-md border border-gray-200">
            {{ selectedOrder.deliveryAddress }}
          </p>
        </div>

        <!-- Order Items -->
        <div>
          <h4 class="font-medium text-gray-900 mb-2">Danh sách món</h4>
          <div class="border border-gray-200 rounded-md overflow-hidden">
            <table class="min-w-full divide-y divide-gray-200">
              <thead class="bg-gray-50">
                <tr>
                  <th class="px-4 py-2 text-left text-xs font-medium text-gray-500 uppercase">Món</th>
                  <th class="px-4 py-2 text-center text-xs font-medium text-gray-500 uppercase">SL</th>
                  <th class="px-4 py-2 text-right text-xs font-medium text-gray-500 uppercase">Tổng</th>
                </tr>
              </thead>
              <tbody class="bg-white divide-y divide-gray-200">
                <tr v-for="item in selectedOrder.items" :key="item.orderItemId">
                  <td class="px-4 py-2 text-sm text-gray-900">
                    {{ item.foodItemName || item.comboName }}
                    <span v-if="item.comboId" class="text-xs text-blue-600 ml-1">(Combo)</span>
                  </td>
                  <td class="px-4 py-2 text-center text-sm text-gray-500">{{ item.quantity }}</td>
                  <td class="px-4 py-2 text-right text-sm text-gray-900">{{ formatCurrency(item.subtotal) }}</td>
                </tr>
              </tbody>
              <tfoot class="bg-gray-50">
                <tr>
                  <td colspan="2" class="px-4 py-2 text-right text-sm font-medium text-gray-900">Tổng tiền:</td>
                  <td class="px-4 py-2 text-right text-sm font-bold text-gray-900">{{ formatCurrency(selectedOrder.totalAmount) }}</td>
                </tr>
              </tfoot>
            </table>
          </div>
        </div>

        <!-- Status Management -->
        <div class="border-t border-gray-200 pt-4">
          <h4 class="font-medium text-gray-900 mb-2">Cập nhật trạng thái</h4>
          <div class="flex gap-2">
            <select v-model="newStatus" class="flex-1 rounded-md border-gray-300 shadow-sm focus:border-primary focus:ring focus:ring-primary focus:ring-opacity-50">
              <option value="NotDelivered">Chưa giao</option>
              <option value="BeingDelivered">Đang giao</option>
              <option value="Delivered">Đã giao</option>
              <option value="Cancelled">Đã hủy</option>
            </select>
            <button @click="updateStatus" class="bg-primary hover:bg-primary-dark text-white px-4 py-2 rounded-lg transition-colors" :disabled="updating">
              {{ updating ? 'Đang cập nhật...' : 'Cập nhật' }}
            </button>
          </div>
        </div>
      </div>
      <template #footer>
        <button type="button" class="w-full inline-flex justify-center rounded-md border border-gray-300 shadow-sm px-4 py-2 bg-white text-base font-medium text-gray-700 hover:bg-gray-50 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-indigo-500 sm:w-auto sm:text-sm" @click="closeModal">
          Đóng
        </button>
      </template>
    </Modal>
  </div>
</template>

<script setup>
import { ref, onMounted, inject } from 'vue'
import AdminService from '../../services/adminService'
import Modal from '../../components/ui/Modal.vue'

const toast = inject('toast')
const orders = ref([])
const statusFilter = ref('')
const isModalOpen = ref(false)
const selectedOrder = ref(null)
const newStatus = ref('')
const updating = ref(false)

const fetchOrders = async () => {
  try {
    const params = {}
    if (statusFilter.value) {
      params.status = statusFilter.value
    }
    const response = await AdminService.getOrders(params)
    orders.value = response.data
  } catch (error) {
    toast.value?.addToast({
      title: 'Error',
      message: 'Failed to fetch orders',
      type: 'error'
    })
  }
}

const formatCurrency = (value) => {
  return new Intl.NumberFormat('vi-VN', { style: 'currency', currency: 'VND' }).format(value * 1000)
}

const formatDate = (dateString) => {
  return new Date(dateString).toLocaleDateString('vi-VN', {
    year: 'numeric',
    month: 'short',
    day: 'numeric',
    hour: '2-digit',
    minute: '2-digit'
  })
}

const getStatusClass = (status) => {
  switch (status) {
    case 'Delivered':
      return 'bg-green-100 text-green-800'
    case 'BeingDelivered':
      return 'bg-blue-100 text-blue-800'
    case 'NotDelivered':
      return 'bg-yellow-100 text-yellow-800'
    case 'Cancelled':
      return 'bg-red-100 text-red-800'
    default:
      return 'bg-gray-100 text-gray-800'
  }
}

const getStatusText = (status) => {
  switch (status) {
    case 'Delivered':
      return 'Đã giao'
    case 'BeingDelivered':
      return 'Đang giao'
    case 'NotDelivered':
      return 'Chưa giao'
    case 'Cancelled':
      return 'Đã hủy'
    default:
      return status
  }
}

const openDetailsModal = (order) => {
  selectedOrder.value = order
  newStatus.value = order.status
  isModalOpen.value = true
}

const closeModal = () => {
  isModalOpen.value = false
  selectedOrder.value = null
}

const updateStatus = async () => {
  if (!selectedOrder.value || newStatus.value === selectedOrder.value.status) return

  updating.value = true
  try {
    await AdminService.updateOrderStatus(selectedOrder.value.orderId, newStatus.value)
    
    // Update local state
    selectedOrder.value.status = newStatus.value
    const orderIndex = orders.value.findIndex(o => o.orderId === selectedOrder.value.orderId)
    if (orderIndex !== -1) {
      orders.value[orderIndex].status = newStatus.value
    }

    toast.value?.addToast({ title: 'Success', message: 'Order status updated successfully', type: 'success' })
  } catch (error) {
    toast.value?.addToast({
      title: 'Error',
      message: error.response?.data?.message || 'Failed to update status',
      type: 'error'
    })
  } finally {
    updating.value = false
  }
}

onMounted(() => {
  fetchOrders()
})
</script>
