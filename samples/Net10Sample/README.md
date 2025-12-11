# .NET 10 SoapCore Sample

This sample demonstrates how to use SoapCore with .NET 10, showcasing both a SOAP server and client implementation.

## Projects

### Net10Server
A .NET 10 web application that hosts a SOAP service using SoapCore.

- **Port**: 5060
- **Endpoints**:
  - `/Service.svc` - DataContractSerializer endpoint
  - `/Service.asmx` - XmlSerializer endpoint

### Net10Client
A .NET 10 console application that consumes the SOAP service.

## Running the Sample

1. **Start the Server**:
   ```bash
   cd Net10Server
   dotnet run
   ```

2. **Run the Client** (in a separate terminal):
   ```bash
   cd Net10Client
   dotnet run
   ```

## Features Demonstrated

- Basic string operations (Ping)
- Complex model serialization/deserialization
- Void methods with out parameters
- Async methods
- XML element handling
- Array return types
- Nullable types

## Key Differences from Previous Samples

- Uses modern .NET 10 hosting model with `IHostBuilder`
- Updated to use System.ServiceModel 8.0.* packages for better .NET 10 compatibility
- Demonstrates implicit usings and nullable reference types
- Uses a different port (5060) to avoid conflicts with other sample projects

## Requirements

- .NET 10 SDK
- SoapCore library
- Models project (shared across all samples)
