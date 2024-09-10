FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

COPY src/Services/CatalogService.API/CatalogService.API.csproj ./CatalogService.API/
RUN dotnet restore CatalogService.API/CatalogService.API.csproj

COPY src/Services/CatalogService.API/. ./CatalogService.API/
WORKDIR /app/CatalogService.API
RUN dotnet build -c Release -o /app/build

RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 5001
ENTRYPOINT ["dotnet", "CatalogService.API.dll"]
