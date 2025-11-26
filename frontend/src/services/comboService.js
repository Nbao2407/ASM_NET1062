import api from './api'

const API_URL = ''

// TypeScript interfaces removed for .js file

class ComboService {
  async getAll() {
    try {
      const response = await api.get(`${API_URL}/combos`)
      return response.data
    } catch (error) {
      console.error('Error fetching combos:', error)
      // Return mock data for development
      return this.getMockData()
    }
  }

  async getById(id) {
    try {
      const response = await api.get(`${API_URL}/combos/${id}`)
      return response.data
    } catch (error) {
      console.error('Error fetching combo:', error)
      throw error
    }
  }

  getMockData() {
    return [
      {
        comboId: 1,
        name: 'Family Feast',
        description: '2 Large Burgers, Large Fries, 4 Drinks - Perfect for the whole family!',
        price: 24.99,
        imageUrl: 'https://images.unsplash.com/photo-1619894991152-52cc28f0a475?w=500',
        items: [
          { name: 'Classic Burger', quantity: 2 },
          { name: 'Large Fries', quantity: 1 },
          { name: 'Soft Drink', quantity: 4 }
        ],
        isAvailable: true
      },
      {
        comboId: 2,
        name: 'Chicken Lover',
        description: '12 Chicken Wings, Regular Fries, 2 Dipping Sauces',
        price: 18.99,
        imageUrl: 'https://images.unsplash.com/photo-1626082927389-6cd097cdc6ec?w=500',
        items: [
          { name: 'Chicken Wings', quantity: 12 },
          { name: 'Regular Fries', quantity: 1 },
          { name: 'Dipping Sauce', quantity: 2 }
        ],
        isAvailable: true
      },
      {
        comboId: 3,
        name: 'Pizza Party',
        description: '2 Large Pizzas (any flavor), Garlic Bread, 1.5L Soda',
        price: 29.99,
        imageUrl: 'https://images.unsplash.com/photo-1513104890138-7c749659a591?w=500',
        items: [
          { name: 'Large Pizza', quantity: 2 },
          { name: 'Garlic Bread', quantity: 1 },
          { name: 'Soda 1.5L', quantity: 1 }
        ],
        isAvailable: true
      }
    ]
  }
}

export const comboService = new ComboService()
