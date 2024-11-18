FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY . .
RUN dotnet restore "src/Gateway/Frontliners.Gateway/Frontliners.Gateway.csproj"

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "src/Gateway/Frontliners.Gateway/Frontliners.Gateway.csproj" -c $BUILD_CONFIGURATION -o /app/publish 

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Frontliners.Gateway.dll"]
