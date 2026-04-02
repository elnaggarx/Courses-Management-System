# Courses Management System

## Overview
This project is a complete ASP.NET Core (minimal API + controllers) application for managing students, courses, instructors, and enrollments.

It includes:
- CRUD for students/courses/instructors
- Student enrollment / de-enrollment in courses
- Role-based authorization (Admin, Instructor, User)
- JWT bearer authentication
- Projection with DTOs and query optimizations (`AsNoTracking`, `Select`)
- EF Core data seeding and PostgreSQL persistence
- Swagger API docs

## Project structure
- `Controllers/` - API endpoints for Students, Courses, Instructors, Auth
- `Services/` + `Interfaces/` - business logic
- `Database/AppDbContext.cs` - DB context, model config, seed data
- `Entities/` - entity models and relationships
- `DTOs/` - request/response models
- `Program.cs` - app startup and middleware
- `Courses_Management_System.http` - HTTP test scripts for all endpoints

## Requirements implemented
1. Swagger UI with `AddEndpointsApiExplorer` and `AddSwaggerGen`.
2. PostgreSQL via EF Core provider `Npgsql.EntityFrameworkCore.PostgreSQL`.
3. Model relationships:
   - `Enrollment` composite key (studentId, courseId)
   - Course has Instructor, Student has Enrollment collection
   - Instructor one-to-one with InstructorProfile
4. DTO + validation in request/response objects.
5. JWT Authentication + auth policies.
6. 401 behavior on missing/invalid token.
7. LINQ optimization: `Select`, `AsNoTracking()`.
8. Seed initial data via EF Core `OnModelCreating` + migrations.
9. `Courses_Management_System.http` includes fully executable endpoint tests.
10. Added `GET` endpoints:
    - `/api/students/{studentId}/courses` 
    - `/api/courses/{courseId}/instructor`
    - `/api/courses/{courseId}/students`

## Technologies used
- .NET 10 / ASP.NET Core
  - Web API platform used for routing, middleware, auth and hosting.
- Entity Framework Core
  - ORM for database mapping, migrations and LINQ support.
- Npgsql.EntityFrameworkCore.PostgreSQL
  - PostgreSQL provider for EF Core.
- Swashbuckle.AspNetCore (Swagger)
  - Auto-generates API docs and UI.
- Microsoft.AspNetCore.Authentication.JwtBearer
  - JWT token validation and auth flow.
- AutoMapper (if used) / manual mapping via LINQ projections
  - DTO mapping.
- Fluent validation attributes via `System.ComponentModel.DataAnnotations`
  - Validation on model properties.

## How to run the project
1. Install .NET SDK (10 or later).
2. Install PostgreSQL and create a database.
3. In `appsettings.json`, configure:
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Host=localhost;Database=courses_db;Username=youruser;Password=yourpass"
   }
   "JwtSettings": {
     "Key": "super-secret-key",
     "Issuer": "CoursesManagementSystem",
     "Audience": "CoursesManagementSystemUsers",
     "DurationInMinutes": 60
   }
   ```
4. Restore dependencies:
   ```bash
dotnet restore
```
5. Apply migrations:
   ```bash
dotnet ef database update
```
6. Run the app:
   ```bash
dotnet run --urls http://localhost:5295
```
7. Open Swagger at `http://localhost:5295/swagger`.
8. Use `Courses_Management_System.http` for full endpoint testing.

### Authentication test
1. `POST /api/auth/login` with email/password (admin@localhost / Admin@123).
2. Copy returned token.
3. Use `Authorization: Bearer <token>` on protected endpoints.

## Why HTTP-only cookies are industry standard for auth security
HTTP-only cookies are considered secure for session-based authentication because:
- **Not accessible from JavaScript** (`HttpOnly`), so they mitigate XSS token theft.
- **Automatically sent with same origin requests**, simplifying auth flow for browsers.
- **Combined with `SameSite` and `Secure` flags**, they harden CSRF protection and ensure HTTPS-only transport.

While JWT in Authorization headers (bearer tokens) is workable for APIs, HTTP-only cookies are still preferred in many web applications because they reduce token exposure and provide better browser session semantics.

## Endpoint summary
- `/api/auth/login` (POST)
- `/api/students` (GET, POST)
- `/api/students/{id}` (GET, PUT, DELETE)
- `/api/students/{id}/enroll/{courseId}` (POST)
- `/api/students/{id}/deenroll/{courseId}` (POST)
- `/api/students/{id}/courses` (GET)
- `/api/courses` (GET, POST)
- `/api/courses/{id}` (GET, PUT, DELETE)
- `/api/courses/{id}/students` (GET)
- `/api/courses/{id}/instructor` (GET)
- `/api/instructors` (GET, POST)
- `/api/instructors/{id}` (GET, PUT, DELETE)

## Notes
- Ensure no running app process locks `bin/Debug/net10.0/Courses_Management_System.exe` before build.
- Use `wmic process where ProcessId=<pid> delete` or close running instance as needed.
