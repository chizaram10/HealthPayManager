# HealthPayManager

## Setup Instructions
Before running the application, you must set up the database. Follow these steps:

Create Database: Create a new database in your SQL Server instance.

Run SQL Queries: Use the following SQL queries to create the necessary tables in your database:

CREATE TABLE Customers (
    Id BIGINT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(50) NOT NULL DEFAULT '',
    PatientId NVARCHAR(20) NOT NULL DEFAULT '',
    TimeCreated DATETIME NOT NULL DEFAULT GETDATE(),
    TimeUpdated DATETIME NOT NULL DEFAULT GETDATE()
);

CREATE TABLE Payments (
    Id BIGINT PRIMARY KEY IDENTITY(1,1),
    Amount DECIMAL(18, 2) NOT NULL,
    CustomerId BIGINT NOT NULL,
    TimeCreated DATETIME NOT NULL DEFAULT GETDATE(),
    TimeUpdated DATETIME NOT NULL DEFAULT GETDATE(),
    FOREIGN KEY (CustomerId) REFERENCES Customers(Id)
);
