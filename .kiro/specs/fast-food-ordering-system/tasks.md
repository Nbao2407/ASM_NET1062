# Implementation Plan

- [x] 1. Set up project structure and database





  - Create ASP.NET Core Web API project with appropriate folder structure
  - Create Vue.js project with Vue Router and Pinia
  - Set up SQL Server database and connection strings
  - Configure Entity Framework Core with DbContext
  - _Requirements: 17.1, 20.1, 21.1, 23.1_
- [x] 2. Implement database schema and entities



- [x] 2. Implement database schema and entities




- [x] 2.1 Create Entity Framework Core entity models





  - Implement User, FoodItem, Combo, ComboItem, Order, OrderItem, and Invoice entities
  - Configure entity relationships and constraints
  - _Requirements: 20.2, 20.3_

- [x] 2.2 Create and apply database migrations




  - Generate initial migration from entity models
  - Apply migration to create database schema
  - _Requirements: 20.2, 20.3_

- [x] 2.3 Create database seed data





  - Implement seed data for food items across categories
  - Create sample combos with food item associations
  - Create test user accounts (customers and admin)
  - _Requirements: 20.5_

- [ ]* 2.4 Write property test for password encryption
  - **Property 4: Password encryption**
  - **Validates: Requirements 1.5**


- [x] 3. Implement authentication and authorization




- [x] 3.1 Implement JWT token generation and validation





  - Create JWT service for token generation
  - Implement token validation middleware
  - Configure authentication in API startup
  - _Requirements: 6.1, 11.1_




- [x] 3.2 Implement password hashing with BCrypt


  - Create password hashing service
  - Implement password verification
  - _Requirements: 1.5_



- [x] 3.3 Implement role-based authorization


  - Create authorization policies for Customer and Admin roles
  - Apply authorization attributes to protected endpoints
  - _Requirements: 11.4_

- [ ]* 3.4 Write property tests for authentication
  - **Property 18: Valid credential authentication**
  - **Property 20: Invalid credential rejection**
  - **Property 37: Admin authentication**
  - **Property 38: Invalid admin credential rejection**
  - **Validates: Requirements 6.1, 6.4, 11.1, 11.2**

- [ ]* 3.5 Write property test for role-based access
  - **Property 39: Admin site access restriction**
  - **Validates: Requirements 11.4**




- [x] 4. Implement user registration and profile management APIs


- [x] 4.1 Create registration endpoint


  - Implement POST /api/auth/register endpoint
  - Validate registration data (6 required fields)
  - Check for duplicate email addresses
  - Hash password and create user account
  - _Requirements: 1.1, 1.2, 1.3, 1.4, 1.5_



- [-]* 4.2 Write property tests for registration

  - **Property 1: Registration validation completeness**
  - **Property 2: Account creation from valid registration**



  - **Property 3: Duplicate email rejection**
  - **Validates: Requirements 1.2, 1.3, 1.4**

- [x] 4.3 Create login endpoints


  - Implement POST /api/auth/login for email/password authentication
  - Return JWT token on successful authentication
  - _Requirements: 6.1, 6.4_


- [x] 4.4 Implement Google OAuth authentication


  - Configure Google OAuth client
  - Implement POST /api/auth/google endpoint
  - Create or link account on successful OAuth
  - _Requirements: 6.2, 6.3_

- [ ]* 4.5 Write property test for OAuth account linking
  - **Property 19: Google OAuth account linking**
  - **Validates: Requirements 6.3**

- [x] 4.6 Create profile management endpoints





  - Implement GET /api/customers/profile endpoint
  - Implement PUT /api/customers/profile endpoint
  - Implement PUT /api/customers/password endpoint
  - Validate profile updates and check email uniqueness
  - _Requirements: 7.1, 7.2, 7.3, 7.4, 7.5_

