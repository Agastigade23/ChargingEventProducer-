using System;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
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
                TimeSpan.FromSeconds(5));

            return Task.CompletedTask;
        }

        public void ProduceChargeEvent(object? state)
        {
            string sessionID = string.Empty;
            sessionID = Guid.NewGuid().ToString();
            

            _logger.LogInformation(
                "sessionID: {Count}", sessionID);

            produceStartEvent(sessionID);
            produceStopEvent(sessionID);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            return base.StopAsync(cancellationToken);
        }

        

        private void produceStartEvent(string sessionID)
        {
            Random rnd = new Random();
            int randomnumber = rnd.Next(1, 5);
            
            string doc=CreateProduceStartEventXML(sessionID);
            _logger.LogInformation(
               "Start event xml: {Count}", doc.ToString());
            if (randomnumber == 3)
            {
                produceUpdateEvent(sessionID);
            }
            
            
        }
        private void produceUpdateEvent(string sessionID)
        {
            string Updatedoc = CreateProduceUpdateEventXML(sessionID);
            _logger.LogInformation(
               "update event xml : {Count}", Updatedoc.ToString());
        }

        private void produceStopEvent(string sessionID)
        {
            string stopdoc = CreateProduceStopEventXML(sessionID);
            _logger.LogInformation(
               "stop event xml: {Count}", stopdoc.ToString());

        }

        public string CreateProduceStartEventXML(string sessionID)
        {
            XmlDocument doc = new XmlDocument();
            try
            {
                XmlNode docNode = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
                doc.AppendChild(docNode);

                XmlElement chargeDataNode = doc.CreateElement("ChargePointEvents");

                doc.AppendChild(chargeDataNode);
                
                XmlNode chargeRecordNode = doc.CreateElement("Event");
                doc.DocumentElement.AppendChild(chargeRecordNode);


                XmlNode chargeNameNode = doc.CreateElement("feedEventName");
                chargeNameNode.AppendChild(doc.CreateTextNode("A"));
                chargeRecordNode.AppendChild(chargeNameNode);

                XmlNode chargeTypeNode = doc.CreateElement("stationID");
                chargeTypeNode.AppendChild(doc.CreateTextNode("123"));
                chargeRecordNode.AppendChild(chargeTypeNode);
               
                XmlNode chargeStatusNode = doc.CreateElement("sessionID");

                chargeStatusNode.AppendChild(doc.CreateTextNode(sessionID));
                chargeRecordNode.AppendChild(chargeStatusNode);

                //doc.Save(Console.Out);
                //var basePath = Path.Combine(Environment.CurrentDirectory, @"XmlFiles\");
                //if (!Directory.Exists(basePath))
                //{
                //    Directory.CreateDirectory(basePath);
                //}
                //var newFileName = string.Format("{0}{1}", Guid.NewGuid().ToString("N"), ".xml");
                //doc.Save(basePath + newFileName);


               
            }
            catch (Exception ex)
            {
                
            }
            return doc.InnerXml.ToString();
        }

        public string CreateProduceUpdateEventXML(string sessionID)
        {
            XmlDocument doc = new XmlDocument();
            try
            {
                XmlNode docNode = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
                doc.AppendChild(docNode);

                XmlElement chargeDataNode = doc.CreateElement("ChargePointEvents");

                doc.AppendChild(chargeDataNode);

                XmlNode chargeRecordNode = doc.CreateElement("Event");
                doc.DocumentElement.AppendChild(chargeRecordNode);
                
                XmlNode chargeNameNode = doc.CreateElement("feedEventName");
                chargeNameNode.AppendChild(doc.CreateTextNode("A"));
                chargeRecordNode.AppendChild(chargeNameNode);
                
                XmlNode chargeTypeNode = doc.CreateElement("stationTime");
                chargeTypeNode.AppendChild(doc.CreateTextNode(DateTime.UtcNow.ToString()));
                chargeRecordNode.AppendChild(chargeTypeNode);

                XmlNode chargeStatusNode = doc.CreateElement("sessionID");

                chargeStatusNode.AppendChild(doc.CreateTextNode(sessionID));
                chargeRecordNode.AppendChild(chargeStatusNode);
            }
            catch (Exception ex)
            {

            }
            return doc.InnerXml.ToString();
        }


        public string CreateProduceStopEventXML(string sessionID)
        {
            XmlDocument doc = new XmlDocument();
            try
            {
                XmlNode docNode = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
                doc.AppendChild(docNode);
                XmlElement chargeDataNode = doc.CreateElement("ChargePointEvents");

                doc.AppendChild(chargeDataNode);
                XmlNode chargeRecordNode = doc.CreateElement("Event");
                doc.DocumentElement.AppendChild(chargeRecordNode);
                
                XmlNode chargeNameNode = doc.CreateElement("feedEventName");
                chargeNameNode.AppendChild(doc.CreateTextNode("A"));
                chargeRecordNode.AppendChild(chargeNameNode);
                
                XmlNode chargeTypeNode = doc.CreateElement("stationId");
                chargeTypeNode.AppendChild(doc.CreateTextNode("123"));
                chargeRecordNode.AppendChild(chargeTypeNode);

                XmlNode chargePortNode = doc.CreateElement("PortNumber");
                chargePortNode.AppendChild(doc.CreateTextNode("3"));
                chargePortNode.AppendChild(chargeTypeNode);
                
                XmlNode chargeStatusNode = doc.CreateElement("sessionID");

                chargeStatusNode.AppendChild(doc.CreateTextNode(sessionID));
                chargeRecordNode.AppendChild(chargeStatusNode);

            }
            catch (Exception ex)
            {

            }
            return doc.InnerXml.ToString();
        }
    }

}