# Inspection System API

A comprehensive inspection management system built with .NET 8, Entity Framework Core, and Clean Architecture principles. The system manages inspectors, entities to inspect, inspection visits, and violations with role-based authentication.

## 🏗️ Architecture

- **Clean Architecture** with separation of concerns
- **CQRS Pattern** using MediatR
- **Repository Pattern** with Unit of Work
- **JWT Authentication** with role-based authorization
- **Entity Framework Core** with SQL Server
- **AutoMapper** for object mapping
- **Serilog** for structured logging
- **Swagger/OpenAPI** for API documentation

## 🚀 Features

- **Authentication & Authorization**
  - JWT token-based authentication
  - Role-based access control (Admin, Inspector)
  - User registration and login

- **Entity Management**
  - CRUD operations for entities to inspect
  - Category-based organization
  - Active/inactive status management

- **Inspector Management**
  - Inspector profile management
  - User account integration

- **Inspection Visits**
  - Schedule and manage inspection visits
  - Track inspection status and scores
  - Add notes and violations

- **Violation Tracking**
  - Record violations during inspections
  - Severity levels and categorization

## 📋 Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) (LocalDB, Express, or Full)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) or [VS Code](https://code.visualstudio.com/)
- [Git](https://git-scm.com/)

## 🛠️ Setup Instructions

### 1. Clone the Repository

```bash
git clone <repository-url>
cd Inspection
```

### 2. Restore Dependencies

```bash
dotnet restore
```

### 3. Database Setup

#### Update Connection String
Edit `Inspection/appsettings.json` and update the connection string:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=InspectionDB;Trusted_Connection=true;MultipleActiveResultSets=true;TrustServerCertificate=true;Encrypt=false"
  }
}
```

#### Run Database Migrations

```bash
# Navigate to the API project
cd Inspection

# Add migration (if needed)
dotnet ef migrations add InitialCreate --project ../Inspection.DataAccessLayer

# Update database
dotnet ef database update --project ../Inspection.DataAccessLayer
```

### 4. Configure JWT Settings (Optional)

Update JWT settings in `appsettings.json`:

```json
{
  "JwtSettings": {
    "SecretKey": "YourSecretKeyHere123456789!@#$%^&*()",
    "Issuer": "InspectionAPI",
    "Audience": "InspectionClient",
    "ExpiryInDays": 7
  }
}
```

## 🏃‍♂️ Running the Application

### Development Mode

```bash
# Navigate to API project
cd Inspection

# Run the application
dotnet run
```

The API will be available at:
- HTTP: `http://localhost:5124`
- HTTPS: `https://localhost:7265`
- Swagger UI: `https://localhost:7265/swagger`

### Production Mode

```bash
dotnet run --configuration Release
```

### Using Visual Studio

1. Open `Inspection.sln`
2. Set `Inspection.API` as startup project
3. Press F5 or click "Start Debugging"

## 🧪 Testing

### Manual Testing with HTTP Files

The project includes HTTP test files for manual testing:

1. **Authentication Tests**: `Inspection/test-auth.http`
2. **General API Tests**: `Inspection/Inspection.http`

#### Using VS Code REST Client

1. Install the "REST Client" extension
2. Open the `.http` files
3. Click "Send Request" above each request

#### Using Visual Studio

1. Open the `.http` files
2. Click the "Send Request" button

### Sample Test Workflow

1. **Get Available Roles**
   ```http
   GET https://localhost:7265/api/auth/roles
   ```

2. **Register Admin User**
   ```http
   POST https://localhost:7265/api/auth/register
   Content-Type: application/json

   {
     "fullName": "System Admin",
     "email": "admin@inspection.com",
     "phone": "1234567890",
     "roleId": "<admin-role-id>",
     "password": "Admin123!"
   }
   ```

3. **Login**
   ```http
   POST https://localhost:7265/api/auth/login
   Content-Type: application/json

   {
     "email": "admin@inspection.com",
     "password": "Admin123!"
   }
   ```

4. **Use JWT Token**
   Copy the token from login response and use in subsequent requests:
   ```http
   GET https://localhost:7265/api/inspectors
   Authorization: Bearer <your-jwt-token>
   ```

### Unit Testing

```bash
# Run all tests
dotnet test

# Run tests with coverage
dotnet test --collect:"XPlat Code Coverage"
```

## 📁 Project Structure

```
Inspection/
├── Inspection/                     # API Layer
│   ├── Controllers/               # API Controllers
│   ├── Middleware/               # Custom Middleware
│   ├── Properties/               # Launch Settings
│   └── Program.cs               # Application Entry Point
├── Inspection.Application/        # Application Layer
│   ├── Abstractions/            # Interfaces
│   ├── Dto/                     # Data Transfer Objects
│   ├── Features/                # CQRS Commands/Queries
│   ├── Mappings/                # AutoMapper Profiles
│   ├── Services/                # Application Services
│   └── Validators/              # FluentValidation Rules
├── Inspection.Domain/             # Domain Layer
│   ├── Constants/               # Domain Constants
│   ├── Entities/                # Domain Entities
│   └── Enum/                    # Enumerations
└── Inspection.DataAccessLayer/    # Infrastructure Layer
    ├── Configuration/           # Entity Configurations
    ├── Context/                 # DbContext
    └── Repository/              # Repository Implementations
```

