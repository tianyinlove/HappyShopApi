FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
COPY . .
ENTRYPOINT ["dotnet", "HappyShop.Api.dll"]