TODO Clean up

## Setup

Setup timeseriesdb

`docker run -d --name powerwalldb -p 5432:5432 -e POSTGRES_PASSWORD=password timescale/timescaledb:latest-pg12`

Setup PowerwallSniffer

```
docker run -d --name powerwall-sniffer \
-e AppConfig__DatabaseConnectionString="INSERT_HERE" \
-e AppConfig__GatewayUrl="INSERT_HERE" \
-e AppConfig__Email="INSERT_HERE" \
-e AppConfig__Password="INSERT_HERE" \
benjiroau/powerwall-sniffer:latest
```

## Environment Variables

- AppConfig__DatabaseConnectionString - Connection string to timeseriesdb
- AppConfig__GatewayUrl	- Base URL to Powerwall Gateway 2
- AppConfig__Email - Email used to login to Powerwall Gateway 2
- AppConfig__Password - Password used to login to Powerwall Gateway 2