# Customer Order Management API

A production-oriented RESTful Web API for managing **Customers** and **Orders**, built with **ASP.NET Web API** and **Entity Framework 6**.

The project demonstrates clean backend architecture, authentication and authorization, CRUD operations, many-to-many relationships, validation, centralized exception handling, structured logging, pagination, and JWT-based security.

---

## 🚀 Features

* Customer CRUD operations
* Order CRUD operations
* Many-to-Many relationship between Customers and Orders
* JWT Authentication
* Role-Based Authorization

  * `user`
  * `admin`
* Password hashing using ASP.NET Identity
* FluentValidation
* Pagination
* AutoMapper
* Repository Pattern
* Unit of Work Pattern
* Dependency Injection
* Centralized Exception Handling
* Custom Application Exceptions
* Standardized API Responses
* Structured Logging with Serilog
* Request/Response logging
* Database error handling
* Audit fields

  * `CreatedAt`
  * `CreatedBy`
  * `UpdatedAt`
  * `UpdatedBy`

---

## 🏗️ Architecture

The solution follows a layered architecture:

```text
CustomerOrderManagement
│
├── CustomerOrderManagement.Domain
│   ├── Entities
│   └── Common
│
├── CustomerOrderManagement.Application
│   ├── DTOs
│   ├── Authentication
│   ├── Interfaces
│   ├── Mapping
│   ├── Services
│   ├── Validators
│   └── Exceptions
│
├── CustomerOrderManagement.Infrastructure
│   ├── Data
│   │   ├── Contexts
│   │   └── Repositories
│   ├── Identity
│   ├── Migrations
│   └── Security
│
└── CustomerOrderManagement.API
    ├── Controllers
    ├── ExceptionHandling
    ├── Logging
    └── Helpers
```

### Layers

**Domain**

Contains the core entities and common domain abstractions.

**Application**

Contains business logic, DTOs, services, validators, exceptions, and application interfaces.

**Infrastructure**

Contains Entity Framework, repositories, Unit of Work, ASP.NET Identity, and JWT implementation.

**API**

Contains HTTP controllers, authentication endpoints, exception handling, request/response logging, and API configuration.

---

## 🛠️ Technologies

* C#
* ASP.NET Web API 2
* .NET Framework
* Entity Framework 6
* ASP.NET Identity
* JWT
* SQL Server
* FluentValidation
* AutoMapper
* Serilog
* Dependency Injection
* REST API

---

# 🔐 Authentication

The API uses **JWT Bearer Authentication**.

Users can register and login through the authentication endpoints.

After successful login, the API returns a JWT token.

Example:

```json
{
    "Success": true,
    "Message": "Login successful.",
    "Data": {
        "Token": "YOUR_JWT_TOKEN",
        "ExpiresAt": "2026-08-17T13:24:34Z",
        "UserId": "USER_ID",
        "Email": "user@example.com",
        "Role": "user"
    },
    "Errors": [],
    "ErrorCode": null
}
```

Use the token in protected endpoints:

```http
Authorization: Bearer YOUR_JWT_TOKEN
```

---

# 👥 Roles

The API supports two roles:

| Role    | Permissions                                          |
| ------- | ---------------------------------------------------- |
| `user`  | Read Customers and Orders                            |
| `admin` | Read, Create, Update and Delete Customers and Orders |

### Authorization Examples

```csharp
[Authorize(Roles = "user,admin")]
```

and:

```csharp
[Authorize(Roles = "admin")]
```

---

# 📌 API Endpoints

## Authentication

### Register

```http
POST /api/auth/register
```

### Login

```http
POST /api/auth/login
```

---

# 👤 Customers

## Get All Customers

```http
GET /api/Customers?pageNumber=1&pageSize=10
```

Roles:

```text
user
admin
```

---

## Get Customer By ID

```http
GET /api/Customers/{id}
```

Roles:

```text
user
admin
```

