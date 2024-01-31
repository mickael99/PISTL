# Use the official ASP.NET Core SDK image to build the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG buildconfig=Release
WORKDIR /src
# Copy the .csproj file and restore dependencies
COPY . .
RUN dotnet restore "src/Project.csproj" --disable-parallel
RUN dotnet publish "src/Project.csproj" -c ${buildconfig} -o /app --no-restore

# Use the official ASP.NET Core runtime image as a base image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
COPY --from=build /app ./
EXPOSE 5050
ENTRYPOINT ["dotnet", "Project.dll"]
