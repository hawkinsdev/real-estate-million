FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["src/RealEstate.WebApi/RealEstate.WebApi.csproj", "src/RealEstate.WebApi/"]
COPY ["src/RealEstate.Application/RealEstate.Application.csproj", "src/RealEstate.Application/"]
COPY ["src/RealEstate.Domain/RealEstate.Domain.csproj", "src/RealEstate.Domain/"]
COPY ["src/RealEstate.Infrastructure/RealEstate.Infrastructure.csproj", "src/RealEstate.Infrastructure/"]

RUN dotnet restore "src/RealEstate.WebApi/RealEstate.WebApi.csproj"

COPY . .
WORKDIR "/src/src/RealEstate.WebApi"
RUN dotnet publish "RealEstate.WebApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 8080
ENTRYPOINT ["dotnet", "RealEstate.WebApi.dll"]
