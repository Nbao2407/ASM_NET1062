import { defineStore } from 'pinia';
import { ref } from 'vue';

interface User {
  userId: number;
  email: string;
  fullName: string;
  role: string;
}

export const useAuthStore = defineStore('auth', () => {
  const token = ref<string | null>(localStorage.getItem('token'));
  const user = ref<User | null>(null);

  function setToken(newToken: string) {
    token.value = newToken;
    localStorage.setItem('token', newToken);
  }

  function clearToken() {
    token.value = null;
    user.value = null;
    localStorage.removeItem('token');
  }

  function setUser(userData: User) {
    user.value = userData;
  }

  const isAuthenticated = () => !!token.value;
  const isAdmin = () => user.value?.role === 'Admin';

  return {
    token,
    user,
    setToken,
    clearToken,
    setUser,
    isAuthenticated,
    isAdmin,
  };
});
