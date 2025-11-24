import apiClient from './api';

export interface FoodItem {
  foodItemId: number;
  name: string;
  description: string;
  price: number;
  category: string;
  theme?: string;
  imageUrl?: string;
  ingredients?: string;
  nutritionalInfo?: string;
  allergenWarnings?: string;
  isActive: boolean;
}

export interface SearchParams {
  name?: string;
  category?: string;
  theme?: string;
  minPrice?: number;
  maxPrice?: number;
}

export const foodItemService = {
  getAll: async (category?: string): Promise<FoodItem[]> => {
    const params = category ? { category } : {};
    const response = await apiClient.get('/items', { params });
    return response.data;
  },

  getById: async (id: number): Promise<FoodItem> => {
    const response = await apiClient.get(`/items/${id}`);
    return response.data;
  },

  search: async (params: SearchParams): Promise<FoodItem[]> => {
    const response = await apiClient.get('/items/search', { params });
    return response.data;
  },

  getCategories: async (): Promise<string[]> => {
    const response = await apiClient.get('/items/categories');
    return response.data;
  },

  // Admin only
  create: async (data: Partial<FoodItem>): Promise<FoodItem> => {
    const response = await apiClient.post('/items', data);
    return response.data;
  },

  update: async (id: number, data: Partial<FoodItem>): Promise<FoodItem> => {
    const response = await apiClient.put(`/items/${id}`, data);
    return response.data;
  },

  delete: async (id: number): Promise<void> => {
    await apiClient.delete(`/items/${id}`);
  },
};
