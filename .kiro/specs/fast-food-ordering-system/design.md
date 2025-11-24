# Design Document

## Overview

The Fast Food Ordering System is a full-stack web application that provides a digital platform for fast food ordering and management. The system consists of two primary interfaces: a Customer Site for public users to browse menus and place orders, and an Admin Site for administrators to manage the menu, orders, and user accounts.

The architecture follows a modern client-server pattern with a Vue.js frontend communicating with an ASP.NET Core backend via REST APIs, backed by a Microsoft SQL Server database. This separation enables independent scaling, deployment, and maintenance of frontend and backend components.

### Key Design Goals

- **User Experience**: Provide intuitive, responsive interfaces for both customers and administrators
- **Security**: Implement robust authentication, authorization, and input validation
- **Scalability**: Design for growth in users, orders, and menu items
- **Maintainability**: Use modular architecture with clear separation of concerns
- **Performance**: Optimize database queries and API responses for fast load times

## Architecture

### System Architecture

The system follows a three-tier architecture:

```
┌─────────────────────────────────────────────────────────┐
│                    Presentation Layer                    │
│                                                          │
│  ┌──────────────────┐         ┌──────────────────┐     │
│  │  Customer Site   │         │   Admin Site     │     │
│  │    (Vue.js)      │         │    (Vue.js)      │     │
│  └──────────────────┘         └──────────────────┘     │
└─────────────────────────────────────────────────────────┘
                         │
                         │ REST API (HTTPS)
                         │
┌─────────────────────────────────────────────────────────┐
│                   Application Layer                      │
│                                                          │
│  ┌──────────────────────────────────────────────────┐  │
│  │         ASP.NET Core Web API                     │  │
│  │  ┌────────────┐  ┌────────────┐  ┌───────────┐  │  │
│  │  │Controllers │  │  Services  │  │   Auth    │  │  │
│  │  └────────────┘  └────────────┘  └───────────┘  │  │
│  └──────────────────────────────────────────────────┘  │
└─────────────────────────────────────────────────────────┘
                         │
                         │ Entity Framework Core
                         │
┌─────────────────────────────────────────────────────────┐
│                      Data Layer                          │
│                                                          │
│  ┌──────────────────────────────────────────────────┐  │
│  │           Microsoft SQL Server                   │  │
│  │  ┌────────┐ ┌────────┐ ┌────────┐ ┌─────────┐  │  │
│  │  │ Users  │ │ Items  │ │ Orders │ │ Combos  │  │  │
│  │  └────────┘ └────────┘ └────────┘ └─────────┘  │  │
│  └──────────────────────────────────────────────────┘  │
└─────────────────────────────────────────────────────────┘
```

### Technology Stack

**Frontend:**
- Vue.js 3 with Composition API
- Vue Router for navigation
- Pinia for state management
- Axios for HTTP requests
- Tailwind CSS or Bootstrap for styling

**Backend:**
- ASP.NET Core 8.0
- Minimal API or Controller-based API
- Entity Framework Core for ORM
- JWT for authentication
- BCrypt for password hashing

**Database:**
- Microsoft SQL Server 2019+
- Entity Framework Core migrations

**Authentication:**
- JWT tokens for session management
- Google OAuth 2.0 for social login
- BCrypt for password encryption

## Components and Interfaces

### Frontend Components

#### Customer Site Components

**Layout Components:**
- `AppHeader`: Navigation bar with logo, search, cart icon, and user menu
- `AppFooter`: Footer with links and contact information
- `Sidebar`: Category navigation for filtering menu items

**Page Components:**
- `HomePage`: Landing page with featured items and categories
- `MenuPage`: Grid/list view of food items with filtering and search
- `ItemDetailPage`: Detailed view of a single food item or combo
- `CartPage`: Shopping cart with item management
- `CheckoutPage`: Order summary and payment processing
- `LoginPage`: Customer authentication with email/password and Google OAuth
- `RegisterPage`: New customer registration form
- `ProfilePage`: Customer profile management
- `OrderHistoryPage`: List of past orders with invoice details
- `OrderTrackingPage`: Real-time order status display

