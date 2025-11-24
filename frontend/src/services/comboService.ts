import apiClient from './api';

export interface ComboItem {
  foodItemId: number;
  name: string;
  quantity: number;
}

export interface Combo {
  comboId: number;
  name: string;
  description: string;
  price: number;
  imageUrl?: string;
  items: ComboItem[];
  isActive: boolean;
}

export const comboService = {
  getAll: async (): Promise<Combo[]> => {
    const response = await apiClient.get('/combos');
    return response.data;
  },

  getById: async (id: number): Promise<Combo> => {
    const response = await apiClient.get(`/combos/${id}`);
    return response.data;
  },

  // Admin only
  create: async (data: Partial<Combo>): Promise<Combo> => {
    const response = await apiClient.post('/combos', data);
    return response.data;
  },

  update: async (id: number, data: Partial<Combo>): Promise<Combo> => {
    const response = await apiClient.put(`/combos/${id}`, data);
    return response.data;
  },

  delete: async (id: number): Promise<void> => {
    await apiClient.delete(`/combos/${id}`);
  },
};
