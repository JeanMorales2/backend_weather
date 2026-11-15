FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["src/MarcasAutos.Domain/MarcasAutos.Domain.csproj", "src/MarcasAutos.Domain/"]
COPY ["src/MarcasAutos.Application/MarcasAutos.Application.csproj", "src/MarcasAutos.Application/"]
COPY ["src/MarcasAutos.Infrastructure/MarcasAutos.Infrastructure.csproj", "src/MarcasAutos.Infrastructure/"]
COPY ["src/MarcasAutos.Main/MarcasAutos.Main.csproj", "src/MarcasAutos.Main/"]
RUN dotnet restore "src/MarcasAutos.Main/MarcasAutos.Main.csproj"
COPY . .
RUN dotnet publish "src/MarcasAutos.Main/MarcasAutos.Main.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "MarcasAutos.Main.dll"]