#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 10909

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["src/Microservice.PagoDeServicios/NuGet.Config", "src/Microservice.PagoDeServicios/NuGet.Config"]
COPY ["src/Microservice.PagoDeServicios/Microservice.PagoDeServicios.csproj", "src/Microservice.PagoDeServicios/"]
COPY ["src/Microservice.PagoDeServicios.EntityFramework.SqlServer/Microservice.PagoDeServicios.EntityFramework.SqlServer.csproj", "src/Microservice.PagoDeServicios.EntityFramework.SqlServer/"]
COPY ["src/Microservice.PagoDeServicios.EntityFramework/Microservice.PagoDeServicios.EntityFramework.csproj", "src/Microservice.PagoDeServicios.EntityFramework/"]
RUN dotnet restore "src/Microservice.PagoDeServicios/Microservice.PagoDeServicios.csproj" --configfile "src/Microservice.PagoDeServicios/NuGet.Config"
COPY . .
WORKDIR "/src/src/Microservice.PagoDeServicios"
RUN dotnet build "Microservice.PagoDeServicios.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Microservice.PagoDeServicios.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Microservice.PagoDeServicios.dll"]