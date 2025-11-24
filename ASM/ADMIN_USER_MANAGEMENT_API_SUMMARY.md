# Admin User Management API Summary

## Overview
This document summarizes the implementation of the Admin User Management API endpoints for the Fast Food Ordering System. These endpoints allow administrators to manage user accounts in the system.

## Implemented Endpoints

### 1. Get All Users
- **Endpoint**: `GET /api/admin/users`
- **Authorization**: Admin only
- **Description**: Retrieves a list of all users in the system
- **Response**: Array of UserResponse objects ordered by creation date (newest first)
- **Status Codes**:
  - 200 OK: Successfully retrieved users
  - 401 Unauthorized: Invalid or missing token
  - 403 Forbidden: User is not an admin

### 2. Get User by ID
- **Endpoint**: `GET /api/admin/users/{id}`
- **Authorization**: Admin only
- **Description**: Retrieves detailed information about a specific user
- **Parameters**:
  - `id` (path): User ID
- **Response**: UserResponse object
- **Status Codes**:
  - 200 OK: Successfully retrieved user
  - 401 Unauthorized: Invalid or missing token
  - 403 Forbidden: User is not an admin
  - 404 Not Found: User not found

### 3. Create User
- **Endpoint**: `POST /api/admin/users`
- **Authorization**: Admin only
- **Description**: Creates a new user account (Customer or Admin)
- **Request Body**: CreateUserRequest
  ```json
  {
    "email": "user@example.com",
    "password": "SecurePassword123!",
    "fullName": "John Doe",
    "phoneNumber": "1234567890",
    "address": "123 Main St",
    "dateOfBirth": "1990-01-01",
    "role": "Customer"
  }
  ```
- **Validation**:
  - Email must be valid and unique
  - Password must be at least 8 characters
  - All required fields must be provided
  - Role must be either "Customer" or "Admin"
- **Response**: UserResponse object with 201 Created status
- **Status Codes**:
  - 201 Created: User successfully created
  - 400 Bad Request: Validation error or duplicate email
  - 401 Unauthorized: Invalid or missing token
  - 403 Forbidden: User is not an admin

### 4. Update User
- **Endpoint**: `PUT /api/admin/users/{id}`
- **Authorization**: Admin only
- **Description**: Updates an existing user's information
- **Parameters**:
  - `id` (path): User ID
- **Request Body**: UpdateUserRequest
  ```json
  {
    "email": "updated@example.com",
    "fullName": "John Doe Updated",
    "phoneNumber": "0987654321",
    "address": "456 New St",
    "dateOfBirth": "1990-01-01",
    "role": "Customer",
    "isActive": true
  }
  ```
- **Validation**:
  - Email must be valid and unique (if changed)
  - All required fields must be provided
  - Role must be either "Customer" or "Admin"
- **Response**: UserResponse object
- **Status Codes**:
  - 200 OK: User successfully updated
  - 400 Bad Request: Validation error or duplicate email
  - 401 Unauthorized: Invalid or missing token
  - 403 Forbidden: User is not an admin
  - 404 Not Found: User not found

### 5. Delete User
- **Endpoint**: `DELETE /api/admin/users/{id}`
- **Authorization**: Admin only
- **Description**: Deletes a user account from the system
- **Parameters**:
  - `id` (path): User ID
- **Business Rules**:
  - **Cannot delete currently logged-in admin** (prevents self-deletion)
- **Response**: Success message
- **Status Codes**:
  - 200 OK: User successfully deleted
  - 400 Bad Request: Attempting to delete own account
  - 401 Unauthorized: Invalid or missing token
  - 403 Forbidden: User is not an admin
  - 404 Not Found: User not found

## Data Transfer Objects (DTOs)

### UserResponse
```csharp
public class UserResponse
{
    public int UserId { get; set; }
    public string Email { get; set; }
    public string FullName { get; set; }
    public string PhoneNumber { get; set; }
    public string? Address { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string Role { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
```

### CreateUserRequest
```csharp
public class CreateUserRequest
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    
    [Required]
    [MinLength(8)]
    public string Password { get; set; }
    
    [Required]
    public string FullName { get; set; }
    
    [Required]
    [Phone]
    public string PhoneNumber { get; set; }
    
    public string? Address { get; set; }
    public DateTime? DateOfBirth { get; set; }
    
    [Required]
    [RegularExpression("^(Customer|Admin)$")]
    public string Role { get; set; }
}
```

### UpdateUserRequest
```csharp
public class UpdateUserRequest
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    
    [Required]
    public string FullName { get; set; }
    
    [Required]
    [Phone]
    public string PhoneNumber { get; set; }
    
    public string? Address { get; set; }
    public DateTime? DateOfBirth { get; set; }
    
    [Required]
    [RegularExpression("^(Customer|Admin)$")]
    public string Role { get; set; }
    
    public bool IsActive { get; set; }
}
```

## Security Features

1. **Authorization**: All endpoints require admin authentication via JWT token
2. **Password Hashing**: Passwords are hashed using BCrypt before storage
3. **Self-Deletion Prevention**: Admins cannot delete their own account while logged in
4. **Email Uniqueness**: System prevents duplicate email addresses
5. **Input Validation**: All inputs are validated using data annotations
6. **Audit Logging**: All user management actions are logged

## Requirements Validation

This implementation satisfies the following requirements:

- **Requirement 13.1**: Display list of all customer accounts ✓
- **Requirement 13.2**: Create new user accounts with validation ✓
- **Requirement 13.3**: Update user accounts and maintain history ✓
- **Requirement 13.4**: Delete user accounts ✓
- **Requirement 13.5**: Prevent deletion of currently logged-in admin ✓

## Testing

Test endpoints are available in `API_TESTS.http` file. To test:

1. Login as admin to get admin token
2. Replace `YOUR_ADMIN_TOKEN_HERE` with actual token
3. Execute the test requests

## Error Handling

All endpoints implement proper error handling:
- Validation errors return 400 Bad Request with detailed messages
- Authentication errors return 401 Unauthorized
- Authorization errors return 403 Forbidden
- Not found errors return 404 Not Found
- Server errors return 500 Internal Server Error with logged details

## Implementation Notes

- Users are permanently deleted (hard delete) rather than soft deleted
- The system maintains audit trail through logging
- Email addresses are stored in lowercase for consistency
- Timestamps are stored in UTC
- The IsActive flag allows deactivating users without deletion