- [ ]* 4.7 Write property tests for profile management
  - **Property 21: Profile update validation**
  - **Property 22: Email uniqueness on update**
  - **Property 23: Password change verification**
  - **Property 24: Profile update persistence**
  - **Validates: Requirements 7.2, 7.3, 7.4, 7.5**
-

- [x] 5. Implement food item management APIs




- [x] 5.1 Create food item CRUD endpoints


  - Implement GET /api/items (with category filtering)
  - Implement GET /api/items/{id}
  - Implement POST /api/items (admin only)
  - Implement PUT /api/items/{id} (admin only)
  - Implement DELETE /api/items/{id} (admin only, soft delete)
  - _Requirements: 14.1, 14.2, 14.3, 14.4_

- [ ]* 5.2 Write property tests for food item management
  - **Property 45: Food item list completeness**
  - **Property 46: Food item creation validation**
  - **Property 47: Food item update visibility**
  - **Property 48: Food item soft deletion**
  - **Property 49: Food item categorization**
  - **Validates: Requirements 14.1, 14.2, 14.3, 14.4, 14.5**


- [x] 5.3 Implement search endpoints



  - Implement GET /api/items/search for basic search by name
  - Implement advanced search with multiple criteria (name, price, category, theme)
  - Implement case-insensitive search
  - _Requirements: 4.1, 4.2, 4.5, 5.1, 5.2, 5.3, 5.4, 5.5_

- [ ]* 5.4 Write property tests for search functionality
  - **Property 12: Basic search accuracy**
  - **Property 13: Case-insensitive search**
  - **Property 14: Advanced search conjunction**
  - **Property 15: Price range filtering**
  - **Property 16: Category search accuracy**
  - **Property 17: Theme search accuracy**
  - **Validates: Requirements 4.1, 4.2, 4.5, 5.2, 5.3, 5.4, 5.5**

- [x] 5.5 Implement category endpoints


  - Implement GET /api/items/categories
  - Return list of all available categories
  - _Requirements: 2.3_

- [ ]* 5.6 Write property test for category filtering
  - **Property 7: Category filtering accuracy**
  - **Validates: Requirements 2.3**

- [ ] 6. Implement combo management APIs


- [x] 6.1 Create combo CRUD endpoints


  - Implement GET /api/combos
  - Implement GET /api/combos/{id}
  - Implement POST /api/combos (admin only)
  - Implement PUT /api/combos/{id} (admin only)
  - Implement DELETE /api/combos/{id} (admin only, soft delete)
  - Validate combo price is less than sum of item prices
  - _Requirements: 15.1, 15.2, 15.3, 15.4, 15.5_

- [ ]* 6.2 Write property tests for combo management
  - **Property 50: Combo list completeness**
  - **Property 51: Combo creation validation**
  - **Property 52: Combo update visibility**
  - **Property 53: Combo soft deletion**
  - **Property 54: Combo pricing validation**
  - **Validates: Requirements 15.1, 15.2, 15.3, 15.4, 15.5**

- [ ]* 6.3 Write property tests for combo display
  - **Property 6: Combo display with items**
  - **Property 9: Combo detail completeness**
  - **Property 11: Combo items listing**
  - **Validates: Requirements 2.2, 3.2, 3.5**

- [x] 7. Implement order and invoice APIs





- [x] 7.1 Create order placement endpoint


  - Implement POST /api/orders
  - Validate cart items and calculate totals
  - Generate unique order number
  - Create order and order items
  - Process payment
  - Create invoice record
  - _Requirements: 8.1, 8.2, 8.3, 8.4, 8.5_

- [ ]* 7.2 Write property tests for order creation
  - **Property 25: Cart item storage**
  - **Property 26: Checkout summary accuracy**
  - **Property 27: Invoice creation on order**
  - **Property 28: Unique order number generation**
  - **Validates: Requirements 8.1, 8.2, 8.3, 8.4, 8.5**


