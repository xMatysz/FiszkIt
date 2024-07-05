FROM mcr.microsoft.com/dotnet/sdk:8.0 as build

WORKDIR /src

COPY ["FiszkIt.Api/FiszkIt.Api.csproj", "FiszkIt.Api/"]
COPY ["FiszkIt.Domain/FiszkIt.Domain.csproj", "FiszkIt.Domain/"]
COPY ["FiszkIt.Core/FiszkIt.Core.csproj", "FiszkIt.Core/"]

RUN dotnet restore "FiszkIt.Api/FiszkIt.Api.csproj"

COPY . .
WORKDIR "/src/FiszkIt.Api"
RUN dotnet publish "FiszkIt.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 as base
WORKDIR /app
EXPOSE 5000
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "FiszkIt.Api.dll"]