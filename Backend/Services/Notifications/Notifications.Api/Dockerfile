FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY . .

WORKDIR /src/Services/Notifications/Notifications.Api
RUN dotnet build Notifications.Api.csproj -c "$BUILD_CONFIGURATION" -o /app/build

FROM build AS publish
RUN dotnet publish Notifications.Api.csproj --no-restore -c "$BUILD_CONFIGURATION" -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Notifications.Api.dll"]
