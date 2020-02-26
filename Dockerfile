FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS build-env
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY ./*.sln ./
COPY ./Business/*.csproj ./Business/
COPY ./Data/*.csproj ./Data/
COPY ./UnitTests/*.csproj ./UnitTests/
COPY ./VulnusCloud/*.csproj ./VulnusCloud/
RUN dotnet restore

# Copy everything else and build
COPY ./Business/. ./Business/
COPY ./Data/. ./Data/
COPY ./UnitTests/. ./UnitTests/
COPY ./VulnusCloud/. ./VulnusCloud/
RUN dotnet publish -c Release -o out

# Unit tests
RUN dotnet test "./UnitTests/UnitTests.csproj" -c Release --no-build --no-restore

# Build runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:2.2
WORKDIR /app
COPY --from=build-env /app/VulnusCloud/out .
ENTRYPOINT ["dotnet", "VulnusCloud.dll"]