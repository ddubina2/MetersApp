FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY Services/Directory.Build.props Services/
COPY Services/stylecop.json Services/
COPY Services/GraphQLGatewayService/GraphQLGateway.Api/*.csproj Services/GraphQLGatewayService/GraphQLGateway.Api/
COPY Services/GraphQLGatewayService/GraphQLGateway.Core/*.csproj Services/GraphQLGatewayService/GraphQLGateway.Core/
COPY Services/GraphQLGatewayService/GraphQLGateway.Data/*.csproj Services/GraphQLGatewayService/GraphQLGateway.Data/
COPY Services/Shared/MetersApp.Shared/*.csproj Services/Shared/MetersApp.Shared/

RUN --mount=type=cache,id=nuget,target=/root/.nuget/packages \
    dotnet restore Services/GraphQLGatewayService/GraphQLGateway.Api/GraphQLGateway.Api.csproj

COPY . .

RUN --mount=type=cache,id=nuget,target=/root/.nuget/packages \
    dotnet publish Services/GraphQLGatewayService/GraphQLGateway.Api/GraphQLGateway.Api.csproj \
    --no-restore -c "$BUILD_CONFIGURATION" -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "GraphQLGateway.Api.dll"]
