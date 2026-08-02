  FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
  USER $APP_UID
  WORKDIR /app

  FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
  ARG BUILD_CONFIGURATION=Release
  WORKDIR /src

  COPY Services/Directory.Build.props .
  COPY Services/DataProcessor/DataProcessor.Api/DataProcessor.Api.csproj Services/DataProcessor/DataProcessor.Api/
  COPY Services/DataProcessor/DataProcessor.Core/DataProcessor.Core.csproj Services/DataProcessor/DataProcessor.Core/
  COPY Services/DataProcessor/DataProcessor.Data/DataProcessor.Data.csproj Services/DataProcessor/DataProcessor.Data/
  COPY Services/Shared/MetersApp.Shared/MetersApp.Shared.csproj Services/Shared/MetersApp.Shared/
  RUN --mount=type=cache,id=nuget,target=/root/.nuget/packages,sharing=locked \
      dotnet restore Services/DataProcessor/DataProcessor.Api/DataProcessor.Api.csproj

  COPY . .
  WORKDIR /src/Services/DataProcessor/DataProcessor.Api
  RUN --mount=type=cache,id=nuget,target=/root/.nuget/packages,sharing=locked \
    dotnet build DataProcessor.Api.csproj --no-restore -c "$BUILD_CONFIGURATION" && \
      dotnet publish DataProcessor.Api.csproj --no-build -c "$BUILD_CONFIGURATION" -o /app/publish /p:UseAppHost=false

  FROM base AS final
  WORKDIR /app
  COPY --from=build /app/publish .
  ENTRYPOINT ["dotnet", "DataProcessor.Api.dll"]