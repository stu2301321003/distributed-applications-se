#Vasil Banchev 2301321003

# Vacation Manager

**Vacation Manager** is a comprehensive full-stack application designed to streamline the management of employee leave requests, teams, and organizational roles within companies.

## Overview

The system features a robust backend built with ASP.NET Core Web API and a responsive Blazor WebAssembly frontend enhanced by Radzen components. It leverages JWT-based authentication and role-based authorization to support different user roles such as CEO, Manager, Employee, Dev, and Unverified users.

---

## Features

- **User Authentication & Authorization**  
  Secure registration and login with JWT tokens. Role-based access control for CEO, Manager, Employee, Dev, and Unverified users.

- **Company Management (CEO)**  
  Create, update, view, and delete companies. Manage teams under a company.

- **Team Management (CEO & Manager)**  
  Create teams, assign employees, designate managers, rename, and delete teams.

- **Leave Requests (Employee & Manager)**  
  Employees submit leave requests. Managers and CEOs can view, approve, or reject them. Supports filtering and pagination.

- **Admin Panel (Dev)**  
  Manage unverified users with options to view and verify accounts.

---

## Tech Stack

- **Backend:** ASP.NET Core 8, Entity Framework Core (In-Memory or SQL Server)  
- **Frontend:** Blazor WebAssembly, Radzen.Blazor components, Blazored.LocalStorage  
- **Authentication:** JWT Bearer tokens

---

## Setup Instructions

1. **Clone the repository.**

2. **Configure Backend:**  
   In the `VacationManager` API project, update your connection string in `appsettings.json`.  
   - To use SQL Server, set `"UseInMemoryDatabase": false` and provide a valid connection string.  
   - For quick testing, you can leave `"UseInMemoryDatabase": true` to use the in-memory provider.

3. **Run the API project.**

4. **Configure Frontend:**  
   In the UI project, update the `BaseUrl` in `appsettings.json` to point to your running API URL.

5. **Run the UI project.**

You are now ready to use the application.
