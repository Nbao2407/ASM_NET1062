<template>
  <div class="min-h-screen bg-gray-50 py-12 px-4 sm:px-6 lg:px-8">
    <div class="max-w-3xl mx-auto">
      <div class="bg-white shadow rounded-lg">
        <!-- Header -->
        <div class="px-4 py-5 sm:px-6 border-b border-gray-200">
          <h3 class="text-lg leading-6 font-medium text-gray-900">
            Profile Management
          </h3>
          <p class="mt-1 max-w-2xl text-sm text-gray-500">
            Manage your personal information and account settings
          </p>
        </div>

        <!-- Loading State -->
        <div v-if="loadingProfile" class="px-4 py-5 sm:p-6">
          <div class="flex justify-center">
            <div class="animate-spin rounded-full h-12 w-12 border-b-2 border-blue-600"></div>
          </div>
        </div>

        <!-- Profile Form -->
        <div v-else class="px-4 py-5 sm:p-6">
          <form @submit.prevent="handleUpdateProfile" class="space-y-6">
            <div class="grid grid-cols-1 gap-6 sm:grid-cols-2">
              <!-- Full Name -->
              <div class="sm:col-span-2">
                <label for="fullName" class="block text-sm font-medium text-gray-700">
                  Full Name
                </label>
                <input
                  id="fullName"
                  v-model="profileData.fullName"
                  type="text"
                  class="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-blue-500 focus:border-blue-500 sm:text-sm"
                  :class="{ 'border-red-500': profileErrors.fullName }"
                />
                <p v-if="profileErrors.fullName" class="mt-1 text-sm text-red-600">
                  {{ profileErrors.fullName }}
                </p>
              </div>

              <!-- Email -->
              <div class="sm:col-span-2">
                <label for="email" class="block text-sm font-medium text-gray-700">
                  Email Address
                </label>
                <input
                  id="email"
                  v-model="profileData.email"
                  type="email"
                  class="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-blue-500 focus:border-blue-500 sm:text-sm"
                  :class="{ 'border-red-500': profileErrors.email }"
                />
                <p v-if="profileErrors.email" class="mt-1 text-sm text-red-600">
                  {{ profileErrors.email }}
                </p>
              </div>

              <!-- Phone Number -->
              <div>
                <label for="phoneNumber" class="block text-sm font-medium text-gray-700">
                  Phone Number
                </label>
                <input
                  id="phoneNumber"
                  v-model="profileData.phoneNumber"
                  type="tel"
                  class="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-blue-500 focus:border-blue-500 sm:text-sm"
                  :class="{ 'border-red-500': profileErrors.phoneNumber }"
                />
                <p v-if="profileErrors.phoneNumber" class="mt-1 text-sm text-red-600">
                  {{ profileErrors.phoneNumber }}
                </p>
              </div>

              <!-- Date of Birth -->
              <div>
                <label for="dateOfBirth" class="block text-sm font-medium text-gray-700">
                  Date of Birth
                </label>
                <input
                  id="dateOfBirth"
                  v-model="profileData.dateOfBirth"
                  type="date"
                  class="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-blue-500 focus:border-blue-500 sm:text-sm"
                  :class="{ 'border-red-500': profileErrors.dateOfBirth }"
                />
                <p v-if="profileErrors.dateOfBirth" class="mt-1 text-sm text-red-600">
                  {{ profileErrors.dateOfBirth }}
                </p>
              </div>

              <!-- Address -->
              <div class="sm:col-span-2">
                <label for="address" class="block text-sm font-medium text-gray-700">
                  Address
                </label>
                <textarea
                  id="address"
                  v-model="profileData.address"
                  rows="3"
                  class="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-blue-500 focus:border-blue-500 sm:text-sm"
                  :class="{ 'border-red-500': profileErrors.address }"
                />
                <p v-if="profileErrors.address" class="mt-1 text-sm text-red-600">
                  {{ profileErrors.address }}
                </p>
              </div>
            </div>

            <!-- Profile Error Message -->
            <div v-if="profileErrorMessage" class="rounded-md bg-red-50 p-4">
              <div class="flex">
                <div class="ml-3">
                  <h3 class="text-sm font-medium text-red-800">{{ profileErrorMessage }}</h3>
                </div>
              </div>
            </div>

            <!-- Profile Success Message -->
            <div v-if="profileSuccessMessage" class="rounded-md bg-green-50 p-4">
              <div class="flex">
                <div class="ml-3">
                  <h3 class="text-sm font-medium text-green-800">{{ profileSuccessMessage }}</h3>
                </div>
              </div>
            </div>

            <div class="flex justify-end">
              <button
                type="submit"
                :disabled="updatingProfile"
                class="inline-flex justify-center py-2 px-4 border border-transparent shadow-sm text-sm font-medium rounded-md text-white bg-blue-600 hover:bg-blue-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-blue-500 disabled:opacity-50 disabled:cursor-not-allowed"
              >
                <span v-if="updatingProfile">Updating...</span>
                <span v-else>Update Profile</span>
              </button>
            </div>
          </form>
        </div>
      </div>

      <!-- Change Password Section -->
      <div class="mt-6 bg-white shadow rounded-lg">
        <div class="px-4 py-5 sm:px-6 border-b border-gray-200">
          <h3 class="text-lg leading-6 font-medium text-gray-900">
            Change Password
          </h3>
          <p class="mt-1 max-w-2xl text-sm text-gray-500">
            Update your password to keep your account secure
          </p>
        </div>

        <div class="px-4 py-5 sm:p-6">
          <form @submit.prevent="handleChangePassword" class="space-y-6">
            <!-- Current Password -->
            <div>
              <label for="currentPassword" class="block text-sm font-medium text-gray-700">
                Current Password
              </label>
              <input
                id="currentPassword"
                v-model="passwordData.currentPassword"
                type="password"
                class="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-blue-500 focus:border-blue-500 sm:text-sm"
                :class="{ 'border-red-500': passwordErrors.currentPassword }"
              />
              <p v-if="passwordErrors.currentPassword" class="mt-1 text-sm text-red-600">
                {{ passwordErrors.currentPassword }}
              </p>
            </div>

            <!-- New Password -->
            <div>
              <label for="newPassword" class="block text-sm font-medium text-gray-700">
                New Password
              </label>
              <input
                id="newPassword"
                v-model="passwordData.newPassword"
                type="password"
                class="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-blue-500 focus:border-blue-500 sm:text-sm"
                :class="{ 'border-red-500': passwordErrors.newPassword }"
              />
              <p v-if="passwordErrors.newPassword" class="mt-1 text-sm text-red-600">
                {{ passwordErrors.newPassword }}
              </p>
            </div>

            <!-- Confirm New Password -->
            <div>
              <label for="confirmPassword" class="block text-sm font-medium text-gray-700">
                Confirm New Password
              </label>
              <input
                id="confirmPassword"
                v-model="passwordData.confirmPassword"
                type="password"
                class="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-blue-500 focus:border-blue-500 sm:text-sm"
                :class="{ 'border-red-500': passwordErrors.confirmPassword }"
              />
              <p v-if="passwordErrors.confirmPassword" class="mt-1 text-sm text-red-600">
                {{ passwordErrors.confirmPassword }}
              </p>
            </div>

            <!-- Password Error Message -->
            <div v-if="passwordErrorMessage" class="rounded-md bg-red-50 p-4">
              <div class="flex">
                <div class="ml-3">
                  <h3 class="text-sm font-medium text-red-800">{{ passwordErrorMessage }}</h3>
                </div>
              </div>
            </div>

            <!-- Password Success Message -->
            <div v-if="passwordSuccessMessage" class="rounded-md bg-green-50 p-4">
              <div class="flex">
                <div class="ml-3">
                  <h3 class="text-sm font-medium text-green-800">{{ passwordSuccessMessage }}</h3>
                </div>
              </div>
            </div>

            <div class="flex justify-end">
              <button
                type="submit"
                :disabled="changingPassword"
                class="inline-flex justify-center py-2 px-4 border border-transparent shadow-sm text-sm font-medium rounded-md text-white bg-blue-600 hover:bg-blue-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-blue-500 disabled:opacity-50 disabled:cursor-not-allowed"
              >
                <span v-if="changingPassword">Changing...</span>
                <span v-else>Change Password</span>
              </button>
            </div>
          </form>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, reactive, onMounted } from 'vue';
