# syntax=docker/dockerfile:1
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY ["TicTacToe.sln", "./"]
COPY ["TicTacToe.Api/TicTacToe.Api.csproj", "TicTacToe.Api/"]
COPY ["TicTacToe.Contracts/TicTacToe.Contracts.csproj", "TicTacToe.Contracts/"]
COPY ["TicTacToe.Core/TicTacToe.Core.csproj", "TicTacToe.Core/"]
COPY ["TicTacToe.IntegrationTests/TicTacToe.IntegrationTests.csproj", "TicTacToe.IntegrationTests/"]
COPY ["TicTacToe.Services/TicTacToe.Services.csproj", "TicTacToe.Services/"]
COPY ["TicTacToe.UnitTests/TicTacToe.UnitTests.csproj", "TicTacToe.UnitTests/"]
RUN dotnet restore

COPY . .
RUN dotnet publish TicTacToe.Api -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080
ENTRYPOINT ["dotnet", "TicTacToe.Api.dll"]
