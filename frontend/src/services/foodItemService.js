import api from './api'

const API_URL = ''

// TypeScript interfaces removed for .js file

class FoodItemService {
  async getAll() {
    try {
      const response = await api.get(`${API_URL}/items`)
      return response.data
    } catch (error) {
      console.error('Error fetching food items:', error)
      // Return mock data for development
      return this.getMockData()
    }
  }

  async getById(id) {
    try {
      const response = await api.get(`${API_URL}/items/${id}`)
      return response.data
    } catch (error) {
      console.error('Error fetching food item:', error)
      throw error
    }
  }

  async search(params) {
    try {
      const response = await api.get(`${API_URL}/items/search`, { params })
      return response.data
    } catch (error) {
      console.error('Error searching food items:', error)
      return this.getMockData()
    }
  }

  getMockData() {
    return [
      {
        foodItemId: 1,
        name: 'Classic Burger',
        description: 'Juicy beef patty with fresh lettuce, tomatoes, and our special sauce',
        price: 8.99,
        category: 'Burgers',
        imageUrl: 'https://images.unsplash.com/photo-1568901346375-23c9450c58cd?w=500',
        isAvailable: true
      },
      {
        foodItemId: 2,
        name: 'Chicken Wings',
        description: 'Crispy chicken wings with your choice of sauce',
        price: 10.99,
        category: 'Chicken',
        imageUrl: 'https://images.unsplash.com/photo-1527477396000-e27163b481c2?w=500',
        isAvailable: true
      },
      {
        foodItemId: 3,
        name: 'Pepperoni Pizza',
        description: 'Classic pepperoni pizza with mozzarella cheese',
        price: 12.99,
        category: 'Pizza',
        imageUrl: 'https://images.unsplash.com/photo-1628840042765-356cda07504e?w=500',
        isAvailable: true
      },
      {
        foodItemId: 4,
        name: 'French Fries',
        description: 'Golden crispy fries with sea salt',
        price: 3.99,
        category: 'Sides',
        imageUrl: 'https://images.unsplash.com/photo-1573080496219-bb080dd4f877?w=500',
        isAvailable: true
      },
      {
        foodItemId: 5,
        name: 'Caesar Salad',
        description: 'Fresh romaine lettuce with Caesar dressing and croutons',
        price: 7.99,
        category: 'Salads',
        imageUrl: 'https://images.unsplash.com/photo-1546793665-c74683f339c1?w=500',
        isAvailable: true
      },
      {
        foodItemId: 6,
        name: 'Chocolate Shake',
        description: 'Creamy chocolate milkshake topped with whipped cream',
        price: 4.99,
        category: 'Drinks',
        imageUrl: 'https://images.unsplash.com/photo-1572490122747-3968b75cc699?w=500',
        isAvailable: true
      }
    ]
  }
}

export const foodItemService = new FoodItemService()
