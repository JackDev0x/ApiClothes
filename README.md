# Clothing Marketplace (Prototype) – ASP.NET Core API

This project is a prototype of a web platform for listing and browsing clothing advertisements. It shares core features with a full classifieds portal but is currently under development, focusing on testing the backend architecture and storage solutions.

Unlike the full production version, this prototype uses **local Azurite Blob Storage** (running in a separate Docker container) to simulate Azure Blob Storage for image storage. The image upload and retrieval functionality is still in development and may not work as expected.

---

## Project Status

⚠️ **Prototype status:** This project is in active development and testing. The local Azurite Blob Storage container is used to simulate Azure Blob Storage but image upload/download features are not fully functional yet.

---

## Key Features

- User registration and authentication with **JWT tokens**  
- Secure backend built with **ASP.NET Core** and **EF Core (Code First)**  
- Relational **database integration** via dependency injection (connection string configuration)  
- **Local Azurite Blob Storage** container used for image storage simulation (in testing phase)  
- Modular API architecture with partial separation of concerns (controllers with selected service classes)  
- Role-based access and authentication for protected endpoints  
- Users can **create, edit, delete** clothing listings  
- Visitors can **browse, search, filter** clothing ads  
- Hybrid mapping strategy: **AutoMapper** for simple transformations, manual mapping for complex cases  

---

## Technologies

- ASP.NET Core Web API  
- Entity Framework Core (Code First)  
- SQL Server  
- Azurite (local Azure Blob Storage emulator)  
- Docker (multi-container setup with app and Azurite)  
- JWT Authentication  
- Dependency Injection  
- AutoMapper  
- Swagger  

---

## Setup Notes

- This project runs via Docker Compose with two containers:  
  - **API container** running the ASP.NET Core backend with database
  - **Azurite container** simulating Azure Blob Storage locally  

- All necessary configuration values such as database connection strings, JWT settings, and storage connection strings are provided to the application via environment variables configured in Docker Compose.

---

## Project Structure

- `Controllers` – API controllers  
- `DtoModels` – Data Transfer Objects (DTOs)  
- `Entities` – EF Core entity classes and `DbContext`  
- `Migrations` – EF Core migration classes
- `RequestsModels` - Requests Models classes
- `Services` – Business logic and service classes  

---

## How to run

The application is started using Docker Compose. The `docker-compose.yml` file is located in the root folder, alongside `ApiClothes.sln`.

Open a terminal in this folder and run the following commands in order (application will be available on port 8080):

```bash
docker compose build
```
```bash
docker compose up --build
```
---
