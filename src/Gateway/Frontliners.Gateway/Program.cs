using Frontliners.Common.Logger;
using Frontliners.Common.Middleware;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.SeriLogConfig(builder.Configuration);

builder.Configuration
    .AddJsonFile($"ocelot.{builder.Environment.EnvironmentName}.json", false, true)
    .Build();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSwaggerForOcelot(builder.Configuration,
    opt=>
    {
        opt.GenerateDocsForGatewayItSelf = false;
    });

builder.Services.AddOcelot(builder.Configuration);

var app = builder.Build();

app.UseSerilogRequestLogging();


    app.UseSwagger();
    app.UseSwaggerForOcelotUI(opt =>
    {
        opt.PathToSwaggerGenerator = "/swagger/docs";
    });

    app.UseSwaggerUI();


app.UseHttpsRedirection();

app.UseAuthorization();

await app.UseOcelot();


await app.RunAsync();