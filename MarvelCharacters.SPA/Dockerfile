FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["MarvelCharacters.SPA/MarvelCharacters.SPA.csproj", "MarvelCharacters.SPA/"]
RUN dotnet restore "MarvelCharacters.SPA/MarvelCharacters.SPA.csproj"
COPY . .
WORKDIR "/src/MarvelCharacters.SPA"
RUN dotnet build "MarvelCharacters.SPA.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MarvelCharacters.SPA.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MarvelCharacters.SPA.dll"]