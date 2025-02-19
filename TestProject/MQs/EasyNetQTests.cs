using System.Threading.Tasks;
using EasyNetQ;
using EasyNetQ.Topology;
using Xunit;

namespace TestProject.MQs;

public class EasyNetQTests
{
    [Fact]
    public async Task Start()
    {
        var bus = RabbitHutch.CreateBus("host=localhost;publisherConfirms=true;timeout=10");
        await bus.SendReceive.SendAsync("test", "start");
        await bus.Advanced.PublishAsync(Exchange.Default, "test", true, new Message<string>("publish"));
    }
}