- [ ] 7.3 Create order retrieval endpoints


  - Implement GET /api/orders (customer's orders or all for admin)
  - Implement GET /api/orders/{id}
  - Filter orders by status
  - _Requirements: 10.1, 16.1_

- [ ]* 7.4 Write property tests for order retrieval
  - **Property 33: Active order status display**
  - **Property 35: Multiple order status separation**
  - **Property 55: Order list completeness**
  - **Validates: Requirements 10.1, 10.4, 16.1**


- [ ] 7.5 Create order status management endpoint
  - Implement PUT /api/orders/{id}/status (admin only)
  - Support three statuses: NotDelivered, BeingDelivered, Delivered
  - Record timestamp on status changes
  - _Requirements: 16.2, 16.3, 16.4, 16.5_

- [ ]* 7.6 Write property tests for order status
  - **Property 34: Valid order status values**
  - **Property 36: Delivered order removal from active**
  - **Property 56: Status change timestamp**
  - **Property 57: Delivered status update**
  - **Property 58: Order status filtering**

  - **Validates: Requirements 10.3, 10.5, 16.2, 16.3, 16.4, 16.5**

- [ ] 7.7 Create invoice endpoints
  - Implement GET /api/orders/invoices (customer's invoices)
  - Implement GET /api/orders/invoices/{id}
  - Support date range filtering
  - Sort invoices in reverse chronological order
  - _Requirements: 9.1, 9.2, 9.3, 9.4, 9.5_

- [ ]* 7.8 Write property tests for invoices
  - **Property 29: Order history completeness**
  - **Property 30: Invoice detail completeness**
  - **Property 31: Invoice date filtering**
  - **Property 32: Invoice chronological ordering**
  - **Validates: Requirements 9.1, 9.2, 9.3, 9.4, 9.5**

- [x] 8. Implement admin user management APIs



- [x] 8.1 Create user management endpoints


  - Implement GET /api/admin/users (admin only)
  - Implement GET /api/admin/users/{id} (admin only)
  - Implement POST /api/admin/users (admin only)
  - Implement PUT /api/admin/users/{id} (admin only)
  - Implement DELETE /api/admin/users/{id} (admin only)
  - Prevent deletion of currently logged-in admin
  - _Requirements: 13.1, 13.2, 13.3, 13.4, 13.5_

- [ ]* 8.2 Write property tests for user management
  - **Property 40: User list completeness**
  - **Property 41: User account creation validation**
  - **Property 42: User account update persistence**

  - **Property 43: User account deletion**
  - **Property 44: Self-deletion prevention**
  - **Validates: Requirements 13.1, 13.2, 13.3, 13.4, 13.5**

- [ ] 9. Implement input validation and security

- [ ] 9.1 Create validation attributes and services
  - Implement required field validation
  - Implement data type and format validation
  - Implement custom validation for business rules
  - _Requirements: 18.1, 18.2_

- [ ] 9.2 Implement input sanitization
  - Create input sanitization service
  - Sanitize all user inputs to prevent XSS and SQL injection
  - _Requirements: 18.4_

- [ ]* 9.3 Write property tests for validation
  - **Property 59: Required field validation**
  - **Property 60: Data type and format validation**
  - **Property 61: Validation error messaging**
  - **Property 62: Input sanitization**
  - **Validates: Requirements 18.1, 18.2, 18.3, 18.4**

- [ ] 9.4 Implement file upload validation
  - Validate image file type, size, and dimensions
  - Implement secure file storage
  - _Requirements: 18.5_


- [ ]* 9.5 Write property test for file upload validation
  - **Property 63: Image upload validation**
  - **Validates: Requirements 18.5**

- [ ] 10. Implement error handling and logging

- [ ] 10.1 Create global exception handling middleware
  - Handle database errors
  - Handle validation errors
  - Handle authentication/authorization errors
  - Return appropriate HTTP status codes
  - _Requirements: 21.5_

- [ ] 10.2 Implement logging infrastructure
  - Configure structured logging
  - Log API requests and responses
  - Log authentication attempts
  - Log errors and exceptions
  - _Requirements: 21.5_

- [x] 11. Set up Vue.js frontend project structure



- [x] 11.1 Create Vue.js project with routing and state management


  - Initialize Vue 3 project with Vite
  - Install and configure Vue Router
  - Install and configure Pinia for state management
  - Install Axios for HTTP requests
  - Install UI framework (Tailwind CSS or Bootstrap)
  - _Requirements: 17.1, 17.2, 17.3, 17.5_

- [x] 11.2 Create frontend folder structure


  - Create folders for components, views, stores, services, and utilities
  - Set up API service layer for backend communication
  - _Requirements: 17.2, 22.1_



- [ ] 11.3 Configure API client and interceptors
  - Create Axios instance with base URL
  - Implement request interceptor for JWT tokens
  - Implement response interceptor for error handling
  - _Requirements: 22.1, 22.5_
-

- [x] 12. Implement authentication UI components



- [x] 12.1 Create registration page


  - Build registration form with 6 required fields
  - Implement client-side validation
  - Connect to registration API
  - Handle success and error responses
  - _Requirements: 1.1, 1.2, 1.3, 1.4_



- [ ] 12.2 Create login page
  - Build login form with email and password
  - Add Google OAuth login button
  - Implement authentication logic
  - Store JWT token securely
  - Redirect to appropriate page on success


  - _Requirements: 6.1, 6.2, 6.3, 6.4_

- [ ] 12.3 Create profile management page
  - Display current user information
  - Implement profile edit form

  - Implement password change form
  - Connect to profile update APIs
  - _Requirements: 7.1, 7.2, 7.3, 7.4, 7.5_

- [ ] 13. Implement customer-facing menu UI

- [x] 13.1 Create menu browsing components

  - Build FoodItemCard component
  - Build ComboCard component
  - Create menu page with category filtering
  - Display all active food items and combos
  - _Requirements: 2.1, 2.2, 2.3, 2.4_

- [ ]* 13.2 Write property tests for menu display
  - **Property 5: Menu display completeness**
  - **Validates: Requirements 2.1**

- [x] 13.3 Create item detail pages


  - Build food item detail page
  - Build combo detail page
  - Display all required information (name, description, price, images, etc.)
  - Display nutritional info and allergen warnings if available
  - List combo items for combos
  - _Requirements: 3.1, 3.2, 3.3, 3.5_

- [ ]* 13.4 Write property tests for detail display
  - **Property 8: Food item detail completeness**
  - **Property 10: Conditional nutritional display**
  - **Validates: Requirements 3.1, 3.3**

- [x] 13.4 Implement search functionality



  - Create SearchBar component with basic and advanced modes
  - Implement basic search by name
  - Implement advanced search with multiple criteria
  - Display search results
  - Handle empty results
  - _Requirements: 4.1, 4.2, 4.3, 4.4, 4.5, 5.1, 5.2, 5.3, 5.4, 5.5_


- [x] 14. Implement shopping cart and checkout



- [ ] 14.1 Create cart state management
  - Create Pinia store for cart
  - Implement add to cart functionality
  - Implement update quantity functionality
  - Implement remove from cart functionality
  - Calculate cart totals


  - _Requirements: 8.1_

- [ ] 14.2 Create cart page
  - Display cart items with quantities and prices
  - Allow quantity updates and item removal


  - Display cart total
  - Provide checkout button
  - _Requirements: 8.1, 8.2_

- [ ] 14.3 Create checkout page
  - Display order summary with itemized pricing
  - Collect delivery address
  - Select payment method
  - Confirm and submit order

  - Display order confirmation with order number
  - _Requirements: 8.2, 8.3, 8.4_

- [ ] 15. Implement order tracking UI

- [ ] 15.1 Create order history page
  - Display all customer invoices
  - Sort invoices in reverse chronological order
  - Implement date range filtering
  - Show invoice summary information
  - _Requirements: 9.1, 9.3, 9.4_

- [ ] 15.2 Create invoice detail page
  - Display complete invoice details
  - Show all items, quantities, prices, and total
  - Display order date and payment information
  - _Requirements: 9.2, 9.5_

- [ ] 15.3 Create active orders tracking page
  - Display all active orders with current status
  - Show status for each order separately
  - Update display when status changes
  - Hide delivered orders from active list
  - _Requirements: 10.1, 10.2, 10.4, 10.5_

- [ ] 16. Implement admin site UI
- [ ] 16.1 Create admin login page
  - Build admin login form
  - Implement admin authentication
  - Redirect to admin dashboard on success
  - _Requirements: 11.1, 11.2, 11.3_

- [ ] 16.2 Create admin layout and navigation
  - Build admin header with navigation
  - Build admin sidebar with menu items
  - Create admin dashboard with overview
  - _Requirements: 11.1_

- [ ] 16.3 Create user management interface
  - Display list of all users in DataTable
  - Implement create user modal
  - Implement edit user modal
  - Implement delete user with confirmation
  - Prevent deletion of current admin
  - _Requirements: 13.1, 13.2, 13.3, 13.4, 13.5_

- [ ] 16.4 Create food item management interface
  - Display list of all food items in DataTable
  - Implement create food item modal with image upload
  - Implement edit food item modal
  - Implement delete food item with confirmation
  - _Requirements: 14.1, 14.2, 14.3, 14.4_

- [ ] 16.5 Create combo management interface
  - Display list of all combos in DataTable
  - Implement create combo modal with item selection
  - Validate combo price is less than sum of items
  - Implement edit combo modal
  - Implement delete combo with confirmation
  - _Requirements: 15.1, 15.2, 15.3, 15.4, 15.5_

- [ ] 16.6 Create order management interface
  - Display list of all orders in DataTable
  - Show current delivery status for each order
  - Implement status update functionality
  - Implement status filtering
  - Record timestamp on status changes
  - _Requirements: 16.1, 16.2, 16.3, 16.4, 16.5_

- [ ] 17. Implement responsive design
- [ ] 17.1 Apply responsive styling to customer site
  - Ensure mobile-optimized layouts
  - Test on tablet screen sizes
  - Ensure desktop layouts work properly
  - Handle device rotation
  - _Requirements: 19.1, 19.2, 19.3, 19.4, 19.5_

- [ ] 17.2 Apply responsive styling to admin site
  - Ensure admin interface works on tablets
  - Optimize DataTable for smaller screens
  - Test responsive navigation
  - _Requirements: 19.1, 19.2, 19.3, 19.4, 19.5_

- [ ] 18. Implement frontend error handling
- [ ] 18.1 Create error handling components
  - Create ErrorMessage component
  - Create SuccessMessage component
  - Create LoadingSpinner component
  - _Requirements: 22.5_

- [ ] 18.2 Implement error handling in API service
  - Handle network errors
  - Handle authentication errors
  - Handle validation errors
  - Display user-friendly error messages
  - _Requirements: 22.5_

- [ ] 19. Final integration and testing
- [ ] 19.1 Test complete user workflows
  - Test guest browsing and registration
  - Test customer login and ordering
  - Test order tracking
  - Test admin login and management functions
  - _Requirements: All_

- [ ] 19.2 Test cross-browser compatibility
  - Test on Chrome, Firefox, Safari, Edge
  - Fix any browser-specific issues
  - _Requirements: 19.1, 19.2, 19.3_

- [ ] 19.3 Perform security testing
  - Test authentication and authorization
  - Test input validation and sanitization
  - Test for SQL injection vulnerabilities
  - Test for XSS vulnerabilities
  - _Requirements: 18.4_

- [ ] 20. Checkpoint - Ensure all tests pass
  - Ensure all tests pass, ask the user if questions arise.
