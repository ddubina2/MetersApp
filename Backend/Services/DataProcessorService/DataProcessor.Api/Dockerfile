FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY Services/Directory.Build.props Services/
COPY Services/stylecop.json Services/
COPY Services/DataProcessorService/DataProcessor.Api/*.csproj Services/DataProcessorService/DataProcessor.Api/
COPY Services/DataProcessorService/DataProcessor.Core/*.csproj Services/DataProcessorService/DataProcessor.Core/
COPY Services/DataProcessorService/DataProcessor.Data/*.csproj Services/DataProcessorService/DataProcessor.Data/
COPY Services/Shared/MetersApp.Shared/*.csproj Services/Shared/MetersApp.Shared/

RUN --mount=type=cache,id=nuget,target=/root/.nuget/packages \
    dotnet restore Services/DataProcessorService/DataProcessor.Api/DataProcessor.Api.csproj

COPY . .

RUN --mount=type=cache,id=nuget,target=/root/.nuget/packages \
    dotnet publish Services/DataProcessorService/DataProcessor.Api/DataProcessor.Api.csproj \
    --no-restore -c "$BUILD_CONFIGURATION" -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "DataProcessor.Api.dll"]
