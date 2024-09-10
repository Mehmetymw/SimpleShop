FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

COPY src/Services/BasketService.API/BasketService.API.csproj ./BasketService.API/
RUN dotnet restore BasketService.API/BasketService.API.csproj

COPY src/Services/BasketService.API/. ./BasketService.API/
WORKDIR /app/BasketService.API
RUN dotnet build -c Release -o /app/build

RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "BasketService.API.dll"]
