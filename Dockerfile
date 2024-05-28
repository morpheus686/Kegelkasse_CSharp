# Use the official .NET SDK image as a parent image
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy the solution file and restore dependencies
COPY Strafenkatalog_CSharp.sln .
COPY Strafenkatalog/Strafenkatalog.csproj Strafenkatalog/
RUN dotnet restore Strafenkatalog/Strafenkatalog.csproj /p:EnableWindowsTargeting=true

# Copy the remaining project files
COPY . .

# Set working directory to the project folder
WORKDIR /src/Strafenkatalog

# Run the publish command with detailed output for debugging
RUN dotnet publish -c Release -r linux-x64 --self-contained=true -o /app /p:EnableWindowsTargeting=true || \
    (echo "dotnet publish failed" && find . -name '*.csproj' -exec cat {} \; && exit 1)

# Use a base image with a minimal OS
FROM mcr.microsoft.com/dotnet/runtime-deps:8.0 AS runtime
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["./Strafenkatalog"]
