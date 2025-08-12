FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copia apenas arquivos de projeto para aproveitar cache
COPY ./src/ControleReservas.MVC/ControleReservas.MVC.csproj ControleReservas.MVC/
COPY ./src/ControleReservas.API/ControleReservas.API.csproj ControleReservas.API/
COPY ./src/ControleReservas.Application/ControleReservas.Application.csproj ControleReservas.Application/
COPY ./src/ControleReservas.Infrastructure/ControleReservas.Infrastructure.csproj ControleReservas.Infrastructure/

# Restaura dependências do MVC
RUN dotnet restore ControleReservas.MVC/ControleReservas.MVC.csproj

# Copia todo o código
COPY ./src .

# Compila e publica o MVC
WORKDIR /src/ControleReservas.MVC
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "ControleReservas.MVC.dll"]
