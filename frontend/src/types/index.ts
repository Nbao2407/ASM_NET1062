/**
 * Common types used across the application
 */

export type UserRole = 'Customer' | 'Admin';

export type OrderStatus = 'NotDelivered' | 'BeingDelivered' | 'Delivered';

export type PaymentMethod = 'CreditCard' | 'DebitCard' | 'Cash' | 'GooglePay' | 'ApplePay';

export type PaymentStatus = 'Pending' | 'Completed' | 'Failed' | 'Refunded';

export interface ApiError {
  message: string;
  errors?: Record<string, string[]>;
  statusCode?: number;
}

export interface PaginationParams {
  page?: number;
  pageSize?: number;
  sortBy?: string;
  sortOrder?: 'asc' | 'desc';
}

export interface PaginatedResponse<T> {
  data: T[];
  totalCount: number;
  page: number;
  pageSize: number;
  totalPages: number;
}