import { customerService, type UpdateProfileRequest } from '@/services/customerService';
import { useAuthStore } from '@/stores/auth';

const authStore = useAuthStore();

// Profile data
const profileData = reactive<{
  fullName: string;
  email: string;
  phoneNumber: string;
  address: string;
  dateOfBirth: string;
}>({
  fullName: '',
  email: '',
  phoneNumber: '',
  address: '',
  dateOfBirth: '',
});

const profileErrors = reactive({
  fullName: '',
  email: '',
  phoneNumber: '',
  address: '',
  dateOfBirth: '',
});

const loadingProfile = ref(false);
const updatingProfile = ref(false);
const profileErrorMessage = ref('');
const profileSuccessMessage = ref('');

// Password data
const passwordData = reactive({
  currentPassword: '',
  newPassword: '',
  confirmPassword: '',
});

const passwordErrors = reactive({
  currentPassword: '',
  newPassword: '',
  confirmPassword: '',
});

const changingPassword = ref(false);
const passwordErrorMessage = ref('');
const passwordSuccessMessage = ref('');

// Load profile data on mount
onMounted(async () => {
  await loadProfile();
});

const loadProfile = async () => {
  loadingProfile.value = true;
  try {
    const profile = await customerService.getProfile();
    profileData.fullName = profile.fullName;
    profileData.email = profile.email;
    profileData.phoneNumber = profile.phoneNumber;
    profileData.address = profile.address;
    // Format date for input - handle both ISO string and date-only formats
    const dob = profile.dateOfBirth;
    profileData.dateOfBirth = dob.includes('T') ? (dob.split('T')[0] || dob) : dob;
  } catch {
    profileErrorMessage.value = 'Failed to load profile. Please refresh the page.';
  } finally {
    loadingProfile.value = false;
  }
};

