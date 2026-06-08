FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["TechMove.Web/TechMove.Web.csproj", "TechMove.Web/"]
COPY ["TechMove.Shared/TechMove.Shared.csproj", "TechMove.Shared/"]
RUN dotnet restore "TechMove.Web/TechMove.Web.csproj"

COPY . .
WORKDIR /src/TechMove.Web
RUN dotnet publish "TechMove.Web.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:80
COPY --from=build /app/publish .

EXPOSE 80
ENTRYPOINT ["dotnet", "TechMove.Web.dll"]