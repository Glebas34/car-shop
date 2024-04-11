FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 4043
EXPOSE 5053

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["ApiGateway/api-gateway.csproj", "api-gateway_build/"]
RUN dotnet restore "./api-gateway_build/./api-gateway.csproj"
WORKDIR "/src/api-gateway_build"
COPY "ApiGateway/" .
RUN dotnet build "./api-gateway.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./api-gateway.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

RUN mkdir -p /app/certificates
COPY ApiGateway/certificates/aspnetapp.pfx /app/certificates

ENTRYPOINT ["dotnet", "api-gateway.dll"]