**Shared Components:**
- `FoodItemCard`: Reusable card displaying food item summary
- `ComboCard`: Reusable card displaying combo package
- `SearchBar`: Basic and advanced search interface
- `LoadingSpinner`: Loading state indicator
- `ErrorMessage`: Error display component
- `SuccessMessage`: Success notification component

#### Admin Site Components

**Layout Components:**
- `AdminHeader`: Admin navigation with user menu
- `AdminSidebar`: Navigation menu for admin sections

**Page Components:**
- `AdminLoginPage`: Administrator authentication
- `AdminDashboard`: Overview with statistics and quick actions
- `UserManagementPage`: CRUD operations for customer accounts
- `FoodItemManagementPage`: CRUD operations for food items
- `ComboManagementPage`: CRUD operations for combo packages
- `OrderManagementPage`: Order list with status management
- `AdminProfilePage`: Administrator profile management

**Shared Components:**
- `DataTable`: Reusable table with sorting, filtering, and pagination
- `FormModal`: Modal dialog for create/edit operations
- `ConfirmDialog`: Confirmation dialog for delete operations
- `StatusBadge`: Visual indicator for order status

### Backend API Endpoints

#### Authentication Endpoints

```
POST   /api/auth/register          - Register new customer
POST   /api/auth/login             - Login with email/password
POST   /api/auth/google            - Login with Google OAuth
POST   /api/auth/refresh           - Refresh JWT token
POST   /api/auth/logout            - Logout and invalidate token
```

#### Customer Endpoints

```
GET    /api/customers/profile      - Get current customer profile
PUT    /api/customers/profile      - Update customer profile
PUT    /api/customers/password     - Change password
```

#### Food Item Endpoints

```
GET    /api/items                  - Get all food items (with filters)
GET    /api/items/{id}             - Get food item by ID
POST   /api/items                  - Create food item (Admin only)
PUT    /api/items/{id}             - Update food item (Admin only)
DELETE /api/items/{id}             - Delete food item (Admin only)
GET    /api/items/search           - Search food items
GET    /api/items/categories       - Get all categories
```

#### Combo Endpoints

```
GET    /api/combos                 - Get all combos
GET    /api/combos/{id}            - Get combo by ID
POST   /api/combos                 - Create combo (Admin only)
PUT    /api/combos/{id}            - Update combo (Admin only)
DELETE /api/combos/{id}            - Delete combo (Admin only)
```

#### Order Endpoints

```
GET    /api/orders                 - Get customer orders or all orders (Admin)
GET    /api/orders/{id}            - Get order by ID
POST   /api/orders                 - Create new order
PUT    /api/orders/{id}/status     - Update order status (Admin only)
GET    /api/orders/invoices        - Get customer invoices
GET    /api/orders/invoices/{id}   - Get invoice by ID
```

#### User Management Endpoints (Admin only)

```
GET    /api/admin/users            - Get all users
GET    /api/admin/users/{id}       - Get user by ID
POST   /api/admin/users            - Create user
PUT    /api/admin/users/{id}       - Update user
DELETE /api/admin/users/{id}       - Delete user
```

## Data Models

### Database Schema

#### Users Table

```sql
CREATE TABLE Users (
    UserId INT PRIMARY KEY IDENTITY(1,1),
    Email NVARCHAR(255) UNIQUE NOT NULL,
    PasswordHash NVARCHAR(255) NOT NULL,
    FullName NVARCHAR(255) NOT NULL,
    PhoneNumber NVARCHAR(20) NOT NULL,
    Address NVARCHAR(500),
    DateOfBirth DATE,
    Role NVARCHAR(50) NOT NULL, -- 'Customer' or 'Admin'
    GoogleId NVARCHAR(255) NULL,
    IsActive BIT DEFAULT 1,
    CreatedAt DATETIME2 DEFAULT GETDATE(),
    UpdatedAt DATETIME2 DEFAULT GETDATE()
);
```

