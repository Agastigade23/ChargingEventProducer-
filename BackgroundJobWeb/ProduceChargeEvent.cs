using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackgroundJobWeb
{
    class ProduceChargeEvent
    {
        private readonly ILogger<ProduceChargeEvent> _logger;

        public ProduceChargeEvent(ILogger<ProduceChargeEvent> logger)
        {
            _logger = logger;
        }

        public async Task DoSomethingAsync()
        {
            await Task.Delay(100);
            _logger.LogInformation(
                "Sample Service did something.");
        }
    }
}
