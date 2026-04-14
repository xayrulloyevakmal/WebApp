# Build bosqichi
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Kod papka ichidan qidiriladi:
COPY ["Task4WebApp/Task4WebApp.csproj", "Task4WebApp/"]
RUN dotnet restore "Task4WebApp/Task4WebApp.csproj"

COPY . .
# Endi o'sha papka ichiga kiramiz
WORKDIR "/src/Task4WebApp"
RUN dotnet build "Task4WebApp.csproj" -c Release -o /app/build
RUN dotnet publish "Task4WebApp.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "Task4WebApp.dll"]
