FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["src/MarcasAutos.Api/MarcasAutos.Api.csproj", "src/MarcasAutos.Api/"]
RUN dotnet restore "src/MarcasAutos.Api/MarcasAutos.Api.csproj"
COPY . .
RUN dotnet publish "src/MarcasAutos.Api/MarcasAutos.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "MarcasAutos.Api.dll"]
