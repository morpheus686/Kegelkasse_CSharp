# Use the official .NET SDK image as a parent image
FROM mcr.microsoft.com/dotnet/sdk:8.0-windowsservercore-ltsc2022 AS build
WORKDIR /src

# Copy the project file and restore dependencies
COPY Strafenkatalog/Strafenkatalog.csproj Strafenkatalog/
RUN dotnet restore Strafenkatalog/Strafenkatalog.csproj

# Copy the remaining project files and publish the project
COPY Strafenkatalog/ Strafenkatalog/
WORKDIR /src/Strafenkatalog
RUN dotnet publish -c Release -r win-x64 --self-contained=false -o /app

# Use the ASP.NET runtime image as a base image
FROM mcr.microsoft.com/dotnet/aspnet:8.0-windowsservercore-ltsc2022
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet", "Strafenkatalog.exe"]