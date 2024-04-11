FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 4041
EXPOSE 5051

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["CarPageService/car-page-service.csproj", "car-page-service_build/"]
COPY ["Contracts/Contracts.csproj", "Contracts/"]
RUN dotnet restore "./car-page-service_build/./car-page-service.csproj"
WORKDIR "/src/Contracts/"
COPY "Contracts/" .
WORKDIR "/src/car-page-service_build"
COPY "CarPageService/" .
RUN dotnet build "./car-page-service.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./car-page-service.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

RUN mkdir -p /app/certificates
COPY CarPageService/certificates/aspnetapp.pfx /app/certificates

ENTRYPOINT ["dotnet", "car-page-service.dll"]