#### FoodItems Table

```sql
CREATE TABLE FoodItems (
    FoodItemId INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(255) NOT NULL,
    Description NVARCHAR(1000),
    Price DECIMAL(10,2) NOT NULL,
    Category NVARCHAR(100) NOT NULL,
    Theme NVARCHAR(100),
    ImageUrl NVARCHAR(500),
    Ingredients NVARCHAR(1000),
    NutritionalInfo NVARCHAR(1000),
    AllergenWarnings NVARCHAR(500),
    IsActive BIT DEFAULT 1,
    CreatedAt DATETIME2 DEFAULT GETDATE(),
    UpdatedAt DATETIME2 DEFAULT GETDATE()
);
```

#### Combos Table

```sql
CREATE TABLE Combos (
    ComboId INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(255) NOT NULL,
    Description NVARCHAR(1000),
    Price DECIMAL(10,2) NOT NULL,
    ImageUrl NVARCHAR(500),
    IsActive BIT DEFAULT 1,
    CreatedAt DATETIME2 DEFAULT GETDATE(),
    UpdatedAt DATETIME2 DEFAULT GETDATE()
);
```

#### ComboItems Table (Junction Table)

```sql
CREATE TABLE ComboItems (
    ComboItemId INT PRIMARY KEY IDENTITY(1,1),
    ComboId INT NOT NULL,
    FoodItemId INT NOT NULL,
    Quantity INT DEFAULT 1,
    FOREIGN KEY (ComboId) REFERENCES Combos(ComboId),
    FOREIGN KEY (FoodItemId) REFERENCES FoodItems(FoodItemId)
);
```

#### Orders Table

```sql
CREATE TABLE Orders (
    OrderId INT PRIMARY KEY IDENTITY(1,1),
    UserId INT NOT NULL,
    OrderNumber NVARCHAR(50) UNIQUE NOT NULL,
    TotalAmount DECIMAL(10,2) NOT NULL,
    Status NVARCHAR(50) NOT NULL, -- 'NotDelivered', 'BeingDelivered', 'Delivered'
    PaymentMethod NVARCHAR(50),
    PaymentStatus NVARCHAR(50),
    DeliveryAddress NVARCHAR(500),
    OrderDate DATETIME2 DEFAULT GETDATE(),
    StatusUpdatedAt DATETIME2,
    FOREIGN KEY (UserId) REFERENCES Users(UserId)
);
```

#### OrderItems Table

```sql
CREATE TABLE OrderItems (
    OrderItemId INT PRIMARY KEY IDENTITY(1,1),
    OrderId INT NOT NULL,
    FoodItemId INT NULL,
    ComboId INT NULL,
    Quantity INT NOT NULL,
    UnitPrice DECIMAL(10,2) NOT NULL,
    Subtotal DECIMAL(10,2) NOT NULL,
    FOREIGN KEY (OrderId) REFERENCES Orders(OrderId),
    FOREIGN KEY (FoodItemId) REFERENCES FoodItems(FoodItemId),
    FOREIGN KEY (ComboId) REFERENCES Combos(ComboId)
);
```

#### Invoices Table

```sql
CREATE TABLE Invoices (
    InvoiceId INT PRIMARY KEY IDENTITY(1,1),
    OrderId INT NOT NULL UNIQUE,
    InvoiceNumber NVARCHAR(50) UNIQUE NOT NULL,
    InvoiceDate DATETIME2 DEFAULT GETDATE(),
    TotalAmount DECIMAL(10,2) NOT NULL,
    TaxAmount DECIMAL(10,2) DEFAULT 0,
    DiscountAmount DECIMAL(10,2) DEFAULT 0,
    FOREIGN KEY (OrderId) REFERENCES Orders(OrderId)
);
```

### Entity Relationship Diagram

