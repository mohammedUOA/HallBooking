# Use the official ASP.NET Core runtime as a parent image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

# Use the SDK image to build the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Continuous_Learning_Booking.csproj", "./"]
RUN dotnet restore "./Continuous_Learning_Booking.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "Continuous_Learning_Booking.csproj" -c Release -o /app/build

# Publish the application
FROM build AS publish
RUN dotnet publish "Continuous_Learning_Booking.csproj" -c Release -o /app/publish

# Final stage/image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Continuous_Learning_Booking.dll"]