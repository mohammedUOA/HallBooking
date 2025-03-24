# Base image for the runtime environment
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Build image with SDK for building the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Continuous_Learning_Booking.csproj", "."]
RUN dotnet restore "./Continuous_Learning_Booking.csproj"
COPY . . 
WORKDIR "/src/."
RUN dotnet build "./Continuous_Learning_Booking.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish the application for production
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Continuous_Learning_Booking.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final image for running the application
FROM base AS final
WORKDIR /app

# Copy the published files and the SQLite database to the container
COPY --from=publish /app/publish . 

# Copy the SQLite database if it exists in the local directory
COPY hallbooking.db /app/hallbooking.db

# Set the entry point to run the app
ENTRYPOINT ["dotnet", "Continuous_Learning_Booking.dll"]
