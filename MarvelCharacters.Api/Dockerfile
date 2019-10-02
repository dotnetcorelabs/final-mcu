FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["MarvelCharacters.Api/MarvelCharacters.Api.csproj", "MarvelCharacters.Api/"]
RUN dotnet restore "MarvelCharacters.Api/MarvelCharacters.Api.csproj"
COPY . .
WORKDIR "/src/MarvelCharacters.Api"
RUN dotnet build "MarvelCharacters.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MarvelCharacters.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MarvelCharacters.Api.dll"]