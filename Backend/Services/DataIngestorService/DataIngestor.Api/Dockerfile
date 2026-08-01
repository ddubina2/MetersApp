FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY Services/Directory.Build.props Services/
COPY Services/stylecop.json Services/
COPY Services/DataIngestorService/DataIngestor.Api/*.csproj Services/DataIngestorService/DataIngestor.Api/
COPY Services/DataIngestorService/DataIngestor.Core/*.csproj Services/DataIngestorService/DataIngestor.Core/
COPY Services/Shared/MetersApp.Shared/*.csproj Services/Shared/MetersApp.Shared/

RUN --mount=type=cache,id=nuget,target=/root/.nuget/packages \
    dotnet restore Services/DataIngestorService/DataIngestor.Api/DataIngestor.Api.csproj

COPY . .

RUN --mount=type=cache,id=nuget,target=/root/.nuget/packages \
    dotnet publish Services/DataIngestorService/DataIngestor.Api/DataIngestor.Api.csproj \
    --no-restore -c "$BUILD_CONFIGURATION" -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "DataIngestor.Api.dll"]