## 🔐 Authentication & Authorization

### Roles

- **Admin**: Full system access, can manage all entities
- **Inspector**: Limited access, can manage assigned inspections

### JWT Token

The API uses JWT tokens for authentication. Include the token in the Authorization header:

```
Authorization: Bearer <your-jwt-token>
```

### Password Requirements

- Minimum 6 characters
- At least one uppercase letter
- At least one lowercase letter
- At least one digit
- At least one special character

## 📊 API Endpoints

### Authentication
- `POST /api/auth/login` - User login
- `POST /api/auth/register` - User registration
- `GET /api/auth/roles` - Get available roles

### Entities
- `GET /api/entities` - Get all entities
- `GET /api/entities/{id}` - Get entity by ID
- `POST /api/entities` - Create new entity (Admin only)
- `PUT /api/entities/{id}` - Update entity (Admin only)
- `DELETE /api/entities/{id}` - Delete entity (Admin only)
- `GET /api/entities/categories` - Get entity categories

### Inspectors
- `GET /api/inspectors` - Get all inspectors (Admin only)
- `GET /api/inspectors/{id}` - Get inspector by ID
- `POST /api/inspectors` - Create inspector (Admin only)
- `PUT /api/inspectors/{id}` - Update inspector (Admin only)
- `DELETE /api/inspectors/{id}` - Delete inspector (Admin only)

### Inspection Visits
- `GET /api/inspectionvisits` - Get inspection visits
- `GET /api/inspectionvisits/{id}` - Get visit by ID
- `POST /api/inspectionvisits` - Create visit (Admin only)
- `PUT /api/inspectionvisits/{id}` - Update visit
- `POST /api/inspectionvisits/{id}/complete` - Complete visit
- `DELETE /api/inspectionvisits/{id}` - Delete visit (Admin only)

## ⚙️ Configuration

### Environment Variables

You can override configuration using environment variables:

```bash
# Database
export ConnectionStrings__DefaultConnection="Server=myserver;Database=InspectionDB;..."

# JWT Settings
export JwtSettings__SecretKey="YourSecretKey"
export JwtSettings__Issuer="YourIssuer"
export JwtSettings__Audience="YourAudience"

# Logging
export Serilog__MinimumLevel__Default="Information"
```

### CORS Configuration

The API is configured to allow requests from `http://localhost:4200` (Angular development server). To modify CORS settings, update `Program.cs`:

```csharp
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp", policy =>
    {
        policy.WithOrigins("http://localhost:4200", "https://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});
```

## 🐛 Troubleshooting

### Common Issues

1. **Database Connection Issues**
   - Ensure SQL Server is running
   - Verify connection string in `appsettings.json`
   - Check if database exists and migrations are applied

2. **JWT Token Issues**
   - Ensure JWT secret key is properly configured
   - Check token expiration
   - Verify token format in Authorization header

3. **CORS Issues**
   - Check if your client origin is allowed in CORS policy
   - Ensure preflight requests are handled correctly

4. **Migration Issues**
   ```bash
   # Reset migrations
   dotnet ef database drop --project ../Inspection.DataAccessLayer
   dotnet ef migrations remove --project ../Inspection.DataAccessLayer
   dotnet ef migrations add InitialCreate --project ../Inspection.DataAccessLayer
   dotnet ef database update --project ../Inspection.DataAccessLayer
   ```

### Logs

Application logs are written to:
- Console (Development)
- Files in `logs/` directory
- Structured logging with Serilog

## 🚀 Deployment

### Docker (Optional)

Create a `Dockerfile` in the root directory:

```dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Inspection/Inspection.API.csproj", "Inspection/"]
COPY ["Inspection.Application/Inspection.Application.csproj", "Inspection.Application/"]
COPY ["Inspection.Domain/Inspection.Domain.csproj", "Inspection.Domain/"]
COPY ["Inspection.DataAccessLayer/Inspection.DataAccessLayer.csproj", "Inspection.DataAccessLayer/"]
RUN dotnet restore "Inspection/Inspection.API.csproj"
COPY . .
WORKDIR "/src/Inspection"
RUN dotnet build "Inspection.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Inspection.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Inspection.API.dll"]
```

### Production Checklist

- [ ] Update connection strings for production database
- [ ] Configure proper JWT secret keys
- [ ] Set up HTTPS certificates
- [ ] Configure logging for production
- [ ] Set up health checks
- [ ] Configure CORS for production domains
- [ ] Set up monitoring and alerting

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'Add some amazing feature'`)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

## 📝 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 📞 Support

For support and questions:
- Create an issue in the repository
- Contact the development team
- Check the documentation and troubleshooting guide

---

**Happy Coding! 🎉**