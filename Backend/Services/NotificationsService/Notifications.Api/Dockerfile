FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY Services/Directory.Build.props Services/
COPY Services/stylecop.json Services/
COPY Services/NotificationsService/Notifications.Api/*.csproj Services/NotificationsService/Notifications.Api/
COPY Services/NotificationsService/Notifications.Core/*.csproj Services/NotificationsService/Notifications.Core/
COPY Services/Shared/MetersApp.Shared/*.csproj Services/Shared/MetersApp.Shared/

RUN --mount=type=cache,id=nuget,target=/root/.nuget/packages \
    dotnet restore Services/NotificationsService/Notifications.Api/Notifications.Api.csproj

COPY . .

RUN --mount=type=cache,id=nuget,target=/root/.nuget/packages \
    dotnet publish Services/NotificationsService/Notifications.Api/Notifications.Api.csproj \
    --no-restore -c "$BUILD_CONFIGURATION" -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Notifications.Api.dll"]
