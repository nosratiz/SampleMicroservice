using Frontliners.Common;
using Frontliners.Common.Logger;
using Frontliners.Identity;
using Frontliners.Identity.Application.Systems;
using Frontliners.Identity.Endpoints;
using MediatR;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.SeriLogConfig(builder.Configuration);
builder.Services.AddCommon(builder.Configuration,typeof(Program).Assembly);
builder.Services.AddIdentityService(builder.Configuration);


var app = builder.Build();

using var serviceScope = app.Services.CreateScope();
var services = serviceScope.ServiceProvider;

var mediator = services.GetRequiredService<IMediator>();
await mediator.Send(new SeedDataCommand(), CancellationToken.None);


app.UseSerilogRequestLogging();


    app.UseSwagger();
    app.UseSwaggerUI();

app.UseExceptionHandler();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapAuthEndpoints();

await app.RunAsync();