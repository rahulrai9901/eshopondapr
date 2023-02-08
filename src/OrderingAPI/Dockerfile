#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src

COPY ["src/eShopEvent/eShopEvent.csproj", "src/eShopEvent/"]
COPY ["src/eShopHealthCheck/eShopHealthCheck.csproj", "src/eShopHealthCheck/"]
COPY ["src/OrderingAPI/OrderingAPI.csproj", "src/OrderingAPI/"]
RUN dotnet restore "src/OrderingAPI/OrderingAPI.csproj"
COPY . .
WORKDIR "/src/src/OrderingAPI"
RUN dotnet build "OrderingAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "OrderingAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OrderingAPI.dll"]
