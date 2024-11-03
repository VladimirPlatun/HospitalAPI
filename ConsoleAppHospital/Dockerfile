# Use the official .NET SDK image to build the app
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /app

# Copy the project files
COPY ../HospitalAPI/HospitalAPI.csproj ./HospitalAPI/
COPY ./ConsoleAppHospital/ConsoleAppHospital.csproj ./ConsoleAppHospital/

# Restore dependencies
RUN dotnet restore ./ConsoleAppHospital/ConsoleAppHospital.csproj

# Copy the entire project folder
COPY ../HospitalAPI ./HospitalAPI/
COPY ./ConsoleAppHospital ./ConsoleAppHospital/

# Publish the application
RUN dotnet publish ./ConsoleAppHospital/ConsoleAppHospital.csproj -c Release -o /out

# Build the final image
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build-env /out .
ENTRYPOINT ["dotnet", "ConsoleAppHospital.dll"]
