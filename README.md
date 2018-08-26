# Everbank Ledger
Introduction to the app and what it does

## Requirements
* .NET Core >= 2.1

**Helpful for development but not required**:
* Visual Studio Code
* Visual Studio Code Solution Explorer Plugin
* Visual Studio Code C# Plugin

## Dependencies
At the moment this project doesn't require any kind of significant packages

## Build Instructions
Before beginning you will need to restore dependencies for all of the projects
```
$ dotnet restore
```

## Running the Console Application
```
$ cd Everbank.Console
$ dotnet run
```

## Running the Web Application
```
$ cd Everbank.Web
$ dotnet run
```
You should now be able to connect to the application locally using port 5000 (http) or port 5001 (https)

## Running Unit Tests
```
$ dotnet test
```

## Application Structure and Overview
TODO: Describe the pattern/architecture and what each project does, why this was chosen
* Service Project is responsible for transforming data based on business rules
* Repository Project is responsible for data access
* This solution uses a mocked up dataset stored in a singleton but the Repository layer can be modified to connect to a database

## Improvements for the future
* Make the ApplicationData singleton thread-safe
* Improve encryption for stored passwords by salting them
* Add Delete account feature
* Increase password complexity criteria