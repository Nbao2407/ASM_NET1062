# Fast Food Ordering System

A full-stack web application for fast food ordering and management built with Vue.js and ASP.NET Core.

## Project Structure

```
ASM/                          # Backend (ASP.NET Core Web API)
├── Controllers/              # API Controllers
├── Data/                     # Database Context
├── Models/                   # Entity Models
├── Services/                 # Business Logic Services
├── Program.cs               # Application Entry Point
└── appsettings.json         # Configuration

frontend/                     # Frontend (Vue.js)
├── src/
│   ├── components/          # Reusable Vue Components
│   ├── views/               # Page Components
│   ├── stores/              # Pinia State Management
│   ├── services/            # API Service Layer
│   ├── router/              # Vue Router Configuration
│   └── assets/              # Static Assets
└── package.json
```

## Technology Stack

### Backend
- ASP.NET Core 10.0
- Entity Framework Core 9.0
- SQL Server (LocalDB)
- JWT Authentication
- BCrypt for Password Hashing

### Frontend
- Vue.js 3 with TypeScript
- Vue Router for Navigation
- Pinia for State Management
- Axios for HTTP Requests
- Tailwind CSS for Styling

## Getting Started

### Prerequisites
- .NET 10.0 SDK
- Node.js 18+ and npm
- SQL Server LocalDB (included with Visual Studio)

### Backend Setup

1. Navigate to the ASM directory:
   ```bash
   cd ASM
   ```

2. Restore NuGet packages:
   ```bash
   dotnet restore
   ```

3. Update the database:
   ```bash
   dotnet ef database update
   ```

4. Run the backend:
   ```bash
   dotnet run
   ```

The API will be available at `https://localhost:7000` (or the port shown in the console).

### Frontend Setup

1. Navigate to the frontend directory:
   ```bash
   cd frontend
   ```

2. Install dependencies:
   ```bash
   npm install
   ```

3. Update the API base URL in `src/services/api.ts` if needed.

4. Run the development server:
   ```bash
   npm run dev
   ```

The frontend will be available at `http://localhost:5173`.

## Database

The application uses SQL Server LocalDB with the following connection string:
```
Server=(localdb)\\mssqllocaldb;Database=FastFoodOrderingDB;Trusted_Connection=true;MultipleActiveResultSets=true;TrustServerCertificate=true
```

### Database Schema

The database includes the following tables:
- **Users**: Customer and Admin accounts
- **FoodItems**: Individual food products
- **Combos**: Bundled meal packages
- **ComboItems**: Junction table for combo-food item relationships
- **Orders**: Customer orders
- **OrderItems**: Items in each order
- **Invoices**: Order invoices and payment records

## Entity Framework Migrations

To create a new migration:
```bash
cd ASM
dotnet ef migrations add MigrationName
```

To update the database:
```bash
dotnet ef database update
```

To remove the last migration:
```bash
dotnet ef migrations remove
```

## API Endpoints

The API follows RESTful conventions. Key endpoint groups include:

- `/api/auth` - Authentication (register, login, OAuth)
- `/api/customers` - Customer profile management
- `/api/items` - Food item operations
- `/api/combos` - Combo package operations
- `/api/orders` - Order management
- `/api/admin` - Admin-only operations

## Development

### Running Both Projects

You can run both the backend and frontend simultaneously:

1. Terminal 1 (Backend):
   ```bash
   cd ASM
   dotnet run
   ```

2. Terminal 2 (Frontend):
   ```bash
   cd frontend
   npm run dev
   ```

### CORS Configuration

The backend is configured to allow requests from:
- `http://localhost:5173` (Vite default)
- `http://localhost:3000` (Alternative port)

Update the CORS policy in `Program.cs` if you use a different port.

## Next Steps

1. Implement authentication endpoints
2. Create entity services and repositories
3. Build API controllers
4. Develop Vue.js components and views
5. Implement state management
6. Add validation and error handling
7. Create seed data for testing

## License

This project is for educational purposes.
