FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/services/Frontliners.Inventory/Frontliners.Inventory.csproj", "src/services/Frontliners.Inventory/"]
COPY ["src/shared/Frontliners.Common/Frontliners.Common.csproj", "src/shared/Frontliners.Common/"]
COPY ["src/shared/Frontliners.Contract/Frontliners.Contract.csproj", "src/shared/Frontliners.Contract/"]
RUN dotnet restore "src/services/Frontliners.Inventory/Frontliners.Inventory.csproj"
COPY . .
WORKDIR "/src/src/services/Frontliners.Inventory"
RUN dotnet build "Frontliners.Inventory.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Frontliners.Inventory.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Frontliners.Inventory.dll"]
