# Project Setup Documentation

## Completed Setup Tasks

### Backend (ASP.NET Core)

✅ **Project Structure Created**
- ASP.NET Core Web API project with .NET 10.0
- Proper folder structure:
  - `Models/` - Entity models (User, FoodItem, Combo, ComboItem, Order, OrderItem, Invoice)
  - `Data/` - Database context (ApplicationDbContext)
  - `Controllers/` - API controllers (to be implemented)
  - `Services/` - Business logic services (to be implemented)

✅ **NuGet Packages Installed**
- Microsoft.EntityFrameworkCore (9.0.0)
- Microsoft.EntityFrameworkCore.SqlServer (9.0.0)
- Microsoft.EntityFrameworkCore.Tools (9.0.0)
- Microsoft.EntityFrameworkCore.Design (9.0.0)
- BCrypt.Net-Next (4.0.3)
- Microsoft.AspNetCore.Authentication.JwtBearer (9.0.0)
- System.IdentityModel.Tokens.Jwt (8.2.1)

✅ **Database Configuration**
- SQL Server LocalDB connection string configured
- Connection string: `Server=(localdb)\\mssqllocaldb;Database=FastFoodOrderingDB;Trusted_Connection=true;MultipleActiveResultSets=true;TrustServerCertificate=true`
- Entity Framework Core DbContext created with all entity configurations
- Initial migration created: `InitialCreate`

✅ **Entity Models Created**
All entity models match the design document specifications:
- User (with authentication fields)
- FoodItem (with menu details)
- Combo (meal packages)
- ComboItem (junction table)
- Order (with status tracking)
- OrderItem (order line items)
- Invoice (payment records)

✅ **Entity Relationships Configured**
- User → Orders (one-to-many)
- Order → OrderItems (one-to-many)
- Order → Invoice (one-to-one)
- FoodItem → OrderItems (one-to-many)
- FoodItem → ComboItems (one-to-many)
- Combo → ComboItems (one-to-many)
- Combo → OrderItems (one-to-many)

✅ **CORS Configuration**
- Configured to allow requests from Vue.js frontend
- Allowed origins: http://localhost:5173, http://localhost:3000

✅ **Program.cs Configuration**
- Entity Framework Core registered with dependency injection
- CORS policy configured
- Authentication and Authorization middleware added

### Frontend (Vue.js)

✅ **Project Structure Created**
- Vue.js 3 project with TypeScript
- Vite as build tool
- Proper folder structure:
  - `src/components/` - Reusable components
  - `src/views/` - Page components
  - `src/stores/` - Pinia state management
  - `src/services/` - API service layer
  - `src/router/` - Vue Router configuration
  - `src/assets/` - Static assets

✅ **NPM Packages Installed**
- Vue 3 with TypeScript
- Vue Router (for navigation)
- Pinia (for state management)
- Axios (for HTTP requests)
- Tailwind CSS (for styling)
- @tailwindcss/postcss (PostCSS plugin)
- Autoprefixer
- ESLint (for code quality)

✅ **Tailwind CSS Configuration**
- tailwind.config.js created
- postcss.config.js configured
- Tailwind directives added to main.css

✅ **API Service Layer**
- Axios instance configured with base URL
- Request interceptor for JWT token injection
- Response interceptor for error handling
- Automatic redirect to login on 401 errors

✅ **Pinia Stores Created**
- `auth.ts` - Authentication state management
  - Token storage and retrieval
  - User information
  - Authentication status checks
  - Admin role checks
- `cart.ts` - Shopping cart state management
  - Cart items management
  - Quantity updates
  - Total calculations
  - Add/remove/clear operations

## Database Migration Status

✅ Migration created: `InitialCreate`
⚠️ Migration not yet applied to database

To apply the migration and create the database:
```bash
cd ASM
dotnet ef database update
```

## Build Verification

✅ Backend builds successfully
✅ Frontend builds successfully

## Next Steps

The following tasks are ready to be implemented:

1. **Apply Database Migration**
   - Run `dotnet ef database update` to create the database

2. **Create Seed Data** (Task 2.3)
   - Implement seed data for food items
   - Create sample combos
   - Create test user accounts

3. **Implement Authentication** (Task 3)
   - JWT token generation
   - Password hashing with BCrypt
   - Role-based authorization

4. **Build API Endpoints** (Tasks 4-8)
   - Authentication endpoints
   - User management
   - Food item management
   - Combo management
   - Order management

5. **Develop Frontend Components** (Tasks 11-18)
   - Authentication UI
   - Menu browsing
   - Shopping cart
   - Order tracking
   - Admin interface

## Configuration Notes

### Backend Port
The backend API runs on HTTPS. Check the console output or `Properties/launchSettings.json` for the exact port (typically 7000 or 5001).

### Frontend Port
The frontend development server runs on http://localhost:5173 by default.

### API Base URL
Update `frontend/src/services/api.ts` if your backend runs on a different port:
```typescript
baseURL: 'https://localhost:YOUR_PORT/api'
```

## Requirements Validation

This setup satisfies the following requirements:

- ✅ **Requirement 17.1**: Vue.js framework implemented
- ✅ **Requirement 17.2**: Vue.js component architecture ready
- ✅ **Requirement 17.3**: Vue Router configured
- ✅ **Requirement 20.1**: SQL Server database configured
- ✅ **Requirement 20.2**: Entity-Relationship model implemented
- ✅ **Requirement 20.3**: Database tables designed with relationships
- ✅ **Requirement 21.1**: ASP.NET Core REST API project created
- ✅ **Requirement 23.1**: Client-server architecture established

## Troubleshooting

### Database Connection Issues
If you encounter database connection errors:
1. Ensure SQL Server LocalDB is installed (comes with Visual Studio)
2. Check the connection string in `appsettings.json`
3. Try running: `sqllocaldb start mssqllocaldb`

### Frontend Build Issues
If Tailwind CSS causes build errors:
1. Ensure `@tailwindcss/postcss` is installed
2. Check `postcss.config.js` uses `@tailwindcss/postcss`
3. Clear node_modules and reinstall: `npm ci`

### CORS Issues
If the frontend can't connect to the backend:
1. Verify the backend is running
2. Check the CORS policy in `Program.cs` includes your frontend URL
3. Ensure the API base URL in `api.ts` matches your backend URL
