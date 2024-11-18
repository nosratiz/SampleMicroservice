using Frontliners.Common;
using Frontliners.Common.Logger;
using Frontliners.Order;
using Frontliners.Order.EndPoints;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.SeriLogConfig(builder.Configuration);
builder.Services.AddCommon(builder.Configuration,typeof(Program).Assembly);
builder.Services.AddOrderService(builder.Configuration);

var app = builder.Build();

app.UseSerilogRequestLogging();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseExceptionHandler();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapBasketEndpoints();
app.MapOrdersEndpoints();

await app.RunAsync();