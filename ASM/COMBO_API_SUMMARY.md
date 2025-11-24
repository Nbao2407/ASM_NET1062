# Combo Management API Summary

## Overview
The Combo Management API provides endpoints for managing combo packages in the Fast Food Ordering System. Combos are bundled meal deals that include multiple food items at a discounted price.

## Endpoints

### 1. Get All Combos
**GET** `/api/combos`

Returns a list of all active combos with their included food items.

**Response:**
```json
[
  {
    "comboId": 1,
    "name": "Classic Burger Meal",
    "description": "Classic burger with fries and a drink",
    "price": 10.99,
    "imageUrl": "/images/burger-meal.jpg",
    "isActive": true,
    "createdAt": "2024-11-23T18:33:40Z",
    "updatedAt": "2024-11-23T18:33:40Z",
    "comboItems": [
      {
        "foodItemId": 1,
        "foodItemName": "Classic Burger",
        "foodItemPrice": 8.99,
        "quantity": 1
      },
      {
        "foodItemId": 12,
        "foodItemName": "Coca-Cola",
        "foodItemPrice": 2.49,
        "quantity": 1
      }
    ]
  }
]
```

### 2. Get Combo by ID
**GET** `/api/combos/{id}`

Returns a specific combo with all its details and included items.

**Response:** Same structure as individual combo in the list above.

### 3. Create Combo (Admin Only)
**POST** `/api/combos`

Creates a new combo package. Requires admin authentication.

**Request Body:**
```json
{
  "name": "Family Meal Deal",
  "description": "Perfect combo for the whole family",
  "price": 24.99,
  "imageUrl": "/images/family-meal.jpg",
  "comboItems": [
    {
      "foodItemId": 1,
      "quantity": 2
    },
    {
      "foodItemId": 2,
      "quantity": 1
    },
    {
      "foodItemId": 12,
      "quantity": 4
    }
  ]
}
```

**Validation Rules:**
- Name is required (max 255 characters)
- Price must be between 0.01 and 999999.99
- At least one combo item is required
- All food items must exist and be active
- **Combo price must be less than the sum of individual item prices**

**Response:** Returns the created combo with status 201 Created.

### 4. Update Combo (Admin Only)
**PUT** `/api/combos/{id}`

Updates an existing combo. Requires admin authentication.

**Request Body:**
```json
{
  "name": "Family Meal Deal Updated",
  "description": "Updated description for family combo",
  "price": 26.99,
  "imageUrl": "/images/family-meal-updated.jpg",
  "isActive": true,
  "comboItems": [
    {
      "foodItemId": 1,
      "quantity": 3
    },
    {
      "foodItemId": 2,
      "quantity": 2
    },
    {
      "foodItemId": 12,
      "quantity": 4
    }
  ]
}
```

**Validation Rules:** Same as Create, plus:
- IsActive flag can be set to true/false

**Response:** Returns the updated combo with status 200 OK.

### 5. Delete Combo (Admin Only)
**DELETE** `/api/combos/{id}`

Soft deletes a combo by setting IsActive to false. Requires admin authentication.

**Response:**
```json
{
  "message": "Combo deleted successfully"
}
```

## Key Features

### Price Validation
The API enforces that combo prices must be less than the sum of individual item prices to ensure combos provide value to customers. If this validation fails, the API returns:

```json
{
  "message": "Combo price must be less than the sum of individual item prices",
  "comboPrice": 30.00,
  "sumOfItemPrices": 28.50
}
```

### Soft Delete
Deleted combos are not removed from the database but marked as inactive. This preserves historical order data while removing the combo from the active menu.

### Authorization
- GET endpoints are public (no authentication required)
- POST, PUT, DELETE endpoints require admin authentication with "AdminOnly" policy

## Testing

Use the provided API_TESTS.http file to test all endpoints. Make sure to:
1. Login as admin to get an authentication token
2. Replace `YOUR_ADMIN_TOKEN_HERE` with the actual JWT token
3. Adjust food item IDs based on your seeded data

## Requirements Validated

This implementation satisfies the following requirements:
- **15.1**: Display list of all combo packages
- **15.2**: Create new combo with validation
- **15.3**: Update combo and reflect changes immediately
- **15.4**: Soft delete combo while preserving historical data
- **15.5**: Validate combo price is less than sum of item prices
