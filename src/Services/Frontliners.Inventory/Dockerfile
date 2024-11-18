FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 5166

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore "src/Services/Frontliners.Inventory/Frontliners.Inventory.csproj"


FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "src/Services/Frontliners.Inventory/Frontliners.Inventory.csproj" -c Release -o /app/publish 

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Frontliners.Inventory.dll"]