```
Users (1) ──────< (N) Orders
                       │
                       │ (1)
                       │
                       └──────> (1) Invoices
                       │
                       │ (1)
                       │
                       └──────< (N) OrderItems
                                     │
                                     ├──> (N:1) FoodItems
                                     │
                                     └──> (N:1) Combos
                                                  │
                                                  │ (1)
                                                  │
                                                  └──< (N) ComboItems >──(N) FoodItems
```

### C# Entity Models

**User Entity:**
```csharp
public class User
{
    public int UserId { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public string FullName { get; set; }
    public string PhoneNumber { get; set; }
    public string Address { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string Role { get; set; } // "Customer" or "Admin"
    public string GoogleId { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
    public ICollection<Order> Orders { get; set; }
}
```

**FoodItem Entity:**
```csharp
public class FoodItem
{
    public int FoodItemId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public string Category { get; set; }
    public string Theme { get; set; }
    public string ImageUrl { get; set; }
    public string Ingredients { get; set; }
    public string NutritionalInfo { get; set; }
    public string AllergenWarnings { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
    public ICollection<OrderItem> OrderItems { get; set; }
    public ICollection<ComboItem> ComboItems { get; set; }
}
```

**Combo Entity:**
```csharp
public class Combo
{
    public int ComboId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public string ImageUrl { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
    public ICollection<ComboItem> ComboItems { get; set; }
    public ICollection<OrderItem> OrderItems { get; set; }
}
```

**Order Entity:**
```csharp
public class Order
{
    public int OrderId { get; set; }
    public int UserId { get; set; }
    public string OrderNumber { get; set; }
    public decimal TotalAmount { get; set; }
    public string Status { get; set; } // "NotDelivered", "BeingDelivered", "Delivered"
    public string PaymentMethod { get; set; }
    public string PaymentStatus { get; set; }
    public string DeliveryAddress { get; set; }
    public DateTime OrderDate { get; set; }
    public DateTime? StatusUpdatedAt { get; set; }
    
    public User User { get; set; }
    public ICollection<OrderItem> OrderItems { get; set; }
    public Invoice Invoice { get; set; }
}
```


## Correctness Properties

*A property is a characteristic or behavior that should hold true across all valid executions of a system-essentially, a formal statement about what the system should do. Properties serve as the bridge between human-readable specifications and machine-verifiable correctness guarantees.*

### Property 1: Registration validation completeness
*For any* registration attempt, the system should validate all required fields (name, email, password, phone number, address, date of birth) and reject submissions with missing or invalid data.
**Validates: Requirements 1.2**

### Property 2: Account creation from valid registration
*For any* valid registration data, the system should successfully create a new Customer account and the account should be retrievable from the database.
**Validates: Requirements 1.3**

### Property 3: Duplicate email rejection
*For any* email address that already exists in the system, attempting to register a new account with that email should fail with an appropriate error message.
**Validates: Requirements 1.4**

### Property 4: Password encryption
*For any* password provided during registration, the stored password hash in the database should never match the plain text password.
**Validates: Requirements 1.5**

### Property 5: Menu display completeness
*For any* set of active food items in the database, all items should be displayed in the menu grouped by their respective categories.
**Validates: Requirements 2.1**

### Property 6: Combo display with items
*For any* active combo in the database, the combo display should include all food items that are part of that combo.
**Validates: Requirements 2.2**

### Property 7: Category filtering accuracy
*For any* category filter selection, all displayed food items should belong to the selected category and no items from other categories should be shown.
**Validates: Requirements 2.3**

### Property 8: Food item detail completeness
*For any* food item, the detail view should display all required fields: name, description, price, ingredients, and images.
**Validates: Requirements 3.1**

### Property 9: Combo detail completeness
*For any* combo, the detail view should display name, description, price, included items, and images.
**Validates: Requirements 3.2**

### Property 10: Conditional nutritional display
*For any* food item with nutritional information or allergen warnings, that information should be displayed in the detail view.
**Validates: Requirements 3.3**

