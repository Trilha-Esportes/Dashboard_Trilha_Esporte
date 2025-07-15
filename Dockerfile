FROM --platform=linux/amd64 mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

COPY . ./

RUN dotnet restore DashboardTrilhaEsporte.sln

RUN dotnet publish DashboardTrilhaEsporte.csproj -c Release -o /app/publish

FROM --platform=linux/amd64 mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "DashboardTrilhaEsporte.dll"]
