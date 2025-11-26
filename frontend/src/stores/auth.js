import { defineStore } from 'pinia'
import AuthService from '../services/authService'

export const useAuthStore = defineStore('auth', {
  state: () => ({
    user: null,
    isAuthenticated: false
  }),
  actions: {
    async login(email, password) {
      try {
        const data = await AuthService.login(email, password)
        this.user = data
        this.isAuthenticated = true
        return Promise.resolve(data)
      } catch (error) {
        this.isAuthenticated = false
        return Promise.reject(error)
      }
    },
    async register(userData) {
      try {
        const data = await AuthService.register(userData)
        return Promise.resolve(data)
      } catch (error) {
        return Promise.reject(error)
      }
    },
    async googleLogin(token) {
        try {
            const data = await AuthService.googleLogin(token)
            this.user = data
            this.isAuthenticated = true
            return Promise.resolve(data)
        } catch (error) {
            this.isAuthenticated = false
            return Promise.reject(error)
        }
    },
    logout() {
      AuthService.logout()
      this.user = null
      this.isAuthenticated = false
    },
    checkAuth() {
        const user = AuthService.getCurrentUser()
        if (user && user.token) {
            this.user = user
            this.isAuthenticated = true
        } else {
            this.user = null
            this.isAuthenticated = false
        }
    }
  }
})
