using System;
using Microsoft.Extensions.DependencyInjection;

namespace eShopHealthCheck;

public static class DaprHealthCheckBuilderExtensions
{
    public static IHealthChecksBuilder AddDaprHealth(this IHealthChecksBuilder builder) =>
        builder.AddCheck<DaprHealthCheck>("dapr");
}

