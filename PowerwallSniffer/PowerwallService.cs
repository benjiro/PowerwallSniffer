namespace PowerwallSniffer
{
    using System;
    using System.Text.Json.Serialization;
    using System.Threading;
    using System.Threading.Tasks;
    using DatabaseProvider.TimescaleDB.Repository;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using Model;

    public class PowerwallService : IHostedService
    {
        private readonly ILogger<PowerwallService> _logger;
        private readonly PowerwallClient _powerwallClient;
        
        private readonly SolarRepository _solarRepository;
        private readonly SiteRepository _siteRepository;
        private readonly LoadRepository _loadRepository;
        private readonly BatteryRepository _batteryRepository;
        
        private Task _task;
        
        public PowerwallService(ILogger<PowerwallService> logger, PowerwallClient powerwallClient, AppConfig appConfig)
        {
            _logger = logger;
            _powerwallClient = powerwallClient;
            _solarRepository = new SolarRepository(appConfig.DatabaseConnectionString);
            _siteRepository = new SiteRepository(appConfig.DatabaseConnectionString);
            _loadRepository = new LoadRepository(appConfig.DatabaseConnectionString);
            _batteryRepository = new BatteryRepository(appConfig.DatabaseConnectionString);
        }

        private async Task GetLatestData(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                await Task.Run(async () =>
                    {
                        _logger.LogDebug($"Logging Data Started @ {DateTime.Now:h:mm:ss.fff tt zz}");
                        
                        // Read aggregates data from gateway
                        var response = await _powerwallClient.GetAggregates();
                        var responseSoe = await _powerwallClient.GetSystemStatusState();
                        var data = await response.Content.ReadAsStringAsync();
                        var batteryData = await responseSoe.Content.ReadAsStringAsync();
                        
                        var aggregatesData = JsonSerializer.Parse<AggregateModel>(data);
                        // The battery percentage information is from a separate service
                        // Read it and set it on battery model
                        var batteryObj = JsonSerializer.Parse<PercentageModel>(batteryData);
                        aggregatesData.Battery.Percentage = batteryObj.Percentage;
                        
                        // Insert data into DB
                        // TODO abstract this to support multiple providers, restructure to use same connection
                        _solarRepository.Insert(aggregatesData.Solar);
                        _siteRepository.Insert(aggregatesData.Site);
                        _loadRepository.Insert(aggregatesData.Load);
                        _batteryRepository.Insert(aggregatesData.Battery);
                        
                        _logger.LogDebug($"Logging Data Finished @ {DateTime.Now:h:mm:ss.fff tt zz}");
                    },
                    cancellationToken);
                
                _logger.LogDebug("Polling in 10 seconds");
                await Task.Delay(TimeSpan.FromSeconds(10), cancellationToken);
            }
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _task = GetLatestData(cancellationToken);
            return _task;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return _task;
        }
    }
}