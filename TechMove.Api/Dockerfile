FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["TechMove.Api/TechMove.Api.csproj", "TechMove.Api/"]
COPY ["TechMove.Shared/TechMove.Shared.csproj", "TechMove.Shared/"]
RUN dotnet restore "TechMove.Api/TechMove.Api.csproj"

COPY . .
WORKDIR /src/TechMove.Api
RUN dotnet publish "TechMove.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:80

# ✅ Create wwwroot folder
RUN mkdir -p /app/wwwroot/uploads

COPY --from=build /app/publish .

EXPOSE 80
ENTRYPOINT ["dotnet", "TechMove.Api.dll"]