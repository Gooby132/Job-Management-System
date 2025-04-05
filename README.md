# 🛠️ Full Stack Job Management System

This is a full-stack application built with **.NET Core** on the backend and **React + Vite** on the frontend. It is designed to manage and monitor jobs, with real-time updates via SignalR, secure authentication using JWT, and a structured, scalable architecture.

---

## 📂 Project Structure

### 🔧 Backend - .NET Core

The backend is structured using a **layered architecture**, promoting separation of concerns and scalability.

#### ✅ API Layer
- **REST API** for handling HTTP requests
- **SignalR Hub** for real-time job updates
- **JWT Authentication** for securing endpoints
- **Swagger** for API documentation
- Responsible for user authorization and routing requests to the appropriate services

#### 🧱 Infrastructure Layer
- Handles **JWT token generation and validation**
- Hosts a **Background Service** for executing and monitoring jobs
- Manages all third-party or system-level dependencies

#### 💾 Persistence Layer
- Built with **Entity Framework Core**
- Implements:
  - **Repository Pattern**
  - **Unit of Work Pattern**
- Responsible for:
  - **Database Access**
  - **Data Seeding**

#### 📦 Domain Layer
- Contains the **core business logic**
- Defines **Job Management** logic and domain models

---

### 🌐 Frontend - React + Vite

The frontend is a **React** application bootstrapped with **Vite** for lightning-fast development experience.

#### Pages & Features

- **Landing Page**: Simple introduction to the system
- **Login Page**: Authenticates users using JWT
- **Dashboard Page**: 
  - Displays a table of all jobs
  - Automatically updates in real-time via **SignalR**
- **Job Detail Page**:
  - Shows extended job information
  - Operational Button (Start, Restart, Stop, Delete (for authenticated users))
  - Includes job logs and extra metadata
  - State management by Redux
- **Retry Pattern**
  - Client side retry pattern using axios-retry
- **Notifications**
  - Implements notification system for invalid request bodies 
---

## 🔒 Authentication

- Users are authenticated via **JWT**
- Token is issued by the API and stored on the client side
- Authorization is enforced on both frontend and backend

---

## ⚙️ How to Run the Project

### 🖥️ Backend

1. Navigate to the backend project folder
2. Setup your database connection string in `appsettings.json`
3. Run database migration

### 🌐 Frontend
Navigate to the frontend folder

Install dependencies:
```
npm i
cd bankmanagement.client
npm run dev
```

📡 Real-time Updates
This project uses SignalR to push job updates to the frontend instantly:

Backend pushes updates when a job status changes

Frontend listens via a SignalR client and updates the UI accordingly

# 🧪 Tech Stack
## Backend
- .NET 8
- Entity Framework Core
- SignalR
- JWT Authentication
- Background Services
- Clean Architecture principles

## Frontend
- React + Vite
- Axios (for HTTP)
- SignalR Client
- React Router
- Mantine
- Redux

## 🧠 Future Improvements

* Job history and retry logic
* Email notifications on job failure
* Pagination and filtering in dashboard

## 👨‍💻 Author
Crafted with care 💜
