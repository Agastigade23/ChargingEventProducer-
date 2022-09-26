using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
namespace BackgroundJobWeb
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IServiceScopeFactory _factory;
        private Timer? _timer = null;
        private int executionCount = 0;
        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
        //    while (!stoppingToken.IsCancellationRequested)
        //    {
        //        _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
        //        await using AsyncServiceScope asyncScope = _factory.CreateAsyncScope();

        //        // Get service from scope
        //        ProduceChargeEvent produceChargeEvent = asyncScope.ServiceProvider.GetRequiredService<ProduceChargeEvent>();
        //        await produceChargeEvent.DoSomethingAsync();
        //        //string test= ProduceChargeEvent();
        //        await Task.Delay(5000, stoppingToken);
        //    }
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            _timer = new Timer(ProduceChargeEvent, null, TimeSpan.Zero,
                TimeSpan.FromSeconds(1));

            return Task.CompletedTask;
            
            
        }

        public void ProduceChargeEvent(object? state)
        {
            string sessionID = string.Empty;
            sessionID = Guid.NewGuid().ToString();
            

            _logger.LogInformation(
                "Count: {Count}", sessionID);

            produceStartEvent(sessionID);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            return base.StopAsync(cancellationToken);
        }

        

        private void produceStartEvent(string sessionID)
        {
            Random rnd = new Random();
            int randomnumber = rnd.Next(1, 5);
            _logger.LogInformation(
               "randomnumber Count: {Count}", randomnumber);


            produceUpdateEvent(sessionID, randomnumber);

        }
        private void produceUpdateEvent(string sessionID, int randomnumber)
        {

        }

        private void produceStopEvent(string sessionID)
        {

        }
    }
}