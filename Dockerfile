FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS build
WORKDIR /app

COPY *.sln .
COPY PowerwallSniffer/*.csproj ./PowerwallSniffer/
RUN dotnet restore

COPY PowerwallSniffer/. ./PowerwallSniffer/
WORKDIR /app/PowerwallSniffer
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/core/runtime:2.2 AS runtime
WORKDIR /app
COPY --from=build /app/PowerwallSniffer/out ./
ENTRYPOINT ["dotnet", "PowerwallSniffer.dll"]
