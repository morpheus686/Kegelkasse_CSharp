# Use the official .NET image as a parent image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Strafenkatalog_CSharp.sln", "."]
COPY ["Strafenkatalog/Strafenkatalog.csproj", "Strafenkatalog/"]
RUN dotnet restore
COPY . .
WORKDIR "/src/Strafenkatalog"
RUN dotnet build -c Release -o /app/build

FROM build AS publish
RUN dotnet publish -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Strafenkatalog.exe"]
