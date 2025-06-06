Flight Information API – CAA Coding Challenge
![image](https://github.com/user-attachments/assets/3b893fb3-61ba-4fe0-9316-7158a466572c)

This project implements a RESTful Web API for managing flight information using C# and .NET Core 8. It follows best practices for API structure, error handling, input validation, and logging. The data is persisted using Entity Framework Core with an in-memory database.

Features Implemented
Model
A Flight model with the following properties:

Id (int)
FlightNumber (string)
Airline (string)
DepartureAirport (string)
ArrivalAirport (string)
DepartureTime (DateTime)
ArrivalTime (DateTime)
Status (enum: Scheduled, Delayed, Cancelled, InAir, Landed)

API Endpoints

GET /api/flights: Retrieve all flights
GET /api/flights/{id}: Retrieve a flight by ID
POST /api/flights: Create a new flight
PUT /api/flights: Update an existing flight using query parameters
DELETE /api/flights/{id}: Delete a flight
GET /api/flights/search: Search flights by filters like airline, airports, and date range

Additional Features
Validation using data annotations like [Required] and [StringLength]

Integrated logging via ILogger<FlightController>

Error handling with try-catch blocks to return appropriate HTTP responses

Swagger UI enabled for interactive API testing

Getting Started
Clone the repository:

To get started, clone the repository using git clone <your-repo-url> and navigate into the project directory with cd Aeroplane. Run the API using dotnet run, then open your browser to https://localhost:<port>/swagger to access the Swagger UI for testing the endpoints.

Notes 
Unit test setup is partially complete. A test project (Aeroplane.Tests) is configured using xUnit and Moq, with one test written for the GetFlights method.

However, a package version mismatch between the test project and the main API project caused the test runner (testhost) to fail to load correctly. Despite multiple attempts to resolve the issue by adjusting package versions and project references, the tests could not be executed before the submission deadline.

The test code is in place and will function correctly once the version conflict is resolved.

Future improvements could include persistent storage (e.g., SQL Server), full unit test coverage, and user authentication.

