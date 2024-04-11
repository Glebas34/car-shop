FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 4042
EXPOSE 5052

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["CardService/card-service.csproj", "card-service_build/"]
COPY ["Contracts/Contracts.csproj", "Contracts/"]
RUN dotnet restore "./card-service_build/./card-service.csproj"
WORKDIR "/src/Contracts/"
COPY "Contracts/" .
WORKDIR "/src/card-service_build"
COPY "CardService/" .
RUN dotnet build "./card-service.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./card-service.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

RUN mkdir -p /app/certificates
COPY CardService/certificates/aspnetapp.pfx /app/certificates

ENTRYPOINT ["dotnet", "card-service.dll"]