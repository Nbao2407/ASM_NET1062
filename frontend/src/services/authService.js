import api from './api'

const API_URL = '/auth'

class AuthService {
  async login(email, password) {
    console.log('AuthService: login', API_URL)
    try {
      const response = await api.post(`${API_URL}/login`, {
        email,
        password
      })
      if (response.data.token) {
        localStorage.setItem('user', JSON.stringify(response.data))
      }
      return response.data
    } catch (error) {
      console.error('Login error:', error)
      throw error
    }
  }

  async register(userData) {
    console.log('AuthService: register', API_URL, userData)
    try {
      const response = await api.post(`${API_URL}/register`, userData)
      return response.data
    } catch (error) {
      console.error('Registration error:', error)
      throw error
    }
  }

  async googleLogin(token) {
    try {
        const response = await api.post(`${API_URL}/google`, { token })
        if (response.data.token) {
            localStorage.setItem('user', JSON.stringify(response.data))
        }
        return response.data
    } catch (error) {
        console.error('Google login error:', error)
        throw error
    }
  }

  logout() {
    localStorage.removeItem('user')
  }

  getCurrentUser() {
    return JSON.parse(localStorage.getItem('user'))
  }
}

export default new AuthService()
