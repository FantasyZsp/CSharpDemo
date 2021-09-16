namespace WebApplication
{
    public class MqConnectionsProperties
    {
        public MqProperties Mq1 { get; set; }
    }

    public class MqProperties
    {
        public string ExchangeName { get; set; }
        public string ExchangeType { get; set; }
        public string RoutingKey { get; set; } = string.Empty;
        public string QueueName { get; set; }
    }
}