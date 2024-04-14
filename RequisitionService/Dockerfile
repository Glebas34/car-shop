FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 4044
EXPOSE 5054

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["RequisitionService/requisition-service.csproj", "requisition-service_build/"]
COPY ["Contracts/Contracts.csproj", "Contracts/"]
RUN dotnet restore "./requisition-service_build/./requisition-service.csproj"
WORKDIR "/src/Contracts/"
COPY "Contracts/" .
WORKDIR "/src/requisition-service_build"
COPY "RequisitionService/" .
RUN dotnet build "./requisition-service.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./requisition-service.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

RUN mkdir -p /app/certificates
COPY RequisitionService/certificates/aspnetapp.pfx /app/certificates

ENTRYPOINT ["dotnet", "requisition-service.dll"]