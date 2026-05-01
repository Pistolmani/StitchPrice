FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy build props first so layer cache is invalidated only when they change
COPY global.json Directory.Build.props ./

# Copy csproj files and restore — separate layer from source so restore is cached
COPY src/StitchPrice.Domain/StitchPrice.Domain.csproj src/StitchPrice.Domain/
COPY src/StitchPrice.Application/StitchPrice.Application.csproj src/StitchPrice.Application/
COPY src/StitchPrice.Infrastructure/StitchPrice.Infrastructure.csproj src/StitchPrice.Infrastructure/
COPY src/StitchPrice.Api/StitchPrice.Api.csproj src/StitchPrice.Api/
RUN dotnet restore src/StitchPrice.Api/StitchPrice.Api.csproj

COPY src/ src/
RUN dotnet publish src/StitchPrice.Api/StitchPrice.Api.csproj \
    -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 8080
ENTRYPOINT ["dotnet", "StitchPrice.Api.dll"]