const validateProfileForm = (): boolean => {
  let isValid = true;
  
  // Reset errors
  Object.keys(profileErrors).forEach(key => {
    profileErrors[key as keyof typeof profileErrors] = '';
  });
  profileErrorMessage.value = '';
  profileSuccessMessage.value = '';

  // Full Name validation
  if (!profileData.fullName.trim()) {
    profileErrors.fullName = 'Full name is required';
    isValid = false;
  } else if (profileData.fullName.trim().length < 2) {
    profileErrors.fullName = 'Full name must be at least 2 characters';
    isValid = false;
  }

  // Email validation
  if (!profileData.email.trim()) {
    profileErrors.email = 'Email is required';
    isValid = false;
  } else if (!/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(profileData.email)) {
    profileErrors.email = 'Please enter a valid email address';
    isValid = false;
  }

  // Phone Number validation
  if (!profileData.phoneNumber.trim()) {
    profileErrors.phoneNumber = 'Phone number is required';
    isValid = false;
  } else if (!/^[\d\s\-\+\(\)]+$/.test(profileData.phoneNumber)) {
    profileErrors.phoneNumber = 'Please enter a valid phone number';
    isValid = false;
  }

  // Address validation
  if (!profileData.address.trim()) {
    profileErrors.address = 'Address is required';
    isValid = false;
  } else if (profileData.address.trim().length < 5) {
    profileErrors.address = 'Please enter a complete address';
    isValid = false;
  }

  // Date of Birth validation
  if (!profileData.dateOfBirth) {
    profileErrors.dateOfBirth = 'Date of birth is required';
    isValid = false;
  }

  return isValid;
};

