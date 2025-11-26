<template>
  <div>
    <h1 class="text-2xl font-bold mb-6">Tổng quan</h1>
    
    <!-- Stats Grid -->
    <div class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-6 mb-8">
      <div class="bg-white p-6 rounded-lg shadow-sm border border-gray-200">
        <div class="flex items-center justify-between mb-4">
          <h3 class="text-gray-500 text-sm font-medium">Tổng doanh thu</h3>
          <span class="p-2 bg-green-100 text-green-600 rounded-full">💰</span>
        </div>
        <p class="text-2xl font-bold text-gray-900">{{ formatCurrency(stats.totalRevenue) }}</p>
        <p class="text-sm text-green-600 mt-2 flex items-center gap-1">
          <span>↑</span>
          <span>Từ tất cả đơn hàng</span>
        </p>
      </div>

      <div class="bg-white p-6 rounded-lg shadow-sm border border-gray-200">
        <div class="flex items-center justify-between mb-4">
          <h3 class="text-gray-500 text-sm font-medium">Tổng đơn hàng</h3>
          <span class="p-2 bg-blue-100 text-blue-600 rounded-full">📦</span>
        </div>
        <p class="text-2xl font-bold text-gray-900">{{ stats.totalOrders }}</p>
        <p class="text-sm text-gray-500 mt-2">
          {{ stats.pendingOrders }} đơn chờ xử lý
        </p>
      </div>

      <div class="bg-white p-6 rounded-lg shadow-sm border border-gray-200">
        <div class="flex items-center justify-between mb-4">
          <h3 class="text-gray-500 text-sm font-medium">Tổng người dùng</h3>
          <span class="p-2 bg-purple-100 text-purple-600 rounded-full">👥</span>
        </div>
        <p class="text-2xl font-bold text-gray-900">{{ stats.totalUsers }}</p>
        <p class="text-sm text-gray-500 mt-2">
          Khách hàng đăng ký
        </p>
      </div>

      <div class="bg-white p-6 rounded-lg shadow-sm border border-gray-200">
        <div class="flex items-center justify-between mb-4">
          <h3 class="text-gray-500 text-sm font-medium">Món ăn</h3>
          <span class="p-2 bg-orange-100 text-orange-600 rounded-full">🍔</span>
        </div>
        <p class="text-2xl font-bold text-gray-900">{{ stats.totalItems }}</p>
        <p class="text-sm text-gray-500 mt-2">
          Món đang bán
        </p>
      </div>
    </div>

    <!-- Recent Orders -->
    <div class="bg-white rounded-lg shadow-sm border border-gray-200 overflow-hidden">
      <div class="px-6 py-4 border-b border-gray-200 flex justify-between items-center">
        <h2 class="text-lg font-semibold text-gray-900">Đơn hàng gần đây</h2>
        <router-link to="/admin/orders" class="text-primary hover:text-primary-dark text-sm font-medium">Xem tất cả</router-link>
      </div>
      <div class="overflow-x-auto">
        <table class="min-w-full divide-y divide-gray-200">
          <thead class="bg-gray-50">
            <tr>
              <th class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Mã đơn</th>
              <th class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Khách hàng</th>
              <th class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Ngày</th>
              <th class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Tổng tiền</th>
              <th class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Trạng thái</th>
            </tr>
          </thead>
          <tbody class="bg-white divide-y divide-gray-200">
            <tr v-for="order in recentOrders" :key="order.orderId">
              <td class="px-6 py-4 whitespace-nowrap text-sm font-medium text-gray-900">#{{ order.orderNumber }}</td>
              <td class="px-6 py-4 whitespace-nowrap text-sm text-gray-500">{{ order.userId }}</td> <!-- Ideally fetch user name -->
              <td class="px-6 py-4 whitespace-nowrap text-sm text-gray-500">{{ formatDate(order.orderDate) }}</td>
              <td class="px-6 py-4 whitespace-nowrap text-sm text-gray-900">{{ formatCurrency(order.totalAmount) }}</td>
              <td class="px-6 py-4 whitespace-nowrap">
                <span :class="getStatusClass(order.status)" class="px-2 inline-flex text-xs leading-5 font-semibold rounded-full">
                  {{ order.status }}
                </span>
              </td>
            </tr>
            <tr v-if="recentOrders.length === 0">
              <td colspan="5" class="px-6 py-4 text-center text-gray-500">Không tìm thấy đơn hàng nào</td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import AdminService from '../../services/adminService'

const stats = ref({
  totalRevenue: 0,
  totalOrders: 0,
  pendingOrders: 0,
  totalUsers: 0,
  totalItems: 0
})

const recentOrders = ref([])

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

const fetchDashboardData = async () => {
  try {
    // Fetch all data in parallel
    const [usersRes, itemsRes, ordersRes] = await Promise.all([
      AdminService.getUsers(),
      AdminService.getFoodItems(),
      AdminService.getOrders()
    ])

    const users = usersRes.data
    const items = itemsRes.data
    const orders = ordersRes.data

    // Calculate stats
    stats.value.totalUsers = users.length
    stats.value.totalItems = items.length
    stats.value.totalOrders = orders.length
    stats.value.totalRevenue = orders.reduce((sum, order) => sum + order.totalAmount, 0)
    stats.value.pendingOrders = orders.filter(o => o.status === 'NotDelivered').length

    // Get recent 5 orders
    recentOrders.value = orders.slice(0, 5)

  } catch (error) {
    console.error('Error fetching dashboard data:', error)
  }
}

onMounted(() => {
  fetchDashboardData()
})
</script>
