# 1. Build-Image mit SDK
FROM mcr.microsoft.com/dotnet/sdk:8.0-windowsservercore-ltsc2022 AS build
WORKDIR /src

# Copy solution and project files
COPY Kegelkasse/Kegelkasse.csproj Kegelkasse/

# Restore dependencies
RUN dotnet restore Kegelkasse/Kegelkasse.csproj

# Build and publish
RUN dotnet publish Kegelkasse/Kegelkasse.csproj -c Release -r win-x64 --self-contained=false -o /app

# 2. Runtime-Image
FROM mcr.microsoft.com/dotnet/aspnet:8.0-windowsservercore-ltsc2022
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet", "Kegelkasse.exe"]
