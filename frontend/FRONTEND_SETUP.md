# Frontend Setup Summary

## Project Structure

The Vue.js frontend has been fully configured with the following structure:

```
frontend/
├── src/
│   ├── assets/          # Static assets (CSS, images)
│   ├── components/      # Reusable Vue components
│   ├── composables/     # Vue composables for shared logic
│   │   └── useApi.ts    # API call handler with loading/error states
│   ├── router/          # Vue Router configuration
│   ├── services/        # API service layer
│   │   ├── api.ts       # Axios instance with interceptors
│   │   ├── authService.ts
│   │   ├── customerService.ts
│   │   ├── foodItemService.ts
│   │   ├── comboService.ts
│   │   ├── orderService.ts
│   │   ├── adminService.ts
│   │   └── index.ts     # Centralized exports
│   ├── stores/          # Pinia state management
│   │   ├── auth.ts      # Authentication store
│   │   └── cart.ts      # Shopping cart store
│   ├── types/           # TypeScript type definitions
│   │   └── index.ts
│   ├── utils/           # Utility functions
│   │   ├── validators.ts
│   │   └── formatters.ts
│   └── views/           # Page components
├── .env.development     # Development environment variables
├── .env.production      # Production environment variables
└── package.json
```

## Installed Dependencies

### Core Dependencies
- **Vue 3** - Progressive JavaScript framework
- **Vue Router** - Official router for Vue.js
- **Pinia** - State management library
- **Axios** - HTTP client for API requests
- **Tailwind CSS** - Utility-first CSS framework

### Development Dependencies
- **Vite** - Build tool and dev server
- **TypeScript** - Type safety
- **ESLint** - Code linting
- **Vue TSC** - TypeScript compiler for Vue

## API Configuration

### Base URL
- Development: `https://localhost:7000/api`
- Production: `/api` (relative path)

### Interceptors

**Request Interceptor:**
- Automatically adds JWT token from localStorage to Authorization header
- Configurable timeout (30 seconds)

**Response Interceptor:**
- Handles 401 (Unauthorized) - clears token and redirects to login
- Handles 403 (Forbidden) - logs access denied
- Handles 404 (Not Found) - logs resource not found
- Handles 422 (Validation Error) - logs validation errors
- Handles 500 (Server Error) - logs server errors
- Handles network errors gracefully

## Service Layer

All API endpoints are organized into service modules:

1. **authService** - Registration, login, Google OAuth
2. **customerService** - Profile management, password changes
3. **foodItemService** - Browse, search, CRUD operations
4. **comboService** - Combo management
5. **orderService** - Order placement, tracking, invoices
6. **adminService** - User management

## State Management

### Auth Store
- Token management
- User information
- Authentication status
- Role-based checks (isAdmin)

### Cart Store
- Cart items management
- Quantity updates
- Total calculations
- Clear cart functionality

## Utilities

### Validators
- Email validation
- Password validation
- Phone number validation
- Required field validation

### Formatters
- Currency formatting
- Date formatting
- DateTime formatting

## Composables

### useApi
- Handles API calls with loading states
- Error handling with detailed error messages
- Validation error support
- Reset functionality

## Type Safety

All services include TypeScript interfaces for:
- Request payloads
- Response data
- Common types (UserRole, OrderStatus, etc.)

## Next Steps

The frontend structure is now ready for implementing:
- Authentication UI components (Task 12)
- Customer-facing menu UI (Task 13)
- Shopping cart and checkout (Task 14)
- Order tracking UI (Task 15)
- Admin site UI (Task 16)

## Running the Project

```bash
# Development
npm run dev

# Type checking
npm run type-check

# Build for production
npm run build

# Preview production build
npm run preview

# Lint code
npm run lint
```
