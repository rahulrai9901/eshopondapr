
using OrderingAPI.Controllers;
using OrderingAPI.Infrastructure;
using Actors;
using Dapr;
using Dapr.Client;
using Dapr.Extensions.Configuration;
using eShopEvent;
using Microsoft.EntityFrameworkCore;
using OrderingAPI.Infrastructure.Repository;
using eShopHealthCheck;
using HealthChecks.UI.Client;
using Microsoft.Extensions.Diagnostics.HealthChecks;

var appName = "Ordering API";

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDaprClient();
builder.Configuration.AddDaprSecretStore(
          "eshopondapr-secretstore",
          new DaprClientBuilder().Build());

builder.Services.AddHealthChecks()
           .AddCheck("self", () => HealthCheckResult.Healthy())
           .AddDaprHealth()
           .AddSqlServer(
               builder.Configuration["ConnectionStrings:OrderingDB"]!,
               name: "OrderingDB-check",
               tags: new[] { "orderdb" });

builder.Services.AddDbContext<OrderDBContext>(
    options => options.UseSqlServer(builder.Configuration["ConnectionStrings:OrderingDB"]!));

builder.Services.AddScoped<IEventPublish, EventPublish>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddControllers();
builder.Services.AddActors(options =>
{
    options.Actors.RegisterActor<OrderingProcessActor>();
});
builder.Services.AddSignalR();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCloudEvents();
//app.UseAuthorization();

app.MapControllers();
app.MapActorsHandlers();
app.MapSubscribeHandler();
app.MapCustomHealthChecks("/hc", "/liveness", UIResponseWriter.WriteHealthCheckUIResponse);

app.MapHub<NotificationsHub>("/hub/notificationhub",
    options => options.Transports = Microsoft.AspNetCore.Http.Connections.HttpTransportType.LongPolling);

try
{
    app.Logger.LogInformation("Applying database migration ({ApplicationName})...", appName);
    var scope = app.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<OrderDBContext>();
    context.Database.Migrate();

    app.Logger.LogInformation("Starting web host ({ApplicationName})...", appName);
    app.Run();
}
catch (Exception ex)
{
    app.Logger.LogCritical(ex, "Host terminated unexpectedly ({ApplicationName})...", appName);
}
finally
{
    //Serilog.Log.CloseAndFlush();
}

