# Database Seed Data Summary

This document provides an overview of the seed data created for the Fast Food Ordering System.

## Users (3 total)

### Admin Account (1)
- **Email**: admin@fastfood.com
- **Password**: Admin123!
- **Role**: Admin
- **Name**: System Administrator

### Customer Accounts (2)
1. **Email**: customer1@example.com
   - **Password**: Customer123!
   - **Name**: John Doe
   
2. **Email**: customer2@example.com
   - **Password**: Customer123!
   - **Name**: Jane Smith

## Food Items (17 total across 5 categories)

### Burgers (4 items)
1. Classic Burger - $8.99
2. Cheese Burger - $9.99
3. Bacon Burger - $11.99
4. Veggie Burger - $9.49

### Pizza (3 items)
1. Margherita Pizza - $12.99
2. Pepperoni Pizza - $14.99
3. Supreme Pizza - $16.99

### Chicken (3 items)
1. Crispy Chicken Tenders - $9.99
2. Buffalo Wings - $11.99
3. Grilled Chicken Sandwich - $10.49

### Beverages (4 items)
1. Coca-Cola - $2.49
2. Orange Juice - $3.49
3. Milkshake - Vanilla - $4.99
4. Iced Coffee - $3.99

### Desserts (3 items)
1. Chocolate Brownie - $4.49
2. Apple Pie - $5.49
3. Ice Cream Sundae - $4.99

## Combos (4 total)

### 1. Classic Burger Meal - $10.99
- Classic Burger (1x)
- Coca-Cola (1x)
- **Items Sum**: $11.48
- **Savings**: $0.49

### 2. Family Pizza Deal - $22.99
- Pepperoni Pizza (1x)
- Coca-Cola (4x)
- **Items Sum**: $24.95
- **Savings**: $1.96

### 3. Chicken Feast - $19.99
- Crispy Chicken Tenders (1x)
- Buffalo Wings (1x)
- Coca-Cola (2x)
- **Items Sum**: $26.96
- **Savings**: $6.97

### 4. Dessert Combo - $14.99
- Chocolate Brownie (1x)
- Ice Cream Sundae (1x)
- Iced Coffee (2x)
- **Items Sum**: $17.46
- **Savings**: $2.47

## Combo Items (10 associations)
All combos have been properly associated with their respective food items through the ComboItems junction table.

## Notes
- All passwords are encrypted using BCrypt hashing algorithm
- All combo prices are less than the sum of their individual items (as per Requirement 15.5)
- All food items include detailed information (description, ingredients, nutritional info, allergen warnings)
- The seeder checks if data already exists and will not duplicate data on subsequent runs
