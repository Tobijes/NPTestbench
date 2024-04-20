# Inspiration: https://medium.com/@oluwabukunmi.aluko/dockerize-asp-net-core-web-app-with-multiple-layers-projects-part1-2256aa1b0511
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /App

# Copy everything
COPY NpTestBench.sln ./
COPY NPTestbench.csproj ./

# Restore as distinct layers
RUN dotnet restore
RUN dotnet tool restore

COPY ./ ./

# Build and publish a release
# WORKDIR /App/Homeplan.Api
RUN dotnet publish -c Release -o out 

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /App
COPY --from=build-env /App/out .

EXPOSE 5000

ENTRYPOINT ["dotnet", "NPTestbench.dll"]