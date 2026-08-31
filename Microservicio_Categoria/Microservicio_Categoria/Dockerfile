# Etapa de compilación
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["Microservicio_Categoria.csproj", "./"]
RUN dotnet restore "Microservicio_Categoria.csproj"
COPY . .
RUN dotnet publish -c Release -o /app/publish

# Etapa final / ejecución
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
EXPOSE 80
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Microservicio_Categoria.dll"]