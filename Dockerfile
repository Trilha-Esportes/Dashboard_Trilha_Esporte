# Etapa 1: build
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

# Copia o arquivo de solução e os arquivos necessários para o build
COPY . ./

# Restaura as dependências do projeto
RUN dotnet restore DashboardTrilhaEsporte.sln

# Publica o projeto no modo Release
RUN dotnet publish DashboardTrilhaEsporte.csproj -c Release -o /app/publish

# Etapa 2: runtime
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app

# Define a URL do ASP.NET Core
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

# Copia os arquivos publicados do estágio anterior
COPY --from=build /app/publish .

# Define o ponto de entrada da aplicação
ENTRYPOINT ["dotnet", "DashboardTrilhaEsporte.dll"]
