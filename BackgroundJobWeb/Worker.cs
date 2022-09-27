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
        private Timer? _timer = null;
        
        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            _timer = new Timer(ProduceChargeEvent, null, TimeSpan.Zero,
                TimeSpan.FromSeconds(10));

            return Task.CompletedTask;
        }

        public void ProduceChargeEvent(object? state)
        {
            string sessionID = string.Empty;
            sessionID = Guid.NewGuid().ToString();
            

            _logger.LogInformation(
                "sessionID: {Count}", sessionID);

            produceStartEvent(sessionID);
            produceUpdateEvent(sessionID);
            produceStopEvent(sessionID);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            return base.StopAsync(cancellationToken);
        }

        

        private void produceStartEvent(string sessionID)
        {
            string doc=CreateProduceStartEventXML(sessionID);
            _logger.LogInformation(
               "Start event xml: {Count}", doc.ToString());
            
        }
        private void produceUpdateEvent(string sessionID)
        {
            Random rnd = new Random();
            int randomnumber = rnd.Next(1, 5);
            XmlDocument xmlUpdatedoc = CreateProduceUpdateEventXML(sessionID);

            for (int i = 0; i < randomnumber; i++)
            {
                XmlNode node = xmlUpdatedoc.SelectSingleNode("/ChargePointEvents/Event/energy");
                node.InnerText = i.ToString();
            }

            _logger.LogInformation(
               "update event xml : {Count}", xmlUpdatedoc.InnerXml.ToString());
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

        public XmlDocument CreateProduceUpdateEventXML(string sessionID)
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

                XmlNode chargenergyNode = doc.CreateElement("energy");
                chargenergyNode.AppendChild(doc.CreateTextNode("B"));
                chargeRecordNode.AppendChild(chargenergyNode);

            }
            catch (Exception ex)
            {

            }
            return doc;
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