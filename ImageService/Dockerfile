FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 4046
EXPOSE 5055

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["ImageService/image-service.csproj", "image-service_build/"]
COPY ["Contracts/Contracts.csproj", "Contracts/"]
RUN dotnet restore "./image-service_build/./image-service.csproj"
WORKDIR "/src/Contracts/"
COPY "Contracts/" .
WORKDIR "/src/image-service_build"
COPY "ImageService/" .
RUN dotnet build "./image-service.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./image-service.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

RUN mkdir -p /app/certificates
COPY ImageService/certificates/aspnetapp.pfx /app/certificates

ENTRYPOINT ["dotnet", "image-service.dll"]