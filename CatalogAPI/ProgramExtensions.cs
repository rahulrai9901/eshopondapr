using System;
using CatalogAPI.Infrastructure;

namespace CatalogAPI;

public static class ProgramExtensions
{
    public static void AddCustomDatabase(this WebApplicationBuilder builder)
    {
        builder.Services.AddDbContext<CatalogDbContext>(
            options => options.UseSqlServer(builder.Configuration["ConnectionStrings:CatalogDB"]!));
    }
    public static void ApplyDatabaseMigration(this WebApplication app)
    {
        // Apply database migration automatically. Note that this approach is not
        // recommended for production scenarios. Consider generating SQL scripts from
        // migrations instead.
        using var scope = app.Services.CreateScope();

        var retryPolicy = CreateRetryPolicy(app.Configuration);
        var context = scope.ServiceProvider.GetRequiredService<CatalogDbContext>();

        retryPolicy.Execute(context.Database.Migrate);
    }

    public static void AddCustomConfiguration(this WebApplicationBuilder builder)
    {
        builder.Configuration.AddDaprSecretStore(
           "eshopondapr-secretstore",
           new DaprClientBuilder().Build());
    }

    public static void AddCustomHealthChecks(this WebApplicationBuilder builder) =>
     builder.Services.AddHealthChecks()
         .AddCheck("self", () => HealthCheckResult.Healthy())
         .AddSqlServer(
             builder.Configuration["ConnectionStrings:CatalogDB"]!,
             name: "CatalogDB-check",
             tags: new[] { "catalogdb" });

    private static Policy CreateRetryPolicy(IConfiguration configuration)
    {
        // Only use a retry policy if configured to do so.
        // When running in an orchestrator/K8s, it will take care of restarting failed services.
        if (bool.TryParse(configuration["RetryMigrations"], out bool _))
        {
            return Policy.Handle<Exception>().
                WaitAndRetryForever(
                    sleepDurationProvider: _ => TimeSpan.FromSeconds(5),
                    onRetry: (exception, retry, _) =>
                    {
                        
                    }
                );
        }

        return Policy.NoOp();
    }
}

