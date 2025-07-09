FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Segrom.СrossesProject.Host/Segrom.СrossesProject.Host.csproj", "src/Segrom.СrossesProject.Host/"]
COPY ["src/Segrom.СrossesProject.Application/Segrom.СrossesProject.Application.csproj", "src/Segrom.СrossesProject.Application/"]
COPY ["src/Segrom.СrossesProject.Domain/Segrom.СrossesProject.Domain.csproj", "src/Segrom.СrossesProject.Domain/"]
COPY ["src/Segrom.СrossesProject.GameProvider/Segrom.СrossesProject.GameProvider.csproj", "src/Segrom.СrossesProject.GameProvider/"]
COPY ["src/Segrom.СrossesProject.PostgresRepository/Segrom.СrossesProject.PostgresRepository.csproj", "src/Segrom.СrossesProject.PostgresRepository/"]
RUN dotnet restore "src/Segrom.СrossesProject.Host/Segrom.СrossesProject.Host.csproj"
COPY . .
WORKDIR "/src/src/Segrom.СrossesProject.Host"
RUN dotnet build "Segrom.СrossesProject.Host.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Segrom.СrossesProject.Host.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Segrom.СrossesProject.Host.dll"]
