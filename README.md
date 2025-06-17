# 📄 IMS – Installment Management System

**IMS** is a custom-built Installment Management System designed to help businesses efficiently manage customer payments, monitor defaults, and gain actionable insights through a user-friendly dashboard. Developed using **Angular** and **ASP.NET Core Web API**, IMS solves the challenge of tracking installment plans, automating payment notifications, and providing clear financial visibility to administrators.

---

## 🚀 Features

- 💳 **Automated Payment Collection**  
  Integrated with **Paystack** to securely collect and verify installment payments.

- 📲 **Smart Notifications via SMS**  
  Sends automated reminders and payment updates using **Hubtel SMS** integration.

- 🔄 **Real-Time Payment Handling**  
  Utilizes **webhooks** to instantly process the effect of incoming payments on customer accounts.

- 📅 **Payment Tracking**  
  Identifies and displays **pending** and **due** payments clearly on the admin dashboard.

- 📊 **Business Analytics**  
  Provides key metrics and visual statistics to help businesses understand performance at a glance.

- 🧩 **Background Job Scheduling**  
  Integrated **Hangfire** for managing recurring background tasks and payment reminders.

- 🧑‍💼 **Admin Dashboard**  
  Equipped with powerful table filters, meaningful statistics, and clear UI enhancements for improved user experience and operational efficiency.

---

## 🛠️ Tech Stack

- **Frontend**: Angular  
- **Backend**: ASP.NET Core Web API  
- **Task Scheduling**: Hangfire  
- **SMS Integration**: Hubtel  
- **Payments**: Paystack  
- **Notifications**: Webhooks  

---

## 📦 Getting Started

### Prerequisites

- .NET SDK 6.0+
- Node.js & Angular CLI
- SQL Server (or your preferred database)

### Installation

#### 1. Clone this repository

```bash
git clone https://github.com/NiiAnyetei/InstallmentManagementSystem.git
cd InstallmentManagementSystem
```

#### 2. Backend Setup

- Navigate to the API project folder  
- Configure `appsettings.json` with your Paystack, Hubtel, and DB credentials  
- Run migrations and start the API

```bash
dotnet ef database update
dotnet run
```

#### 3. Frontend Setup

- Navigate to the Angular frontend folder  
- Install dependencies and run the development server  

```bash
npm install
ng serve
```

