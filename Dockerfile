# Base image for running the app
From mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
Expose 80

# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj and restore dependencies
COPY *.csproj ./
RUN dotnet restore

# Copy everything else and publish
COPY . ./
RUN dotnet publish -c Release -o /app/publish

# Final Image
FROM base as final
WORKDIR /app
COPY --from=build /app/publish .

ENTRYPOINT [ "dotnet", "ProductswebAPI.dll" ]