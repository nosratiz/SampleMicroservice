# Microservices Architecture with .NET

This solution demonstrates a microservices architecture using .NET, implementing CQRS pattern, MediatR, Ocelot API Gateway, MassTransit, MongoDB, and comprehensive testing approaches.

## 🏗️ Architecture Overview

```
├── src/
│   ├── Gateway/
│   │   └── API.Gateway
│   ├── Services/
│   │   ├── Service.A/
│   │   ├── Service.B/
│   │   └── Service.C/
│   └── Shared/
│       ├── Common.Logging
│       ├── Common.Security
│       └── Common.MongoDB
├── tests/
│   ├── Service.A.Tests/
│   ├── Service.B.Tests/
│   └── Service.C.Tests/
└── docker-compose.yml
```

## 🚀 Technologies

- **.NET 8.0**
- **MediatR** - CQRS implementation
- **Ocelot** - API Gateway
- **MassTransit** - Message Broker integration
- **MongoDB** - Database
- **xUnit** - Testing framework
-**testContainer** - IntegrationTest 
- **Docker** - Containerization

## 🔧 Prerequisites

- .NET 8.0 SDK
- Docker Desktop
- MongoDB
- RabbitMQ

## ⚙️ Configuration

### Setting Up User Secrets

Each service uses .NET User Secrets for secure configuration management. To initialize:

```bash
# Navigate to service directory
cd src/Services/Service.A

# Initialize user secrets
dotnet user-secrets init

# Add secrets
dotnet user-secrets set "MongoDB:ConnectionString" "your_connection_string"
dotnet user-secrets set "RabbitMQ:Host" "your_rabbitmq_host"
```

### Environment Variables Template
```env
ASPNETCORE_ENVIRONMENT=Development
MongoDB__ConnectionString=mongodb://localhost:27017
RabbitMQ__Host=localhost
RabbitMQ__Username=guest
RabbitMQ__Password=guest
```

## 🏃‍♂️ Running the Application

1. Start infrastructure:
```bash
docker-compose up -d
```

2. Run the Gateway:
```bash
cd src/Gateway/API.Gateway
dotnet run
```

3. Run Services:
```bash
cd src/Services/Service.A
dotnet run

# Repeat for other services
```

## 🎯 CQRS Pattern Implementation

Each service follows CQRS pattern using MediatR:

```csharp
// Command
public class CreateItemCommand : IRequest<Guid>
{
    public string Name { get; set; }
}

// Command Handler
public class CreateItemCommandHandler : IRequestHandler<CreateItemCommand, Guid>
{
    private readonly IMongoRepository<Item> _repository;
    
    public async Task<Guid> Handle(CreateItemCommand request, CancellationToken cancellationToken)
    {
        // Implementation
    }
}
```

### Pipeline Behaviors

```csharp
public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
{
    public async Task<TResponse> Handle(TRequest request, 
        RequestHandlerDelegate<TResponse> next, 
        CancellationToken cancellationToken)
    {
        // Validation logic
        return await next();
    }
}
```

## 🔌 API Gateway Configuration

Ocelot configuration example (`ocelot.json`):

```json
{
  "Routes": [
    {
      "DownstreamPathTemplate": "/api/v1/{everything}",
      "DownstreamScheme": "http",
      "DownstreamHostAndPorts": [
        {
          "Host": "service.a",
          "Port": 80
        }
      ],
      "UpstreamPathTemplate": "/service-a/{everything}",
      "UpstreamHttpMethod": [ "GET", "POST", "PUT", "DELETE" ]
    }
  ]
}
```

## 📚 Swagger Configuration

Gateway aggregates all service Swagger docs:

## 🔄 Message Broker Integration

MassTransit configuration:

```csharp
services.AddMassTransit(x =>
{
    x.AddConsumer<OrderCreatedConsumer>();
    
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("localhost", "/", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });
        
        cfg.ConfigureEndpoints(context);
    });
});
```

## 🧪 Testing

### Unit Tests

```csharp
public class CreateItemCommandHandlerTests
{
    [Fact]
    public async Task Handle_ValidCommand_ReturnsId()
    {
        // Arrange
        var repository = new Mock<IMongoRepository<Item>>();
        var handler = new CreateItemCommandHandler(repository.Object);
        var command = new CreateItemCommand { Name = "Test Item" };
        
        // Act
        var result = await handler.Handle(command, CancellationToken.None);
        
        // Assert
        Assert.NotEqual(Guid.Empty, result);
    }
}
```

### Integration Tests

```csharp
public class ItemsControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    [Fact]
    public async Task CreateItem_ValidRequest_ReturnsCreated()
    {
        // Arrange
        var client = _factory.CreateClient();
        var command = new CreateItemCommand { Name = "Test" };
        
        // Act
        var response = await client.PostAsJsonAsync("/api/items", command);
        
        // Assert
        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }
}
```

## 📦 Shared Package

The shared projects are packaged as NuGet packages. To create and use:

1. Package creation:
```bash
cd src/Shared/Common.Logging
dotnet pack -c Release
```

2. Package usage:
```xml
<PackageReference Include="Common.Logging" Version="1.0.0" />
```


## 📝 License

This project is licensed under the MIT License - see the LICENSE file for details
