
# Use the base image for running the app
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 5147

# Set the environment variable for ASP.NET Core
ENV ASPNETCORE_URLS=http://+:80

# Use the SDK image for building and publishing the app
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG configuration=Release
WORKDIR /src

# Copy csproj and restore any dependencies (via nuget)
COPY ["TodoApi.csproj", "TodoApi/"]
RUN dotnet restore "TodoApi/TodoApi.csproj"

# Copy the rest of the source code
COPY . .

# Set the working directory to the project folder
WORKDIR "/src/TodoApi"

# Publish the app
RUN dotnet publish "TodoApi.csproj" -c $configuration -o /app/publish /p:UseAppHost=false

# Final stage: Copy the published app to the base image and set the entry point
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "TodoApi.dll"]
