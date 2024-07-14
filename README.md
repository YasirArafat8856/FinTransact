# FinTransact API

## Overview
The FinTransact API .NET 8 supported microservices-based system.

## Architecture & Design
- **Microservices Architecture**: The system is designed with separate microservices for different functionalities.
- **Event-Driven Architecture**: Uses RabbitMQ for asynchronous processing and communication between microservices.
- **Real-Time Processing**: Uses SignalR for real-time updates and notifications.

## Microservices
1. **AccountService**: Manages bank accounts and processes transactions.
2. **TransactionService**: Handles the processing of transactions.
3. **NotificationService**: Sends real-time notifications about transactions.
4. **AuditService**: Logs and audits transaction activities.

## Technologies Used
- .NET Core / .NET
- ASP.NET Core for API development
- Entity Framework Core
- RabbitMQ for message queues
- SignalR for real-time communication
- JWT for authentication
- Redis for caching
- Swagger for API documentation

## Prerequisites
- .NET Core SDK
- Visual Studio 2022
- RabbitMQ
- Redis

### Clone the Repository
git clone https://github.com/YasirArafat8856/FinTransact

