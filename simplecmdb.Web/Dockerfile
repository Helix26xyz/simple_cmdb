# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["simplecmdb.Web/simplecmdb.Web.csproj", "simplecmdb.Web/"]
COPY ["simplecmdb.ApiService/simplecmdb.ApiService.csproj", "simplecmdb.ApiService/"]
COPY ["simplecmdb.ServiceDefaults/simplecmdb.ServiceDefaults.csproj", "simplecmdb.ServiceDefaults/"]
COPY ["simplecmdb.SharedModels/simplecmdb.SharedModels.csproj", "simplecmdb.SharedModels/"]
RUN dotnet restore "./simplecmdb.Web/simplecmdb.Web.csproj"
COPY . .
WORKDIR "/src/simplecmdb.Web"
RUN dotnet build "./simplecmdb.Web.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
# docker build complains about multiple publish profiles, so removing the others.
# probably bad but #insertshrugemoji
RUN rm /src/simplecmdb.ApiService/appsettings.Development.json /src/simplecmdb.Web/appsettings.Development.json /src/simplecmdb.ApiService/appsettings.json
RUN dotnet publish "./simplecmdb.Web.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "simplecmdb.Web.dll"]