# Everbank Ledger
The Everbank Ledger is a demonstration project by Randolph Jones. It is built in .NET Core. It includes the necessary logic for basic account and transaction management for an imaginary bank. This solution contains a web interface (Everbank.Web) and a console interface (Everbank.Console). The interfaces share a common data access layer and a common service (business logic) layer. 

This solution uses a mock object to simulate a database but the intent is that this solution can be easily adapted to work with a persistent database by updating the data access layer (Everbank.Repositories).

## Why .NET Core?
My background is primarily in JavaScript and Node.js. Within the last few years I have also been working quite extensively with .NET environments. Working with .NET Core allows me to use familiar development workflows while also working with the C# language. I've enjoyed my deep dive with .NET Core over the course of this project.

## Requirements
* .NET Core >= 2.1

    ### Helpful for development but not required:
    * Visual Studio Code
    * Visual Studio Code Solution Explorer Plugin
    * Visual Studio Code C# Plugin (OmniSharp)

## Dependencies
At the moment this project doesn't require any kind of significant packages

## Build Instructions
Before beginning you will need to restore dependencies for all of the projects
```bash
$ dotnet restore
```

## Running the Console Application
```bash
$ cd Everbank.Console
$ dotnet run
```

## Running the Web Application
```bash
$ cd Everbank.Web
$ dotnet run
```
You should now be able to connect to the application locally using port 5000 (http) or port 5001 (https)

## Running Unit Tests
```bash
$ cd Everbank.UnitTests
$ dotnet test
```

## Test Accounts
Two test accounts are available when first running the app:
* test@test.com
* test2@test.com

Both accounts use the password *password123*

These apps are also used for unit testing as well. 

**Note**: test2@test.com does not have a transaction history. This allows us to check for certain conditions when unit testing.

## Application Structure and Overview
The solution uses several layers that are organized in projects.
* **Everbank.Web** is responsible for running the web interface using ASP.NET MVC
* **Everbank.Console** is responsible for running the console interface
* **Everbank.Service** is responsible for transforming data based on business rules and turning any kind of errors into friendly messaging for the user. It talks directly to the data access layer.
* **Everbank.Repositories** is responsible for accessing data from the mock database object
* **Everbank.Mocking** contains the logic needed to create a mock object (DataSet) to simulate a database. It currently runs as a singleton for the lifetime of either the web app or the console app. **NOTE**: The web app and console app cannot concurrently access the same mock dataset.
* **Everbank.UnitTests** contains unit tests that check the Everbank.Service and Everbank.Repositories layers

## Future Improvements that would make this better
* Make the ApplicationData singleton thread-safe
* Salt passwords as they are stored
* Fill mock database from a flat file on startup
* Make password criteria more complex
* Change Everbank.Service namespace to Everbank.Service**s** to be grammatically correct
* Add error checking and try catch blocks when working with IdentityClaims
* More efficiently calculate the account balance in the repository layer using SQL

## Known Bugs
* Cookies are not cleared in the web app after it is recycled and it creates a "ghost" dashboard until logout
* Cookies do not expire within the planned 5-10 minutes
* Passwords are not masked in the console application
