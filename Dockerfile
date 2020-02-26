FROM microsoft/dotnet:sdk AS build-env
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
FROM microsoft/dotnet:aspnetcore-runtime
WORKDIR /app
COPY --from=build-env /app/VulnusCloud/out .
ENTRYPOINT ["dotnet", "VulnusCloud.dll"]