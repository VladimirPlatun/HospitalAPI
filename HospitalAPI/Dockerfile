FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 5000
EXPOSE 5001

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["HospitalAPI/HospitalAPI.csproj", "HospitalAPI/"]
RUN dotnet restore "./HospitalAPI/HospitalAPI.csproj"
COPY . .
WORKDIR "/src/HospitalAPI"
RUN dotnet build "./HospitalAPI.csproj" -c ${BUILD_CONFIGURATION} -o /app/build

FROM build AS publish
RUN dotnet publish "./HospitalAPI.csproj" -c ${BUILD_CONFIGURATION} -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .

RUN dotnet tool install --global dotnet-ef --version 6.0.10
ENV PATH="$PATH:/root/.dotnet/tools"

ENTRYPOINT ["dotnet", "HospitalAPI.dll"]
