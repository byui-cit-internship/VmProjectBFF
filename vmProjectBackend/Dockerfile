
FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY *.csproj ./
RUN dotnet restore

# Copy everything else and build
COPY . ./
WORKDIR /app
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:3.1
WORKDIR /app
COPY --from=build /app/out ./

# not sure what this does
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT 0


ENTRYPOINT ["dotnet", "vmProjectBackend.dll"]

EXPOSE 80