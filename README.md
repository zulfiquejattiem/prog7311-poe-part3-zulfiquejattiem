TechMove – README

Prerequisites:

Install Docker and Docker Compose

Make sure ports 1433, 5000, and 5001 are free

Running the Project:

Start the stack:
docker-compose up -d

Rebuild everything from scratch (delete containers, networks, and volumes, then rebuild):
docker-compose down -v && docker-compose up --build -d

Stop the stack:
docker-compose down

View logs:
docker-compose logs -f

Restart a single service:
docker-compose restart techmove-api

URLs:

API base URL: http://localhost:5000

Swagger UI: http://localhost:5000/swagger

Web front-end: http://localhost:5001

Database: localhost,1433

Database connection string:
Server=localhost,1433;Database=TechMoveDb;User Id=sa;Password=Your_password123;TrustServerCertificate=True;

Navigation Flow:

Login at /Auth/Login (JWT stored in session)

Manage Clients at /Clients/Index

Manage Contracts at /Contracts/Index (create, upload PDFs, update status)

Manage Service Requests at /ServiceRequests/Index (linked to contracts, USD→ZAR conversion, workflow rules)

Summary:
TechMove is a full-stack containerized system with:

SQL Server database

ASP.NET Core API (JWT-secured, Swagger-enabled)

ASP.NET Core MVC web front-end

Dockerized deployment with health checks and seed data

Run with docker-compose up -d, access API at http://localhost:5000/swagger, and Web front-end at http://localhost:5001.


Username: admin

Password: password
