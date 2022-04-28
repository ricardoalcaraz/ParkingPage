FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["ParkingPage/ParkingPage.csproj", "ParkingPage/"]
RUN dotnet restore "ParkingPage/ParkingPage.csproj"
COPY . .
WORKDIR "/src/ParkingPage"
RUN dotnet build "ParkingPage.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ParkingPage.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ParkingPage.dll"]
