FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY Services/Directory.Build.props .
COPY Services/DataIngestor/DataIngestor.Api/DataIngestor.Api.csproj Services/DataIngestor/DataIngestor.Api/
COPY Services/DataIngestor/DataIngestor.Core/DataIngestor.Core.csproj Services/DataIngestor/DataIngestor.Core/
COPY Services/Shared/MetersApp.Shared/MetersApp.Shared.csproj Services/Shared/MetersApp.Shared/
RUN --mount=type=cache,id=nuget,target=/root/.nuget/packages \
    dotnet restore Services/DataIngestor/DataIngestor.Api/DataIngestor.Api.csproj

COPY . .
WORKDIR /src/Services/DataIngestor/DataIngestor.Api
RUN dotnet build DataIngestor.Api.csproj --no-restore -c "$BUILD_CONFIGURATION" && \
    dotnet publish DataIngestor.Api.csproj --no-build -c "$BUILD_CONFIGURATION" -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "DataIngestor.Api.dll"]