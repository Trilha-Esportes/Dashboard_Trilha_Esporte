FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

# Limpa cache NuGet para evitar problemas
RUN dotnet nuget locals all --clear

COPY *.sln ./
COPY *.csproj ./

RUN dotnet restore DashboardTrilhaEsporte.sln

COPY . ./

RUN dotnet publish DashboardTrilhaEsporte.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "DashboardTrilhaEsporte.dll"]
