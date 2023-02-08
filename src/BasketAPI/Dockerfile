FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 5150

ENV ASPNETCORE_URLS=http://+:5150

# Creates a non-root user with an explicit UID and adds permission to access the /app folder
# For more info, please refer to https://aka.ms/vscode-docker-dotnet-configure-containers
RUN adduser -u 5678 --disabled-password --gecos "" appuser && chown -R appuser /app
USER appuser

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/eShopEvent/eShopEvent.csproj", "src/eShopEvent/"]
COPY ["src/eShopHealthCheck/eShopHealthCheck.csproj", "src/eShopHealthCheck/"]
COPY ["src/BasketAPI/BasketAPI.csproj", "src/BasketAPI/"]
RUN dotnet restore "src/BasketAPI/BasketAPI.csproj"
COPY . .
WORKDIR "/src/src/BasketAPI"
RUN dotnet build "BasketAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BasketAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BasketAPI.dll"]
