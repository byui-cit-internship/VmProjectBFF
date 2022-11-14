
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /app
ENV ASPNETCORE_ENVIRONMENT Development

# Copy csproj and restore as distinct layers
COPY *.csproj ./
RUN dotnet restore

# Copy everything else and build
COPY . ./

RUN dotnet publish -c Release -o out /p:EnvironmentName=Development

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS runtime
WORKDIR /app
COPY --from=build /app/out ./

# not sure what this does
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT 0
ENV ASPNETCORE_ENVIRONMENT Development

ENTRYPOINT ["dotnet", "vmProjectBFF.dll"]

