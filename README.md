Shopping Basket Application
This project is a distributed application for managing shopping baskets, built using C# and .NET and Angular17. It leverages MySQL as the database and integrates with Testcontainers for containerized testing.

<hr></hr>
Features
Distributed Application Framework: Built using DistributedApplication.CreateBuilder.
MySQL Integration: Persistent MySQL database with PhpMyAdmin support.
API Services: Shopping Basket API for managing baskets and products.
Containerized Testing: Integration tests using Testcontainers for MySQL.
<hr></hr>
Prerequisites
.NET SDK: Version 8.0 or higher.
Docker: Required for running MySQL containers.
MySQL: Used as the primary database.
PhpMyAdmin: Optional for database management.
<hr></hr>
Setup Instructions
Clone the Repository:


git clone <repository-url>
cd <repository-folder>
Build the Application:


dotnet build
Run the Application:


dotnet run --project ShoppingBasket.AppHost
Run Tests:


dotnet test
<hr></hr>
<hr></hr>
Project Structure
ShoppingBasket.AppHost: Entry point for the application.
ShoppingBasket.Tests.Integration: Integration tests using Testcontainers.
ShoppingBasket.API: API for managing shopping baskets.
ShoppingBasket.Domain: Core domain logic and models.
ShoppingBasket.Application: Contains business logic, use cases, and service definitions. It acts as a mediator between the domain and external layers.
ShoppingBasket.Infrastructure: Handles database access, external services, and other low-level details.
ShoppingBasket.Presentation: layer is implemented using Angular to provide a user-friendly interface for managing shopping baskets. It interacts with the ShoppingBasket.API to perform operations such as creating, updating, and closing baskets.
<hr></hr>
ShoppingBasket.Presentation Setup Instructions
Navigate to the Presentation Folder:

cd ShoppingBasket.Presentation
Install Dependencies:
npm install

Run the Application:
npm run start
Access the Application: Open your browser and navigate to http://localhost:4200.

<hr></hr>
License
This project is licensed under MIT License.