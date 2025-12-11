# SoapCore Samples

This directory contains various sample projects demonstrating SoapCore functionality across different .NET versions.

## Sample Projects

### Classic Samples (.NET Core 3.1)

#### Server
A .NET Core 3.1 web application hosting a SOAP service.
- **Port**: 5050
- **Target Framework**: netcoreapp3.1

#### Client
A .NET Core 3.1 console application consuming the SOAP service.
- **Target Framework**: netcoreapp3.1

#### ServiceReferenceClient
A .NET 8 console application with generated service reference consuming the SOAP service.
- **Target Framework**: net8.0

### .NET 10 Sample

#### Net10Server
A modern .NET 10 web application hosting a SOAP service using SoapCore.
- **Port**: 5060
- **Target Framework**: net10.0
- **Features**: Modern hosting model, latest System.ServiceModel packages (8.0.*)

#### Net10Client
A .NET 10 console application demonstrating SOAP client capabilities.
- **Target Framework**: net10.0
- **Features**: Implicit usings, nullable reference types, latest ServiceModel packages

### Shared Projects

#### Models
Shared model library used across all samples.
- **Target Framework**: netstandard2.0
- **Contains**: Service contracts and data models

## Getting Started

1. **Build the solution**:
   ```bash
   dotnet build
   ```

2. **Run a server** (choose one):
   ```bash
   # Classic server (.NET Core 3.1)
   cd Server
   dotnet run

   # Or .NET 10 server
   cd Net10Server
   dotnet run
   ```

3. **Run a client** (in a separate terminal):
   ```bash
   # Classic client
   cd Client
   dotnet run

   # Or .NET 10 client
   cd Net10Client
   dotnet run
   ```

## Port Assignments

- **5050**: Classic Server (.NET Core 3.1)
- **5060**: Net10Server (.NET 10)

Make sure the client connects to the correct port based on which server you're running.

## Requirements

- .NET Core 3.1 SDK (for classic samples)
- .NET 8 SDK (for ServiceReferenceClient)
- .NET 10 SDK (for .NET 10 samples)
- SoapCore library