const handleUpdateProfile = async () => {
  if (!validateProfileForm()) {
    return;
  }

  updatingProfile.value = true;
  profileErrorMessage.value = '';
  profileSuccessMessage.value = '';

  try {
    const updateData: UpdateProfileRequest = {
      fullName: profileData.fullName,
      email: profileData.email,
      phoneNumber: profileData.phoneNumber,
      address: profileData.address,
      dateOfBirth: profileData.dateOfBirth,
    };

    await customerService.updateProfile(updateData);
    
    // Update user in auth store if email or name changed
    if (authStore.user) {
      authStore.setUser({
        ...authStore.user,
        fullName: profileData.fullName,
        email: profileData.email,
      });
    }

    profileSuccessMessage.value = 'Profile updated successfully!';
    
    // Clear success message after 3 seconds
    setTimeout(() => {
      profileSuccessMessage.value = '';
    }, 3000);
  } catch (error: unknown) {
    const err = error as { response?: { data?: { message?: string }; status?: number } };
    if (err.response?.status === 409) {
      profileErrorMessage.value = 'This email is already in use by another account.';
    } else if (err.response?.data?.message) {
      profileErrorMessage.value = err.response.data.message;
    } else {
      profileErrorMessage.value = 'Failed to update profile. Please try again.';
    }
  } finally {
    updatingProfile.value = false;
  }
};

const validatePasswordForm = (): boolean => {
  let isValid = true;
  
  // Reset errors
  Object.keys(passwordErrors).forEach(key => {
    passwordErrors[key as keyof typeof passwordErrors] = '';
  });
  passwordErrorMessage.value = '';
  passwordSuccessMessage.value = '';

  // Current Password validation
  if (!passwordData.currentPassword) {
    passwordErrors.currentPassword = 'Current password is required';
    isValid = false;
  }

  // New Password validation
  if (!passwordData.newPassword) {
    passwordErrors.newPassword = 'New password is required';
    isValid = false;
  } else if (passwordData.newPassword.length < 6) {
    passwordErrors.newPassword = 'Password must be at least 6 characters';
    isValid = false;
  }

  // Confirm Password validation
  if (!passwordData.confirmPassword) {
    passwordErrors.confirmPassword = 'Please confirm your new password';
    isValid = false;
  } else if (passwordData.newPassword !== passwordData.confirmPassword) {
    passwordErrors.confirmPassword = 'Passwords do not match';
    isValid = false;
  }

  return isValid;
};

const handleChangePassword = async () => {
  if (!validatePasswordForm()) {
    return;
  }

  changingPassword.value = true;
  passwordErrorMessage.value = '';
  passwordSuccessMessage.value = '';

  try {
    await customerService.changePassword({
      currentPassword: passwordData.currentPassword,
      newPassword: passwordData.newPassword,
    });

    passwordSuccessMessage.value = 'Password changed successfully!';
    
    // Clear form
    passwordData.currentPassword = '';
    passwordData.newPassword = '';
    passwordData.confirmPassword = '';
    
    // Clear success message after 3 seconds
    setTimeout(() => {
      passwordSuccessMessage.value = '';
    }, 3000);
  } catch (error: unknown) {
    const err = error as { response?: { data?: { message?: string }; status?: number } };
    if (err.response?.status === 401) {
      passwordErrorMessage.value = 'Current password is incorrect.';
    } else if (err.response?.data?.message) {
      passwordErrorMessage.value = err.response.data.message;
    } else {
      passwordErrorMessage.value = 'Failed to change password. Please try again.';
    }
  } finally {
    changingPassword.value = false;
  }
};
</script>