### Property 11: Combo items listing
*For any* combo, the detail view should list all individual food items included in the combo package.
**Validates: Requirements 3.5**

### Property 12: Basic search accuracy
*For any* search term, all returned food items should have names containing the search term, and all items with matching names should be returned.
**Validates: Requirements 4.1, 4.2**

### Property 13: Case-insensitive search
*For any* search term, the search results should be identical regardless of the case (uppercase, lowercase, or mixed) of the search term.
**Validates: Requirements 4.5**

### Property 14: Advanced search conjunction
*For any* combination of search criteria (name, price range, category, description, theme), all returned items should match all specified criteria.
**Validates: Requirements 5.2**

### Property 15: Price range filtering
*For any* price range with minimum and maximum values, all returned food items should have prices within that range (inclusive).
**Validates: Requirements 5.3**

### Property 16: Category search accuracy
*For any* category selection in advanced search, all returned items should belong to the selected category.
**Validates: Requirements 5.4**

### Property 17: Theme search accuracy
*For any* theme selection in advanced search, all returned items should be tagged with the specified theme.
**Validates: Requirements 5.5**

### Property 18: Valid credential authentication
*For any* valid email and password combination in the database, the authentication should succeed and grant access to the Customer Site.
**Validates: Requirements 6.1**

### Property 19: Google OAuth account linking
*For any* successful Google OAuth authentication, either a new account should be created or an existing account should be linked, and a session should be established.
**Validates: Requirements 6.3**

### Property 20: Invalid credential rejection
*For any* invalid email or password combination, the authentication should fail and display an error message.
**Validates: Requirements 6.4**

### Property 21: Profile update validation
*For any* profile information update, the system should validate the data before persisting changes to the database.
**Validates: Requirements 7.2**

### Property 22: Email uniqueness on update
*For any* email address update, if the new email already exists for another user, the update should be rejected.
**Validates: Requirements 7.3**

### Property 23: Password change verification
*For any* password change request, the system should require and verify the current password before accepting the new password.
**Validates: Requirements 7.4**

### Property 24: Profile update persistence
*For any* valid profile update, the changes should be saved to the database and retrievable in subsequent profile views.
**Validates: Requirements 7.5**

### Property 25: Cart item storage
*For any* item (food item or combo) added to cart with a quantity, the cart should contain that item with the correct quantity.
**Validates: Requirements 8.1**

### Property 26: Checkout summary accuracy
*For any* cart contents, the checkout summary should display all items with correct individual prices and the total should equal the sum of all item subtotals.
**Validates: Requirements 8.2**

### Property 27: Invoice creation on order
*For any* confirmed and paid order, an invoice record should be created in the database.
**Validates: Requirements 8.3, 8.5**

### Property 28: Unique order number generation
*For any* completed order, the system should generate a unique order number that does not conflict with any existing order numbers.
**Validates: Requirements 8.4**

### Property 29: Order history completeness
*For any* customer, accessing order history should display all invoices associated with that customer's account.
**Validates: Requirements 9.1**

### Property 30: Invoice detail completeness
*For any* invoice, the display should include order date, all items purchased, quantities, prices, and total amount.
**Validates: Requirements 9.2, 9.5**

### Property 31: Invoice date filtering
*For any* date range filter, only invoices with order dates within that range should be displayed.
**Validates: Requirements 9.3**

### Property 32: Invoice chronological ordering
*For any* set of invoices, they should be displayed in reverse chronological order with the most recent invoice first.
**Validates: Requirements 9.4**

### Property 33: Active order status display
*For any* active order belonging to a customer, the current status should be displayed when viewing active orders.
**Validates: Requirements 10.1**

### Property 34: Valid order status values
*For any* order in the system, the status should be one of three valid values: "NotDelivered", "BeingDelivered", or "Delivered".
**Validates: Requirements 10.3, 16.4**

### Property 35: Multiple order status separation
*For any* customer with multiple active orders, each order should display its own independent status.
**Validates: Requirements 10.4**

