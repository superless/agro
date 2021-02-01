#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/azure-functions/dotnet:3.0 AS base
WORKDIR /home/site/wwwroot
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["trifenix.agro.functions/trifenix.agro.functions.csproj", "trifenix.agro.functions/"]
COPY ["trifenix.connect.agro/trifenix.connect.agro.csproj", "trifenix.connect.agro/"]
COPY ["trifenix.connect.agro.external/trifenix.connect.agro.external.csproj", "trifenix.connect.agro.external/"]
COPY ["trifenix.connect.agro.queries/trifenix.connect.agro.queries.csproj", "trifenix.connect.agro.queries/"]
COPY ["trifenix.connect.agro.interfaces.cosmos/trifenix.connect.agro.interfaces.cosmos.csproj", "trifenix.connect.agro.interfaces.cosmos/"]
COPY ["trifenix.connect.agro.interfaces.search/trifenix.connect.agro.interfaces.search.csproj", "trifenix.connect.agro.interfaces.search/"]
COPY ["trifenix.connect.agro.interfaces.external/trifenix.connect.agro.interfaces.external.csproj", "trifenix.connect.agro.interfaces.external/"]
COPY ["trifenix.connect.agro.interfaces/trifenix.connect.agro.interfaces.csproj", "trifenix.connect.agro.interfaces/"]
RUN dotnet restore "trifenix.agro.functions/trifenix.agro.functions.csproj"
COPY . .
WORKDIR "/src/trifenix.agro.functions"
RUN dotnet build "trifenix.agro.functions.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "trifenix.agro.functions.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /home/site/wwwroot
COPY --from=publish /app/publish .
ENV AzureWebJobsScriptRoot=/home/site/wwwroot \
    AzureFunctionsJobHost__Logging__Console__IsEnabled=true