using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Testcontainers.MongoDb;
using Testcontainers.RabbitMq;

namespace Frontliners.Inventory.IntegrationTest;

public class CustomWebApplicationFactory<TProgram>
    : WebApplicationFactory<TProgram>, IAsyncLifetime
    where TProgram : class
{
    private readonly MongoDbContainer _mongoDbContainer = new MongoDbBuilder()
        .WithImage("mongo:latest")
        .WithPassword("123QWEQWE")
        .WithUsername("Admin")
        .Build();
    private readonly RabbitMqContainer _rabbitMqContainer = new RabbitMqBuilder()
        .WithImage("rabbitmq:3-management")
        .WithUsername("guest")
        .WithPassword("guest")
        .Build();
    
    private readonly IConfiguration _configuration = new ConfigurationBuilder()
        .AddJsonFile("appsettings.Development.json").Build();
    
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        _configuration["MongoDbOptions:connectionString"] = _mongoDbContainer.GetConnectionString();
        
        _configuration["RabbitMqSettings:HostName"] = _rabbitMqContainer.GetConnectionString();
        
        _configuration["RabbitMqSettings:UserName"] = "guest";
        
        _configuration["RabbitMqSettings:Password"] = "guest";
        
        builder.UseEnvironment("Development");
        
        builder.UseConfiguration(_configuration);
    }
    
   

    public async Task InitializeAsync()
    {
        await Task.WhenAll(_mongoDbContainer.StartAsync(), _rabbitMqContainer.StartAsync());
    }

    public new async Task DisposeAsync()
    {
        await _mongoDbContainer.DisposeAsync();
        
        await _rabbitMqContainer.DisposeAsync();
    }
}