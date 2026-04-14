# Build bosqichi
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
# Loyihangiz nomini tekshiring, agar nomi 'WebApp' bo'lsa shunday qolsin
COPY ["WebApp.csproj", "."]
RUN dotnet restore "./WebApp.csproj"
COPY . .
RUN dotnet build "WebApp.csproj" -c Release -o /app/build
RUN dotnet publish "WebApp.csproj" -c Release -o /app/publish

# Run bosqichi
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .
# 'WebApp.dll' loyihangizning asosiy DLL nomi bo'lishi kerak
ENTRYPOINT ["dotnet", "WebApp.dll"]