---

## Create Customer

```http
POST /api/Customers
```

Role:

```text
admin
```

Example:

```json
{
    "FirstName": "Mohamed",
    "LastName": "Hany",
    "Address": "Cairo, Egypt",
    "Email": "mohamed.hany@gmail.com",
    "Phone": "+201012345678"
}
```

---

## Update Customer

```http
PUT /api/Customers/{id}
```

Role:

```text
admin
```

---

## Delete Customer

```http
DELETE /api/Customers/{id}
```

Role:

```text
admin
```

---

# 📦 Orders

## Get All Orders

```http
GET /api/orders?pageNumber=1&pageSize=10
```

Roles:

```text
user
admin
```

---

## Get Order By ID

```http
GET /api/orders/{id}
```

Roles:

```text
user
admin
```

---

## Create Order

```http
POST /api/orders
```

Role:

```text
admin
```

An Order can be associated with multiple Customers through the many-to-many relationship.

---

## Update Order

```http
PUT /api/orders/{id}
```

Role:

```text
admin
```

---

## Delete Order

```http
DELETE /api/orders/{id}
```

Role:

```text
admin
```

---

# 🔗 Database Relationship

Customers and Orders have a **Many-to-Many** relationship.

```text
Customer
    │
    │
    │
CustomerOrder
    │
    │
    │
Order
```

The junction table `CustomerOrder` stores the relationship:

```text
CustomerOrder
-------------
CustomerId
OrderId
```

This allows:

* One Customer to have multiple Orders
* One Order to be associated with multiple Customers

---

# ✅ Validation

The project uses **FluentValidation** for application-level validation.

Examples include:

* Required fields
* Email format validation
* Unique customer email
* Unique customer phone
* Egyptian mobile number validation
* Order validation
* Customer relationship validation

Example validation response:

```json
{
    "Success": false,
    "Message": "Validation failed.",
    "Data": null,
    "Errors": [
        "Phone must be a valid Egyptian mobile number."
    ],
    "ErrorCode": "VALIDATION_ERROR"
}
```

---

# ⚠️ Exception Handling

The API uses a centralized exception handling mechanism instead of handling exceptions individually inside every controller.

Supported exceptions include:

* `ValidationException`
* `NotFoundException`
* `BusinessException`
* `UnauthorizedException`
* Database exceptions
* Unexpected exceptions

Example:

```json
{
    "Success": false,
    "Message": "Customer not found.",
    "Data": null,
    "Errors": [],
    "ErrorCode": "CUSTOMER_NOT_FOUND"
}
```

---

# 📊 Standardized Response

API responses follow a common structure:

```json
{
    "Success": true,
    "Message": "Operation completed successfully.",
    "Data": {},
    "Errors": [],
    "ErrorCode": null
}
```

This provides a consistent response format across the API.

---

# 📝 Structured Logging

The API uses **Serilog** for structured logging.

The logging system records information such as:

* Timestamp
* HTTP Method
* Endpoint
* Request
* Response
* Status Code
* Duration
* User
* Exception

Example:

```text
[INF] HTTP Request |
Method: GET |
Endpoint: /api/Customers/1 |
User: user@example.com
```

For failed requests:

```text
[WRN] HTTP Exception |
Method: GET |
Endpoint: /api/Customers/999 |
Duration: 120ms |
User: user@example.com

CustomerOrderManagement.Application.Exceptions.NotFoundException:
Customer not found.
```

### Log Levels

| Event                 | Level       |
| --------------------- | ----------- |
| Successful requests   | Information |
| Validation errors     | Warning     |
| Not Found             | Warning     |
| Unauthorized          | Warning     |
| Business exceptions   | Warning     |
| Database exceptions   | Error       |
| Unexpected exceptions | Error       |

Logs are written to files for easier monitoring and troubleshooting.

---

# 📄 Pagination

Customer and Order listing endpoints support pagination.

Example:

