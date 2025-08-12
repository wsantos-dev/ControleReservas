# 1. Build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copia apenas arquivos de projeto para aproveitar cache
COPY ./src/ControleReservas.API/ControleReservas.API.csproj ControleReservas.API/
COPY ./src/ControleReservas.MVC/ControleReservas.MVC.csproj ControleReservas.MVC/
COPY ./src/ControleReservas.Application/ControleReservas.Application.csproj ControleReservas.Application/
COPY ./src/ControleReservas.Infrastructure/ControleReservas.Infrastructure.csproj ControleReservas.Infrastructure/

# Restaura dependências da API (vai puxar as referências também)
RUN dotnet restore ControleReservas.API/ControleReservas.API.csproj

# Copia todo o código
COPY ./src .

# Compila e publica a API
WORKDIR /src/ControleReservas.API
RUN dotnet publish -c Release -o /app/publish

# 2. Runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "ControleReservas.API.dll"]
