FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY Services/Directory.Build.props Services/
COPY Services/stylecop.json Services/
COPY Services/DataCleanerService/DataCleaner.Api/*.csproj Services/DataCleanerService/DataCleaner.Api/
COPY Services/DataCleanerService/DataCleaner.Core/*.csproj Services/DataCleanerService/DataCleaner.Core/
COPY Services/DataCleanerService/DataCleaner.Data/*.csproj Services/DataCleanerService/DataCleaner.Data/
COPY Services/Shared/MetersApp.Shared/*.csproj Services/Shared/MetersApp.Shared/

RUN --mount=type=cache,id=nuget,target=/root/.nuget/packages \
    dotnet restore Services/DataCleanerService/DataCleaner.Api/DataCleaner.Api.csproj

COPY . .

RUN --mount=type=cache,id=nuget,target=/root/.nuget/packages \
    dotnet publish Services/DataCleanerService/DataCleaner.Api/DataCleaner.Api.csproj \
    --no-restore -c "$BUILD_CONFIGURATION" -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "DataCleaner.Api.dll"]
