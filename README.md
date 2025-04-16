Task Management API 
Description 📌
RESTful API built with .NET for task management featuring:
✔ JWT Authentication (register/login).
✔ CRUD operations for tasks.
✔ SQL Server database.

Technologies ⚙️
.NET 6/7/8

Entity Framework Core

JWT (JSON Web Tokens)

Swagger/OpenAPI

SQL Server
Installation 🛠️
Requirements
.NET SDK

SQL Server

Postman (optional)
Endpoints 📡
Authentication
POST /api/auth/register → User registration.

POST /api/auth/login → Login (get JWT).

Tasks (requires JWT token)
GET /api/tasks → List all tasks.

POST /api/tasks → Create task.

PUT /api/tasks/{id} → Update task.

DELETE /api/tasks/{id} → Delete task.
License 📜
MIT License.
