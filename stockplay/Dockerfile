FROM mcr.microsoft.com/dotnet/runtime:5.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["stockplay/stockplay.csproj", "stockplay/"]
RUN dotnet restore "stockplay/stockplay.csproj"
COPY . .
WORKDIR "/src/stockplay"
RUN dotnet build "stockplay.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "stockplay.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "stockplay.dll"]
