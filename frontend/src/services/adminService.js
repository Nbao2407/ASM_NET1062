import api from './api'

const AdminService = {
  // Users
  getUsers() {
    return api.get('/admin/users')
  },
  getUser(id) {
    return api.get(`/admin/users/${id}`)
  },
  createUser(userData) {
    return api.post('/admin/users', userData)
  },
  updateUser(id, userData) {
    return api.put(`/admin/users/${id}`, userData)
  },
  deleteUser(id) {
    return api.delete(`/admin/users/${id}`)
  },

  // Food Items
  getFoodItems(params) {
    return api.get('/items', { params })
  },
  getFoodItem(id) {
    return api.get(`/items/${id}`)
  },
  createFoodItem(itemData) {
    return api.post('/items', itemData)
  },
  updateFoodItem(id, itemData) {
    return api.put(`/items/${id}`, itemData)
  },
  deleteFoodItem(id) {
    return api.delete(`/items/${id}`)
  },
  getCategories() {
    return api.get('/items/categories')
  },

  // Combos
  getCombos() {
    return api.get('/combos')
  },
  getCombo(id) {
    return api.get(`/combos/${id}`)
  },
  createCombo(comboData) {
    return api.post('/combos', comboData)
  },
  updateCombo(id, comboData) {
    return api.put(`/combos/${id}`, comboData)
  },
  deleteCombo(id) {
    return api.delete(`/combos/${id}`)
  },

  // Orders
  getOrders(params) {
    return api.get('/orders', { params })
  },
  getOrder(id) {
    return api.get(`/orders/${id}`)
  },
  updateOrderStatus(id, status) {
    return api.put(`/orders/${id}/status`, { status })
  },
  getInvoices(params) {
    return api.get('/orders/invoices', { params })
  }
}

export default AdminService