### Property 36: Delivered order removal from active
*For any* order that is marked as "Delivered", it should no longer appear in the active orders list.
**Validates: Requirements 10.5**

### Property 37: Admin authentication
*For any* valid administrator credentials, authentication should succeed and grant access to the Admin Site with administrative privileges.
**Validates: Requirements 11.1, 11.3**

### Property 38: Invalid admin credential rejection
*For any* invalid administrator credentials, authentication should fail and display an error message.
**Validates: Requirements 11.2**

### Property 39: Admin site access restriction
*For any* user without the Administrator role, access to the Admin Site should be denied.
**Validates: Requirements 11.4**

### Property 40: User list completeness
*For any* administrator accessing user management, all customer accounts in the database should be displayed.
**Validates: Requirements 13.1**

### Property 41: User account creation validation
*For any* new user account creation by an administrator, all required fields should be validated before the account is created.
**Validates: Requirements 13.2**

### Property 42: User account update persistence
*For any* user account update by an administrator, the changes should be saved to the database.
**Validates: Requirements 13.3**

### Property 43: User account deletion
*For any* user account deletion by an administrator, the account should be removed from active users.
**Validates: Requirements 13.4**

### Property 44: Self-deletion prevention
*For any* administrator attempting to delete their own currently logged-in account, the deletion should be prevented and an error message should be displayed.
**Validates: Requirements 13.5**

### Property 45: Food item list completeness
*For any* administrator accessing food item management, all food items in the database should be displayed.
**Validates: Requirements 14.1**

### Property 46: Food item creation validation
*For any* new food item creation, the system should require and validate name, description, price, category, and image.
**Validates: Requirements 14.2**

### Property 47: Food item update visibility
*For any* food item update by an administrator, the changes should be immediately visible in the customer-facing menu.
**Validates: Requirements 14.3**

### Property 48: Food item soft deletion
*For any* deleted food item, it should not appear in the active menu but should remain associated with historical orders.
**Validates: Requirements 14.4**

### Property 49: Food item categorization
*For any* food item, it should be assignable to a category (burgers, pizza, chicken, beverages, desserts, etc.).
**Validates: Requirements 14.5**

### Property 50: Combo list completeness
*For any* administrator accessing combo management, all combo packages in the database should be displayed.
**Validates: Requirements 15.1**

### Property 51: Combo creation validation
*For any* new combo creation, the system should require and validate combo name, description, price, and at least one included food item.
**Validates: Requirements 15.2**

### Property 52: Combo update visibility
*For any* combo update by an administrator, the changes should be immediately visible in the customer-facing menu.
**Validates: Requirements 15.3**

### Property 53: Combo soft deletion
*For any* deleted combo, it should not appear in the active menu but should remain associated with historical orders.
**Validates: Requirements 15.4**

### Property 54: Combo pricing validation
*For any* combo, the combo price should be less than the sum of the individual prices of all included food items.
**Validates: Requirements 15.5**

### Property 55: Order list completeness
*For any* administrator accessing order management, all orders in the system should be displayed with their current delivery status.
**Validates: Requirements 16.1**

### Property 56: Status change timestamp
*For any* order status update, the system should record a timestamp of when the status change occurred.
**Validates: Requirements 16.2**

### Property 57: Delivered status update
*For any* order marked as "Delivered" by an administrator, the order status should be updated in the database.
**Validates: Requirements 16.3**

### Property 58: Order status filtering
*For any* delivery status filter selection, only orders with the matching status should be displayed.
**Validates: Requirements 16.5**

### Property 59: Required field validation
*For any* form submission, the system should validate that all required fields are populated before processing.
**Validates: Requirements 18.1**

### Property 60: Data type and format validation
*For any* user input field, the system should validate that the data type and format match expected values.
**Validates: Requirements 18.2**

### Property 61: Validation error messaging
*For any* invalid data submission, the system should display specific error messages indicating which validation rules failed.
**Validates: Requirements 18.3**

