# Microservices Architecture with .NET

This solution demonstrates a microservices architecture using .NET, implementing CQRS pattern, MediatR, Ocelot API Gateway, MassTransit, MongoDB, and comprehensive testing approaches.

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
 [Fact]
    public void User_Creation_Should_Set_Properties_Correctly()
    {
        var email = "test@example.com";
        var password = "password";
        var firstName = "John";
        var lastName = "Doe";

        var user = new User(email, password, firstName, lastName);

        user.Email.Should().Be(email);
        user.FirstName.Should().Be(firstName);
        user.LastName.Should().Be(lastName);
        user.Status.Should().Be(Status.Inactive);
        user.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }
```

### Integration Tests

```csharp
public sealed class ProductsEndpointTest(CustomWebApplicationFactory<Program> factory)
    : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task GetAllProducts_Should_Return_Ok()
    {
        var response = await _client.GetAsync("/api/products");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

}
```



## 📝 License

This project is licensed under the MIT License - see the LICENSE file for details
