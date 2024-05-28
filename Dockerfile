# Use the official .NET SDK image as a parent image
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy the solution file and restore dependencies
COPY Strafenkatalog_CSharp.sln .
COPY Strafenkatalog/Strafenkatalog.csproj Strafenkatalog/
RUN dotnet restore Strafenkatalog/Strafenkatalog.csproj /p:EnableWindowsTargeting=true

# Copy the remaining project files and publish the project
COPY . .
WORKDIR /src/Strafenkatalog
RUN dotnet publish -c Release -o /app /p:EnableWindowsTargeting=true

# Use the ASP.NET runtime image as a base image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet", "Strafenkatalog.dll"]
