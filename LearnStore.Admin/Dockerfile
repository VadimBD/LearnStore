# Base runtime image (ASP.NET Core runtime only, lighter than SDK)
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080
# Set environment to Production by default
ENV DOTNET_ENVIRONMENT=Production

# Build stage (uses full SDK to compile the project)
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
# Copy only the project file first to leverage Docker layer caching
COPY ["LearnStore.Admin/LearnStore.Admin.csproj", "LearnStore.Admin/"]
RUN dotnet restore "LearnStore.Admin/LearnStore.Admin.csproj"
# Copy the rest of the source code
COPY . .
WORKDIR "/src/LearnStore.Admin"
# Build the project into /app/build
RUN dotnet build "LearnStore.Admin.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish stage (produces optimized binaries for runtime)
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "LearnStore.Admin.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final stage (production image, only runtime + published app)
FROM base AS final
WORKDIR /app
# Copy published output from the publish stage
COPY --from=publish /app/publish .
# Define entrypoint for the container
ENTRYPOINT ["dotnet", "LearnStore.Admin.dll"]