#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/runtime:3.1-buster-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["PortaCapena.OdooJsonRpcClient.Example/PortaCapena.OdooJsonRpcClient.Example.csproj", "PortaCapena.OdooJsonRpcClient.Example/"]
COPY ["PortaCapena.OdooJsonRpcClient/PortaCapena.OdooJsonRpcClient.csproj", "PortaCapena.OdooJsonRpcClient/"]
COPY ["PortaCapena.OdooJsonRpcClient.Shared/PortaCapena.OdooJsonRpcClient.Shared.csproj", "PortaCapena.OdooJsonRpcClient.Shared/"]
RUN dotnet restore "PortaCapena.OdooJsonRpcClient.Example/PortaCapena.OdooJsonRpcClient.Example.csproj"
COPY . .
WORKDIR "/src/PortaCapena.OdooJsonRpcClient.Example"
RUN dotnet build "PortaCapena.OdooJsonRpcClient.Example.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PortaCapena.OdooJsonRpcClient.Example.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PortaCapena.OdooJsonRpcClient.Example.dll"]