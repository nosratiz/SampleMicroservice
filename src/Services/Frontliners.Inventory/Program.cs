using Frontliners.Common;
using Frontliners.Common.Logger;
using Frontliners.Inventory;
using Frontliners.Inventory.EndPoints;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.SeriLogConfig(builder.Configuration);
builder.Services.AddCommon(builder.Configuration,typeof(Program).Assembly);
builder.Services.AddInventoryService(builder.Configuration);

var app = builder.Build();

app.UseSerilogRequestLogging();


    app.UseSwagger();
    app.UseSwaggerUI();

app.UseExceptionHandler();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapProductEndpoints();

await app.RunAsync();


//for integration test
public partial class Program
{
}

