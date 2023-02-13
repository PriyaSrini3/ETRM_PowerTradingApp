using Axpo;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.Extensions.Options;
using PowerTradingApplication.Helper;
using PowerTradingApplication.Modal;
using System.Diagnostics.Metrics;
using System.Globalization;
using System.Linq;
using static PowerTradingApplication.SchedulerService;

namespace PowerTradingApplication
{
    public class SchedulerService : BackgroundService
    {
        private readonly ILogger<SchedulerService> _logger;
        private readonly IServiceProvider _serviceProvider;

        public SchedulerService(IServiceProvider serviceProvider, ILogger<SchedulerService> logger) {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }


        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    await DoWorkAsync(scope);
                    await Task.Delay(TimeSpan.FromMinutes(ConfigurationSetup.myConfig.TimeInterval), stoppingToken);
                }
            }
        }
        private async Task DoWorkAsync(IServiceScope scope)
        {
            try
            {
                _logger.LogInformation($"Timer Service Starts: {DateTime.Now.ToString("HH:mm:ss")}");
                Console.WriteLine($"Timer Service Starts: {DateTime.Now.ToString("HH:mm:ss")}");

                List<PowerPeriod> powerPeriodList = new List<PowerPeriod>();
                List<CsvFileOutputModel> outputs = new List<CsvFileOutputModel>();

                var transientService = scope.ServiceProvider.GetRequiredService<IPowerService>();
                var tradesList = transientService.GetTrades(DateTime.UtcNow).ToList();
                
                tradesList.ForEach(p => powerPeriodList.AddRange(p.Periods)) ;
                var outputPowerPeriodList = powerPeriodList.GroupBy(x => x.Period).Select(x => x.Sum(x => x.Volume)).ToList();

                int counter = 0;
                foreach (var output in outputPowerPeriodList)
                {
                    var previousDayHours = DateTime.Today.AddDays(-1).AddHours(23);
                    var SelectedDateDayHours = previousDayHours.AddHours(counter).ToString("HH:mm");
                    outputs.Add(new CsvFileOutputModel { LocalTime = SelectedDateDayHours, Volume = output });
                    counter++;
                }

                CsvFileGenerator.generateCSV(outputs);

                _logger.LogInformation($"Timer Service Ends: {DateTime.Now.ToString("HH:mm:ss")}");
                Console.WriteLine($"Timer Service Ends: {DateTime.Now.ToString("HH:mm:ss")}");

            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception thrown: {ex} and DataTime: {DateTime.Now.ToString("HH:mm:ss")}");
                Console.WriteLine($"Exception thrown: {ex} and DataTime: {DateTime.Now.ToString("HH:mm:ss")}");
            }
        }
        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Timer Service Stopped: {DateTime.Now.ToString("HH:mm:ss")}");
            Console.WriteLine($"Timer Service Stopped: {DateTime.Now.ToString("HH:mm:ss")}");
            return base.StopAsync(cancellationToken);
        }

   
    }
}
