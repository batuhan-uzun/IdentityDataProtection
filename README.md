# Identity Data Protection with JWT

This project demonstrates a secure implementation of user registration and login using **JWT Authentication** and **IDataProtection** for password encryption. It provides a robust API for managing user authentication and authorization in an ASP.NET Core Web API application.

## Overview

### Features
- **User Registration**:
  - Encrypts user passwords using IDataProtection.
  - Stores user information securely in the database.

- **User Login**:
  - Verifies user credentials.
  - Generates JWT tokens for authenticated users.
  - Includes user roles in the token for role-based authorization.

- **JWT Authentication**:
  - Implements role-based access control using JWT.
  - Configures Swagger to support JWT token authentication.

## Key Components

### 1. Entities
#### `UserEntity`
Represents the user in the database.
```csharp
public class UserEntity
{
    public UserEntity()
    {
        CreatedDate = DateTime.Now;
    }
    public int Id { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public DateTime CreatedDate { get; set; }
    public bool IsDeleted { get; set; }
    public UserRole UserRole { get; set; }
}
```

### 2. Controllers
#### `AuthController`
Handles user registration and login requests.
```csharp
[HttpPost("register")]
public async Task<IActionResult> Register(RegisterRequest request)
{
    var addUserDto = new AddUserDto { Email = request.Email, Password = request.Password };
    var result = await _userService.AddUser(addUserDto);

    if (result.IsSucceed)
        return Ok(result.Message);
    return BadRequest();
}

[HttpPost("login")]
public async Task<IActionResult> Login(LoginRequest request)
{
    var loginUserDto = new LoginUserDto { Email = request.Email, Password = request.Password };
    var result = await _userService.LoginUser(loginUserDto);

    if (!result.IsSucceed)
        return BadRequest(result.Message);

    var user = result.Data;

    var config = HttpContext.RequestServices.GetRequiredService<IConfiguration>();

    var token = JwtHelper.GenerateJwt(new JwtDto
    {
        Id = user.Id,
        Email = user.Email,
        UserRole = user.UserRole,
        SecretKey = config["Jwt:SecretKey"],
        Issuer = config["Jwt:Issuer"],
        Audience = config["Jwt:Audience"],
        ExpireMinutes = int.Parse(config["Jwt:ExpireMinutes"])
    });

    return Ok(new LoginResponse { Message = "Login successful", Token = token });
}
```

### 3. Services
- **IDataProtection**: Encrypts and decrypts sensitive data (e.g., passwords).
- **IUserService**: Manages user-related operations.

### 4. JWT Helper
Generates JWT tokens for authenticated users, including role-based claims.

### 5. Database Context
#### `DataProtectionJwtDbContext`
Handles database operations with `UserEntity`.

## Setup Instructions

### Prerequisites
- .NET 6 SDK or higher
- SQL Server

### Steps
1. Clone the repository:
   ```bash
   git clone https://github.com/your-username/identity-data-protection.git
   ```

2. Configure the database connection in `appsettings.json`:
   ```json
   "ConnectionStrings": {
       "default": "Your SQL Server connection string"
   },
   "Jwt": {
       "SecretKey": "YourSecretKey",
       "Issuer": "YourIssuer",
       "Audience": "YourAudience",
       "ExpireMinutes": "60"
   }
   ```

3. Apply database migrations:
   ```bash
   dotnet ef migrations add InitialCreate
   dotnet ef database update
   ```

4. Run the application:
   ```bash
   dotnet run
   ```

5. Test the API using Swagger or Postman.

## API Endpoints

### AuthController
- **POST /api/auth/register**: Registers a new user.
- **POST /api/auth/login**: Authenticates a user and returns a JWT token.

## Technologies Used
- ASP.NET Core Web API
- Entity Framework Core
- IDataProtection for secure password encryption
- JWT for authentication and authorization
- Swagger for API documentation

## Author

Created by **Batuhan Uzun**
