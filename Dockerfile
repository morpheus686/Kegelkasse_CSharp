# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0-windowsservercore-ltsc2022 AS build
WORKDIR /src
COPY Strafenkatalog/Strafenkatalog.csproj Strafenkatalog/
RUN dotnet restore Strafenkatalog/Strafenkatalog.csproj
COPY Strafenkatalog/ Strafenkatalog/
WORKDIR /src/Strafenkatalog
RUN dotnet publish -c Release -r win-x64 --self-contained=true -o /app

# Runtime stage
FROM mcr.microsoft.com/dotnet/runtime:8.0-windowsservercore-ltsc2022
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["./Strafenkatalog.exe"]
