#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/runtime:2.1-stretch-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:2.1-stretch AS build
WORKDIR /src
COPY ["trifenix.agro.console/trifenix.agro.console.csproj", "trifenix.agro.console/"]
COPY ["trifenix.connect.agro.external/trifenix.connect.agro.external.csproj", "trifenix.connect.agro.external/"]
COPY ["trifenix.connect.agro.queries/trifenix.connect.agro.queries.csproj", "trifenix.connect.agro.queries/"]
COPY ["trifenix.connect.agro/trifenix.connect.agro.csproj", "trifenix.connect.agro/"]
COPY ["trifenix.connect.agro.interfaces.cosmos/trifenix.connect.agro.interfaces.cosmos.csproj", "trifenix.connect.agro.interfaces.cosmos/"]
COPY ["trifenix.connect.agro.interfaces.search/trifenix.connect.agro.interfaces.search.csproj", "trifenix.connect.agro.interfaces.search/"]
COPY ["trifenix.connect.agro.interfaces.external/trifenix.connect.agro.interfaces.external.csproj", "trifenix.connect.agro.interfaces.external/"]
COPY ["trifenix.connect.agro.interfaces/trifenix.connect.agro.interfaces.csproj", "trifenix.connect.agro.interfaces/"]
RUN dotnet restore "trifenix.agro.console/trifenix.agro.console.csproj"
COPY . .
WORKDIR "/src/trifenix.agro.console"
RUN dotnet build "trifenix.agro.console.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "trifenix.agro.console.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "trifenix.agro.console.dll"]