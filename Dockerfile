FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["E_Commerce.API/E_Commerce.API.csproj", "E_Commerce.API/"]
COPY ["E_Commerce.Application/E_Commerce.Application.csproj", "E_Commerce.Application/"]
COPY ["E_Commerce.Domain/E_Commerce.Domain.csproj", "E_Commerce.Domain/"]
COPY ["E_Commerce.Infrastructure/E_Commerce.Infrastructure.csproj", "E_Commerce.Infrastructure/"]

RUN dotnet restore "E_Commerce.API/E_Commerce.API.csproj"

COPY . .
WORKDIR "/src/E_Commerce.API"
RUN dotnet publish "E_Commerce.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080
ENTRYPOINT ["dotnet", "E_Commerce.API.dll"]
