FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# w katalogu kontekstu (ApiClothes) jest teraz ApiClothes.csproj
COPY *.csproj ./
RUN dotnet restore

COPY . ./
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 80
ENTRYPOINT ["dotnet", "ApiClothes.dll"]
