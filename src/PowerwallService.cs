using System.Text.Json;

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
        private readonly AppConfig _appConfig;
        
        private readonly SolarRepository _solarRepository;
        private readonly SiteRepository _siteRepository;
        private readonly LoadRepository _loadRepository;
        private readonly BatteryRepository _batteryRepository;
        private readonly CreateDatabase _createDatabase;
        
        private Task _task;
        
        public PowerwallService(ILogger<PowerwallService> logger, PowerwallClient powerwallClient, AppConfig appConfig)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));;
            _powerwallClient = powerwallClient ?? throw new ArgumentNullException(nameof(powerwallClient));;
            _appConfig = appConfig ?? throw new ArgumentNullException(nameof(appConfig));
            
            _createDatabase = new CreateDatabase(appConfig.DatabaseConnectionString);
            _solarRepository = new SolarRepository(appConfig.DatabaseConnectionString);
            _siteRepository = new SiteRepository(appConfig.DatabaseConnectionString);
            _loadRepository = new LoadRepository(appConfig.DatabaseConnectionString);
            _batteryRepository = new BatteryRepository(appConfig.DatabaseConnectionString);
        }

        private async Task GetLatestData(CancellationToken cancellationToken)
        {
            await _createDatabase.Create();
            
            while (!cancellationToken.IsCancellationRequested)
            {
                await Task.Run(async () =>
                    {
                        _logger.LogDebug($"Logging Data Started @ {DateTime.Now:h:mm:ss.fff tt zz}");

                        await _powerwallClient.Authenticate(_appConfig.Email, _appConfig.Password);
                        
                        // Read aggregates data from gateway
                        var response = await _powerwallClient.GetAggregates();
                        var responseSoe = await _powerwallClient.GetSystemStatusState();
                        var data = await response.Content.ReadAsStringAsync(cancellationToken);
                        var batteryData = await responseSoe.Content.ReadAsStringAsync(cancellationToken);
                        
                        var aggregatesData = JsonSerializer.Deserialize<AggregateModel>(data);
                        // The battery percentage information is from a separate service
                        // Read it and set it on battery model
                        var batteryObj = JsonSerializer.Deserialize<PercentageModel>(batteryData);
                        aggregatesData.Battery.Percentage = batteryObj.Percentage;
                        
                        // Insert data into DB
                        // TODO abstract this to support multiple providers, restructure to use same connection
                        await _solarRepository.Insert(aggregatesData.Solar);
                        await _siteRepository.Insert(aggregatesData.Site);
                        await _loadRepository.Insert(aggregatesData.Load);
                        await _batteryRepository.Insert(aggregatesData.Battery);
                        
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