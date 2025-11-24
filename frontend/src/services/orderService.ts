import apiClient from './api';

export interface OrderItem {
  foodItemId?: number;
  comboId?: number;
  quantity: number;
}

export interface CreateOrderRequest {
  items: OrderItem[];
  deliveryAddress: string;
  paymentMethod: string;
}

export interface Order {
  orderId: number;
  orderNumber: string;
  userId: number;
  totalAmount: number;
  status: 'NotDelivered' | 'BeingDelivered' | 'Delivered';
  paymentMethod: string;
  paymentStatus: string;
  deliveryAddress: string;
  orderDate: string;
  statusUpdatedAt?: string;
  items: OrderItem[];
}

export interface Invoice {
  invoiceId: number;
  orderId: number;
  invoiceNumber: string;
  invoiceDate: string;
  totalAmount: number;
  taxAmount: number;
  discountAmount: number;
  order: Order;
}

export interface UpdateOrderStatusRequest {
  status: 'NotDelivered' | 'BeingDelivered' | 'Delivered';
}

export const orderService = {
  create: async (data: CreateOrderRequest): Promise<Order> => {
    const response = await apiClient.post('/orders', data);
    return response.data;
  },

  getAll: async (status?: string): Promise<Order[]> => {
    const params = status ? { status } : {};
    const response = await apiClient.get('/orders', { params });
    return response.data;
  },

  getById: async (id: number): Promise<Order> => {
    const response = await apiClient.get(`/orders/${id}`);
    return response.data;
  },

  getInvoices: async (startDate?: string, endDate?: string): Promise<Invoice[]> => {
    const params: Record<string, string> = {};
    if (startDate) params.startDate = startDate;
    if (endDate) params.endDate = endDate;
    const response = await apiClient.get('/orders/invoices', { params });
    return response.data;
  },

  getInvoiceById: async (id: number): Promise<Invoice> => {
    const response = await apiClient.get(`/orders/invoices/${id}`);
    return response.data;
  },

  // Admin only
  updateStatus: async (id: number, data: UpdateOrderStatusRequest): Promise<Order> => {
    const response = await apiClient.put(`/orders/${id}/status`, data);
    return response.data;
  },
};
