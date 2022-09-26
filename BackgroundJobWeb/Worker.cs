namespace BackgroundJobWeb
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                ProduceChargeEvent();
                await Task.Delay(5000, stoppingToken);
            }
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            //await Task.Delay(5000, cancellationToken);
            return base.StartAsync(cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            return base.StopAsync(cancellationToken);
        }

        public void ProduceChargeEvent()
        {
            string sessionID = string.Empty;
            sessionID=Guid.NewGuid().ToString();
            _logger.LogInformation("sessionID", sessionID);
            produceStartEvent(sessionID);
        }

        private void produceStartEvent(string sessionID)
        {
            Random rnd = new Random();
            int randomnumber = rnd.Next(1, 5);
        }
        private void produceUpdateEvent(string sessionID)
        {

        }

        private void produceStopEvent(string sessionID)
        {

        }
    }
}