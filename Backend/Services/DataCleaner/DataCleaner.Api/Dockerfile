  FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
  WORKDIR /app

  FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
  ARG BUILD_CONFIGURATION=Release
  WORKDIR /src
  COPY . .

  WORKDIR /src/Services/DataCleaner/DataCleaner.Api
  RUN dotnet build DataCleaner.Api.csproj -c "$BUILD_CONFIGURATION" -o /app/build

  FROM build AS publish
  RUN dotnet publish DataCleaner.Api.csproj --no-restore -c "$BUILD_CONFIGURATION" -o /app/publish /p:UseAppHost=false

  FROM base AS final
  WORKDIR /app
  COPY --from=publish /app/publish .
  ENTRYPOINT ["dotnet", "DataCleaner.Api.dll"]
