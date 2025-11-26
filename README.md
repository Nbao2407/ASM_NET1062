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
