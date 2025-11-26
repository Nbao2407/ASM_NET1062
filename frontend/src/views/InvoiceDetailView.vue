<template>
  <div class="container mx-auto px-4 py-8">
    <div class="mb-6">
      <router-link to="/orders" class="text-primary hover:text-primary-dark flex items-center gap-2">
        <svg xmlns="http://www.w3.org/2000/svg" class="h-5 w-5" viewBox="0 0 20 20" fill="currentColor">
          <path fill-rule="evenodd" d="M9.707 16.707a1 1 0 01-1.414 0l-6-6a1 1 0 010-1.414l6-6a1 1 0 011.414 1.414L5.414 9H17a1 1 0 110 2H5.414l4.293 4.293a1 1 0 010 1.414z" clip-rule="evenodd" />
        </svg>
        Quay lại đơn hàng
      </router-link>
    </div>

    <!-- Loading State -->
    <div v-if="loading" class="text-center py-12">
      <div class="animate-spin rounded-full h-12 w-12 border-b-2 border-primary mx-auto"></div>
      <p class="mt-4 text-gray-600">Đang tải chi tiết hóa đơn...</p>
    </div>

    <!-- Error State -->
    <div v-else-if="error" class="bg-red-100 border border-red-400 text-red-700 px-4 py-3 rounded mb-6">
      {{ error }}
    </div>

    <div v-else-if="invoice" class="bg-white rounded-lg shadow-lg overflow-hidden">
      <!-- Header -->
      <div class="bg-gray-50 px-6 py-4 border-b border-gray-200 flex justify-between items-center flex-wrap gap-4">
        <div>
          <h1 class="text-2xl font-bold text-gray-800">Hóa đơn {{ invoice.invoiceNumber }}</h1>
          <p class="text-sm text-gray-500">Đặt ngày {{ formatDate(invoice.invoiceDate) }}</p>
        </div>
        <div class="flex flex-col items-end">
          <span 
            class="px-3 py-1 rounded-full text-sm font-semibold mb-1"
            :class="getStatusClass(invoice.order.status)"
          >
            {{ formatStatus(invoice.order.status) }}
          </span>
          <p class="text-sm text-gray-500">Đơn hàng #{{ invoice.order.orderNumber }}</p>
        </div>
      </div>

      <div class="p-6">
        <!-- Order Info Grid -->
        <div class="grid grid-cols-1 md:grid-cols-2 gap-8 mb-8">
          <div>
            <h3 class="text-lg font-semibold text-gray-800 mb-3">Thông tin giao hàng</h3>
            <div class="bg-gray-50 p-4 rounded-md">
              <p class="text-gray-700"><span class="font-medium">Địa chỉ:</span></p>
              <p class="text-gray-600 ml-2">{{ invoice.order.deliveryAddress }}</p>
            </div>
          </div>
          <div>
            <h3 class="text-lg font-semibold text-gray-800 mb-3">Thông tin thanh toán</h3>
            <div class="bg-gray-50 p-4 rounded-md">
              <p class="text-gray-700 mb-1"><span class="font-medium">Phương thức:</span> {{ invoice.order.paymentMethod }}</p>
              <p class="text-gray-700"><span class="font-medium">Trạng thái:</span> {{ invoice.order.paymentStatus }}</p>
            </div>
          </div>
        </div>

        <!-- Order Items -->
        <h3 class="text-lg font-semibold text-gray-800 mb-4">Danh sách món ăn</h3>
        <div class="border rounded-lg overflow-hidden mb-8">
          <table class="min-w-full divide-y divide-gray-200">
            <thead class="bg-gray-50">
              <tr>
                <th scope="col" class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                  Món ăn
                </th>
                <th scope="col" class="px-6 py-3 text-center text-xs font-medium text-gray-500 uppercase tracking-wider">
                  Số lượng
                </th>
                <th scope="col" class="px-6 py-3 text-right text-xs font-medium text-gray-500 uppercase tracking-wider">
                  Đơn giá
                </th>
                <th scope="col" class="px-6 py-3 text-right text-xs font-medium text-gray-500 uppercase tracking-wider">
                  Thành tiền
                </th>
              </tr>
            </thead>
            <tbody class="bg-white divide-y divide-gray-200">
              <tr v-for="item in invoice.order.items" :key="item.orderItemId">
                <td class="px-6 py-4">
                  <div class="text-sm font-medium text-gray-900">
                    {{ item.foodItemName || item.comboName }}
                  </div>
                  <div class="text-xs text-gray-500">
                    {{ item.foodItemId ? 'Món lẻ' : 'Combo' }}
                  </div>
                </td>
                <td class="px-6 py-4 text-center text-sm text-gray-500">
                  {{ item.quantity }}
                </td>
                <td class="px-6 py-4 text-right text-sm text-gray-500">
                  {{ formatPrice(item.unitPrice) }}
                </td>
                <td class="px-6 py-4 text-right text-sm font-medium text-gray-900">
                  {{ formatPrice(item.subtotal) }}
                </td>
              </tr>
            </tbody>
          </table>
        </div>

        <!-- Summary -->
        <div class="flex justify-end">
          <div class="w-full md:w-1/3 bg-gray-50 p-6 rounded-lg">
            <div class="flex justify-between mb-2">
              <span class="text-gray-600">Tạm tính</span>
              <span class="font-medium">{{ formatPrice(invoice.totalAmount) }}</span>
            </div>
            <div class="flex justify-between mb-2">
              <span class="text-gray-600">Thuế</span>
              <span class="font-medium">{{ formatPrice(invoice.taxAmount) }}</span>
            </div>
            <div class="flex justify-between mb-4">
              <span class="text-gray-600">Giảm giá</span>
              <span class="font-medium">-{{ formatPrice(invoice.discountAmount) }}</span>
            </div>
            <div class="border-t border-gray-200 pt-4 flex justify-between items-center">
              <span class="text-lg font-bold text-gray-800">Tổng cộng</span>
              <span class="text-xl font-bold text-primary">{{ formatPrice(invoice.totalAmount + invoice.taxAmount - invoice.discountAmount) }}</span>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue';
import { useRoute } from 'vue-router';
import api from '../services/api';

const route = useRoute();
const invoice = ref(null);
const loading = ref(true);
const error = ref(null);

const fetchInvoice = async () => {
  loading.value = true;
  error.value = null;
  
  try {
    const response = await api.get(`/orders/invoices/${route.params.id}`);
    invoice.value = response.data;
  } catch (err) {
    console.error('Error fetching invoice:', err);
    error.value = 'Failed to load invoice details. Please try again later.';
  } finally {
    loading.value = false;
  }
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
  fetchInvoice();
});
</script>