```http
GET /api/Customers?pageNumber=1&pageSize=10
```

Example response:

```json
{
    "PageNumber": 1,
    "PageSize": 10,
    "TotalCount": 25,
    "TotalPages": 3,
    "HasPreviousPage": false,
    "HasNextPage": true,
    "Items": []
}
```

---

# 🧩 Design Patterns

The project applies several backend design patterns:

### Repository Pattern

Provides an abstraction over data access.

### Unit of Work

Coordinates database operations and commits changes through a single `SaveChanges()` operation.

### Dependency Injection

Dependencies are injected into controllers and services instead of being created manually.

### DTO Pattern

DTOs are used to control data exchanged through the API.

### Service Layer

Business logic is kept outside controllers.

---

# 🔒 Audit Fields

Entities inherit common audit properties:

```csharp
public int Id { get; set; }

public DateTime CreatedAt { get; set; }

public string CreatedBy { get; set; }

public DateTime? UpdatedAt { get; set; }

public string UpdatedBy { get; set; }
```

These fields provide basic tracking of entity creation and modification.

---

# ⚙️ Getting Started

## 1. Clone the Repository

```bash
git clone https://github.com/mostafaroute7-debug/CustomerOrderManagement.git
```

```bash
cd CustomerOrderManagement
```

---

## 2. Configure the Database

Update the connection string in the API configuration:

```xml
<connectionStrings>
    <add
        name="DefaultConnection"
        connectionString="YOUR_CONNECTION_STRING"
        providerName="System.Data.SqlClient" />
</connectionStrings>
```

Use your local SQL Server configuration.

---

## 3. Create the Database

Run the required Entity Framework migrations or database initialization configured in the project.

---

## 4. Configure JWT

Configure the JWT settings required by the application, including:

* Secret Key
* Issuer
* Audience
* Expiration

**Do not commit production secrets or JWT keys to GitHub.**

Use configuration/environment-specific settings instead.

---

## 5. Run the API

Open the solution in Visual Studio and run the API project.

The API will be available at the configured application URL.

---

# 🧪 Testing with Postman

The API can be tested using Postman.

Recommended testing order:

```text
1. Register
2. Login
3. Copy JWT Token
4. Test GET Customers
5. Test Customer CRUD
6. Test GET Orders
7. Test Create Order
8. Test Update Order
9. Test Delete Order
10. Test Authentication & Authorization
11. Test Validation
12. Test NotFound scenarios
```

For protected endpoints, use:

```text
Authorization
    ↓
Bearer Token
    ↓
YOUR_JWT_TOKEN
```

---

# 🧪 Authorization Test Matrix

| Endpoint        | User | Admin |
| --------------- | :--: | :---: |
| GET Customers   |   ✅  |   ✅   |
| GET Customer    |   ✅  |   ✅   |
| POST Customer   |   ❌  |   ✅   |
| PUT Customer    |   ❌  |   ✅   |
| DELETE Customer |   ❌  |   ✅   |
| GET Orders      |   ✅  |   ✅   |
| GET Order       |   ✅  |   ✅   |
| POST Order      |   ❌  |   ✅   |
| PUT Order       |   ❌  |   ✅   |
| DELETE Order    |   ❌  |   ✅   |

---

# 🎯 Project Goals

The main goal of this project is to demonstrate how to build a maintainable and production-oriented ASP.NET Web API with:

* Clean separation of responsibilities
* Secure authentication
* Role-based authorization
* Reliable exception handling
* Structured logging
* Data validation
* Scalable repository architecture
* Consistent API responses
* Proper relational database design

---

## 👨‍💻 Author

**Mostafa Hany**

.NET Developer | Backend Developer

* GitHub: `https://github.com/mostafaroute7-debug`
* LinkedIn: `www.linkedin.com/in/mostafa-hany-43b294232`

---

## 📜 License

This project is developed for educational and technical demonstration purposes.
