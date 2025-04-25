# Użyj obrazu SDK .NET 8.0
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Ustaw katalog roboczy
WORKDIR /src

# Kopiuj plik projektu do kontenera
COPY ApiClothes/ApiClothes.csproj ApiClothes/

# Przywróć zależności
RUN dotnet restore ApiClothes/ApiClothes.csproj

# Kopiuj całą resztę aplikacji
COPY . .

# Publikuj aplikację
WORKDIR /src/ApiClothes
RUN dotnet publish -c Release -o /app/publish

# Użyj obrazu bazowego ASP.NET
FROM mcr.microsoft.com/dotnet/aspnet:8.0

# Ustaw katalog roboczy
WORKDIR /app

# Kopiuj opublikowaną aplikację z poprzedniego etapu
COPY --from=build /app/publish .

# Wystaw port
EXPOSE 80

# Uruchom aplikację
ENTRYPOINT ["dotnet", "ApiClothes.dll"]
