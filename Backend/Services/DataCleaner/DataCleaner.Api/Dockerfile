  FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
  USER $APP_UID
  WORKDIR /app

  FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
  ARG BUILD_CONFIGURATION=Release
  WORKDIR /src

  COPY Services/Directory.Build.props .
  COPY Services/DataCleaner/DataCleaner.Api/DataCleaner.Api.csproj Services/DataCleaner/DataCleaner.Api/
  COPY Services/DataCleaner/DataCleaner.Core/DataCleaner.Core.csproj Services/DataCleaner/DataCleaner.Core/
  COPY Services/DataCleaner/DataCleaner.Data/DataCleaner.Data.csproj Services/DataCleaner/DataCleaner.Data/
  COPY Services/Shared/MetersApp.Shared/MetersApp.Shared.csproj Services/Shared/MetersApp.Shared/
  RUN --mount=type=cache,id=nuget,target=/root/.nuget/packages,sharing=locked \
      dotnet restore Services/DataCleaner/DataCleaner.Api/DataCleaner.Api.csproj

  COPY . .
  WORKDIR /src/Services/DataCleaner/DataCleaner.Api
  RUN --mount=type=cache,id=nuget,target=/root/.nuget/packages,sharing=locked \
    dotnet build DataCleaner.Api.csproj --no-restore -c "$BUILD_CONFIGURATION" && \
      dotnet publish DataCleaner.Api.csproj --no-build -c "$BUILD_CONFIGURATION" -o /app/publish /p:UseAppHost=false

  FROM base AS final
  WORKDIR /app
  COPY --from=build /app/publish .
  ENTRYPOINT ["dotnet", "DataCleaner.Api.dll"]