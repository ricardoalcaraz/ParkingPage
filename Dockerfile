FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY . .
WORKDIR "/src/ParkingPageApi"
RUN dotnet restore "ParkingPageApi.csproj"
RUN dotnet build "ParkingPageApi.csproj" --no-restore -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ParkingPageApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ParkingPageApi.dll"]
