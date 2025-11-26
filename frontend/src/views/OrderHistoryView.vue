<template>
  <!-- Order History View -->
  <div class="container mx-auto px-4 py-8">
    <h1 class="text-3xl font-bold mb-8 text-gray-800">Lịch sử đơn hàng</h1>

    <!-- Filters -->
    <div class="bg-white p-6 rounded-lg shadow-md mb-8">
      <h2 class="text-xl font-semibold mb-4 text-gray-700">Lọc đơn hàng</h2>
      <div class="flex flex-wrap gap-4 items-end">
        <div>
          <label class="block text-sm font-medium text-gray-700 mb-1">Từ ngày</label>
          <input 
            type="date" 
            v-model="startDate"
            class="w-full px-4 py-2 border border-gray-300 rounded-md focus:ring-primary focus:border-primary"
          >
        </div>
        <div>
          <label class="block text-sm font-medium text-gray-700 mb-1">Đến ngày</label>
          <input 
            type="date" 
            v-model="endDate"
            class="w-full px-4 py-2 border border-gray-300 rounded-md focus:ring-primary focus:border-primary"
          >
        </div>
        <button 
          @click="fetchInvoices"
          class="px-6 py-2 bg-primary text-white rounded-md hover:bg-primary-dark transition-colors"
        >
          Áp dụng
        </button>
        <button 
          @click="clearFilters"
          class="px-6 py-2 bg-gray-200 text-gray-700 rounded-md hover:bg-gray-300 transition-colors"
        >
          Xóa
        </button>
      </div>
    </div>

    <!-- Loading State -->
    <div v-if="loading" class="text-center py-12">
      <div class="animate-spin rounded-full h-12 w-12 border-b-2 border-primary mx-auto"></div>
      <p class="mt-4 text-gray-600">Đang tải đơn hàng...</p>
    </div>

    <!-- Error State -->
    <div v-else-if="error" class="bg-red-100 border border-red-400 text-red-700 px-4 py-3 rounded mb-6">
      {{ error }}
    </div>

    <!-- Empty State -->
    <div v-else-if="invoices.length === 0" class="text-center py-12 bg-white rounded-lg shadow-md">
      <svg xmlns="http://www.w3.org/2000/svg" class="h-16 w-16 mx-auto text-gray-400 mb-4" fill="none" viewBox="0 0 24 24" stroke="currentColor">
        <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 5H7a2 2 0 00-2 2v12a2 2 0 002 2h10a2 2 0 002-2V7a2 2 0 00-2-2h-2M9 5a2 2 0 002 2h2a2 2 0 002-2M9 5a2 2 0 012-2h2a2 2 0 012 2" />
      </svg>
      <h3 class="text-xl font-medium text-gray-900 mb-2">Không tìm thấy đơn hàng</h3>
      <p class="text-gray-500 mb-6">Bạn chưa đặt đơn hàng nào.</p>
      <router-link 
        to="/menu" 
        class="inline-block px-6 py-3 bg-primary text-white rounded-md hover:bg-primary-dark transition-colors"
      >
        Xem thực đơn
      </router-link>
    </div>

    <!-- Invoices List -->
    <div v-else class="bg-white rounded-lg shadow-md overflow-hidden">
      <div class="overflow-x-auto">
        <table class="min-w-full divide-y divide-gray-200">
          <thead class="bg-gray-50">
            <tr>
              <th scope="col" class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                Mã hóa đơn
              </th>
              <th scope="col" class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                Ngày đặt
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
            <tr v-for="invoice in invoices" :key="invoice.invoiceId" class="hover:bg-gray-50">
              <td class="px-6 py-4 whitespace-nowrap text-sm font-medium text-primary">
                {{ invoice.invoiceNumber }}
              </td>
              <td class="px-6 py-4 whitespace-nowrap text-sm text-gray-500">
                {{ formatDate(invoice.invoiceDate) }}
              </td>
              <td class="px-6 py-4 whitespace-nowrap text-sm font-medium text-gray-900">
                {{ formatPrice(invoice.totalAmount) }}
              </td>
              <td class="px-6 py-4 whitespace-nowrap">
                <span 
                  class="px-2 inline-flex text-xs leading-5 font-semibold rounded-full"
                  :class="getStatusClass(invoice.order.status)"
                >
                  {{ formatStatus(invoice.order.status) }}
                </span>
              </td>
              <td class="px-6 py-4 whitespace-nowrap text-right text-sm font-medium">
                <router-link 
                  :to="{ name: 'invoice-detail', params: { id: invoice.invoiceId } }"
                  class="text-primary hover:text-primary-dark"
                >
                  Xem chi tiết
                </router-link>
              </td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue';
import api from '../services/api';

const invoices = ref([]);
const loading = ref(true);
const error = ref(null);
const startDate = ref('');
const endDate = ref('');

const fetchInvoices = async () => {
  loading.value = true;
  error.value = null;
  
  try {
    const params = {};
    
    if (startDate.value) {
      params.startDate = new Date(startDate.value).toISOString();
    }
    
    if (endDate.value) {
      // Set end date to end of day
      const end = new Date(endDate.value);
      end.setHours(23, 59, 59, 999);
      params.endDate = end.toISOString();
    }
    
    const response = await api.get('/orders/invoices', { params });
    invoices.value = response.data;
  } catch (err) {
    console.error('Error fetching invoices:', err);
    error.value = 'Failed to load your order history. Please try again later.';
  } finally {
    loading.value = false;
  }
};

const clearFilters = () => {
  startDate.value = '';
  endDate.value = '';
  fetchInvoices();
};

const formatDate = (dateString) => {
  return new Date(dateString).toLocaleDateString('vi-VN', {
    year: 'numeric',
    month: 'long',
    day: 'numeric',
    hour: '2-digit',
    minute: '2-digit'
  });
};

const formatPrice = (price) => {
  return new Intl.NumberFormat('vi-VN', { style: 'currency', currency: 'VND' }).format(price * 1000)
};

const formatStatus = (status) => {
  // Convert CamelCase to Spaced Title Case
  return status.replace(/([A-Z])/g, ' $1').trim();
};

const getStatusClass = (status) => {
  switch (status) {
    case 'Delivered':
      return 'bg-green-100 text-green-800';
    case 'BeingDelivered':
      return 'bg-blue-100 text-blue-800';
    case 'NotDelivered':
      return 'bg-yellow-100 text-yellow-800';
    default:
      return 'bg-gray-100 text-gray-800';
  }
};

onMounted(() => {
  fetchInvoices();
});
</script>
