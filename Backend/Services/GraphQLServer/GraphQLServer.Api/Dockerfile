FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY Services/Directory.Build.props .
COPY Services/GraphQLServer/GraphQLServer.Api/GraphQLServer.Api.csproj Services/GraphQLServer/GraphQLServer.Api/
COPY Services/GraphQLServer/GraphQLServer.Core/GraphQLServer.Core.csproj Services/GraphQLServer/GraphQLServer.Core/
COPY Services/GraphQLServer/GraphQLServer.Data/GraphQLServer.Data.csproj Services/GraphQLServer/GraphQLServer.Data/
COPY Services/Shared/MetersApp.Shared/MetersApp.Shared.csproj Services/Shared/MetersApp.Shared/
RUN --mount=type=cache,id=nuget,target=/root/.nuget/packages,sharing=locked \
    dotnet restore Services/GraphQLServer/GraphQLServer.Api/GraphQLServer.Api.csproj

COPY . .
WORKDIR /src/Services/GraphQLServer/GraphQLServer.Api
RUN --mount=type=cache,id=nuget,target=/root/.nuget/packages,sharing=locked \
    dotnet build GraphQLServer.Api.csproj --no-restore -c "$BUILD_CONFIGURATION" && \
    dotnet publish GraphQLServer.Api.csproj --no-build -c "$BUILD_CONFIGURATION" -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "GraphQLServer.Api.dll"]