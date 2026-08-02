FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY Services/Directory.Build.props .
COPY Services/Notifications/Notifications.Api/Notifications.Api.csproj Services/Notifications/Notifications.Api/
COPY Services/Notifications/Notifications.Core/Notifications.Core.csproj Services/Notifications/Notifications.Core/
COPY Services/Shared/MetersApp.Shared/MetersApp.Shared.csproj Services/Shared/MetersApp.Shared/
RUN --mount=type=cache,id=nuget,target=/root/.nuget/packages \
    dotnet restore Services/Notifications/Notifications.Api/Notifications.Api.csproj

COPY . .
WORKDIR /src/Services/Notifications/Notifications.Api
RUN dotnet build Notifications.Api.csproj --no-restore -c "$BUILD_CONFIGURATION" && \
    dotnet publish Notifications.Api.csproj --no-build -c "$BUILD_CONFIGURATION" -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Notifications.Api.dll"]