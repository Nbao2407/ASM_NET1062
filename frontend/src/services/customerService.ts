import apiClient from './api';

export interface ProfileResponse {
  userId: number;
  email: string;
  fullName: string;
  phoneNumber: string;
  address: string;
  dateOfBirth: string;
}

export interface UpdateProfileRequest {
  email?: string;
  fullName?: string;
  phoneNumber?: string;
  address?: string;
  dateOfBirth?: string;
}

export interface ChangePasswordRequest {
  currentPassword: string;
  newPassword: string;
}

export const customerService = {
  getProfile: async (): Promise<ProfileResponse> => {
    const response = await apiClient.get('/customers/profile');
    return response.data;
  },

  updateProfile: async (data: UpdateProfileRequest): Promise<ProfileResponse> => {
    const response = await apiClient.put('/customers/profile', data);
    return response.data;
  },

  changePassword: async (data: ChangePasswordRequest): Promise<void> => {
    await apiClient.put('/customers/password', data);
  },
};
