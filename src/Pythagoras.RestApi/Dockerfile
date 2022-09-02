FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Pythagoras.RestApi/Pythagoras.RestApi.csproj", "Pythagoras.RestApi/"]
RUN dotnet restore "Pythagoras.RestApi/Pythagoras.RestApi.csproj"
COPY . .
WORKDIR "/src/Pythagoras.RestApi"
RUN dotnet build "Pythagoras.RestApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Pythagoras.RestApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Pythagoras.RestApi.dll"]