### Property 62: Input sanitization
*For any* user input, the system should sanitize the input to prevent SQL injection and cross-site scripting attacks.
**Validates: Requirements 18.4**

### Property 63: Image upload validation
*For any* image file upload, the system should validate file type, size, and dimensions before accepting the upload.
**Validates: Requirements 18.5**

## Error Handling

### Frontend Error Handling

**Network Errors:**
- Display user-friendly error messages when API calls fail
- Implement retry logic for transient failures
- Show offline indicators when network is unavailable
- Cache critical data for offline viewing

**Validation Errors:**
- Display inline validation errors next to form fields
- Highlight invalid fields with visual indicators
- Prevent form submission until all validation passes
- Provide clear guidance on how to fix validation errors

**Authentication Errors:**
- Redirect to login page when session expires
- Display clear messages for invalid credentials
- Handle OAuth failures gracefully with fallback options
- Implement token refresh logic for expired JWT tokens

**Authorization Errors:**
- Display "Access Denied" messages for unauthorized actions
- Redirect non-admin users away from Admin Site
- Hide UI elements for actions user cannot perform

### Backend Error Handling

**Database Errors:**
- Log all database errors with context
- Return generic error messages to clients (don't expose internal details)
- Implement transaction rollback for failed operations
- Use connection pooling and retry logic for connection failures

**Validation Errors:**
- Return 400 Bad Request with detailed validation error messages
- Validate all inputs at API boundary
- Use Data Transfer Objects (DTOs) for request validation
- Implement model validation attributes

**Authentication/Authorization Errors:**
- Return 401 Unauthorized for authentication failures
- Return 403 Forbidden for authorization failures
- Log security-related errors for audit purposes
- Implement rate limiting to prevent brute force attacks

**Business Logic Errors:**
- Return 422 Unprocessable Entity for business rule violations
- Provide clear error messages explaining why operation failed
- Example: "Combo price must be less than sum of item prices"
- Example: "Cannot delete currently logged-in administrator account"

**Server Errors:**
- Return 500 Internal Server Error for unexpected exceptions
- Log full exception details server-side
- Return generic error message to client
- Implement global exception handling middleware

## Testing Strategy

### Unit Testing

**Backend Unit Tests:**
- Test all service layer business logic
- Test validation logic for all DTOs
- Test authentication and authorization logic
- Test data access layer with in-memory database
- Test password hashing and encryption functions
- Mock external dependencies (OAuth, payment gateway)

**Frontend Unit Tests:**
- Test Vue components in isolation
- Test computed properties and methods
- Test form validation logic
- Test state management (Pinia stores)
- Test utility functions and helpers
- Mock API calls using test doubles

### Property-Based Testing

The system will use property-based testing to verify correctness properties across many randomly generated inputs. For C# backend, we will use **FsCheck** library. For Vue.js frontend, we will use **fast-check** library.

**Configuration:**
- Each property-based test should run a minimum of 100 iterations
- Each test must be tagged with a comment referencing the correctness property
- Tag format: `// Feature: fast-food-ordering-system, Property {number}: {property_text}`

**Key Properties to Test:**
- Registration validation (Properties 1-4)
- Search and filtering accuracy (Properties 12-17)
- Authentication and authorization (Properties 18-20, 37-39)
- Cart and order calculations (Properties 25-28)
- Data integrity (Properties 43-44, 48, 53-54)
- Input validation and sanitization (Properties 59-63)

### Integration Testing

**API Integration Tests:**
- Test complete request/response cycles for all endpoints
- Test authentication flow with JWT tokens
- Test Google OAuth integration
- Test database operations with test database
- Test file upload functionality
- Verify proper HTTP status codes and response formats

**End-to-End Tests:**
- Test complete user workflows (registration → login → order → checkout)
- Test admin workflows (login → manage items → manage orders)
- Test cross-browser compatibility
- Test responsive design on different screen sizes
- Use Playwright or Cypress for E2E testing

### Test Data

**Seed Data:**
- Create comprehensive seed data for development and testing
- Include variety of food items across all categories
- Include sample combos with different item combinations
- Include test user accounts (customers and admins)
- Include sample orders in different statuses

**Test Database:**
- Use separate test database for integration tests
- Reset database state between test runs
- Use database transactions for test isolation
- Implement database seeding scripts

## Security Considerations

### Authentication & Authorization

- Use JWT tokens with appropriate expiration times (15 minutes for access, 7 days for refresh)
- Implement refresh token rotation
- Store tokens securely (httpOnly cookies or secure storage)
- Validate tokens on every protected API request
- Implement role-based access control (Customer vs Admin)

### Password Security

- Use BCrypt with appropriate work factor (12+)
- Never store plain text passwords
- Implement password strength requirements (minimum 8 characters, mix of types)
- Implement password reset functionality with time-limited tokens
- Hash passwords before any database operation

### Input Validation & Sanitization

- Validate all inputs on both client and server side
- Sanitize HTML inputs to prevent XSS attacks
- Use parameterized queries to prevent SQL injection
- Validate file uploads (type, size, content)
- Implement rate limiting on all API endpoints

### Data Protection

- Use HTTPS for all communications
- Encrypt sensitive data at rest
- Implement CORS policies appropriately
- Don't expose internal error details to clients
- Log security events for audit purposes

### SQL Injection Prevention

- Use Entity Framework Core parameterized queries
- Never concatenate user input into SQL strings
- Validate and sanitize all user inputs
- Use stored procedures where appropriate
- Implement least privilege database access

## Performance Considerations

### Database Optimization

- Create indexes on frequently queried columns (email, order number, category)
- Implement database connection pooling
- Use eager loading for related entities to avoid N+1 queries
- Implement pagination for large result sets
- Cache frequently accessed data (menu items, categories)

### API Optimization

- Implement response caching for read-heavy endpoints
- Use compression for API responses
- Implement pagination for list endpoints
- Return only necessary data (use DTOs to shape responses)
- Implement API rate limiting

### Frontend Optimization

- Implement lazy loading for routes
- Use virtual scrolling for long lists
- Optimize images (compression, appropriate formats, lazy loading)
- Implement debouncing for search inputs
- Cache API responses where appropriate
- Use Vue's built-in performance features (v-once, v-memo)

### Caching Strategy

- Cache menu items and categories (invalidate on admin updates)
- Cache user profile data (invalidate on updates)
- Implement browser caching for static assets
- Use Redis or in-memory cache for frequently accessed data

## Deployment Architecture

### Development Environment

- Local SQL Server instance or SQL Server Express
- Local ASP.NET Core development server
- Vue.js development server with hot reload
- Separate ports for frontend and backend

### Production Environment

**Frontend:**
- Build Vue.js application for production
- Deploy to CDN or static hosting (Azure Static Web Apps, Netlify, Vercel)
- Configure environment variables for API endpoints

**Backend:**
- Deploy ASP.NET Core API to cloud hosting (Azure App Service, AWS, etc.)
- Configure production database connection strings
- Enable HTTPS and configure SSL certificates
- Set up application logging and monitoring

**Database:**
- Use managed SQL Server instance (Azure SQL Database, AWS RDS)
- Configure automated backups
- Implement database migration strategy
- Set up monitoring and alerts

### CI/CD Pipeline

- Automated testing on pull requests
- Automated builds for frontend and backend
- Automated deployment to staging environment
- Manual approval for production deployment
- Database migration automation
- Rollback procedures

## Monitoring and Logging

### Application Logging

- Log all API requests and responses
- Log authentication attempts and failures
- Log business logic errors and exceptions
- Log performance metrics
- Use structured logging (JSON format)

### Monitoring

- Monitor API response times
- Monitor database query performance
- Monitor error rates and types
- Monitor user activity and patterns
- Set up alerts for critical issues

### Analytics

- Track user registration and login patterns
- Track popular menu items and categories
- Track order completion rates
- Track search queries and results
- Track admin actions for audit purposes
