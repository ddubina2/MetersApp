FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY Services/Directory.Build.props .
COPY Services/ApiGateway/ApiGateway.Api/ApiGateway.Api.csproj Services/ApiGateway/ApiGateway.Api/
COPY Services/Shared/MetersApp.Shared/MetersApp.Shared.csproj Services/Shared/MetersApp.Shared/
RUN --mount=type=cache,id=nuget,target=/root/.nuget/packages,sharing=locked \
    dotnet restore Services/ApiGateway/ApiGateway.Api/ApiGateway.Api.csproj

COPY . .
WORKDIR /src/Services/ApiGateway/ApiGateway.Api
RUN dotnet build ApiGateway.Api.csproj --no-restore -c "$BUILD_CONFIGURATION" && \
    dotnet publish ApiGateway.Api.csproj --no-build -c "$BUILD_CONFIGURATION" -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "ApiGateway.Api.dll"]