FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY MyCv.Domain/MyCv.Domain.csproj MyCv.Domain/
COPY MyCv.Application/MyCv.Application.csproj MyCv.Application/
COPY MyCv.Infrastructure/MyCv.Infrastructure.csproj MyCv.Infrastructure/
COPY MyCv.API/MyCv.API.csproj MyCv.API/
RUN dotnet restore MyCv.API/MyCv.API.csproj

COPY . .
RUN dotnet publish MyCv.API/MyCv.API.csproj -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "MyCv.API.dll"]
