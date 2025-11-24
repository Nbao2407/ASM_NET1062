/**
 * Centralized export for all API services
 */

export { authService } from './authService';
export type { RegisterRequest, LoginRequest, GoogleAuthRequest, AuthResponse } from './authService';

export { customerService } from './customerService';
export type { ProfileResponse, UpdateProfileRequest, ChangePasswordRequest } from './customerService';

export { foodItemService } from './foodItemService';
export type { FoodItem, SearchParams } from './foodItemService';

export { comboService } from './comboService';
export type { Combo, ComboItem } from './comboService';

export { orderService } from './orderService';
export type { Order, OrderItem, Invoice, CreateOrderRequest, UpdateOrderStatusRequest } from './orderService';

export { adminService } from './adminService';
export type { User, CreateUserRequest, UpdateUserRequest } from './adminService';

export { default as apiClient } from './api';
